namespace BlackMirror.WebApi.Events
{
    using System;
    using System.Linq;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.MirrorStore.MongoDB;
    using BlackMirror.SvcRepositoryStore.MongoDB;
    using BlackMirror.SyncStore.MongoDB;

    public class EventHandler
    {
        private readonly ISvcRepositoryStore repositoryStore;
        private readonly IMirrorStore mirrorStore;
        private readonly ISyncStore syncStore;

        public EventHandler(ISvcRepositoryStore repositoryStore, IMirrorStore mirrorStore, ISyncStore syncStore)
        {
            this.repositoryStore = repositoryStore;
            this.mirrorStore = mirrorStore;
            this.syncStore = syncStore;
        }

        // It should handle generic interface
        // but I am a bit lazy today
        public void Handle(GitLabPushEventDto ev)
        {
            if (ev.EventName == "push")
            {
                string repositoryUrl = ev.Repository.GitHttpUrl;

                var repositories = this.repositoryStore.GetByUrl(repositoryUrl);

                if (repositories != null)
                {
                    foreach (var repository in repositories)
                    {
                        var mirrors = this.mirrorStore.GetBySourceRepository(repository);

                        if (mirrors != null)
                        {
                            foreach (var mirror in mirrors)
                            {
                                string fullRefSpec = ev.Ref;
                                string branchName = ev.Ref.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Last();

                                if (mirror.TargetRepositoryRefSpec == fullRefSpec
                                    || mirror.TargetRepositoryRefSpec == branchName)
                                {
                                    Logging.Log()
                                        .Information(
                                            $"Scheduling synchronization based on received GitLab push event: "
                                            + $"MirrorId: {mirror.Id}" + $"MirrorName: {mirror.Name}"
                                            + $"RefSpec: {mirror.TargetRepositoryRefSpec}");

                                    var syncDto =
                                        new SynchronizationDto
                                        {
                                            CreationTime = DateTime.Now,
                                            Mirror = mirror.ToDto(),
                                            MirrorId = mirror.Id,
                                            Status = SynchronizationStatus.Scheduled
                                        };

                                    this.syncStore.Add(syncDto);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Logging.Log().Warning($"Received push event from GitLab, but repository '{repositoryUrl}' not found in database.");
                }
            }
        }
    }
}