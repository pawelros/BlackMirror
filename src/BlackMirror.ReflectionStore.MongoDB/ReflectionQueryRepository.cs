namespace BlackMirror.ReflectionStore.MongoDB
{
    using System;
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using BlackMirror.SyncStore.MongoDB;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public class ReflectionQueryRepository : QueryRepositoryBase<Reflection, ReflectionDto>
    {
        private readonly ISyncStore syncStore;

        public ReflectionQueryRepository(IWebApiConfiguration webApiConfiguration, ISyncStore syncStore, string collectionName) : base(webApiConfiguration, collectionName)
        {
            this.syncStore = syncStore;
        }

        protected override object GenerateIdObject(string id)
        {
            return new ObjectId(id);
        }

        protected override Reflection ConvertToModel(ReflectionDto dto)
        {
            var sync = this.syncStore.GetById(dto.SynchronizationId);

            if (sync == null)
            {
                throw new InvalidOperationException($"Synchronization with id '{dto.SynchronizationId}' not found.");
            }

            dto.Synchronization = sync.ToDto();
            dto.Mirror = dto.Synchronization.Mirror;

            var model = dto.ToReflection();
            return model;
        }

        public IEnumerable<Reflection> GetByMirror(IMirror mirror, int? take)
        {
            var mirrorId = new ObjectId(mirror.Id);
            var filter = Builders<BsonDocument>.Filter.Eq("MirrorId", mirrorId);
            var sort = Builders<BsonDocument>.Sort.Descending("_id");

            var document = base.collection.Find(filter).Sort(sort).Limit(take);

            var result = new List<Reflection>();

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

                                var dto = BsonSerializer.Deserialize<ReflectionDto>(doc);
                                var model = this.ConvertToModel(dto);
                                result.Add(model);
                            }
                        }

                    }
                }
            }

            return result;
        }

        public IEnumerable<Reflection> GetBySync(ISynchronization sync, int? take)
        {
            var syncId = new ObjectId(sync.Id);
            var filter = Builders<BsonDocument>.Filter.Eq("SynchronizationId", syncId);
            var sort = Builders<BsonDocument>.Sort.Descending("_id");

            var document = base.collection.Find(filter).Sort(sort).Limit(take);

            var result = new List<Reflection>();

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

                                var dto = BsonSerializer.Deserialize<ReflectionDto>(doc);
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