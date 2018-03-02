namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    public static class FileSinkParser
    {
        public static SizeLimitedLogFile ParseRollingLogfile(string path, long fileSizeLimitBytes)
        {
            var fileNameComponents = FileNameParser.ParseLogFileName(path);

            return new SizeLimitedLogFile(fileNameComponents, fileSizeLimitBytes);
        }
    }
}
