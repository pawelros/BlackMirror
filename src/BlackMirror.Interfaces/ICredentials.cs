namespace BlackMirror.Interfaces
{
    using System.Collections.Generic;
    using BlackMirror.Models;

    public interface ICredentials
    {
        string Login { get; }

        SvcRepositoryType RepositoryType { get; }

        IEnumerable<string> AllowedRepositories { get; }
    }
}