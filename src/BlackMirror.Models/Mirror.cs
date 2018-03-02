namespace BlackMirror.Models
{
    using System;
    using BlackMirror.Interfaces;

    public class Mirror : IMirror
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ISvcRepository SourceRepository { get; set; }

        public ISvcRepository TargetRepository { get; set; }

        public string TargetRepositoryRefSpec { get; set; }

        public IUser Owner { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastSynced { get; set; }

        public string[] IgnoredFiles { get; set; }
    }
}
