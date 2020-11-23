using System;
using System.IO;
using System.Threading.Tasks;
using DiscordRichPresence.Entities;
using Newtonsoft.Json;

namespace DiscordRichPresence.Core
{
    public class ConfigurationProvider
    {
        public DiscordSettings Discord { get; private set; }
        public AssetSettings Assets { get; private set; }
        public LocalizationSettings Localization { get; private set; }

        public ConfigurationProvider()
        {
            this.Discord = DiscordSettings.Empty;
            this.Assets = AssetSettings.Empty;
            this.Localization = LocalizationSettings.Empty;
        }

        public async Task InitializeAsync()
        {
            this.Discord = await this.GetOrCreateDefaultAsync("discord", DiscordSettings.Empty);
            this.Assets = await this.GetOrCreateDefaultAsync("assets", AssetSettings.Empty);
            this.Localization = await this.GetOrCreateDefaultAsync("localization", LocalizationSettings.Empty);
        }

        async Task<TConfig> GetOrCreateDefaultAsync<TConfig>(string name, TConfig empty)
        {
            var userprofile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var file = new FileInfo(Path.Combine(userprofile, "Storm Development Software", "Discord Rich Presence", $"{name}.json"));

            TConfig instance = empty;
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
                using (var stream = file.Open(FileMode.Create))
                {
                    await stream.WriteAsync(data, 0, data.Length);
                    await stream.FlushAsync();
                }

                using (var ms = new MemoryStream(data))
                using (var sr = new StreamReader(ms))
                    instance = JsonConvert.DeserializeObject<TConfig>(await sr.ReadToEndAsync());
            }
            else
            {
                using (var sr = file.OpenText())
                {
                    var text = await sr.ReadToEndAsync();

                    try
                    {
                        instance = JsonConvert.DeserializeObject<TConfig>(text);
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
