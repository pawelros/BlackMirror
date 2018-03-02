namespace BlackMirror.MongoDB.Abstractions
{
    using System;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using global::MongoDB.Bson;
    using global::MongoDB.Driver;

    public abstract class CommandRepositoryBase<M, D> : ICommandRepository<M, D>
        where M : class, IModel
        where D : class, IDto
    {
        private readonly IWebApiConfiguration webApiConfiguration;
        protected readonly IMongoCollection<BsonDocument> collection;

        protected CommandRepositoryBase(IWebApiConfiguration webApiConfiguration, string collectionName)
        {
            this.webApiConfiguration = webApiConfiguration;
            var client = new MongoClient(webApiConfiguration.MongoConnectionString);
            var database = client.GetDatabase(webApiConfiguration.MongoDatabase);
            this.collection = database.GetCollection<BsonDocument>(collectionName);
        }

        protected abstract M ConvertToModel(D dto);

        protected abstract object GenerateIdObject(string id);

        public M Add(D dto)
        {
            var model = this.ConvertToModel(dto);
            var document = dto.ToBsonDocument();

            this.collection.InsertOne(document);
            model.Id = document.GetValue("_id").AsObjectId.ToString();

            return model;
        }

        public void Update(string id, D dto)
        {
            var objectId = this.GenerateIdObject(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var update = dto.ToBsonDocument();

            var result = this.collection.ReplaceOne(filter, update);

            if (result.ModifiedCount == 0)
            {
                throw new InvalidOperationException("Have not updated any document. Filter: " + filter.ToJson());
            }
        }

        public void Delete(string id)
        {
            var objectId = this.GenerateIdObject(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            var result = this.collection.DeleteOne(filter);

            if (result.DeletedCount == 0)
            {
                throw new InvalidOperationException("Have not deleted any document. Filter: " + filter.ToJson());
            }
        }
    }
}