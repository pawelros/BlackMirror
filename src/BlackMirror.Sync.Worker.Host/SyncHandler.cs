namespace BlackMirror.Sync.Worker.Host
{
    using System;
    using System.Threading;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.HttpClient;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Sync.Worker.Host.Strategies;

    public class SyncHandler
    {
        private readonly IWorkerConfiguration workerConfig;
        private readonly IBlackMirrorHttpClient httpClient;
        private readonly ISyncLogger syncLogger;

        public SyncHandler(IWorkerConfiguration workerConfig, IBlackMirrorHttpClient httpClient, ISyncLogger syncLogger)
        {
            this.workerConfig = workerConfig;
            this.httpClient = httpClient;
            this.syncLogger = syncLogger;
        }

        public  void Handle(ISynchronization sync)
        {
            try
            {
                var passwordRetriever = new UserRetriever(this.workerConfig);

                var message = $"Starting synchronization {sync.Id}";
                Logging.Log().Warning(message);
                this.syncLogger.Log(sync, message);

                SvcFactory factory = new SvcFactory(sync, passwordRetriever, syncLogger);
                ISvc source = factory.Create(sync.Mirror.SourceRepository);
                ISvc target = factory.Create(sync.Mirror.TargetRepository);
                var diffProcessor = new DiffProcessor(
                    this.workerConfig,
                    sync,
                    source,
                    target,
                    this.httpClient,
                    this.syncLogger,
                    sync.Mirror.TargetRepository.PushUser);

                source.CloneOrUpdate();
                target.CloneOrUpdate();
                var sourceLog = source.GetLog();
                var targetLog = target.GetLog();

                var comparer = new RepositoryComparer(this.httpClient, sync.Mirror, sourceLog, targetLog);
                var revisions = comparer.GetRevisionsAwaitingForSync();

                for (int i = revisions.Count - 1; i >= 0; i--)
                {
                    var revision = revisions[i];
                    var m = $"Applying revision {revision.Id} {revision.Author} {revision.Message}";
                    Logging.Log().Warning(m);
                    this.syncLogger.Log(sync, m);

                    diffProcessor.ApplyRevision(revision);
                }

                var o = $"Synchronization {sync.Id} OK";
                Logging.Log().Information(o);
                this.syncLogger.Log(sync, o);

                var cl = $"Cleaning up...";
                Logging.Log().Information(cl);
                this.syncLogger.Log(sync, cl);

                source.CleanUp();
                target.CleanUp();
            }
            catch (Exception ex)
            {
                this.syncLogger.Log(sync, $"Error: {ex}");
                throw;
            }
        }
    }
}