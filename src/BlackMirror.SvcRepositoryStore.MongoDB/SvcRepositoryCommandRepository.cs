namespace BlackMirror.SvcRepositoryStore.MongoDB
{
    using System;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using BlackMirror.Svc.UserStore.MongoDB;
    using global::MongoDB.Bson;

    public class SvcRepositoryCommandRepository : CommandRepositoryBase<SvcRepository, SvcRepositoryDto>
    {
        private readonly IUserStore userStore;

        public SvcRepositoryCommandRepository(IWebApiConfiguration webApiConfiguration, IUserStore userStore)
            :base(webApiConfiguration, webApiConfiguration.MongoSvcCollection)
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

            IUser pushUser = this.userStore.GetById(dto.PushUserId);

            if (pushUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            dto.PushUser = pushUser.ToDto();

            return dto.ToSvcRepository();
        }
    }
}