namespace BlackMirror.Interfaces
{
    using System;

    public interface ISynchronizationProcess
    {
        IMirror Mirror { get; }

        // TODO: change to status enum
        bool InProgress { get; }

        void Start();

        void Terminate();

        DateTime CreationTime { get; }
    }
}