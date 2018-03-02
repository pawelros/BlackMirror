namespace BlackMirror.HttpClient.Sync
{
    using System.Threading.Tasks;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;

    public interface ISyncClient
    {
        SyncLogDto GetSyncLog(ISynchronization sync);

        Task<SyncLogDto> GetSyncLogAsync(ISynchronization sync);

        void AddSyncLog(ISynchronization sync, string text);
        Task AddSyncLogAsync(ISynchronization sync, string text);
    }
}