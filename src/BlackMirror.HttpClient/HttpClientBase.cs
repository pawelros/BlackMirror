namespace BlackMirror.HttpClient
{
    using System.Security.Cryptography.X509Certificates;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;

    public abstract class HttpClientBase
    {
        protected readonly IWorkerConfiguration WorkerConfig;
        protected readonly IClientCertificateProvider ClientCertificateProvider;

        protected HttpClientBase(IWorkerConfiguration workerConfig)
        {
            this.WorkerConfig = workerConfig;
            this.ClientCertificateProvider = new X509StoreCertificateProvider(X509FindType.FindBySubjectName, workerConfig.ClientCertificate, false, StoreName.My, StoreLocation.LocalMachine);
        }
        
    }
}