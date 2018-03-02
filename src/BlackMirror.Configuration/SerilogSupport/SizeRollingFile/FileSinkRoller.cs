namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Serilog.Debugging;

    public static class FileSinkRoller
    {
        public static void RemoveObsoleteFileSinks(IEnumerable<SizeLimitedLogFile> filesToRemove, string folderPath)
        {
            foreach (var obsolete in filesToRemove)
            {
                var fullPath = Path.Combine(folderPath, obsolete.FullName);

                try
                {
                    File.Delete(fullPath);
                }
                catch (Exception ex)
                {
                    SelfLog.WriteLine("Error {0} while removing obsolete file {1}", ex, fullPath);
                }
            }
        }

    }
}
