namespace BlackMirror.Svc.UserStore.MongoDB
{
    using System.Collections.Generic;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Models;

    public interface IUserStore
    {
        IEnumerable<IUser> GetAll();

        IUser GetById(string id);

        IUser GetByName(string name);

        IUser Add(UserWithPasswordsDto user);

        void Delete(string id);

        string GetPassword(string id, SvcRepositoryType repositoryType, string passphrase);

        void Update(string id, UserWithPasswordsDto dto);
    }
}