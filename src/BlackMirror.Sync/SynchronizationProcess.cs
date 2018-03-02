namespace BlackMirror.Sync
{
    using System;
    using System.Threading;
    using BlackMirror.Configuration.SerilogSupport;
    using BlackMirror.Interfaces;
    public class SynchronizationProcess : ISynchronizationProcess
    {
        public SynchronizationProcess(IMirror mirror)
        {
            this.Mirror = mirror;
        }

        public IMirror Mirror { get; }

        public bool InProgress { get; private set; }

        public void Start()
        {
            this.InProgress = true;
            Logging.Log().Warning($"Starting mirror {this.Mirror.Id} synchronization.");
            Thread.Sleep(15000);
            Logging.Log().Warning($"Mirror {this.Mirror.Id} synchronization has finished.");
            this.InProgress = false;
        }

        public void Terminate()
        {
            this.InProgress = false;
        }

        public DateTime CreationTime { get; }
    }
}