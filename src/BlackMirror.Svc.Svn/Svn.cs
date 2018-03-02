using System;
using System.Collections.Generic;
using SharpSvn;

namespace BlackMirror.Svc.Svn
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Interfaces;
    using BlackMirror.Models;

    public class Svn : ISvn
    {
        private readonly ISynchronization sync;
        private readonly ISvcRepository repository;
        private readonly string repoPath;
        private readonly string username;
        private readonly string ownerPassword;
        private readonly ISyncLogger syncLogger;

        public Svn(ISynchronization sync, ISvcRepository repository, string repoPath, string ownerName, string ownerPassword, ISyncLogger syncLogger)
        {
            this.sync = sync;
            this.repository = repository;
            this.repoPath = repoPath;
            this.username = ownerName;
            this.ownerPassword = ownerPassword;
            this.syncLogger = syncLogger;
        }

        public SvcRepositoryType Type => SvcRepositoryType.svn;

        public string WorkingDirectory => this.repoPath;

        public string DefaultCommitMessagePrefix => this.repository.DefaultCommitMessagePrefix;

        public IEnumerable<IRevision> GetLog()
        {
            List<IRevision> result = new List<IRevision>();

            //SvnCheckoutArgs wraps all of the options for the 'svn checkout' function
            SvnLogArgs args = new SvnLogArgs();

            //the using statement is necessary to ensure we are freeing up resources
            using (SvnClient client = new SvnClient())
            {
                try
                {
                    client.Authentication.ForceCredentials(this.username, this.ownerPassword);
                    Collection<SvnLogEventArgs> changes = new Collection<SvnLogEventArgs>();
                    //this is the where 'svn checkout' actually happens.
                    if (client.GetLog(this.repoPath, args, out changes))
                    {
                        Logging.Log().Information("Successfully GetChangeList");
                        this.syncLogger.Log(this.sync, "Successfully GetChangeList");

                        // The oldest change does not contain useful information
                        for (int i = 1; i < changes.Count; i++)
                        {
                            var log = changes[i];

                            SvnRevision dto =
                                new SvnRevision
                                {
                                    Author = log.Author,
                                    Id = log.Revision.ToString(),
                                    Message = log.LogMessage,
                                    LogObject = log
                                };

                            result.Add(dto);
                        }
                    }

                }
                catch (SvnException se)
                {
                    Logging.Log().Error(se, "SVN checkout failed.");
                    this.syncLogger.Log(this.sync, $"SVN checkout failed. {se}");
                    throw;
                }
                catch (UriFormatException ufe)
                {
                    Logging.Log().Error(ufe, "SVN uri format.");
                    this.syncLogger.Log(this.sync, $"SVN uri format. {ufe}");
                    throw;
                }
            }

            return result;
        }

        public void CloneOrUpdate()
        {
            this.Checkout();
        }

        public void Checkout(IRevision revision)
        {
            long rev = long.Parse(revision.Id);
            this.Checkout(rev);
        }

        public void StageAll()
        {
            using (SvnClient client = new SvnClient())
            {
                Collection<SvnStatusEventArgs> changedFiles = new Collection<SvnStatusEventArgs>();

                client.GetStatus(this.repoPath, out changedFiles);

                SvnAddArgs aa = new SvnAddArgs();
                aa.Depth = SvnDepth.Infinity;
                aa.Force = true;
                aa.NoIgnore = true;

                aa.Progress += this.OnProgress;
                aa.SvnError += this.OnSvnError;
                aa.Notify += this.OnNotify;

                SvnDeleteArgs dd = new SvnDeleteArgs();
                dd.Force = true;
                dd.Progress += this.OnProgress;
                dd.SvnError += this.OnSvnError;
                dd.Notify += this.OnNotify;

                foreach (SvnStatusEventArgs changedFile in changedFiles)
                {
                    if (changedFile.LocalContentStatus == SvnStatus.Missing)
                    {
                        client.Delete(changedFile.Path, dd);
                    }
                    if (changedFile.LocalContentStatus == SvnStatus.NotVersioned)
                    {
                        client.Add(changedFile.Path, aa);
                    }
                }

                client.Add(this.repoPath, aa);
            }
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
            var revision = new Revision();
            revision.Author = login;
            revision.Message = message;

            try
            {
                this.Commit(message, login, password, revision);
            }
            catch (SvnWorkingCopyException svnEx)
            {
                Logging.Log().Error(svnEx, $"First chance exception: {svnEx}");
                this.syncLogger.Log(this.sync, svnEx.ToString());
                //if (svnEx.Message.Contains("is out of date"))
                //{
                SvnClient client = new SvnClient();
                client.Update(this.repoPath);
                this.Commit(message, login, password, revision);
                //}
            }

            return revision;
        }

        private void Commit(string message, string login, string password, Revision revision)
        {
            using (SvnClient client = new SvnClient())
            {
                SvnCommitArgs aa = new SvnCommitArgs();
                aa.Depth = SvnDepth.Infinity;

                aa.Progress += this.OnProgress;
                aa.SvnError += this.OnSvnError;
                aa.Notify += this.OnNotify;
                aa.LogMessage = message;

                aa.Committed += (o, args) => { revision.Id = args.Revision.ToString(); };

                client.Authentication.ForceCredentials(login, password);

                client.Commit(this.repoPath, aa);

                // hack

                for (int i = 0; i <= 100; i++)
                {
                    if (revision.Id == null)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        private void Checkout(long? rev = null)
        {
            //SvnCheckoutArgs wraps all of the options for the 'svn checkout' function
            SvnCheckOutArgs args = new SvnCheckOutArgs();
            args.Progress += this.OnProgress;
            args.SvnError += this.OnSvnError;
            args.Notify += this.OnNotify;

            //the using statement is necessary to ensure we are freeing up resources
            using (SvnClient client = new SvnClient())
            {
                try
                {
                    //SvnUriTarget is a wrapper class for SVN repository URIs
                    SvnUriTarget target = rev == null ? new SvnUriTarget(this.repository.Uri) : new SvnUriTarget(this.repository.Uri, rev);

                    client.Authentication.ForceCredentials(this.username, this.ownerPassword);
                    //this is the where 'svn checkout' actually happens.

                    //SvnUpdateResult provides info about what happened during a checkout
                    SvnUpdateResult result;
                    if (client.CheckOut(target, this.repoPath, args, out result))
                    {
                        Logging.Log().Information($"Successfully checked out revision {result.Revision}.");
                        this.syncLogger.Log(this.sync, $"Successfully checked out revision {result.Revision}.");
                    }
                }
                catch (SvnException se)
                {
                    Logging.Log().Error(se, "SVN checkout failed.");
                    this.syncLogger.Log(this.sync, $"SVN checkout failed. {se}");

                    throw;
                }
                catch (UriFormatException ufe)
                {
                    Logging.Log().Error(ufe, "SVN uri format.");
                    this.syncLogger.Log(this.sync, $"SVN uri format. {ufe}");

                    throw;
                }
            }
        }

        private void OnNotify(object sender, SvnNotifyEventArgs svnNotifyEventArgs)
        {
            Logging.Log().Debug($"Notify: Action: {svnNotifyEventArgs.Action}, Command Type: {svnNotifyEventArgs.CommandType}, Content State: {svnNotifyEventArgs.ContentState}, Full Path: {svnNotifyEventArgs.FullPath}");
            //this.syncLogger.Log(this.sync, $"Notify: Action: {svnNotifyEventArgs.Action}, Command Type: {svnNotifyEventArgs.CommandType}, Content State: {svnNotifyEventArgs.ContentState}, Full Path: {svnNotifyEventArgs.FullPath}");

        }

        private void OnSvnError(object sender, SvnErrorEventArgs svnErrorEventArgs)
        {
            Logging.Log().Error($"Error: {svnErrorEventArgs.Exception}");
            this.syncLogger.Log(this.sync, $"Error: {svnErrorEventArgs.Exception}");
        }

        private void OnProgress(object sender, SvnProgressEventArgs svnProgressEventArgs)
        {
            Logging.Log().Debug($"Progress: {svnProgressEventArgs.Progress} / {svnProgressEventArgs.TotalProgress}");
            // this.syncLogger.Log(this.sync, $"Progress: {svnProgressEventArgs.Progress} / {svnProgressEventArgs.TotalProgress}");

        }
    }
}
