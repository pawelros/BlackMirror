namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    using System;
    using System.IO;
    using System.Text;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;

    public class SizeLimitedFileSink : ILogEventSink, IDisposable
    {
        private static readonly string ThisObjectName = typeof(SizeLimitedFileSink).Name;

        private readonly ITextFormatter formatter;
        private readonly SizeLimitedLogFile sizeLimitedLogFile;
        private readonly StreamWriter output;
        private readonly object syncRoot = new object();
        private bool disposed = false;
        private bool sizeLimitReached = false;

        public SizeLimitedFileSink(ITextFormatter formatter, string folderPath, SizeLimitedLogFile sizeLimitedLogFile, Encoding encoding = null)
        {
            this.formatter = formatter;
            this.sizeLimitedLogFile = sizeLimitedLogFile;
            this.output = this.OpenFileForWriting(folderPath, sizeLimitedLogFile, encoding ?? Encoding.UTF8);
        }

        internal SizeLimitedFileSink(ITextFormatter formatter, SizeLimitedLogFile sizeLimitedLogFile, StreamWriter writer)
        {
            this.formatter = formatter;
            this.sizeLimitedLogFile = sizeLimitedLogFile;
            this.output = writer;
        }

        internal bool SizeLimitReached
        {
            get { return this.sizeLimitReached; }
        }

        internal SizeLimitedLogFile LogFile
        {
            get { return this.sizeLimitedLogFile; }
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.output.Dispose();
                this.disposed = true;
            }
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }

            lock (this.syncRoot)
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException(ThisObjectName, "Cannot write to disposed file");
                }

                if (this.output == null)
                {
                    return;
                }

                this.formatter.Format(logEvent, this.output);
                this.output.Flush();

                if (this.output.BaseStream.Length > this.sizeLimitedLogFile.SizeLimitBytes)
                {
                    this.sizeLimitReached = true;
                }
            }
        }

        private static void EnsureDirectoryCreated(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private StreamWriter OpenFileForWriting(string folderPath, SizeLimitedLogFile sizeLimitedLogFile, Encoding encoding)
        {
            EnsureDirectoryCreated(folderPath);
            var fullPath = Path.Combine(folderPath, sizeLimitedLogFile.FullName);
            var stream = File.Open(fullPath, FileMode.Append, FileAccess.Write, FileShare.Read);
            return new StreamWriter(stream, encoding ?? Encoding.UTF8);
        }
    }
}
