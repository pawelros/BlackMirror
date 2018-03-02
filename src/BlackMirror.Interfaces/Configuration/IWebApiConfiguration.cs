namespace BlackMirror.Interfaces.Configuration
{
    public interface IWebApiConfiguration
    {
        string[] UrlNamespace { get; }
        string AuthenticationProvider { get; }
        string MongoConnectionString { get; }
        string MongoDatabase { get; }
        string MongoMirrorCollection { get; }
        string MongoSvcCollection { get; }
        string MongoUserCollection { get; }
        string MongoSyncCollection { get; }
        string MongoReflectionCollection { get; }
        string SecretPassword { get; }
    }
}
