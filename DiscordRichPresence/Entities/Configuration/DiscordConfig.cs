using Newtonsoft.Json;

namespace DiscordRichPresence.Entities.Configuration
{
    public class DiscordConfig
    {
        public static readonly DiscordConfig Default = new DiscordConfig();

        [JsonProperty("application_id")]
        public long ApplicationId { get; set; } = 421688819868237824;

        [JsonProperty("discord_instance_id")]
        public byte DiscordInstanceId { get; private set; } = 0;

        [JsonProperty("auto_reset_timestamp")]
        public bool AutoResetTimestamp { get; private set; } = false;

        [JsonProperty("display_timestamp")]
        public bool DisplayTimestamp { get; private set; } = true;

        [JsonProperty("display_file_name")]
        public bool DisplayFileName { get; private set; } = true;
    }
}
