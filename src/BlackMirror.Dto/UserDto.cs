namespace BlackMirror.Dto
{
    using System.Collections.Generic;
    using BlackMirror.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class UserDto : IDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<CredentialsDto> RepositoryCredentials { get; set; }
    }
}