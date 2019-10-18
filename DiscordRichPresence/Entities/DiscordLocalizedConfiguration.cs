using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordLocalizedConfiguration
    {
        [JsonProperty("strings")]
        protected List<DiscordLocalizedString> Strings { get; set; } = new List<DiscordLocalizedString>();

        public bool FindString(string key, out DiscordLocalizedString value)
        {
            value = null;

            if (string.IsNullOrEmpty(key))
                return false;

            lock (this.Strings)
            {
                var index = this.Strings.FindIndex(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant());

                if (index == -1)
                    return false;

                value = this.Strings[index];
                return true;
            }
        }

        public bool Add(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                return false;

            lock (this.Strings)
            {
                var index = this.Strings.FindIndex(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant());

                if (index == -1)
                    this.Strings.Add(new DiscordLocalizedString { Key = key, Text = value });
                else
                {
                    this.Strings.RemoveAt(index);
                    this.Strings.Insert(index, new DiscordLocalizedString { Key = key, Text = value });
                }

                return true;
            }
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            lock (this.Strings)
            {
                var index = this.Strings.FindIndex(x => x.Key.ToLowerInvariant() == key.ToLowerInvariant());

                if (index == -1)
                    return false;

                this.Strings.RemoveAt(index);
                return true;
            }
        }
    }
}