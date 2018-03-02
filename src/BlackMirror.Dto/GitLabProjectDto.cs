namespace BlackMirror.Dto
{
    using Newtonsoft.Json;

    public class GitLabProjectDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("web_url")]
        public string WebUrl { get; set; }

        [JsonProperty("git_ssh_url")]
        public string GitSshUrl { get; set; }

        [JsonProperty("git_http_url")]
        public string GitHttpUrl { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("path_with_namespace")]
        public string PathWithNamespace { get; set; }

        [JsonProperty("default_branch")]
        public string DefaultBranch { get; set; }
    }
}