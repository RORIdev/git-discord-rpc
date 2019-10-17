using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordConfiguration
    {
        [JsonProperty("application_id")]
        public ulong ApplicationId { get; set; } = 421688819868237824UL;

        [JsonProperty("current_instance_id")]
        public int CurrentInstanceId { get; set; } = 0;

        [JsonProperty("display_timestamp")]
        public bool DisplayTimestamp { get; set; } = true;

        [JsonProperty("display_project")]
        public bool DisplayProject { get; set; } = false;

        [JsonProperty("display_solution")]
        public bool DisplaySolution { get; set; } = true;

        [JsonProperty("auto_reset_timestamp")]
        public bool AutoResetTimestamp { get; set; } = false;
    }
}
