namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    public class SizeLimitedLogFile
    {
        public readonly long SizeLimitBytes;
        public readonly FileNameComponents FileNameComponents;

        public SizeLimitedLogFile(FileNameComponents fileNameComponents, long sizeLimitBytes)
        {
            this.FileNameComponents = fileNameComponents;
            this.SizeLimitBytes = sizeLimitBytes;
        }

        public string FullName
        {
            get { return this.FileNameComponents.FullName; }
        }

        public string NameTemplate
        {
            get { return this.FileNameComponents.Name; }
        }
    }
}
