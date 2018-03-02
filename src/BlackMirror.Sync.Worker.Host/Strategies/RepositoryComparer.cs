namespace BlackMirror.Sync.Worker.Host.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.HttpClient;
    using BlackMirror.Interfaces;

    public class RepositoryComparer
    {
        private readonly IBlackMirrorHttpClient httpClient;
        private readonly IMirror mirror;
        private readonly List<IRevision> sourceRepositoryLog;
        private readonly List<IRevision> targetRepositoryLog;

        public RepositoryComparer(IBlackMirrorHttpClient httpClient, IMirror mirror, IEnumerable<IRevision> sourceRepositoryLog, IEnumerable<IRevision> targetRepositoryLog)
        {
            this.httpClient = httpClient;
            this.mirror = mirror;
            this.sourceRepositoryLog = sourceRepositoryLog.ToList();
            this.targetRepositoryLog = targetRepositoryLog.ToList();
        }

        public List<IRevision> GetRevisionsAwaitingForSync()
        {
            var latestReflection = this.GetLatestReflection();

            // repo's never synced
            if (latestReflection == null)
            {
                Logging.Log().Information($"Did not found any reflection for mirror {this.mirror.Id}.");
                return this.sourceRepositoryLog.ToList();
            }

            Logging.Log().Information($@"The latest known reflection for mirror {this.mirror.Id} is:
Source revision:
Id: {latestReflection.SourceRevision.Id}
Author: {latestReflection.SourceRevision.Author}
Message: {latestReflection.SourceRevision.Message}

Target revision:
Id: {latestReflection.TargetRevision.Id}
Author: {latestReflection.TargetRevision.Author}
Message: {latestReflection.TargetRevision.Message}");

            var latestRev = this.sourceRepositoryLog.First(r => r.Id == latestReflection.SourceRevision.Id);
            int latestRevIndex = this.sourceRepositoryLog.IndexOf(latestRev);
            var range = this.sourceRepositoryLog.Take(latestRevIndex).ToList();

            if (range.Count == 0)
            {
                Logging.Log().Information($@"Mirror {this.mirror.Id} is up to date. There is nothing to synchronize.");
            }

            return range;
        }

        private IReflection GetLatestReflection()
        {
            var reflection = this.httpClient.Mirror.GetLatestReflection(this.mirror);

            return reflection;
        }
    }
}