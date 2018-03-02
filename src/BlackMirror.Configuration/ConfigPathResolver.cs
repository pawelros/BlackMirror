namespace BlackMirror.Configuration
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    // (Pawel): don't make this class static!
    // unless you change powershell scripts
    public class ConfigPathResolver
    {
        public Uri Resolve(string environment, string customExecutionPath = null)
        {
            if (string.IsNullOrWhiteSpace(environment))
            {
                throw new ArgumentNullException(environment, "'environment' parameter cannot be null!");
            }

            const string EnvironmentBasedConfigPath = "{0}/Configuration/{1}/{1}.config";

            var executionPath = customExecutionPath ?? GetExecutionPath();

            var environmentBasedConfig = new Uri(string.Format(CultureInfo.InvariantCulture, EnvironmentBasedConfigPath, executionPath, environment));

            DirectoryInfo environmentBasedConfigDirectory = new FileInfo(environmentBasedConfig.AbsolutePath).Directory;

            var machineSpecificFiles = environmentBasedConfigDirectory.GetFiles("*" + Environment.MachineName + "*.*", SearchOption.AllDirectories);

            if (machineSpecificFiles.Any())
            {
                return new Uri(machineSpecificFiles.First().FullName);
            }
            else
            {
                return environmentBasedConfig;
            }
        }

        private static string GetExecutionPath()
        {
            return Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetName().CodeBase);
        }
    }
}
