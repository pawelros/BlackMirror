namespace BlackMirror.Interfaces
{
    using System.Threading.Tasks;

    public interface ISyncLogger
    {
        void Log(ISynchronization sync, string text);
        Task LogAsync(ISynchronization sync, string text);
    }
}