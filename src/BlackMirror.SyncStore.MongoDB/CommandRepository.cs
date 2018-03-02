namespace BlackMirror.SyncStore.MongoDB
{
    using System;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.MirrorStore.MongoDB;
    using BlackMirror.Models;
    using BlackMirror.Models.Exceptions;
    using BlackMirror.MongoDB.Abstractions;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public class CommandRepository : CommandRepositoryBase<Synchronization, SynchronizationDto>
    {
        private readonly IMirrorStore mirrorStore;

        public CommandRepository(IWebApiConfiguration webApiConfiguration, IMirrorStore mirrorStore, string collectionName)
            : base(webApiConfiguration, collectionName)
        {
            this.mirrorStore = mirrorStore;
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

        protected override object GenerateIdObject(string id)
        {
            return new ObjectId(id);
        }

        public void UpdateStatus(string id, SynchronizationStatus status)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var update = Builders<BsonDocument>.Update.Set("Status", status.ToString());

            var doc = base.collection.FindOneAndUpdate(filter, update);
            var dto = BsonSerializer.Deserialize<SynchronizationDto>(doc);

            if (dto.Status == status)
            {
                throw new StatusNotChangedException($"Status is already {status}");
            }

        }
    }
}