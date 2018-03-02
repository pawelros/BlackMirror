namespace BlackMirror.Dto
{
    using BlackMirror.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class UpdateStatusDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SynchronizationStatus? Status { get; set; }
    }
}
