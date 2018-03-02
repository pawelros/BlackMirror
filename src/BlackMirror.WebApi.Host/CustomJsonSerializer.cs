namespace BlackMirror.WebApi.Host
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public sealed class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            this.Formatting = Formatting.Indented;
            this.MissingMemberHandling = MissingMemberHandling.Ignore;
            this.NullValueHandling = NullValueHandling.Ignore;
            this.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = false
            });
        }
    }
}