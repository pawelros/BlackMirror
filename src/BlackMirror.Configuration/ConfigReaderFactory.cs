namespace BlackMirror.Configuration
{
    using System;
    using LazyCache;

    public static class ConfigReaderFactory
    {
        public static IConfigReader Create(IAppCache lazyCache, string environment, string hostType)
        {

            IConfigReader fileReader = new FileConfigReader(environment);

            if (string.Equals(
                    environment,
                    "ACCEPTANCE",
                    StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(
                    environment,
                    "ACCEPTANCE-WEBSITE",
                    StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(
                    environment,
                    "DEV",
                    StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(
                    environment,
                    "UAT",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                var configReader = fileReader;
                var cachedConfigReader = new InMemoryCachedConfigReader(configReader, lazyCache);

                return cachedConfigReader;
            }


            return fileReader;
        }

    }
}