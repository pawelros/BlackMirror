namespace BlackMirror.MirrorStore.MongoDB
{
    using System.Collections.Generic;
    using System.Linq;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Svc.UserStore.MongoDB;
    using BlackMirror.SvcRepositoryStore.MongoDB;

    public class MirrorStore : IMirrorStore
    {
        private readonly MirrorQueryRepository query;
        private readonly MirrorCommandRepository command;

        public MirrorStore(IWebApiConfiguration webApiConfiguration, ISvcRepositoryStore repositoryStore, IUserStore userStore)
        {
            this.query = new MirrorQueryRepository(webApiConfiguration, repositoryStore, userStore);
            this.command = new MirrorCommandRepository(webApiConfiguration, repositoryStore, userStore);
        }
        public IEnumerable<IMirror> GetAll()
        {
            return this.query.GetAll().OrderBy(m => m.Name);
        }

        public IMirror GetById(string id)
        {
            return this.query.GetById(id);
        }

        public IEnumerable<IMirror> GetBySourceRepository(ISvcRepository repository)
        {
            return this.query.GetBySourceRepository(repository);
        }

        public IMirror Add(MirrorDto dto)
        {
            var mirror = this.command.Add(dto);
            return mirror;
        }

        public void Update(string id, MirrorDto mirror)
        {
            this.command.Update(id, mirror);
        }

        public void Delete(string id)
        {
            this.command.Delete(id);
        }
    }
}
