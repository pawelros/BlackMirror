using System;

namespace BlackMirror.Sync.Worker.Host
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Dto;
    using BlackMirror.HttpClient;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models.Exceptions;
    using Newtonsoft.Json;

    public class ApiHttpClientSyncQueue : ISyncQueue
    {
        private readonly Uri syncUri;
        private readonly HttpClient httpClient;

        public ApiHttpClientSyncQueue(IWorkerConfiguration workerConfig)
        {
            this.syncUri = new Uri(new Uri(workerConfig.ApiUrl), "/sync/");
            var clientCertificateProvider = new X509StoreCertificateProvider(X509FindType.FindBySubjectName, workerConfig.ClientCertificate, false, StoreName.My, StoreLocation.LocalMachine);
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate certificate = clientCertificateProvider.ClientCertificate;
            handler.ClientCertificates.Add(certificate);
            this.httpClient = new HttpClient(handler);
        }

        public ISynchronization Pull()
        {
            ISynchronization result = this.PullAsync().Result;

            return result;
        }

        public async Task<ISynchronization> PullAsync()
        {
            var uri = new Uri(this.syncUri, "?Status=Scheduled");
            var response = await this.httpClient.GetStringAsync(uri);

            var dto = JsonConvert.DeserializeObject<List<SynchronizationDto>>(response);

            if (dto.Any())
            {
                return dto.First().ToSynchronization();
            }

            return null;
        }

        public long Count { get; }

        public async void SetSynchronizationStatus(ISynchronization sync, SynchronizationStatus status, string additionalInfo = null)
        {
            var uri = new Uri(this.syncUri, sync.Id + "/status");
            string body = $@"{{""Status"":""{status}""}}";

            Logging.Log().Debug($"Sending HTTP PUT request to {uri} with body {body}");

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PutAsync(uri, content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new StatusNotChangedException(response.ReasonPhrase);
            }
        }
    }
}
