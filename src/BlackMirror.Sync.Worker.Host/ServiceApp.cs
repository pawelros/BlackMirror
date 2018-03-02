namespace BlackMirror.Sync.Worker.Host
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.HttpClient;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using Nancy.TinyIoc;

    public sealed class ServiceApp : IDisposable, IApplication
    {
        private IDisposable app;
        private IWorkerConfiguration workerConfig;
        private static TinyIoCContainer container = TinyIoCContainer.Current;


        public ServiceApp(IWorkerConfiguration workerConfig)
        {
            this.workerConfig = workerConfig;
        }

        public void Start()
        {
            this.RegisterDependencies();

            new TaskFactory().StartNew(() =>
            {
                var runPath = AppDomain.CurrentDomain.BaseDirectory;
                using (SerilogSupport.InitLogger(new Uri(string.Concat(runPath, "serilog.config"))))
                {
                    AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                        {
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

                    RegisterDependencies();
                    Logging.Log().Information("Hello from Sync Worker!");


                    try
                    {
                        var service = new WorkerService(container.Resolve<ISyncQueue>(), container.Resolve<SyncHandler>());
                        service.Start();
                    }
                    catch (Exception ex)
                    {
                        Logging.Log().Error(ex, "Critical exception occured. Application will close.");
                    }

                    Logging.Log().Information("Application terminated.");
                }
            });
        }

        void RegisterDependencies()
        {
            container.Register<IWorkerConfiguration>(this.workerConfig);
            container.Register<ISyncQueue>(new ApiHttpClientSyncQueue(container.Resolve<IWorkerConfiguration>()));
            container.Register<ISyncLogger>(new ApiHttpSyncLogger(container.Resolve<IWorkerConfiguration>()));
            container.Register<IBlackMirrorHttpClient>(new BlackMirrorHttpClient(container.Resolve<IWorkerConfiguration>()));
            container.Register(new SyncHandler(container.Resolve<IWorkerConfiguration>(), container.Resolve<IBlackMirrorHttpClient>(), container.Resolve<ISyncLogger>()));
            container.Register<IUserRetriever>(
                new UserRetriever(container.Resolve<IWorkerConfiguration>()));
        }

        public void Stop()
        {
            this.app.Dispose();
            this.app = null;

            Logging.Log().Information("Service stopped.");
        }

        public void Dispose()
        {
            if (this.app != null)
            {
                this.app.Dispose();
                this.app = null;
            }
        }
    }
}