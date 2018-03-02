namespace BlackMirror.Interfaces
{
    using System;

    public interface ISynchronizationLogEntry
    {
        DateTime Time { get; set; }

        string Entry { get; set; }
    }
}