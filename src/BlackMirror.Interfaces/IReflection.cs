namespace BlackMirror.Interfaces
{
    using System;

    public interface IReflection : IModel
    {
        DateTime DateTime { get; }
        // For better DB performance
        IMirror Mirror { get; }
        ISynchronization Synchronization { get; }
        IRevision SourceRevision { get; }
        IRevision TargetRevision { get; }
    }
}