namespace BlackMirror.Interfaces
{
    using System.Collections.Generic;
    using BlackMirror.Models;

    public interface ISvc
    {
        SvcRepositoryType Type { get; }

        string DefaultCommitMessagePrefix { get; }

        string WorkingDirectory { get; }
        IEnumerable<IRevision> GetLog();

        void CloneOrUpdate();

        void Checkout(IRevision revision);

        void StageAll();

        void CleanUp();

        IRevision UpdateRemoteWithCommit(string message, string login, string password);
    }
}