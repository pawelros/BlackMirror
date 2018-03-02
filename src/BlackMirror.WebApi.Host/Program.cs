namespace BlackMirror.WebApi.Host
{
    using System;
    using System.Diagnostics;
    using BlackMirror.Configuration;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Interfaces.Configuration;
    using LazyCache;

    class Program
    {
        static void Main(string[] arguments)
        {
            var runPath = AppDomain.CurrentDomain.BaseDirectory;

            using (SerilogSupport.InitLogger(new Uri(string.Concat(runPath, "serilog.config"))))
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) => {
                        var ex = args.ExceptionObject as Exception;
                        Logging.Log().Error(ex, "Unhandled exception: {ExceptionObject} (isTerminating: {IsTerminating})", args.ExceptionObject, args.IsTerminating);
                    };
                Logging.Log().Information("Version: {ApplicationVersion}", typeof(Program).Assembly.GetName().Version);
                var assemblyLocation = typeof(Program).Assembly.Location;

                if (assemblyLocation != null)
                {
                    var productVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;
                    Logging.Log().Information("Product Version: {ApplicationVersion}", typeof(Program).Assembly.GetName().Version);
                }

                var environment = EnvironmentResolver.GetEnvironmentName();
                IAppCache cache = new CachingService { DefaultCacheDuration = 60 * 5 };
                IConfigReader configReader = ConfigReaderFactory.Create(cache, environment, "API");
                IWebApiConfiguration webApiConfig = new WebApiConfig(configReader);

                Host.Run(() => new ServiceApp(webApiConfig));

                Logging.Log().Information("Application terminated.");
            }
        }
    }
}
