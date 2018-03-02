namespace BlackMirror.Sync
{
    using BlackMirror.Interfaces;
    using BlackMirror.Models;

    public interface ISyncService
    {
        ISynchronization Sync(Synchronization synchronization);
    }
}