namespace BlackMirror.Models
{
    using System.Collections.Generic;
    using BlackMirror.Interfaces;
    public class CredentialsWithPassword : Credentials
    {
        public string Password { get; set; }
    }

    public class Credentials : ICredentials
    {
        public string Login { get; set; }

        public SvcRepositoryType RepositoryType { get; set; }

        public IEnumerable<string> AllowedRepositories { get; set; }
    }
}