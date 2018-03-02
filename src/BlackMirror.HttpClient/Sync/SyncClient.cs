namespace BlackMirror.HttpClient.Sync
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Newtonsoft.Json;

    public class SyncClient : HttpClientBase, ISyncClient
    {
        private readonly Uri syncUri;
        private readonly HttpClient httpClient;

        public SyncClient(IWorkerConfiguration workerConfig)
            : base(workerConfig)
        {
            this.syncUri = new Uri(new Uri(workerConfig.ApiUrl), "/sync/");
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate certificate = base.ClientCertificateProvider.ClientCertificate;
            handler.ClientCertificates.Add(certificate);
            this.httpClient = new HttpClient(handler);
        }

        public SyncLogDto GetSyncLog(ISynchronization sync)
        {
            return this.GetSyncLogAsync(sync).Result;
        }

        public async Task<SyncLogDto> GetSyncLogAsync(ISynchronization sync)
        {
            var uri = new Uri(this.syncUri, sync.Id + "/logs");
            Logging.Log().Debug($"Sending HTTP GET request to {uri}");
            
            string response = await this.httpClient.GetStringAsync(uri);
            var dto = JsonConvert.DeserializeObject<SyncLogDto>(response);

            return dto;
        }

        public void AddSyncLog(ISynchronization sync, string text)
        {
            this.AddSyncLogAsync(sync, text);
        }

        public async Task AddSyncLogAsync(ISynchronization sync, string text)
        {
            var uri = new Uri(this.syncUri, sync.Id + "/logs");
            Logging.Log().Debug($"Sending HTTP PUT request to {uri}");

            var dto = new SyncLogDto { Text = text, SyncId = sync.Id };
            var json = JsonConvert.SerializeObject(dto);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                Logging.Log().Error($"HTTP PUT request to {uri} failed {response.StatusCode}");
            }
        }
    }

}