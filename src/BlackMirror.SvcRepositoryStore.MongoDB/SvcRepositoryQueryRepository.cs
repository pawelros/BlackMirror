namespace BlackMirror.SvcRepositoryStore.MongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using BlackMirror.Svc.UserStore.MongoDB;
    using global::MongoDB.Bson;
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    public class SvcRepositoryQueryRepository : QueryRepositoryBase<SvcRepository, SvcRepositoryDto>
    {
        private readonly IUserStore userStore;

        public SvcRepositoryQueryRepository(IWebApiConfiguration webApiConfiguration, IUserStore userStore)
            : base(webApiConfiguration, webApiConfiguration.MongoSvcCollection)
        {
            this.userStore = userStore;
        }

        protected override object GenerateIdObject(string id)
        {
            return new ObjectId(id);
        }

        protected override SvcRepository ConvertToModel(SvcRepositoryDto dto)
        {
            IUser checkoutUser = this.userStore.GetById(dto.CheckoutUserId);

            if (checkoutUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            dto.CheckoutUser = checkoutUser.ToDto();

            CredentialsDto mappedCheckoutCredentials =
                dto.CheckoutUser.RepositoryCredentials.FirstOrDefault(c => c.RepositoryType == dto.Type);
            dto.MappedCheckoutCredentials = mappedCheckoutCredentials;

            IUser pushUser = this.userStore.GetById(dto.PushUserId);

            if (pushUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            dto.PushUser = pushUser.ToDto();

            CredentialsDto mappedPushCredentials =
                dto.PushUser.RepositoryCredentials.FirstOrDefault(c => c.RepositoryType == dto.Type);
            dto.MappedCheckoutCredentials = mappedPushCredentials;

            return dto.ToSvcRepository();
        }

        public IEnumerable<ISvcRepository> GetByUrl(string url)
        {
            var result = new List<ISvcRepository>();
            // TODO: read document async and directly convert to model for a better performance
            var filter = Builders<BsonDocument>.Filter.Eq("Uri", url);

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

                                var dto = BsonSerializer.Deserialize<SvcRepositoryDto>(doc);
                                var model = this.ConvertToModel(dto);
                                result.Add(model);
                            }
                        }

                    }
                }
            }

            return result;
        }
    }
}