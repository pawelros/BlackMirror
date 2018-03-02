namespace BlackMirror.Models
{
    using System;
    using BlackMirror.Interfaces;

    public class SynchronizationLogEntry : ISynchronizationLogEntry
    {
        public DateTime Time { get; set; }

        public string Entry { get; set; }
    }
}