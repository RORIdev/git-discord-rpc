using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using DiscordRichPresence.Core;
using DiscordRichPresence.Entities;
using DiscordRichPresence.Pipe;
using DiscordRichPresence.Pipe.Entities;
using DiscordRichPresence.Pipe.EventArgs;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ConfigurationProvider = DiscordRichPresence.Core.ConfigurationProvider;
using Task = System.Threading.Tasks.Task;

namespace DiscordRichPresence
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(UIContextGuids.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid("a266a262-709b-4be0-a2f9-8587c845f573")]
    [ProvideOptionPage(typeof(SettingsControlProvider), "Discord", "Discord", 0, 0, true)]
    public sealed class DiscordPackage : AsyncPackage
    {
        public ConfigurationProvider Configuration { get; }
        public DiscordPipeClient Client { get; private set; }
        public DiscordActivity LastActivity { get; private set; }
        public DTE DTE { get; private set; }
        public Events Events { get; private set; }

        public DiscordPackage()
        {
            Configuration = new ConfigurationProvider();
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await Configuration.InitializeAsync();
            await SetupPipeAsync();
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            DTE = (await GetServiceAsync(typeof(DTE)).ConfigureAwait(false)) as DTE;

            if (DTE != null)
            {
                Events = DTE.Events;
                Events.WindowEvents.WindowActivated += HandleWindowActivated;
            }
        }

        async Task HandleReadyAsync(ReadyEventArgs e)
        {
            if (LastActivity == null)
            {
                LastActivity = new DiscordActivity
                {
                    Assets = new DiscordActivityAssets
                    {
                        LargeImage = "visualstudio_small",
                        LargeText = "Visual Studio"
                    },
                    Timestamps = new DiscordActivityTimestamps
                    {
                        StartTime = DateTimeOffset.Now
                    }
                };

                if (Configuration.Discord.DisplayTimestamp)
                    LastActivity.Timestamps = new DiscordActivityTimestamps { StartTime = DateTimeOffset.Now };
            }

            await Client.SetActivityAsync(LastActivity);
        }

        async Task HandleDisconnectedAsync()
        {
            await Task.Delay(5000);
            await SetupPipeAsync();
        }

        async Task SetupPipeAsync()
        {
            if (Client != null)
            {
                await Client.DisconnectAsync();

                Client.Ready -= HandleReadyAsync;
                Client.Disconnected -= HandleDisconnectedAsync;
                Client = null;
            }

            Client = new DiscordPipeClient(Configuration.Discord.PipeInstanceId,
                    Configuration.Discord.ApplicationId);

            Client.Ready += HandleReadyAsync;
            Client.Disconnected += HandleDisconnectedAsync;

            await Client.ConnectAsync();
        }

        DateTimeOffset OriginalStartTime = DateTimeOffset.MinValue;

        void HandleWindowActivated(Window window, Window old)
        {
            if (this.OriginalStartTime == DateTimeOffset.MinValue)
                this.OriginalStartTime = DateTimeOffset.Now;

            _ = Task.Run(async () =>
            {
                try
                {
                    var i11n = Configuration.Localization.GetFormatDelegate();

                    if (LastActivity.Assets == null)
                        LastActivity.Assets = new DiscordActivityAssets();

                    if (window?.Project == null && old?.Project == null)
                        return;

                    if (window?.Project == null && old?.Project != null)
                        window = old;

                    if (!Configuration.Discord.DisplayTimestamp) {
                        LastActivity.Timestamps = null;
                    } else {
                        if (LastActivity.Timestamps == null)
                            LastActivity.Timestamps = new DiscordActivityTimestamps { StartTime = OriginalStartTime };

                        if (Configuration.Discord.AutoResetTimestamp)
                            LastActivity.Timestamps.StartTime = DateTimeOffset.Now;
                    }

                    if (!Configuration.Discord.DisplayProject)
                        LastActivity.State = null;
                    else
                    {
                        if (TryGetProjectName(window, out var project_name))
                            LastActivity.State = i11n("base_working_text", project_name);
                        else
                            LastActivity.State = null;
                    }

                    if (!Configuration.Discord.DisplaySolution)
                    {
                        LastActivity.Assets.SmallImage = null;
                        LastActivity.Assets.SmallText = null;
                    }
                    else
                    {
                        if (DTE.Solution != null)
                        {
                            if (!string.IsNullOrEmpty(DTE.Solution.FullName))
                            {
                                try
                                {
                                    var file = new FileInfo(DTE.Solution.FullName);
                                    LastActivity.Assets.SmallText = i11n("base_solution_text", Path.GetFileNameWithoutExtension(file.Name));
                                    LastActivity.Assets.SmallImage = "visualstudio_small";
                                }
                                catch
                                {
                                    LastActivity.Assets.SmallImage = null;
                                    LastActivity.Assets.SmallText = null;
                                }
                            }
                        }
                    }

                    if (TryGetFileName(window, out var file_name, out var ext))
                    {
                        LastActivity.Details = i11n("base_editing_text", file_name);

                        if (Configuration.Assets.FindAsset(ext, out var asset))
                        {
                            var text = string.Empty;
                            var localized_key = asset.Text.StartsWith("@") ? asset.Text.Substring(1) : null;

                            if (!string.IsNullOrEmpty(localized_key))
                                text = i11n(localized_key);
                            else
                                text = asset.Text;

                            LastActivity.Assets.LargeImage = asset.Key;
                            LastActivity.Assets.LargeText = text;
                        }
                        else
                        {
                            asset = Configuration.Assets.DefaultAsset;

                            if (asset == null)
                            {
                                LastActivity.Assets.LargeImage = "visualstudio_small";
                                LastActivity.Assets.LargeText = "Visual Studio";
                            }
                            else
                            {
                                LastActivity.Assets.LargeImage = asset.Key;

                                if (asset.Key == "default_file")
                                    LastActivity.Assets.LargeText = i11n("base_unknown_file_text");
                                else
                                    LastActivity.Assets.LargeText = asset.Text;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(LastActivity.Assets.LargeImage)
                        && string.IsNullOrEmpty(LastActivity.Assets.LargeText)
                        && string.IsNullOrEmpty(LastActivity.Assets.SmallImage)
                        && string.IsNullOrEmpty(LastActivity.Assets.SmallText))
                        LastActivity.Assets = null;

                    await Client.SetActivityAsync(LastActivity).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("\n\nSET ACTIVITY ERROR: {0}\n\n", args: ex);
                }
            });
        }

        DiscordActivity GetNextActivity(Window window , GetStringFormatDelegate i11n) {

            var activity = new DiscordActivity();

            var hasProject = TryGetProjectName(window, out var project);
            var hasFile = TryGetFileName(window, out var file, out var extension);

            if (hasProject) {
                activity.State = i11n("base_working_text", project);
            }

            if (hasFile) {
                LastActivity.Details = i11n("base_editing_text", file);
                var asset = GetAssetFromFileExtension(extension);
                activity.Assets.LargeImage = asset.Key;
                activity.Assets.LargeText = GetAssetName(asset, i11n);
            }

            return activity;
        }

        Asset GetAssetFromFileExtension(string extension) {
            var assetFound = Configuration.Assets.FindAsset(extension, out var asset);

            if (assetFound) {
                return asset;
            }

            return Configuration.Assets.DefaultAsset;
        }

        string GetAssetName(Asset asset, GetStringFormatDelegate i11n) {
            if (IsLocalizedAsset(asset)) {
                return i11n(asset.Text.Substring(1));
            }

            return asset.Text;
        }

        bool IsLocalizedAsset(Asset asset)
            => asset.Text.StartsWith("@");

        bool TryGetProjectName(Window window, out string name)
        {
            name = null;

            if (window == null)
                return false;

            if (window.Project == null)
                return false;

            try
            {
                var file = new FileInfo(window.Project.FullName);
                name = Path.GetFileNameWithoutExtension(file.Name);
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool TryGetFileName(Window window, out string name, out string extension)
        {
            name = null;
            extension = null;

            var doc = window.Document;

            if (doc == null)
                return false;

            if (string.IsNullOrEmpty(doc.FullName))
                return false;

            var file = new FileInfo(doc.FullName);
            name = file.Name;
            extension = Path.GetExtension(file.FullName);

            if (extension.StartsWith("."))
                extension = extension.Substring(1);

            return true;
        }
    }
}