namespace BlackMirror.Dto
{
    using System.Collections.Generic;
    using BlackMirror.Models;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [BsonIgnoreExtraElements] //Password
    public class CredentialsDto
    {
        public string Login { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public SvcRepositoryType RepositoryType { get; set; }

        public IEnumerable<string> AllowedRepositories { get; set; }
    }
}