namespace BlackMirror.Models
{
    using System;
    using System.Collections.Generic;
    using BlackMirror.Interfaces;

    public class Synchronization : ISynchronization, IModel
    {
        public string Id { get; set; }

        public IMirror Mirror { get; set; }

        public DateTime CreationTime { get; set; }

        public SynchronizationStatus Status { get; set; }
    }
}