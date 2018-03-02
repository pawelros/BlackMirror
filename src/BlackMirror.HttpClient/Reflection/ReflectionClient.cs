namespace BlackMirror.HttpClient.Reflection
{
    using System;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Newtonsoft.Json;

    public class ReflectionClient : HttpClientBase, IReflectionClient
    {
        private readonly Uri reflectionUri;
        private readonly HttpClient httpClient;

        public ReflectionClient(IWorkerConfiguration workerConfig)
            : base(workerConfig)
        {
            this.reflectionUri = new Uri(new Uri(this.WorkerConfig.ApiUrl), "/reflection/");
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate certificate = base.ClientCertificateProvider.ClientCertificate;
            handler.ClientCertificates.Add(certificate);
            this.httpClient = new HttpClient(handler);
        }

        public IReflection Add(ReflectionDto dto)
        {
            var model = this.AddAsync(dto).Result;
            return model;
        }

        private async Task<IReflection> AddAsync(ReflectionDto dto)
        {
            string body = JsonConvert.SerializeObject(dto);
            Logging.Log().Debug($"Sending HTTP POST request to {this.reflectionUri} with body {body}");
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await this.httpClient.PostAsync(this.reflectionUri, content);
            string responseString = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ReflectionDto>(responseString);
            var model = responseDto.ToReflection();

            return model;
        }
    }
}