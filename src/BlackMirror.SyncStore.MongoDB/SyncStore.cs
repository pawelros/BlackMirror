namespace BlackMirror.SyncStore.MongoDB
{
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.MirrorStore.MongoDB;

    public class SyncStore : ISyncStore
    {
        private readonly QueryRepository query;
        private readonly CommandRepository command;

        public SyncStore(IWebApiConfiguration webApiConfiguration, IMirrorStore mirrorStore)
        {
            this.query = new QueryRepository(webApiConfiguration, mirrorStore, webApiConfiguration.MongoSyncCollection);
            this.command = new CommandRepository(webApiConfiguration, mirrorStore, webApiConfiguration.MongoSyncCollection);
        }

        public IEnumerable<ISynchronization> GetAll(Dictionary<string, string> filters = null)
        {
            return this.query.GetAll(filters);
        }

        public ISynchronization GetById(string id)
        {
            return this.query.GetById(id);
        }

        public IEnumerable<ISynchronization> GetByMirror(IMirror mirror, int? take = null)
        {
            return this.query.GetByMirror(mirror, take);
        }

        public ISynchronization Add(SynchronizationDto dto)
        {
            return this.command.Add(dto);
        }

        public void UpdateStatus(string id, SynchronizationStatus status)
        {
            this.command.UpdateStatus(id ,status);
        }

        public void Delete(string id)
        {
            this.command.Delete(id);
        }
    }
}