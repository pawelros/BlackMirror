namespace BlackMirror.ReflectionStore.MongoDB
{
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.SyncStore.MongoDB;

    public class ReflectionStore : IReflectionStore
    {
        private readonly IWebApiConfiguration webApiConfiguration;
        private readonly ReflectionQueryRepository query;
        private readonly ReflectionCommandRepository command;

        public ReflectionStore(IWebApiConfiguration webApiConfiguration, ISyncStore syncStore)
        {
            this.webApiConfiguration = webApiConfiguration;
            this.query = new ReflectionQueryRepository(webApiConfiguration, syncStore, webApiConfiguration.MongoReflectionCollection);
            this.command = new ReflectionCommandRepository(webApiConfiguration, webApiConfiguration.MongoReflectionCollection);
        }

        public IReflection Add(ReflectionDto dto)
        {
            var reflection = this.command.Add(dto);
            return reflection;
        }

        public void Delete(string id)
        {
            this.command.Delete(id);
        }

        public IEnumerable<IReflection> GetAll()
        {
            var all = this.query.GetAll();
            return all;
        }

        public IReflection GetById(string id)
        {
            var reflection = this.query.GetById(id);
            return reflection;
        }

        public IEnumerable<IReflection> GetBySynchronization(ISynchronization sync, int? take)
        {
            var reflections = this.query.GetBySync(sync, take);
            return reflections;
        }

        public IEnumerable<IReflection> GetByMirror(IMirror mirror, int? take)
        {
            var result = this.query.GetByMirror(mirror, take);

            return result;
        }
    }
}