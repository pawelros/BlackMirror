namespace BlackMirror.Svc.Git
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Interfaces;
    using BlackMirror.Models;
    using LibGit2Sharp;

    public class Git : IGit
    {
        private readonly ISynchronization sync;
        private readonly ISvcRepository repository;
        private readonly string repoPath;
        private readonly string userName;
        private readonly string refspec;
        private readonly ISyncLogger syncLogger;
        private Repository repo;
        private readonly Credentials credentials;

        public Git(ISynchronization sync, ISvcRepository repository, string repoPath, string userName, string password, string refspec, ISyncLogger syncLogger)
        {
            this.credentials = new UsernamePasswordCredentials { Username = userName, Password = password };
            this.sync = sync;
            this.repository = repository;
            this.repoPath = repoPath;
            this.userName = userName;
            this.refspec = refspec;
            this.syncLogger = syncLogger;
            if (Repository.IsValid(repoPath))
            {
                this.repo = new Repository(repoPath);
            }
        }

        public SvcRepositoryType Type => SvcRepositoryType.git;

        public string DefaultCommitMessagePrefix => this.repository.DefaultCommitMessagePrefix;

        public string WorkingDirectory => this.repoPath;

        public IEnumerable<IRevision> GetLog()
        {
            /*A merge commit is a commit with more than one parent.

            In order to do this with Git, one would issue, for instance, the following command which would list all the commits reachable by HEAD which are not merge commits.

            git log --no-merges HEAD
            Where --no-merges is documented as "Do not print commits with more than one parent. This is exactly the same as --max-parents=1.".*/

            var commits = this.repo.Commits;

            var result = commits./*Where(c=>c.Parents.Count() <= 1).*/Select(
                commit => new GitRevision { Author = commit.Author.Email, Id = commit.Sha, Message = commit.Message, CommitObject = commit })
                .ToList<IRevision>();

            return result;
        }

        public void CloneOrUpdate()
        {
            if (!this.RepoExists)
            {
                if (string.IsNullOrWhiteSpace(this.refspec))
                {
                    throw new InvalidOperationException($"[git] Clone operation requires valid refspec.");
                }

                Repository.Clone(
                    this.repository.Uri,
                    this.repoPath,
                    new CloneOptions
                    {
                        BranchName = this.refspec,
                        CredentialsProvider = (_url, _user, _cred) => this.credentials,
                        OnCheckoutProgress = this.OnCheckoutProgress,
                        OnProgress = this.OnProgress
                    });
                this.repo = new Repository(this.repoPath);
            }
            else
            {
                // what I wanted to achieve here is equivalent of git add --all && git reset --hard && git pull origin HEAD
                var fetchOptions = new FetchOptions
                {
                    CredentialsProvider = (_url, _user, _cred) => this.credentials,
                    OnProgress = this.OnProgress,
                    OnTransferProgress = this.OnTransferProgress,
                    OnUpdateTips = this.OnUpdateTips
                };

                var mergeOptions = new MergeOptions
                {
                    OnCheckoutProgress = this.OnCheckoutProgress,
                    OnCheckoutNotify = this.OnCheckoutNotify,
                    FileConflictStrategy = CheckoutFileConflictStrategy.Theirs,
                    FailOnConflict = true
                };

                Commands.Stage(this.repo, "*");
                this.repo.Reset(ResetMode.Hard, this.repo.Head.Tip);

                Commands.Pull(
                    this.repo,
                    new Signature(this.userName, this.userName + "@your-email.com", DateTimeOffset.Now),
                    new PullOptions
                    {
                        FetchOptions = fetchOptions,
                        MergeOptions = mergeOptions
                    });
            }
        }

        private bool OnUpdateTips(string referenceName, ObjectId oldId, ObjectId newId)
        {
            Logging.Log().Debug($"OnUpdateTips: {oldId.Sha} {newId.Sha}");
            this.syncLogger.Log(this.sync, $"OnUpdateTips: {oldId.Sha} {newId.Sha}");
            return true;
        }

        private bool OnCheckoutNotify(string path, CheckoutNotifyFlags notifyFlags)
        {
            Logging.Log().Debug($"Checkout notify: {notifyFlags} {path}");
            this.syncLogger.Log(this.sync, $"Checkout notify: {notifyFlags} {path}");
            return true;
        }

        private bool OnTransferProgress(TransferProgress progress)
        {
            Logging.Log().Debug($"Transfer progress: Indexed Objects: {progress.IndexedObjects} Received Bytes: {progress.ReceivedBytes}, Received Objects: {progress.ReceivedObjects}, Total Objects: {progress.TotalObjects}");
            this.syncLogger.Log(this.sync, $"Transfer progress: Indexed Objects: {progress.IndexedObjects} Received Bytes: {progress.ReceivedBytes}, Received Objects: {progress.ReceivedObjects}, Total Objects: {progress.TotalObjects}");
            return true;
        }

        private bool RepoExists => this.repo != null;

        public void Checkout(IRevision revision)
        {
            IGitRevision gitRev = (IGitRevision)revision;
            var commit = gitRev.CommitObject;

            if (commit == null)
            {
                throw new InvalidOperationException($"Commit {revision.Id} not found.");
            }

            this.repo.Reset(ResetMode.Hard, commit);
        }

        public void StageAll()
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(this.WorkingDirectory);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Logging.Log().Error($"Error during repository cleanup: {ex.ToString()}");
                this.syncLogger.Log(this.sync, $"Error during repository cleanup: {ex.ToString()}");
            }
        }

        public IRevision UpdateRemoteWithCommit(string message, string login, string password)
        {
            throw new NotImplementedException();
        }

        private bool OnProgress(string serverProgressOutput)
        {
            Logging.Log().Debug($"Server progress: {serverProgressOutput}");
            this.syncLogger.Log(this.sync, $"Server progress: {serverProgressOutput}");
            return true;
        }

        private void OnCheckoutProgress(string path, int completedSteps, int totalSteps)
        {
            Logging.Log().Debug($"Checkout progress: path: {path} completed steps: {completedSteps} / {totalSteps}");
            //this.syncLogger.Log(this.sync, $"Checkout progress: path: {path} completed steps: {completedSteps} / {totalSteps}");
        }
    }
}
