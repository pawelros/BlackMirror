namespace BlackMirror.LogStore.MongoDB
{
    using System;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public class LogStore : ILogStore
    {
        private readonly IMongoCollection<BsonDocument> collection;

        public LogStore(IWebApiConfiguration webApiConfiguration)
        {
            var client = new MongoClient(webApiConfiguration.MongoConnectionString);
            var database = client.GetDatabase(webApiConfiguration.MongoDatabase);
            this.collection = database.GetCollection<BsonDocument>("sync_logs");
        }

        public SyncLogDto Get(ISynchronization sync)
        {
            var objectId = new BsonObjectId(new ObjectId(sync.Id));

            var filter = Builders<BsonDocument>.Filter.Eq("SyncId", objectId);
            var document = this.collection.Find(filter).FirstOrDefault(); ;

            if (document == null)
            {
                return null;
            }

            var dto = BsonSerializer.Deserialize<SyncLogDto>(document);

            return dto;
        }

        public void LogSync(ISynchronization sync, string text)
        {
            var objectId = new BsonObjectId(new ObjectId(sync.Id));

            var updateObject = new
                                   {
                                       Timestamp = DateTime.Now,
                                       Text = text
                                   };
            
            var filter = Builders<BsonDocument>.Filter.Eq("SyncId", objectId);
            var update = Builders<BsonDocument>.Update.Push("Logs", updateObject);

            this.collection.FindOneAndUpdate(
                   filter,
                   update,
                   new FindOneAndUpdateOptions<BsonDocument> { IsUpsert = true });
        }
    }
}
