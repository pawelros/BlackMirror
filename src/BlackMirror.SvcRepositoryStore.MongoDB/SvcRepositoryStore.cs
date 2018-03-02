using System.Collections.Generic;

namespace BlackMirror.SvcRepositoryStore.MongoDB
{
    using System.Linq;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Svc.UserStore.MongoDB;

    public class SvcRepositoryStore : ISvcRepositoryStore
    {
        private readonly SvcRepositoryQueryRepository query;
        private readonly SvcRepositoryCommandRepository command;

        public SvcRepositoryStore(IWebApiConfiguration webApiConfiguration, IUserStore userStore)
        {
            this.query = new SvcRepositoryQueryRepository(webApiConfiguration, userStore);
            this.command = new SvcRepositoryCommandRepository(webApiConfiguration, userStore);
        }

        public IEnumerable<ISvcRepository> GetAll()
        {
            return this.query.GetAll().OrderBy(r => r.Name);
        }

        public ISvcRepository GetById(string id)
        {
            return this.query.GetById(id);
        }

        public IEnumerable<ISvcRepository> GetByUrl(string url)
        {
            return this.query.GetByUrl(url);
        }

        public ISvcRepository Add(SvcRepositoryDto dto)
        {
            var repository = this.command.Add(dto);
            return repository;
        }

        public void Update(string id, SvcRepositoryDto repository)
        {
            this.command.Update(id, repository);
        }

        public void Delete(string id)
        {
            this.command.Delete(id);
        }
    }
}
