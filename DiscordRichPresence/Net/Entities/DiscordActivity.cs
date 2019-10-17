using System;
using Newtonsoft.Json;

namespace DiscordRichPresence.Net.Entities
{
    public class DiscordActivity
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public ActivityType? Type { get; set; } = null;

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; } = null;

        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; } = null;

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; } = null;

        [JsonProperty("timestamps", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordActivityTimestamps Timestamps { get; set; } = null;

        [JsonProperty("assets", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordActivityAssets Assets { get; set; } = null;

        [JsonProperty("party", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordActivityParty Party { get; set; } = null;

        [JsonProperty("secrets", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordActivitySecrets Secrets { get; set; } = null;

        [JsonProperty("instance", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Instance { get; set; } = null;

        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public DiscordActivityFlags? Flags { get; set; } = null;
    }
}