namespace BlackMirror.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using BlackMirror.Configuration.SerilogSupport;
    using Spg.Configuration;

    public class FileConfigReader : IConfigReader
    {
        private readonly Spg.Configuration.IConfigurationProvider configProvider;

        public FileConfigReader(string environment)
        {
            this.configProvider = new ConfigurationProvider().Bootstrap(environment);
        }
        public string Name => this.GetType().Name;

        public T Fetch<T>(string key)
        {
            Type t = typeof(T);

            Type[] supportedTypes = {
                                        typeof(string),
                                        typeof(int),
                                        typeof(double),
                                        typeof(float),
                                        typeof(decimal),
                                        typeof(long),
                                        typeof(short),
                                        typeof(bool),
                                        typeof(DateTime),
                                        typeof(string[])
                                    };

            if (!supportedTypes.Contains(t))
            {
                throw new NotSupportedException($"File config reader supports only string, numeric, DateTime and boolean values; But not supports: {t.ToString()}");
            }

            string appsetting = this.configProvider.GetAppSetting(key);

            if (appsetting == null)
            {
                Logging.Log().Information($"Config key {key} not found in app settings. Trying to find this key in connection strings.");
                var cn = this.FetchConnectionString(key);

                return (T)Convert.ChangeType(cn, typeof(T));
            }

            if (t == typeof(string[]))
            {
                string[] array = appsetting.Split(';', ',');
                return (T)Convert.ChangeType(array, typeof(T));
            }

            return (T)Convert.ChangeType(appsetting, typeof(T));
        }


        private string FetchConnectionString(string key)
        {
            ConnectionStringSettings connectionString;
            try
            {
                connectionString = this.configProvider.GetConnectionString(key);
            }
            catch (NullReferenceException nex)
            {
                Logging.Log().Error($"Config key {key} not found in connection strings section neither.");
                throw new KeyNotFoundException($"Key {key} could not be found. There is no such key in app settings and connection strings.", nex);
            }

            return connectionString.ConnectionString;
        }
    }
}
