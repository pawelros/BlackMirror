namespace BlackMirror.Dto
{
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class UserWithPasswordsDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<CredentialsWithPasswordDto> RepositoryCredentials { get; set; }
    }
}
