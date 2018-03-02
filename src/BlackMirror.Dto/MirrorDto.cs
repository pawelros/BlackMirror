namespace BlackMirror.Dto
{
    using System;
    using BlackMirror.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class MirrorDto : IDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        public string Name { get; set; }

        [BsonIgnore]
        public SvcRepositoryDto SourceRepository { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SourceRepositoryId { get; set; }

        [BsonIgnore]
        public SvcRepositoryDto TargetRepository { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string TargetRepositoryRefSpec { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TargetRepositoryId { get; set; }

        [BsonIgnore]
        public UserDto Owner { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string OwnerId { get; set; }

        public DateTime CreationTime
        {
            get => this.creationTime;
            set => this.creationTime = value == DateTime.MinValue ? DateTime.Now : value;
        }

        private DateTime creationTime;

        public DateTime? LastSynced { get; set; }
    }
}
