namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    using System;
    using System.IO;
    using System.Text;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;

    public sealed class SizeRollingFileSink : ILogEventSink, IDisposable
    {
        private static readonly string ThisObjectName = typeof(SizeLimitedFileSink).Name;
        private readonly object syncRoot = new object();

        private readonly string filePathTemplate;
        private readonly string filePathDirectory;
        private readonly long fileSizeLimitBytes;
        private readonly int retainedFileCountLimit;

        private readonly FileSinkFactory fileSinkFactory;
        private SizeLimitedFileSink currentSink;

        private bool disposed = false;

        public SizeRollingFileSink(
                                string filePathTemplate,
                                ITextFormatter formatter,
                                long fileSizeLimitBytes,
                                int retainedFileCountLimit,
                                Encoding encoding = null)
        {
            this.filePathTemplate = filePathTemplate;
            this.filePathDirectory = Path.GetDirectoryName(filePathTemplate);
            this.fileSizeLimitBytes = fileSizeLimitBytes;
            this.retainedFileCountLimit = retainedFileCountLimit;

            this.fileSinkFactory = new FileSinkFactory(formatter, this.filePathTemplate, this.fileSizeLimitBytes, encoding);

            this.currentSink = this.fileSinkFactory.GetNewSink();
            FileSinkRoller.RemoveObsoleteFileSinks(this.fileSinkFactory.GetObsoleteFileSinks(this.retainedFileCountLimit), this.filePathDirectory);
        }

        public void Dispose()
        {
            lock (this.syncRoot)
            {
                if (!this.disposed && this.currentSink != null)
                {
                    this.currentSink.Dispose();
                    this.currentSink = null;
                    this.disposed = true;
                }
            }
        }

        public void Emit(LogEvent logEvent)
        {
            this.ValidateSinkAndLogState(logEvent);

            lock (this.syncRoot)
            {
                if (this.currentSink.SizeLimitReached)
                {
                    this.currentSink = this.fileSinkFactory.GetNextSink(this.currentSink);
                    FileSinkRoller.RemoveObsoleteFileSinks(this.fileSinkFactory.GetObsoleteFileSinks(this.retainedFileCountLimit), this.filePathDirectory);
                }

                if (this.currentSink != null)
                {
                    this.currentSink.Emit(logEvent);
                }
            }
        }

        private void ValidateSinkAndLogState(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }

            if (this.disposed)
            {
                throw new ObjectDisposedException(ThisObjectName, "The rolling file sink has been disposed");
            }
        }
    }
}
