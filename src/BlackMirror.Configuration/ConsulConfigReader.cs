namespace BlackMirror.Configuration
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Consul;
    using Newtonsoft.Json;

    public class ConsulConfigReader : IConfigReader
    {
        private readonly string environment;
        private readonly string host;

        private string specificConfig => this.environment;

        public string Name => this.GetType().Name;

        private readonly ConsulClient client;

        public ConsulConfigReader(Uri uri, string token, string env, string host)
        {
            this.client = new ConsulClient(configuration =>
            {
                configuration.Address = uri;
                configuration.Token = token;
            });

            this.environment = env;
            this.host = host;
        }

        public T Fetch<T>(string key)
        {
            return this.GetSettingAsync<T>(key).Result;
        }
        
        private async Task<T> GetSettingAsync<T>(string key)
        {
            key = this.UriBuilder(key);
           var result = new QueryResult<KVPair>();

            try
            {
                result = await this.client.KV.Get(key);

                if (string.IsNullOrWhiteSpace(result.Response.Key))
                {
                    throw new Exception(String.Format("Cannot get value from Consul for key {0}", key));
                }
            }
            catch (Exception e)
            {
                throw new Exception(
                String.Format("Unknow error during get key/value from Consul. Message: {0}", e.Message));
            }

            string resultString = Encoding.UTF8.GetString(result.Response.Value);

            T obj = JsonConvert.DeserializeObject<T>(resultString);

            return obj;
        }
        
        private string UriBuilder(string key)
        {
            return "BlackMirror/" + this.host + "/" + this.environment + "/" + this.specificConfig + "/" + key;
        }
    }
}
