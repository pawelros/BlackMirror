namespace BlackMirror.Sync.Worker.Host
{
    using System.Threading.Tasks;
    using BlackMirror.HttpClient.Sync;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;

    public class ApiHttpSyncLogger : ISyncLogger
    {
        public void Log(ISynchronization sync, string text)
        {
            this.httpClient.AddSyncLog(sync, text);
        }

        public async Task LogAsync(ISynchronization sync, string text)
        {
            await this.httpClient.AddSyncLogAsync(sync, text);
        }

        private readonly SyncClient httpClient;

        public ApiHttpSyncLogger(IWorkerConfiguration workerConfig)
        {
            this.httpClient = new SyncClient(workerConfig);
        }
    }
}