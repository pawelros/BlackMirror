namespace BlackMirror.Configuration.SerilogSupport.SizeRollingFile
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    internal static class FileNameParser
    {
        private const string CannotParseLogFileForFilenameExceptionFormat = "Cannot parse log file format for fileName: {0}";
        private const string CannotFindFilenameInPathExceptionFormat = "Cannot find filename in path: {0}";
        private static Regex logFileFormat = new Regex(@"(?<name>\S+)-(?<digits>\d+)|(?<name>\S+)");

        internal static FileNameComponents ParseLogFileName(string logFilePath)
        {
            var name = Path.GetFileNameWithoutExtension(logFilePath);
            if (name == null)
            {
                throw new Exception(string.Format(CannotFindFilenameInPathExceptionFormat, logFilePath));
            }

            uint sequence = 0;
            var matches = logFileFormat.Match(name);
            if (!matches.Success)
            {
                throw new Exception(string.Format(CannotParseLogFileForFilenameExceptionFormat, name));
            }

            var nameMatch = matches.Groups["name"];
            if (nameMatch.Success)
            {
                name = nameMatch.Value;
            }

            var digitMatch = matches.Groups["digits"];
            if (digitMatch.Success)
            {
                var sequenceMatch = digitMatch.Value;
                sequence = uint.Parse(sequenceMatch);
            }

            var extension = (Path.GetExtension(logFilePath) ?? string.Empty).TrimStart('.');
            return new FileNameComponents(name, sequence, extension);
        }
    }
}
