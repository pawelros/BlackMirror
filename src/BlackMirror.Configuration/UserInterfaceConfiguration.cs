namespace BlackMirror.Configuration
{
    using BlackMirror.Interfaces.Configuration;
    public class UserInterfaceConfiguration : IUserInterfaceConfiguration
    {
        private readonly IConfigReader configReader;

        public UserInterfaceConfiguration(IConfigReader configReader)
        {
            this.configReader = configReader;
        }

        public string[] UrlNamespace => this.configReader.Fetch<string[]>(Consts.Configuration.ConfigKeys.UrlNamespace);

        public string ApiUrl => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.ApiUrl);

        public string AuthenticationProvider => this.configReader.Fetch<string>(Consts.Configuration.ConfigKeys.AuthenticationProvider);
    }
}