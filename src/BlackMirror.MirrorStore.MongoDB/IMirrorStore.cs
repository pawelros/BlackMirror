namespace BlackMirror.MirrorStore.MongoDB
{
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;

    public interface IMirrorStore
    {
        IEnumerable<IMirror> GetAll();
        IMirror GetById(string id);
        IEnumerable<IMirror> GetBySourceRepository(ISvcRepository repository);
        IMirror Add(MirrorDto dto);
        void Update(string id, MirrorDto mirror);

        void Delete(string id);
    }
}