namespace BlackMirror.Sync.Worker.Host
{
    using System;
    using System.Linq;
    using BlackMirror.Interfaces;
    using BlackMirror.Models;
    using BlackMirror.Svc.Git;
    using BlackMirror.Svc.Svn;

    public class SvcFactory
    {
        private readonly ISynchronization sync;
        private readonly IUserRetriever retriever;
        private readonly ISyncLogger syncLogger;

        public SvcFactory(ISynchronization sync, IUserRetriever retriever, ISyncLogger syncLogger)
        {
            this.sync = sync;
            this.retriever = retriever;
            this.syncLogger = syncLogger;
        }

        public ISvc Create(ISvcRepository repo)
        {
            switch (repo.Type)
            {
                case SvcRepositoryType.git:
                    {
                        if (string.IsNullOrWhiteSpace(this.sync.Mirror.TargetRepositoryRefSpec))
                        {
                            throw new InvalidOperationException($"Mirror '{this.sync.Mirror.Name}' (Id '{this.sync.Mirror.Id}') does not have valid 'TargetRepositoryRefSpec'. This information is required to clone the repository. Please update this mirror.");
                        }

                        return this.CreateGitSvc(repo, this.sync.Mirror.TargetRepositoryRefSpec);
                    }
                case SvcRepositoryType.svn: return this.CreateSvnSvc(repo);
                default: throw new NotSupportedException($"Repository type {repo.Type} is not supported.");
            }
        }

        private Git CreateGitSvc(ISvcRepository repo, string refspec)
        {
            string repoPath = WorkspacePathProvider.GitRepositoryPath(this.sync);

            var user = this.retriever.GetUser(repo.CheckoutUser.Id);

            string login = user.RepositoryCredentials.FirstOrDefault(c => c.RepositoryType == repo.Type)?.Login;

            if (login == null)
            {
                this.syncLogger.Log(this.sync, $"User {repo.CheckoutUser.Name} does not have {repo.Type} credentials. Trying with anonymous authentication.");
            }

            string password = login == null ? null : this.retriever.GetPassword(repo.CheckoutUser.Id, repo.Type);

            var git = new Git(this.sync, repo, repoPath, login, password, refspec, this.syncLogger);

            return git;
        }

        private Svn CreateSvnSvc(ISvcRepository repo)
        {
            string repoPath = WorkspacePathProvider.SvnRepositoryPath(this.sync);
            var user = this.retriever.GetUser(repo.CheckoutUser.Id);

            string login = user.RepositoryCredentials.FirstOrDefault(c => c.RepositoryType == repo.Type)?.Login;

            if (login == null)
            {
                throw new InvalidOperationException($"User {repo.CheckoutUser.Name} does not have {repo.Type} credentials.");
            }

            string password = this.retriever.GetPassword(repo.CheckoutUser.Id, repo.Type);

            var svn = new Svn(this.sync, repo, repoPath, login, password, this.syncLogger);

            return svn;
        }
    }
}