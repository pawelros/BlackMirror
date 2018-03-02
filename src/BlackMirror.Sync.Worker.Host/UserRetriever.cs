namespace BlackMirror.Sync.Worker.Host
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using Newtonsoft.Json;

    internal class UserRetriever : IUserRetriever
    {
        private readonly IWorkerConfiguration workerConfig;
        private readonly HttpClient httpClient;

        public UserRetriever(IWorkerConfiguration workerConfig)
        {
            this.workerConfig = workerConfig;
            WebRequestHandler handler = new WebRequestHandler();
            var certProvider = new X509StoreCertificateProvider(
                X509FindType.FindBySubjectName,
                workerConfig.ClientCertificate,
                false);
            var cert = certProvider.ClientCertificate;
            handler.ClientCertificates.Add(cert);
            this.httpClient = new HttpClient(handler);
        }

        public async Task<string> GetPasswordAsync(string userId, SvcRepositoryType repositoryType)
        {
            var uri = new Uri(new Uri(this.workerConfig.ApiUrl), "/user/" + userId + $"/credentials/{repositoryType}/password?passphrase=" + this.workerConfig.SecretPassword);
            var response = await this.httpClient.GetStringAsync(uri);

            var dto = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            if (dto.Count == 0)
            {
                throw new KeyNotFoundException($"Could not retrieve {repositoryType} repository credentials for user with id {userId}");
            }

            string password = dto["Password"];
            return password;
        }

        public string GetPassword(string userId, SvcRepositoryType repositoryType)
        {
            return this.GetPasswordAsync(userId, repositoryType).Result;
        }

        public UserDto GetUser(string userId)
        {
            return this.GetUserAsync(userId).Result;
        }

        public async Task<UserDto> GetUserAsync(string userId)
        {
            var uri = new Uri(new Uri(this.workerConfig.ApiUrl), "/user/" + userId);
            var response = await this.httpClient.GetStringAsync(uri);

            UserDto dto = JsonConvert.DeserializeObject<UserDto>(response);

            return dto;
        }

        public List<UserDto> GetUsers()
        {
            return this.GetUsersAsync().Result;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var uri = new Uri(new Uri(this.workerConfig.ApiUrl), "/user/");
            var response = await this.httpClient.GetStringAsync(uri);

            List<UserDto> dto = JsonConvert.DeserializeObject<List<UserDto>>(response);

            return dto;
        }
    }
}
