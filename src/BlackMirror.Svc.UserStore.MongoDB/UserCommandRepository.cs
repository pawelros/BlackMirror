namespace BlackMirror.Svc.UserStore.MongoDB
{
    using BlackMirror.Dto;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;

    public class UserCommandRepository : CommandRepositoryBase<User, UserDto>
    {
        public UserCommandRepository(IWebApiConfiguration webApiConfiguration)
            : base(webApiConfiguration, webApiConfiguration.MongoUserCollection)
        {
        }

        protected override User ConvertToModel(UserDto dto)
        {
            return dto.ToUser();
        }

        protected override object GenerateIdObject(string id)
        {
            return id;
        }
    }
}