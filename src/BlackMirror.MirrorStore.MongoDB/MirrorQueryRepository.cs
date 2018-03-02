namespace BlackMirror.MirrorStore.MongoDB
{
    using System;
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using BlackMirror.Svc.UserStore.MongoDB;
    using BlackMirror.SvcRepositoryStore.MongoDB;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public class MirrorQueryRepository : QueryRepositoryBase<Mirror, MirrorDto>
    {
        private readonly ISvcRepositoryStore repositoryStore;
        private readonly IUserStore userStore;

        public MirrorQueryRepository(IWebApiConfiguration webApiConfiguration, ISvcRepositoryStore repositoryStore, IUserStore userStore)
            :base(webApiConfiguration,webApiConfiguration.MongoMirrorCollection)
        {
            this.repositoryStore = repositoryStore;
            this.userStore = userStore;
        }

        protected override object GenerateIdObject(string id)
        {
            return new ObjectId(id);
        }

        protected override Mirror ConvertToModel(MirrorDto dto)
        {
            var sourceRepository = this.repositoryStore.GetById(dto.SourceRepositoryId);
            var targetRepository = this.repositoryStore.GetById(dto.TargetRepositoryId);
            var owner = this.userStore.GetById(dto.OwnerId);

            if (sourceRepository == null)
            {
                throw new InvalidOperationException($"Repository with id '{dto.SourceRepositoryId}' not found.");
            }
            if (targetRepository == null)
            {
                throw new InvalidOperationException($"Repository with id '{dto.TargetRepositoryId}' not found.");
            }
            if (owner == null)
            {
                throw new InvalidOperationException($"Owner with id '{dto.OwnerId}' not found.");
            }

            dto.SourceRepository = sourceRepository.ToDto();
            dto.TargetRepository = targetRepository.ToDto();
            dto.Owner = owner.ToDto();

            return dto.ToMirror();
        }

        public IEnumerable<IMirror> GetBySourceRepository(ISvcRepository repository)
        {
            // TODO: read document async and directly convert to model for a better performance

            var result = new List<IMirror>();
            var objectId = this.GenerateIdObject(repository.Id);
            var filter = Builders<BsonDocument>.Filter.Eq("SourceRepositoryId", objectId);

            var document = this.collection.Find(filter);

            if (document != null && document.Count() > 0)
            {
                using (var cursor = document.ToCursor())
                {
                    while (cursor.MoveNext())
                    {
                        using (var enumerator = cursor.Current.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                var doc = enumerator.Current;

                                var dto = BsonSerializer.Deserialize<MirrorDto>(doc);
                                var model = this.ConvertToModel(dto);
                                result.Add(model);
                            }
                        }

                    }
                }
            }

            return result;
        }
    }
}