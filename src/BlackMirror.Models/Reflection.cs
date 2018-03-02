namespace BlackMirror.Models
{
    using System;
    using BlackMirror.Interfaces;
    public class Reflection : IReflection, IModel
    {
        public DateTime DateTime { get; set; }

        public IMirror Mirror { get; set; }

        public ISynchronization Synchronization { get; set; }

        public IRevision SourceRevision { get; set; }

        public IRevision TargetRevision { get; set; }

        public string Id { get; set; }
    }
}