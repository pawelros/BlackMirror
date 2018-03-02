namespace BlackMirror.Configuration
{
    using BlackMirror.Configuration.SerilogSupport;
    using LazyCache;

    public class InMemoryCachedConfigReader : IConfigReader
    {
        private readonly IConfigReader reader;
        private readonly IAppCache lazyCache;

        public string Name => this.GetType().Name;

        public InMemoryCachedConfigReader(IConfigReader reader, IAppCache lazyCache)
        {
            this.reader = reader;
            this.lazyCache = lazyCache;
        }

        public T Fetch<T>(string key)
        {
            Logging.Log().Debug($"Fetching config entry '{key}' from cache.");

            T result = this.lazyCache.GetOrAdd(key,
                () =>
                    {
                        Logging.Log().Debug($"Config entry '{key}' does not exist in cache. It will be added now.");

                        return this.reader.Fetch<T>(key);
                    });

            return result;
        }
    }
}