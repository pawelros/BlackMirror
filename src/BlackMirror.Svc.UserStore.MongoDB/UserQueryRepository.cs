namespace BlackMirror.Svc.UserStore.MongoDB
{
    using System.Collections.Generic;
    using System.Linq;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Crypto;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public class UserQueryRepository : QueryRepositoryBase<User, UserDto>
    {
        private readonly IWebApiConfiguration webApiConfiguration;

        public UserQueryRepository(IWebApiConfiguration webApiConfiguration)
            :base(webApiConfiguration, webApiConfiguration.MongoUserCollection)
        {
            this.webApiConfiguration = webApiConfiguration;
        }

        protected override User ConvertToModel(UserDto dto)
        {
            return dto.ToUser();
        }

        protected override object GenerateIdObject(string id)
        {
            return id;
        }

        public string GetPassword(string id, SvcRepositoryType repositoryType, string passphrase)
        {
            // TODO: read document async and directly convert to model for a better performance
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var document = base.collection.Find(filter).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var creds = document["RepositoryCredentials"].AsBsonArray;

            var credentials = new List<CredentialsWithPasswordDto>(creds.Count);
            credentials.AddRange(creds.Select(item => BsonSerializer.Deserialize<CredentialsWithPasswordDto>(item.AsBsonDocument)));

            CredentialsWithPasswordDto dto = credentials.FirstOrDefault(c => c.RepositoryType == repositoryType);

            if (dto == null)
            {
                Logging.Log().Debug($"Password for {id} user and {repositoryType} repository type not found.");
                return null;
            }

            string decrypted = StringCipher.Decrypt(dto.Password, this.webApiConfiguration.SecretPassword);

            return decrypted;
        }

        public IUser GetByName(string name)
        {
            // TODO: read document async and directly convert to model for a better performance
            var filter = Builders<BsonDocument>.Filter.Eq("Name", name);

            var document = this.collection.Find(filter).FirstOrDefault();

            if (document == null)
            {
                return null;
            }

            var dto = BsonSerializer.Deserialize<UserDto>(document);
            var model = this.ConvertToModel(dto);
            return model;
        }
    }
}