namespace BlackMirror.Interfaces.Configuration
{
    public interface IWorkerConfiguration
    {
        string ApiUrl { get; }
        string SecretPassword { get; }
        string ClientCertificate { get; }

        int TimeoutMaxRetryCount { get; }
    }
}