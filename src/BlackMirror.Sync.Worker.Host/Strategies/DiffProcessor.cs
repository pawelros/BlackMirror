namespace BlackMirror.Sync.Worker.Host.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Dto;
    using BlackMirror.HttpClient;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;

    public class DiffProcessor
    {
        //private readonly IMirror mirror;
        private readonly ISynchronization sync;
        private readonly ISvc source;
        private readonly ISvc target;
        private readonly IBlackMirrorHttpClient httpClient;
        private readonly ISyncLogger syncLogger;
        private readonly IUser defaultPushUser;
        private readonly List<string> ignoredFiles;
        private readonly IUserRetriever retriever;
        private readonly int timeoutMaxRetryCount;

        public DiffProcessor(IWorkerConfiguration workerConfig, ISynchronization sync, ISvc source, ISvc target, IBlackMirrorHttpClient httpClient, ISyncLogger syncLogger, IUser defaultPushUser)
        {
            //this.mirror = mirror;
            this.sync = sync;
            this.source = source;
            this.target = target;
            this.httpClient = httpClient;
            this.syncLogger = syncLogger;
            this.defaultPushUser = defaultPushUser;
            this.ignoredFiles = sync.Mirror.IgnoredFiles?.ToList() ?? new List<string>();
            this.ignoredFiles.AddRange(new[]
                                           {
                                               @"^\.svn\\?.*$",
                                               @"(^\.git\\.*$)|(^\.git$)",
                                               @"^.*\\node_modules\\.*$",
                                               @"^.*\\bower_components\\.*$",
                                               @"^packages\\?.*$",
                                               @"^.*\.dll$",
                                               @"^.*\.pdb",
                                               @"^.*\.nupkg",
                                               @"^.*\.tar",
                                               @"^.*\.tgz",
                                               @"^.*\.jar",
                                               @"^.*\.exe",
                                           });

            this.retriever = new UserRetriever(workerConfig);
            this.timeoutMaxRetryCount = workerConfig.TimeoutMaxRetryCount;
        }

        public void ApplyRevision(IRevision revision)
        {
            Logging.Log().Debug($"Revision {revision.Id} {revision.Message} checkout.");
            this.source.Checkout(revision);
            Logging.Log().Debug("Clearing target workspace.");
            this.ClearTarget();
            Logging.Log().Debug("Copying files.");
            this.CopyFiles();
            var users = this.retriever.GetUsers();
            Logging.Log().Debug("Resolving commiter credentials.");
            IUser commiter = this.GetTargetUser(users, revision);

            IRevision newRevision = null;

            int timeoutRetryCount = 0;
            bool invalidIssueIdHasAlreadyHappened = false;
            bool shouldRetry;

            do
            {
                shouldRetry = false;

                try
                {
                    newRevision = this.UpdateRemote(commiter, revision, revision.Message);
                }
                catch (Exception ex)
                {
                    Logging.Log().Error(ex, "Remote returned an error.");
                    this.syncLogger.Log(this.sync, $"Remote returned an error: {ex}");

                    if (ex.InnerException == null)
                    {
                        return;
                    }

                    if (InvalidIssueId(ex))
                    {
                        this.AppendCommitMessageWithPrefix(revision);

                        // retry once
                        if (!invalidIssueIdHasAlreadyHappened)
                        {
                            shouldRetry = true;
                            invalidIssueIdHasAlreadyHappened = true;
                        }
                    }
                    else
                    if (Timeout(ex))
                    {
                        if (timeoutRetryCount + 1 <= this.timeoutMaxRetryCount)
                        {
                            shouldRetry = true;
                            timeoutRetryCount++;
                        }
                    }
                }
            }
            while (shouldRetry);

            this.CreateReflection(revision, newRevision);
        }

        private void AppendCommitMessageWithPrefix(IRevision revision)
        {
            revision.Message = $"{this.target.DefaultCommitMessagePrefix} {revision.Message}";
            Logging.Log().Information($"Re-trying with message: {revision.Message}");
            this.syncLogger.Log(this.sync, $"Re-trying with message: {revision.Message}");
        }

        private static bool Timeout(Exception ex)
        {
            return ex.InnerException.Message.Contains("Unable to connect to a repository at URL");
        }

        private static bool InvalidIssueId(Exception ex)
        {
            return ex.InnerException.Message.Contains(
                       "An issue key needs to be provided to associate with this commit")
                   || ex.InnerException.Message.Contains(
                       "Each issue key in the commit message needs to be viewable by");
        }

        private void CreateReflection(IRevision sourceRevision, IRevision targetRevision)
        {
            var reflection = new Reflection
            {
                DateTime = DateTime.Now,
                Mirror = this.sync.Mirror,
                Synchronization = this.sync,
                SourceRevision = sourceRevision,
                TargetRevision = targetRevision
            };

            var dto = reflection.ToDto();

            this.httpClient.Reflection.Add(dto);
        }

        private IUser GetTargetUser(List<UserDto> users, IRevision revision)
        {
            //UserDto mappedUser = users.FirstOrDefault(
            //    u => u.RepositoryCredentials.Any(
            //        c => string.Equals(c.Login, revision.Author, StringComparison.InvariantCultureIgnoreCase) && c.RepositoryType == this.source.Type));

            UserDto mappedUser = users.FirstOrDefault(
                u => string.Equals(u.Email, revision.Author, StringComparison.InvariantCultureIgnoreCase));

            if (mappedUser == null)
            {
                Logging.Log().Warning($"Missing {revision.Author} user. Using default {this.defaultPushUser.Email}.");
                this.syncLogger.Log(this.sync, $"Missing {revision.Author} user. Using default {this.defaultPushUser.Email}.");

                mappedUser = users.FirstOrDefault(u => u.Id == this.defaultPushUser.Id);

                if (mappedUser == null)
                {
                    Logging.Log().Warning($"Default {this.defaultPushUser.Email} user does not have credentials matching revision author ({revision.Author}).");
                    this.syncLogger.Log(this.sync, $"Default {this.defaultPushUser.Email} user does not have credentials matching revision author ({revision.Author}).");
                    throw new InvalidOperationException($"Cannot find user with {this.source.Type} {revision.Author} credentials.");
                }
            }

            return mappedUser.ToUser();
        }


        private void ClearTarget()
        {
            var di = new DirectoryInfo(this.target.WorkingDirectory);

            var dirs = di.EnumerateDirectories("*", SearchOption.AllDirectories).Where(
                d => !this.ShouldBeIgnored(d.FullName, this.target.WorkingDirectory));

            var files = di.EnumerateFiles("*", SearchOption.AllDirectories).Where(
                f => !this.ShouldBeIgnored(f.FullName, this.target.WorkingDirectory));

            foreach (var file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    Logging.Log().Error(ex, $"Could not delete file {file.FullName}");
                    this.syncLogger.Log(this.sync, $"Could not delete file {file.FullName} {ex}");
                }
            }

            foreach (var dir in dirs)
            {
                try
                {
                    dir.Delete(true);
                }
                catch (Exception ex)
                {
                    Logging.Log().Error(ex, $"Could not delete directory {dir.FullName}");
                    this.syncLogger.Log(this.sync, $"Could not delete directory {dir.FullName} {ex}");
                }
            }
        }

        private void CopyFiles()
        {
            var di = new DirectoryInfo(this.source.WorkingDirectory);

            var dirs = di.EnumerateDirectories("*", SearchOption.AllDirectories).Where(
                d => !this.ShouldBeIgnored(d.FullName, this.source.WorkingDirectory));

            var files = di.EnumerateFiles("*", SearchOption.AllDirectories).Where(
                f => !this.ShouldBeIgnored(f.FullName, this.source.WorkingDirectory));

            foreach (DirectoryInfo directory in dirs)
            {
                string directoryDestinationPath =
                    directory.FullName.Replace(this.source.WorkingDirectory, this.target.WorkingDirectory);
                if (!Directory.Exists(directoryDestinationPath))
                {
                    Directory.CreateDirectory(directoryDestinationPath);
                }
            }

            foreach (FileInfo file in files)
            {
                string fileDestinationPath = file.FullName.Replace(this.source.WorkingDirectory, this.target.WorkingDirectory);

                file.CopyTo(fileDestinationPath, overwrite: true);
            }
        }

        bool ShouldBeIgnored(string fullName, string basePath)
        {
            bool result = false;

            foreach (var ignoredFile in this.ignoredFiles)
            {
                string relativePath = this.GetRelativePath(fullName, basePath);
                if (Regex.IsMatch(relativePath, ignoredFile))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private IRevision UpdateRemote(IUser commiter, IRevision revision, string message)
        {
            string login = commiter.RepositoryCredentials.FirstOrDefault(c => c.RepositoryType == this.target.Type)?.Login;

            if (login == null)
            {
                throw new InvalidOperationException($"User {commiter.Name} does not have {this.target.Type} credentials.");
            }

            string password = this.retriever.GetPassword(commiter.Id, this.target.Type);

            this.target.StageAll();
            var rev = this.target.UpdateRemoteWithCommit(message, login, password);

            return rev;
        }
    }
}