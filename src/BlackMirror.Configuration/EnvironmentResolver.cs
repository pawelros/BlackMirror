namespace BlackMirror.Configuration
{
    using System;
    using BlackMirror.Configuration.SerilogSupport;

    public static class EnvironmentResolver
    {
        public static string GetEnvironmentName(string customEnvironmentName = null)
        {
            if (!string.IsNullOrWhiteSpace(customEnvironmentName))
            {
                Logging.Log().Warning($"Custom environment parameter has been provided. Using {customEnvironmentName} as 'ENVIRONMENT'");
                return customEnvironmentName;
            }

            var env = Environment.GetEnvironmentVariable("ENVIRONMENT");

            if (env == null)
            {
                Logging.Log().Warning("Environment variable 'ENVIRONMENT' is not set. Using default 'DEV' environment.");
                return "DEV";
            }

            Logging.Log().Information("Environment variable 'ENVIRONMENT' is set to {0}", env);

            return env;
        }
    }
}