namespace BlackMirror.Dto
{
    using System;
    using BlackMirror.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class SynchronizationDto : IDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public SynchronizationStatus Status { get; set; }

        [BsonIgnore]
        public MirrorDto Mirror { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string MirrorId { get; set; }

        public DateTime CreationTime
        {
            get => this.creationTime;
            set => this.creationTime = value == DateTime.MinValue ? DateTime.Now : value;
        }

        private DateTime creationTime;
    }
}
