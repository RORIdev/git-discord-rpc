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
        public DiscordLocalizedConfiguration Localization { get; private set; }

        public ConfigurationManager()
        {
            this.Discord = DiscordConfiguration.Empty;
            this.Assets = DiscordAssetsConfiguration.Empty;
            this.Localization = DiscordLocalizedConfiguration.Empty;
        }

        public async Task InitializeAsync()
        {
            this.Discord = await this.GetOrCreateDefaultAsync("discord", DiscordConfiguration.Empty);
            this.Assets = await this.GetOrCreateDefaultAsync("assets", DiscordAssetsConfiguration.Empty);
            this.Localization = await this.GetOrCreateDefaultAsync("localization", DiscordLocalizedConfiguration.Empty);
        }

        async Task<T> GetOrCreateDefaultAsync<T>(string name, T @default)
        {
            var user_profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var file = new FileInfo(Path.Combine(user_profile, "Storm Development Software", "Discord Rich Presence", $"{name}.json"));

            var instance = @default;
            var data = default(byte[]);

            if (name == "discord")
                data = Properties.Resources.DefaultDiscordFile;
            else if (name == "assets")
                data = Properties.Resources.DefaultAssetsFile;
            else if (name == "localization")
                data = Properties.Resources.DefaultLocalizationFile;
            else
                return instance;

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (!file.Exists)
            {
                using (var sw = file.Create())
                {
                    await sw.WriteAsync(data, 0, data.Length);
                    await sw.FlushAsync();
                }

                using (var ms = new MemoryStream(data))
                using (var sr = new StreamReader(ms))
                    instance = JsonConvert.DeserializeObject<T>(await sr.ReadToEndAsync());
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
