namespace BlackMirror.Sync.Worker.Host
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BlackMirror.Dto;
    using BlackMirror.Models;

    public interface IUserRetriever
    {
        string GetPassword(string userId, SvcRepositoryType repositoryType);

        UserDto GetUser(string userId);

        Task<UserDto> GetUserAsync(string userId);

        List<UserDto> GetUsers();

        Task<List<UserDto>> GetUsersAsync();
    }
}