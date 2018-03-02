namespace BlackMirror.SyncStore.MongoDB
{
    using System;
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.MirrorStore.MongoDB;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public class QueryRepository : QueryRepositoryBase<Synchronization, SynchronizationDto>
    {
        private readonly IMirrorStore mirrorStore;

        public QueryRepository(IWebApiConfiguration webApiConfiguration, IMirrorStore mirrorStore, string collectionName)
            : base(webApiConfiguration, collectionName)
        {
            this.mirrorStore = mirrorStore;
        }

        protected override object GenerateIdObject(string id)
        {
            return new ObjectId(id);
        }

        protected override Synchronization ConvertToModel(SynchronizationDto dto)
        {
            var mirror = this.mirrorStore.GetById(dto.MirrorId);

            if (mirror == null)
            {
                throw new InvalidOperationException($"Mirror with id '{dto.MirrorId}' not found.");
            }

            dto.Mirror = mirror.ToDto();

            return dto.ToSynchronization();
        }

        public IEnumerable<Synchronization> GetByMirror(IMirror mirror, int? take)
        {
            var mirrorId = new ObjectId(mirror.Id);
            var filter = Builders<BsonDocument>.Filter.Eq("MirrorId", mirrorId);
            var sort = Builders<BsonDocument>.Sort.Descending("_id");

            var document = base.collection.Find(filter).Sort(sort).Limit(take);

            var result = new List<Synchronization>();

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

                                var dto = BsonSerializer.Deserialize<SynchronizationDto>(doc);
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