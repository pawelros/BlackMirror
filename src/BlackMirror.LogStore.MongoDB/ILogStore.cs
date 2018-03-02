namespace BlackMirror.LogStore.MongoDB
{
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;

    public interface ILogStore
    {
        SyncLogDto Get(ISynchronization sync);

        void LogSync(ISynchronization sync, string text);
    }
}