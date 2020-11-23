using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class LocalizationEntry
    {
        public LocalizationEntry()
        {

        }

        public LocalizationEntry(string key, string text)
        {
            this.Key = key;
            this.Text = text;
        }

        public static LocalizationEntry FromEmpty(string key)
        {
            return new LocalizationEntry
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