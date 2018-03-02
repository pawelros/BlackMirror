namespace BlackMirror.Dto
{
    using System.Linq;
    using BlackMirror.Interfaces;
    using BlackMirror.Models;

    public static class DtoExtensions
    {
        public static Synchronization ToSynchronization(this SynchronizationDto dto)
        {
            var s = new Synchronization
            {
                CreationTime = dto.CreationTime,
                Id = dto.Id,
                Mirror = dto.Mirror.ToMirror(),
                Status = dto.Status
            };

            return s;
        }

        public static SynchronizationDto ToDto(this ISynchronization sync)
        {
            var s = new SynchronizationDto
            {
                CreationTime = sync.CreationTime,
                Id = sync.Id,
                Mirror = sync.Mirror.ToDto(),
                Status = sync.Status
            };

            return s;
        }

        public static Mirror ToMirror(this MirrorDto dto)
        {
            var m = new Mirror
            {
                Id = dto.Id,
                Name = dto.Name,
                SourceRepository = dto.SourceRepository.ToSvcRepository(),
                TargetRepository = dto.TargetRepository.ToSvcRepository(),
                TargetRepositoryRefSpec = dto.TargetRepositoryRefSpec,
                Owner = dto.Owner.ToUser(),
                CreationTime = dto.CreationTime,
                LastSynced = dto.LastSynced
            };

            return m;
        }

        public static MirrorDto ToDto(this IMirror mirror)
        {
            var d = new MirrorDto
            {
                Id = mirror.Id,
                Name = mirror.Name,
                SourceRepository = mirror.SourceRepository.ToDto(),
                TargetRepository = mirror.TargetRepository.ToDto(),
                TargetRepositoryRefSpec = mirror.TargetRepositoryRefSpec,
                Owner = mirror.Owner.ToDto(),
                CreationTime = mirror.CreationTime,
                LastSynced = mirror.LastSynced
            };

            return d;
        }

        public static SvcRepository ToSvcRepository(this SvcRepositoryDto dto)
        {
            var repo = new SvcRepository
            {
                Name = dto.Name,
                Uri = dto.Uri,
                DefaultCommitMessagePrefix = dto.DefaultCommitMessagePrefix,
                Id = dto.Id,
                Type = dto.Type,
                CheckoutUser = dto.CheckoutUser.ToUser(),
                PushUser = dto.PushUser.ToUser(),
                MappedCheckoutCredentials = dto.MappedCheckoutCredentials?.ToCredentials()
            };

            return repo;
        }

        public static SvcRepositoryDto ToDto(this ISvcRepository repo)
        {
            var dto = new SvcRepositoryDto
            {
                CheckoutUser = repo.CheckoutUser.ToDto(),
                PushUser = repo.PushUser.ToDto(),
                Id = repo.Id,
                Type = repo.Type,
                Name = repo.Name,
                Uri = repo.Uri,
                DefaultCommitMessagePrefix = repo.DefaultCommitMessagePrefix,
                MappedCheckoutCredentials = repo.MappedCheckoutCredentials?.ToDto()
            };

            return dto;
        }

        public static User ToUser(this UserDto dto)
        {
            var user = new User { Id = dto.Id, Name = dto.Name, Email = dto.Email, RepositoryCredentials = dto.RepositoryCredentials?.Select(c => c.ToCredentials()) };

            return user;
        }

        public static UserDto ToDto(this IUser user)
        {
            var dto = new UserDto { Id = user.Id, Name = user.Name, Email = user.Email, RepositoryCredentials = user.RepositoryCredentials.Select(c => c.ToDto()) };

            return dto;
        }

        public static Credentials ToCredentials(this CredentialsDto dto)
        {
            var credentials = new Credentials
            {
                AllowedRepositories = dto.AllowedRepositories,
                Login = dto.Login,
                RepositoryType = dto.RepositoryType
            };

            return credentials;
        }

        public static CredentialsDto ToDto(this ICredentials credentials)
        {
            var dto = new CredentialsDto
            {
                AllowedRepositories = credentials.AllowedRepositories,
                Login = credentials.Login,
                RepositoryType = credentials.RepositoryType
            };
            return dto;
        }

        public static CredentialsWithPasswordDto ToDtoWithPassword(this ICredentials credentials)
        {
            var dto = new CredentialsWithPasswordDto
            {
                AllowedRepositories = credentials.AllowedRepositories,
                Login = credentials.Login,
                RepositoryType = credentials.RepositoryType,
                Password = ((CredentialsWithPassword)credentials).Password
            };
            return dto;
        }

        public static Revision ToRevision(this RevisionDto dto)
        {
            var rev = new Revision { Id = dto.Id, Message = dto.Message, Author = dto.Author };
            return rev;
        }

        public static RevisionDto ToDto(this IRevision rev)
        {
            var dto = new RevisionDto { Id = rev.Id, Message = rev.Message, Author = rev.Author };
            return dto;
        }

        public static Reflection ToReflection(this ReflectionDto dto)
        {
            var reflection = new Reflection
            {
                Id = dto.Id,
                Mirror = dto.Mirror.ToMirror(),
                DateTime = dto.DateTime,
                SourceRevision = dto.SourceRevision.ToRevision(),
                TargetRevision = dto.TargetRevision.ToRevision(),
                Synchronization = dto.Synchronization.ToSynchronization()
            };

            return reflection;
        }

        public static ReflectionDto ToDto(this IReflection reflection)
        {
            var dto = new ReflectionDto
            {
                Mirror = reflection.Mirror.ToDto(),
                Id = reflection.Id,
                DateTime = reflection.DateTime,
                TargetRevision = reflection.TargetRevision.ToDto(),
                SourceRevision = reflection.SourceRevision.ToDto(),
                Synchronization = reflection.Synchronization.ToDto(),
                MirrorId = reflection.Mirror.Id,
                SynchronizationId = reflection.Synchronization.Id
            };

            return dto;
        }

    }
}
