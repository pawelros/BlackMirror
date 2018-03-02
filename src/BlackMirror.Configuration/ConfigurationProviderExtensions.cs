namespace BlackMirror.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using BlackMirror.Configuration.SerilogSupport;
    using LiteGuard;
    using Spg.Configuration;

    public static class ConfigurationProviderExtensions
    {
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = "We want to distinguish configuration URI from its string representation."),
        SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "We want to distinguish configuration URI from its string representation.")]
        public static Spg.Configuration.IConfigurationProvider Bootstrap(this ConfigurationProvider configProvider, string environment)
        {
            Guard.AgainstNullArgument("configProvider", configProvider);
            Guard.AgainstNullArgument("configProvider", environment);
            Logging.Log().Information("Resolving configuration url: {ConfigurationUrl}", ConfigurationManager.AppSettings["Configuration.Url"]);

            var loggingConfigProvider = new LoggingConfigurationProvider(configProvider, (key, value) => Logging.Log().Information("{ConfigurationKey}: {ConfigurationValue}", key, value));
            var configUri = new ConfigPathResolver().Resolve(environment, null);

            Logging.Log().Information("Loading config from: {configUri}", configUri.AbsoluteUri);

            configProvider.Load(configUri);

            return loggingConfigProvider;
        }
    }
}
