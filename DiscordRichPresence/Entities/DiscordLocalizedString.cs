using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordLocalizedString
    {
        public DiscordLocalizedString()
        {

        }

        public DiscordLocalizedString(string key, string text)
        {
            this.Key = key;
            this.Text = text;
        }

        public static DiscordLocalizedString FromEmpty(string key)
        {
            return new DiscordLocalizedString
            {
                Key = key,
                Text = key
            };
        }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        public string Format(params object[] args)
        {
            try { return string.Format(this.Text, args); }
            catch { return this.Text; }
        }
    }
}