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

        public static ConfigurationProvider Instance { get; private set; }

        public ConfigurationProvider()
        {
            Instance = this;
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

        public async Task SaveConfigurationsAsync()
        {
            await this.SaveConfigurationAsync(this.Discord, "discord");
            await this.SaveConfigurationAsync(this.Assets, "assets");
            await this.SaveConfigurationAsync(this.Localization, "localization");
        }

        async Task SaveConfigurationAsync<TConfig>(TConfig value, string name)
        {
            var file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Storm Development Software", "Discord Rich Presence", $"{name}.json"));

            if (!file.Directory.Exists)
                file.Directory.Create();

            using (var fs = file.Open(FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                await sw.WriteLineAsync(JsonConvert.SerializeObject(value, Formatting.Indented));
                await sw.FlushAsync();
            }
        }

        async Task<TConfig> GetOrCreateDefaultAsync<TConfig>(string name, TConfig empty)
        {
            var file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Storm Development Software", "Discord Rich Presence", $"{name}.json"));

            TConfig instance = empty;

            byte[] buffer;

            if (name == "discord")
                buffer = Properties.Resources.DefaultDiscordFile;
            else if (name == "assets")
                buffer = Properties.Resources.DefaultAssetsFile;
            else if (name == "localization")
                buffer = Properties.Resources.DefaultLocalizationFile;
            else
                return instance;

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (!file.Exists)
            {
                using (var stream = file.Open(FileMode.Create))
                {
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    await stream.FlushAsync();
                }

                using (var ms = new MemoryStream(buffer))
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
