using System.Collections.Generic;

namespace BlackMirror.SyncStore.MongoDB
{
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;

    public interface ISyncStore
    {
        IEnumerable<ISynchronization> GetAll(Dictionary<string, string> filters = null);

        ISynchronization GetById(string id);

        IEnumerable<ISynchronization> GetByMirror(IMirror mirror, int? take = null);

        ISynchronization Add(SynchronizationDto dto);

        void UpdateStatus(string id, SynchronizationStatus status);

        void Delete(string id);
    }
}
