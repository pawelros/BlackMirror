namespace BlackMirror.HttpClient
{
    using BlackMirror.HttpClient.Mirror;
    using BlackMirror.HttpClient.Reflection;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;

    public class BlackMirrorHttpClient : IBlackMirrorHttpClient
    {
        public BlackMirrorHttpClient(IWorkerConfiguration workerConfig)
        {
            this.Reflection = new ReflectionClient(workerConfig);
            this.Mirror = new MirrorClient(workerConfig);
        }

        public IReflectionClient Reflection { get; }

        public IMirrorClient Mirror { get; }
    }
}