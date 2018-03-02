namespace BlackMirror.Configuration.SerilogSupport
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using BlackMirror.Configuration.SerilogSupport.SizeRollingFile;
    using Serilog;
    using Serilog.Formatting.Json;

    public static class SerilogSupport
    {
        private const string OutputTemplate = "{Timestamp:HH:mm:ss} [{Level}] [{CallerFilePath:l}] {Message}{NewLine}{Exception}";

        private static Uri configFilePath;
        private static SerilogConfig serilogConfig;
        private static LogCleaner logCleaner;
        private static readonly object syncRoot = new object();

        private sealed class LogCleaner : IDisposable
        {
            private readonly ILogger logger;

            public LogCleaner(ILogger logger)
            {
                this.logger = logger;
            }

            public void Dispose()
            {
                var disposable = this.logger as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        // result of this method is IDisposable, that closes / flushes the logger
        public static IDisposable InitLogger(Uri path)
        {
            return InitLogger(
                () => {
                    configFilePath = path;

                    ReadConfig();

                    return new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("CallerFilePath", string.Empty)
                        .Enrich.WithProperty("CallerMemberName", string.Empty)
                        .Enrich.WithProperty("CallerLineNumber", string.Empty)
                        .Enrich.WithProperty("ApplicationName", AppDomain.CurrentDomain.FriendlyName)
                        .WriteTo.Sink(new SizeRollingFileSink(serilogConfig.SerilogLogFilePath, new JsonFormatter(renderMessage: true), serilogConfig.FileSizeLimitBytes, serilogConfig.RetainedFileCountLimit))
                        .WriteTo.Async(config => config.RollingFile(pathFormat: serilogConfig.LogFilePath,
                            outputTemplate: OutputTemplate,
                            retainedFileCountLimit: serilogConfig.RetainedFileCountLimit,
                            flushToDiskInterval: TimeSpan.FromSeconds(2)))
                        .WriteTo.Async(config => config.ColoredConsole(outputTemplate: OutputTemplate));
                });
        }

        private static IDisposable InitLogger(Func<LoggerConfiguration> createConfiguration)//, bool useSyncLogger)
        {
            lock (syncRoot)
            {
                if (logCleaner != null)
                {
                    logCleaner.Dispose();
                    logCleaner = null;
                }

                try
                {
                    var configuration = createConfiguration();
                    Log.Logger = //useSyncLogger
                        //? 
                        configuration.CreateLogger()
                        //: configuration.CreateLoggerAsync();
                        ;
                    logCleaner = new LogCleaner(Log.Logger);
                    return logCleaner;
                }
                catch (Exception)
                {
                    Log.Logger = new LoggerConfiguration().CreateLogger();
                    throw;
                }
            }
        }

        private static void ReadConfig()
        {
            using (var reader = new StreamReader(configFilePath.AbsolutePath))
            {
                var config = XDocument.Parse(reader.ReadToEnd()).Descendants("serilog").First();
                serilogConfig = Deserialize<SerilogConfig>(config);
            }
        }

        private static T Deserialize<T>(XNode data) where T : class, new()
        {
            if (data == null)
            {
                return null;
            }

            var ser = new XmlSerializer(typeof(T));
            return (T)ser.Deserialize(data.CreateReader());
        }
    }
}
