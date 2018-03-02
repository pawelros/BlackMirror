namespace BlackMirror.Models
{
    using System.Collections.Generic;
    using BlackMirror.Interfaces;
    public class User : IUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<ICredentials> RepositoryCredentials { get; set; }
    }
}