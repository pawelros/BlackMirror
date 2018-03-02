namespace BlackMirror.MongoDB.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public abstract class QueryRepositoryBase<M, D> : IQueryRepository<M, D>
        where M : class, IModel
        where D : class, IDto
    {
        protected readonly IMongoCollection<BsonDocument> collection;

        protected QueryRepositoryBase(IWebApiConfiguration webApiConfiguration, string collectionName)
        {
            var client = new MongoClient(webApiConfiguration.MongoConnectionString);
            var database = client.GetDatabase(webApiConfiguration.MongoDatabase);
            this.collection = database.GetCollection<BsonDocument>(collectionName);
        }

        protected abstract M ConvertToModel(D dto);

        protected abstract object GenerateIdObject(string id);

        public IEnumerable<M> GetAll(Dictionary<string, string> filters = null)
        {
            // TODO: read document async and directly convert to model for a better performance

            var result = new List<M>();
            var filter = Builders<BsonDocument>.Filter.Empty;

            if (filters != null && filters.Any())
            {
                var fs = new List<FilterDefinition<BsonDocument>>();

                foreach (var kvp in filters)
                {
                    var f = Builders<BsonDocument>.Filter.Eq(kvp.Key, kvp.Value);
                    fs.Add(f);
                }

                filter = Builders<BsonDocument>.Filter.And(fs);
            }

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

                                var dto = BsonSerializer.Deserialize<D>(doc);
                                var model = this.ConvertToModel(dto);
                                result.Add(model);
                            }
                        }

                    }
                }
            }

            return result;
        }

        public M GetById(string id)
        {
            // TODO: read document async and directly convert to model for a better performance
            var objectId = this.GenerateIdObject(id);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            var document = this.collection.Find(filter).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var dto = BsonSerializer.Deserialize<D>(document);
            var model = this.ConvertToModel(dto);
            return model;
        }
    }
}