namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Serilog.Formatting;

    public class FileSinkFactory
    {
        private readonly ITextFormatter formatter;
        private readonly Encoding encoding;

        private readonly string filePathTemplate;
        private readonly string filePathDirectory;
        private readonly long fileSizeLimitBytes;

        public FileSinkFactory(ITextFormatter formatter, string filePathTemplate, long fileSizeLimitBytes, Encoding encoding)
        {
            this.formatter = formatter;
            this.encoding = encoding;

            this.filePathTemplate = filePathTemplate;
            this.filePathDirectory = Path.GetDirectoryName(filePathTemplate);
            this.fileSizeLimitBytes = fileSizeLimitBytes;
        }

        public SizeLimitedFileSink GetNextSink(SizeLimitedFileSink currentLatestSink)
        {
            if (currentLatestSink != null)
            {
                currentLatestSink.Dispose();
            }

            return this.GetNewSink();
        }

        public SizeLimitedFileSink GetNewSink()
        {
            IList<SizeLimitedLogFile> existingFiles = this.GetExistingFiles(this.filePathTemplate, this.fileSizeLimitBytes).ToList();
            SizeLimitedLogFile nextFile;

            if (existingFiles.Any())
            {
                var latest = existingFiles.OrderByDescending(x => x.FileNameComponents.Sequence).First();
                nextFile = latest.Next();
            }
            else
            {
                nextFile = FileSinkParser.ParseRollingLogfile(this.filePathTemplate, this.fileSizeLimitBytes);
            }

            return new SizeLimitedFileSink(this.formatter, this.filePathDirectory, nextFile, this.encoding);
        }

        public IEnumerable<SizeLimitedLogFile> GetObsoleteFileSinks(int retainedFileCountLimit)
        {
            return
                this.GetExistingFiles(this.filePathTemplate, this.fileSizeLimitBytes)
                    .OrderByDescending(x => x.FileNameComponents.Sequence)
                    .Skip(retainedFileCountLimit)
                    .ToArray();
        }

        private IEnumerable<SizeLimitedLogFile> GetExistingFiles(string filePathTemplate, long fileSizeLimitBytes)
        {
            var directoryName = Path.GetDirectoryName(filePathTemplate);
            if (string.IsNullOrEmpty(directoryName))
            {
#if ASPNETCORE50
                directory = Directory.GetCurrentDirectory();
#else
                directoryName = Environment.CurrentDirectory;
#endif
            }

            directoryName = Path.GetFullPath(directoryName);
            var filePathTemplateName = Path.GetFileNameWithoutExtension(filePathTemplate);

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            return
                Directory.GetFiles(directoryName)
                    .Select(x => FileSinkParser.ParseRollingLogfile(x, fileSizeLimitBytes))
                    .Where(rollingFile => rollingFile != null)
                    .Where(rollingFile => rollingFile.NameTemplate.Equals(filePathTemplateName));
        }
    }
}
