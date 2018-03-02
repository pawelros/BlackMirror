namespace BlackMirror.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ISynchronization
    {
        string Id { get; }

        IMirror Mirror { get; }

        DateTime CreationTime { get; }

        SynchronizationStatus Status { get; }
    }
}