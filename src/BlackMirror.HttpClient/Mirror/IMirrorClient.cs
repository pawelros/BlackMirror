namespace BlackMirror.HttpClient.Mirror
{
    using System.Threading.Tasks;
    using BlackMirror.Interfaces;

    public interface IMirrorClient
    {
        IReflection GetLatestReflection(IMirror mirror);

        Task<IReflection> GetLatestReflectionAsync(IMirror mirror);
    }
}