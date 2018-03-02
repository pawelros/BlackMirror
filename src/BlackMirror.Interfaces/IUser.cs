namespace BlackMirror.Interfaces
{
    using System.Collections.Generic;

    public interface IUser : IModel
    {
        string Name { get; }

        string Email { get; }

        IEnumerable<ICredentials> RepositoryCredentials { get; }
    }
}