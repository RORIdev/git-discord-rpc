using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class DiscordAssetsConfiguration
    {
        [JsonIgnore]
        public static readonly DiscordAssetsConfiguration Empty = new DiscordAssetsConfiguration();

        /// <summary>
        /// Lista com todos os assets carregados da configuração.
        /// </summary>
        [JsonProperty("assets")]
        protected List<DiscordAsset> Assets { get; set; } = new List<DiscordAsset>();

        /// <summary>
        /// Indica o asset padrão, quando houver um tipo de arquivo desconhecido.
        /// </summary>
        [JsonIgnore]
        public DiscordAsset DefaultAsset
        {
            get
            {
                lock (this.Assets)
                {
                    var index = this.Assets.FindIndex(x => x.IsDefault);

                    if (index == -1)
                        return null;

                    return this.Assets[index];
                }
            }
        }

        /// <summary>
        /// Adiciona um asset a lista.
        /// </summary>
        /// <param name="asset">Asset que será adicionado.</param>
        public void Add(DiscordAsset asset)
        {
            if (asset == null)
                return;

            if (string.IsNullOrEmpty(asset.Key))
                return;

            lock(this.Assets)
            {
                var index = this.Assets.FindIndex(x => x.Key.Equals(asset.Key));

                if (index != -1)
                    this.Assets.RemoveAt(index);

                this.Assets.Add(asset);
            }
        }

        /// <summary>
        /// Remove um asset da lista.
        /// </summary>
        /// <param name="asset">Asset que será removido.</param>
        public void Remove(DiscordAsset asset)
        {
            if (asset == null)
                return;

            if (string.IsNullOrEmpty(asset.Key))
                return;

            lock (this.Assets)
            {
                var index = this.Assets.FindIndex(x => x.Key.Equals(asset.Key));

                if (index != -1)
                    return;

                this.Assets.RemoveAt(index);
            }
        }

        /// <summary>
        /// Remove um asset da lista pela chave dele.
        /// </summary>
        /// <param name="key">Chave que será usada na busca.</param>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return;

            lock (this.Assets)
            {
                var index = this.Assets.FindIndex(x => x.Key.Equals(key));

                if (index != -1)
                    return;

                this.Assets.RemoveAt(index);
            }
        }
    }
}
