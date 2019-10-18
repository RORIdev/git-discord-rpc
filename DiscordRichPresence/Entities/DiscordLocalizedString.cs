using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordLocalizedString
    {
        public static DiscordLocalizedString FromEmpty(string key)
        {
            return new DiscordLocalizedString
            {
                Key = key,
                Text = key
            };
        }

        [JsonProperty("text")]
        public string Key { get; set; }

        [JsonProperty("key")]
        public string Text { get; set; }

        public string Format(params object[] args)
        {
            try { return string.Format(this.Text, args); }
            catch { return this.Text; }
        }
    }
}