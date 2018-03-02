namespace BlackMirror.HttpClient
{
    using BlackMirror.HttpClient.Mirror;
    using BlackMirror.HttpClient.Reflection;

    public interface IBlackMirrorHttpClient
    {
        IReflectionClient Reflection { get; }

        IMirrorClient Mirror { get; }
    }
}
