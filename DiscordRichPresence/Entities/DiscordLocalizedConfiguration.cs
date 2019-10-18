using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public delegate string GetStringFormatDelegate(string key, params object[] args);

    public class DiscordLocalizedConfiguration
    {
        [JsonIgnore]
        public static readonly DiscordLocalizedConfiguration Empty = new DiscordLocalizedConfiguration
        {
            Strings = new List<DiscordLocalizedString>
            {
                new DiscordLocalizedString("base_working_text", "Working On: {0}"),
                new DiscordLocalizedString("base_solution_text", "Solution: {0}"),
                new DiscordLocalizedString("base_editing_text", "Editing: {0}"),
                new DiscordLocalizedString("base_unknown_file_text", "Unknown File")
            }
        };

        [JsonProperty("strings")]
        protected List<DiscordLocalizedString> Strings { get; set; } = new List<DiscordLocalizedString>();

        public GetStringFormatDelegate GetFormatDelegate()
        {
            return new GetStringFormatDelegate((key, args) =>
            {
                if (!this.FindString(key, out var str))
                    return key;

                return str.Format(args);
            });
        }

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