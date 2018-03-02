namespace BlackMirror.Svc.Git
{
    using BlackMirror.Interfaces;
    using LibGit2Sharp;

    public interface IGitRevision : IRevision
    {
        Commit CommitObject { get; }
    }
}