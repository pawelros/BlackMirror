namespace TradeR.Web.Extensions
{
    using CommandLine;

    public class StandardCommandLineOptions
    {
        [Option(
            "config",
            Required = false,
            DefaultValue = null,
            HelpText = "Remote config path that will be used.")]
        public string RemoteConfigPath { get; set; }

        public static StandardCommandLineOptions FromArguments(string[] arguments)
        {
            var parsedArguments = new StandardCommandLineOptions();
            if (Parser.Default.ParseArguments(arguments, parsedArguments))
            {
                return parsedArguments;
            }

            return new StandardCommandLineOptions();
        }
    }
}
