namespace BlackMirror.Svc.Git
{
    using LibGit2Sharp;

    public class GitRevision : IGitRevision
    {
        public string Author { get; set; }

        public string Id { get; set; }

        public string Message { get; set; }

        public Commit CommitObject { get; set; }
    }
}