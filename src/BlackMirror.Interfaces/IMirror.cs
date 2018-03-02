namespace BlackMirror.Interfaces
{
    using System;
    using BlackMirror.Models;

    public interface IMirror : IModel
    {
        string Name { get; }
        ISvcRepository SourceRepository { get; }
        ISvcRepository TargetRepository { get; }
        string TargetRepositoryRefSpec { get; }
        IUser Owner { get; }
        DateTime CreationTime { get; }
        DateTime? LastSynced { get; }
        string[] IgnoredFiles { get; }
    }
}