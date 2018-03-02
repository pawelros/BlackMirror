namespace BlackMirror.Configuration.SerilogSupport
{
    using System.Runtime.CompilerServices;
    using Serilog;

    public static class Logging
    {
        public static ILogger Log([CallerFilePath] string callerFileName = "", [CallerMemberName] string callerName = "", [CallerLineNumber] int lineNumber = -1)
        {
            var substringStart = callerFileName.IndexOf("\\src\\", System.StringComparison.Ordinal);

            var l = Serilog.Log
                .ForContext("CallerFilePath", callerFileName.Substring(substringStart > 0 ? substringStart : 0))
                .ForContext("CallerMemberName", callerName)
                .ForContext("CallerLineNumber", lineNumber);

            return l;
        }

    }
}
