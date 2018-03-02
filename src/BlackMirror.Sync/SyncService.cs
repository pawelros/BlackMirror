using System;

namespace BlackMirror.Sync
{
    using System.Collections.Concurrent;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Interfaces;
    using BlackMirror.MirrorStore.MongoDB;
    using BlackMirror.Models;
    using BlackMirror.Svc.Git;
    using BlackMirror.Svc.Svn;

    public class SyncService : ISyncService
    {
        private readonly IMirrorStore mirrorStore;
        private readonly ConcurrentDictionary<string, ISynchronizationProcess> pool;

        public SyncService(IMirrorStore mirrorStore)
        {
            this.mirrorStore = mirrorStore;
            this.pool = new ConcurrentDictionary<string, ISynchronizationProcess>();
        }

        public void Sync(string mirrorId)
        {
            //IMirror mirror = this.mirrorStore.GetById(mirrorId);

            //if (mirror == null)
            //{
            //    throw new InvalidOperationException($"Mirror with id '{mirrorId}' not found.");
            //}

            //ISynchronizationProcess sync = this.pool.GetOrAdd(mirror.Id, new Synchronization(mirror));

            //if (sync.InProgress)
            //{
            //    Logging.Log().Information($"Mirror {mirror.Id} synchronization already in progress.");
            //}
            //else
            //{
            //    sync.Start();
            //}
        }

        public ISynchronization Sync(Synchronization synchronization)
        {
            throw new NotImplementedException();
        }
    }
}
