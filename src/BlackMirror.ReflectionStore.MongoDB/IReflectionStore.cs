namespace BlackMirror.ReflectionStore.MongoDB
{
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;

    public interface IReflectionStore
    {
        IReflection Add(ReflectionDto dto);

        void Delete(string id);

        IReflection GetById(string id);

        IEnumerable<IReflection> GetAll();

        IEnumerable<IReflection> GetBySynchronization(ISynchronization sync, int? take = null);

        IEnumerable<IReflection> GetByMirror(IMirror mirror, int? take = null);
    }
}
