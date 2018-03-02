namespace BlackMirror.Dto
{
    using Newtonsoft.Json;

    public class GitLabPushEventDto
    {
        [JsonProperty("event_name")]
        public string EventName { get; set; }
        [JsonProperty("before")]
        public string Before { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("checkout_sha")]
        public string CheckoutSha { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_username")]
        public string UserUsername { get; set; }

        [JsonProperty("user_email")]
        public string UserEmail { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("project")]
        public GitLabProjectDto Project { get; set; }

        [JsonProperty("repository")]
        public GitLabRepositoryDto Repository { get; set; }
    }
}