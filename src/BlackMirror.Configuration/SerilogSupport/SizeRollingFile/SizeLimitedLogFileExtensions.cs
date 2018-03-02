namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    internal static class SizeLimitedLogFileExtensions
    {
        internal static SizeLimitedLogFile Next(this SizeLimitedLogFile previous)
        {
            var componentsIncremented = new FileNameComponents(previous.FileNameComponents.Name, previous.FileNameComponents.Sequence + 1, previous.FileNameComponents.Extension);
            return new SizeLimitedLogFile(componentsIncremented, previous.SizeLimitBytes);
        }
    }
}
