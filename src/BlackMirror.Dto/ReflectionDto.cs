namespace BlackMirror.Dto
{
    using System;
    using BlackMirror.Interfaces;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class ReflectionDto : IDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        public DateTime DateTime { get; set; }

        [BsonIgnore]
        public MirrorDto Mirror { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string MirrorId { get; set; }

        [BsonIgnore]
        public SynchronizationDto Synchronization { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SynchronizationId { get; set; }

        public RevisionDto SourceRevision { get; set; }

        public RevisionDto TargetRevision { get; set; }
    }
}