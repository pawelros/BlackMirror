namespace BlackMirror.Sync.Worker.Host
{
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using BlackMirror.Configuration;
    using BlackMirror.Interfaces.Configuration;
    using LazyCache;

    public static class Program
    {
        static void Main(string[] argss)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                          X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

            var environment = EnvironmentResolver.GetEnvironmentName();
            IAppCache cache = new CachingService { DefaultCacheDuration = 60 * 5 };
            IConfigReader configReader = ConfigReaderFactory.Create(cache, environment, "WORKER");
            IWorkerConfiguration workerConfig = new WorkerConfig(configReader);
            Host.Run(() => new ServiceApp(workerConfig));
        }
    }
}

