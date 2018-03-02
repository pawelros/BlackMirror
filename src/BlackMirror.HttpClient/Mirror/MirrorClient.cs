namespace BlackMirror.HttpClient.Mirror
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Newtonsoft.Json;

    public class MirrorClient : HttpClientBase, IMirrorClient
    {
        private readonly Uri mirrorUri;
        private readonly HttpClient httpClient;

        public MirrorClient(IWorkerConfiguration workerConfig)
            : base(workerConfig)
        {
            this.mirrorUri = new Uri(new Uri(workerConfig.ApiUrl), "/mirror/");
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate certificate = base.ClientCertificateProvider.ClientCertificate;
            handler.ClientCertificates.Add(certificate);
            this.httpClient = new HttpClient(handler);
        }

        public IReflection GetLatestReflection(IMirror mirror)
        {
            return this.GetLatestReflectionAsync(mirror).Result;
        }

        public async Task<IReflection> GetLatestReflectionAsync(IMirror mirror)
        {
            var uri = new Uri(this.mirrorUri, mirror.Id + "/reflections?take=1");
            Logging.Log().Debug($"Sending HTTP GET request to {uri}");

            string response = await this.httpClient.GetStringAsync(uri);
            var dto = JsonConvert.DeserializeObject<List<ReflectionDto>>(response);
            if (dto.Count == 0)
            {
                return null;
            }

            var model = dto.First().ToReflection();

            return model;
        }
    }
}