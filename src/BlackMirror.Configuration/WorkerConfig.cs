namespace BlackMirror.Configuration
{
    using System;
    using BlackMirror.Interfaces.Configuration;
    public class WorkerConfig : IWorkerConfiguration
    {
        private readonly IConfigReader configReader;

        public WorkerConfig(IConfigReader configReader)
        {
            this.configReader = configReader;
        }

        public string ApiUrl => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.ApiUrl);

        public string SecretPassword => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.SecretPassword);

        public string ClientCertificate => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.ClientCertificate);

        public int TimeoutMaxRetryCount => Int32.Parse(this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.TimeoutMaxRetryCount));
    }
}
