namespace BlackMirror.SvcRepositoryStore.MongoDB
{
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;

    public interface ISvcRepositoryStore
    {
        IEnumerable<ISvcRepository> GetAll();

        ISvcRepository GetById(string id);

        IEnumerable<ISvcRepository> GetByUrl(string url);

        ISvcRepository Add(SvcRepositoryDto repository);

        void Update(string id, SvcRepositoryDto repository);

        void Delete(string id);
    }
}