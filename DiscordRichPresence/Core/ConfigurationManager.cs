using System;
using System.IO;
using System.Threading.Tasks;
using DiscordRichPresence.Entities;
using Newtonsoft.Json;

namespace DiscordRichPresence.Core
{
    public class ConfigurationManager
    {
        public DiscordConfiguration Discord { get; private set; }
        public DiscordAssetsConfiguration Assets { get; private set; }

        public ConfigurationManager()
        {
            this.Discord = DiscordConfiguration.Empty;
            this.Assets = DiscordAssetsConfiguration.Empty;
        }

        public async Task InitializeAsync()
        {
            this.Discord = await this.GetOrCreateDefaultAsync("discord", DiscordConfiguration.Empty);
            this.Assets = await this.GetOrCreateDefaultAsync("assets", DiscordAssetsConfiguration.Empty);
        }

        async Task<T> GetOrCreateDefaultAsync<T>(string name, T @default)
        {
            var user_profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var file = new FileInfo(Path.Combine(user_profile, "Storm Development Software", "Discord Rich Presence", $"{name}.json"));

            var instance = @default;

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (!file.Exists)
            {
                using (var sw = file.CreateText())
                {
                    await sw.WriteLineAsync(JsonConvert.SerializeObject(instance, Formatting.Indented));
                    await sw.FlushAsync();
                }
            }
            else
            {
                using(var sr = file.OpenText())
                {
                    var text = await sr.ReadToEndAsync();

                    try
                    {
                        instance = JsonConvert.DeserializeObject<T>(text);
                    }
                    catch /* (Exception ex) */
                    {
                        return instance;
                    }
                }
            }

            return instance;
        }
    }
}
