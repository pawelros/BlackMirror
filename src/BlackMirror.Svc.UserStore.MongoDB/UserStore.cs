namespace BlackMirror.Svc.UserStore.MongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlackMirror.Crypto;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using global::MongoDB.Bson;
    using global::MongoDB.Driver;

    public class UserStore : IUserStore
    {
        private readonly IWebApiConfiguration webApiConfiguration;
        private readonly UserQueryRepository query;
        private readonly UserCommandRepository command;
        private readonly IMongoCollection<BsonDocument> collection;
        private readonly IMongoDatabase database;

        public UserStore(IWebApiConfiguration webApiConfiguration)
        {
            this.webApiConfiguration = webApiConfiguration;
            this.query = new UserQueryRepository(webApiConfiguration);
            this.command = new UserCommandRepository(webApiConfiguration);

            var client = new MongoClient(webApiConfiguration.MongoConnectionString);
            this.database = client.GetDatabase(webApiConfiguration.MongoDatabase);
            this.collection = this.database.GetCollection<BsonDocument>(webApiConfiguration.MongoUserCollection);
        }
        public IEnumerable<IUser> GetAll()
        {
            return this.query.GetAll().OrderBy(u => u.Id);
        }

        public IUser GetById(string id)
        {
            return this.query.GetById(id);
        }

        public IUser GetByName(string name)
        {
            return this.query.GetByName(name);
        }

        public IUser Add(UserWithPasswordsDto dto)
        {
            var credentials = dto.RepositoryCredentials;
            this.EncryptCredentials(credentials);
            var model = new User
            {
                Id = dto.Id,
                Email = dto.Email,
                Name = dto.Name,
                RepositoryCredentials = credentials?.Select(c => c.ToCredentials())
            };

            var document = dto.ToBsonDocument();

            this.collection.InsertOne(document);

            return model;
        }


        private void EncryptCredentials(IEnumerable<CredentialsDto> credentials)
        {
            if (credentials == null)
            {
                return;
            }

            foreach (var cred in credentials)
            {
                var c = cred as CredentialsWithPasswordDto;
                if (!string.IsNullOrWhiteSpace(c.Password))
                {
                    string encrypted = StringCipher.Encrypt(c.Password, this.webApiConfiguration.SecretPassword);
                    c.Password = encrypted;
                }
            }
        }

        public void Delete(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var result = this.collection.DeleteOne(filter);

            if (result.DeletedCount == 0)
            {
                throw new InvalidOperationException("Have not deleted any document. Filter: " + filter.ToJson());
            }
        }

        public string GetPassword(string id, SvcRepositoryType repositoryType, string passphrase)
        {
            return this.query.GetPassword(id, repositoryType, passphrase);
        }

        public void Update(string id, UserWithPasswordsDto dto)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var update = Builders<BsonDocument>.Update.Set("Name", dto.Name);
            update = update.Set("Email", dto.Email);

            var result = this.collection.UpdateOne(filter, update);

            // update passwords

            var changedPasswords = dto.RepositoryCredentials.Where(c => !string.IsNullOrEmpty(c.Password)).ToArray();

            if (result.ModifiedCount == 0 && !changedPasswords.Any())
            {
                throw new InvalidOperationException("Have not updated any document. Filter: " + filter.ToJson());
            }

            this.EncryptCredentials(dto.RepositoryCredentials);

            foreach (var cp in changedPasswords)
            {
                // perform atomic FindOneAndUpdate operation
                var users = this.database.GetCollection<UserWithPasswordsDto>("users");
                var x = users.FindOneAndUpdate(
                    c => c.Id == dto.Id && c.RepositoryCredentials.Any(
                        s => s.Login == cp.Login && s.RepositoryType == cp.RepositoryType), // find this match
                    Builders<UserWithPasswordsDto>.Update.Set("RepositoryCredentials.$.Password", cp.Password));     // -1 means update first matching array element

                if (x == null)
                {
                    throw new InvalidOperationException($"Have not updated password for user {cp.Login}");
                }
            }
        }
    }
}
