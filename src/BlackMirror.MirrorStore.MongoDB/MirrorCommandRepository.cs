namespace BlackMirror.MirrorStore.MongoDB
{
    using System;
    using BlackMirror.Dto;
    using BlackMirror.Interfaces;
    using BlackMirror.Interfaces.Configuration;
    using BlackMirror.Models;
    using BlackMirror.MongoDB.Abstractions;
    using BlackMirror.Svc.UserStore.MongoDB;
    using BlackMirror.SvcRepositoryStore.MongoDB;
    using global::MongoDB.Bson;

    public class MirrorCommandRepository : CommandRepositoryBase<Mirror, MirrorDto>

    {
        private readonly ISvcRepositoryStore svcRepositoryStore;
        private readonly IUserStore userStore;

        public MirrorCommandRepository(IWebApiConfiguration webApiConfiguration, ISvcRepositoryStore svcRepositoryStore, IUserStore userStore)
            :base(webApiConfiguration, webApiConfiguration.MongoMirrorCollection)
        {
            this.svcRepositoryStore = svcRepositoryStore;
            this.userStore = userStore;
        }

        protected override object GenerateIdObject(string id)
        {
            return new ObjectId(id);
        }

        protected override Mirror ConvertToModel(MirrorDto dto)
        {
            var sourceRepository = this.svcRepositoryStore.GetById(dto.SourceRepositoryId);
            var targetRepository = this.svcRepositoryStore.GetById(dto.TargetRepositoryId);
            var owner = this.userStore.GetById(dto.OwnerId);

            if (sourceRepository == null)
            {
                throw new InvalidOperationException($"Repository with id '{dto.SourceRepositoryId}' not found.");
            }
            if (targetRepository == null)
            {
                throw new InvalidOperationException($"Repository with id '{dto.TargetRepositoryId}' not found.");
            }
            if (owner == null)
            {
                throw new InvalidOperationException($"Owner with id '{dto.OwnerId}' not found.");
            }

            dto.SourceRepository = sourceRepository.ToDto();
            dto.TargetRepository = targetRepository.ToDto();
            dto.Owner = owner.ToDto();

            return dto.ToMirror();
        }
    }
}