namespace BlackMirror.Configuration
{
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;

    public class WebApiConfig : IWebApiConfiguration
    {
        private readonly IConfigReader configReader;

        public WebApiConfig(IConfigReader configReader)
        {
            this.configReader = configReader;
        }

        public string[] UrlNamespace => this.configReader.Fetch<string[]>(Consts.Configuration.ConfigKeys.UrlNamespace);

        public string AuthenticationProvider => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.AuthenticationProvider);

        public string MongoConnectionString => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.MongoConnectionString);

        public string MongoDatabase => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.MongoDatabase);

        public string MongoMirrorCollection => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.MongoMirrorCollection);

        public string MongoSvcCollection => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.MongoSvcCollection);

        public string MongoUserCollection => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.MongoUserCollection);

        public string MongoSyncCollection => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.MongoSyncCollection);

        public string MongoReflectionCollection => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.MongoReflectionCollection);

        public string SecretPassword => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.SecretPassword);
    }
}