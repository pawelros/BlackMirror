namespace BlackMirror.Sync.Worker.Host
{
    using System.Threading.Tasks;
    using BlackMirror.Interfaces;

    public interface ISyncQueue
    {
        ISynchronization Pull();

        Task<ISynchronization> PullAsync();

        long Count { get; }

        void SetSynchronizationStatus(ISynchronization sync, SynchronizationStatus status, string additionalInfo = null);
    }
}