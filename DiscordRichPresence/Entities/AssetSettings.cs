using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscordRichPresence.Entities
{
    public class AssetSettings
    {
        [JsonIgnore]
        public static readonly AssetSettings Empty = new AssetSettings();

        /// <summary>
        /// Lista com todos os assets carregados da configuração.
        /// </summary>
        [JsonProperty("assets")]
        protected List<Asset> Assets { get; set; } = new List<Asset>();

        /// <summary>
        /// Indica o asset padrão, quando houver um tipo de arquivo desconhecido.
        /// </summary>
        [JsonIgnore]
        public Asset DefaultAsset
        {
            get
            {
                lock (Assets)
                {
                    var index = Assets.FindIndex(x => x.IsDefault);

                    if (index == -1)
                        return null;

                    return Assets[index];
                }
            }
        }

        /// <summary>
        /// Procurar nos assets uma extensão suportada.
        /// </summary>
        /// <param name="extension">Extensão que será buscada.</param>
        /// <returns>Asset que corresponde a pesquisa; Nulo se não foi encontrado.</returns>
        public bool FindAsset(string extension, out Asset asset)
        {
            asset = null;

            lock (Assets)
            {
                var index = Assets.FindIndex(x => x.SupportExtension(extension));

                if (index == -1)
                    return false;

                asset = Assets[index];
                return true;
            }
        }

        /// <summary>
        /// Adiciona um asset a lista.
        /// </summary>
        /// <param name="asset">Asset que será adicionado.</param>
        public void Add(Asset asset)
        {
            if (asset == null)
                return;

            if (string.IsNullOrEmpty(asset.Key))
                return;

            lock (Assets)
            {
                var index = Assets.FindIndex(x => x.Key.Equals(asset.Key));

                if (index != -1)
                    Assets.RemoveAt(index);

                Assets.Add(asset);
            }
        }

        /// <summary>
        /// Remove um asset da lista.
        /// </summary>
        /// <param name="asset">Asset que será removido.</param>
        public void Remove(Asset asset)
        {
            if (asset == null)
                return;

            if (string.IsNullOrEmpty(asset.Key))
                return;

            lock (Assets)
            {
                var index = Assets.FindIndex(x => x.Key.Equals(asset.Key));

                if (index != -1)
                    return;

                Assets.RemoveAt(index);
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

            lock (Assets)
            {
                var index = Assets.FindIndex(x => x.Key.Equals(key));

                if (index != -1)
                    return;

                Assets.RemoveAt(index);
            }
        }
    }
}
