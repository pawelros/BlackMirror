namespace BlackMirror.Dto
{
    using Newtonsoft.Json;

    public class GitLabRepositoryDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("git_http_url")]
        public string GitHttpUrl { get; set; }

        [JsonProperty("git_ssh_url")]
        public string GitSshUrlme { get; set; }
    }
}