namespace BlackMirror.Sync.Worker.Host
{
    using System;
    using System.IO;
    using BlackMirror.Interfaces;

    public static class WorkspacePathProvider
    {
        public static string GitRepositoryPath(ISynchronization sync)
        {
            var runPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(runPath, "workspace", sync.Id, "git");

            return path;
        }

        public static string SvnRepositoryPath(ISynchronization sync)
        {
            var runPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(runPath, "workspace", sync.Id, "svn");

            return path;
        }
    }
}