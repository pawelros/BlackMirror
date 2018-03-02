namespace BlackMirror.Interfaces
{
    using System.Security.Cryptography.X509Certificates;

    public interface IClientCertificateProvider
    {
        X509Certificate ClientCertificate { get; }
    }
}