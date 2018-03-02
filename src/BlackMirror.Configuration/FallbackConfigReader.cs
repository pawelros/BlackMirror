namespace BlackMirror.Configuration
{
    using System;
    using System.Collections;
    using BlackMirror.Configuration.SerilogSupport;

    public class FallbackConfigReader : IConfigReader
    {
        private readonly IConfigReader mainConfigReader;
        private readonly IConfigReader fallbackConfigReader;
        public string Name => this.GetType().Name;

        public FallbackConfigReader(IConfigReader mainReader, IConfigReader fallbackReader)
        {
            this.mainConfigReader = mainReader;
            this.fallbackConfigReader = fallbackReader;
        }

        public T Fetch<T>(string key)
        {
            try
            {
                var result = this.mainConfigReader.Fetch<T>(key);

                Logging.Log().Debug($"Reading configuration key '{key}' from {this.mainConfigReader.Name}.");

                string valueToLog = (result is IEnumerable) ? 
                    string.Join(",", result) 
                    : result.ToString();

                Logging.Log().Debug($"Configuration key '{key}' value: '{valueToLog}'.");

                return result;
            }
            catch (Exception ex)
            {
                Logging.Log().Warning($"Could not get configuration key '{key}' from {this.mainConfigReader.Name}. Exception: {ex}");

                T fallbackResult;

                try
                {
                    fallbackResult = this.fallbackConfigReader.Fetch<T>(key);
                }
                catch (Exception exx)
                {
                    throw new AggregateException(ex, exx);
                }

                Logging.Log().Debug($"Configuration key '{key}' value: '{fallbackResult}' from {this.fallbackConfigReader.Name}.");

                return fallbackResult;
            }
        }
    }
}
