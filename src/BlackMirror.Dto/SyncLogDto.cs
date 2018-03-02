namespace BlackMirror.Dto
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;

    public class SyncLogDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SyncId { get; set; }

        [BsonIgnore]
        [JsonProperty("Text")]
        public string Text { get; set; }

        public SyncLogEntryDto[] Logs { get; set; }
    }
}