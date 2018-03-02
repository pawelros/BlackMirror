namespace BlackMirror.Dto
{
    using BlackMirror.Interfaces;
    using BlackMirror.Models;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class SvcRepositoryDto : IDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public SvcRepositoryType Type { get; set; }

        public string Name { get; set; }

        public string Uri { get; set; }

        public string DefaultCommitMessagePrefix { get; set; }

        [BsonIgnore]
        public UserDto CheckoutUser { get; set; }

        [BsonIgnore]
        public UserDto PushUser { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string CheckoutUserId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string PushUserId { get; set; }

        [BsonIgnore]
        public CredentialsDto MappedCheckoutCredentials { get; set; }
    }
}