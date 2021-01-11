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
        public DiscordActivity InitialActivity { get; private set; }

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
            if (InitialActivity == null)
            {
                InitialActivity = new DiscordActivity
                {
                    Assets = new DiscordActivityAssets
                    {
                        LargeImage = "gitrpc",
                        LargeText = "roridev",
                        SmallImage = "git",
                        SmallText = "narcisism, yay!"
                    },
                    State = "Loading Visual Studio",
                    Details = "Extension"
                };

                if (Configuration.Discord.DisplayTimestamp)
                    InitialActivity.Timestamps = new DiscordActivityTimestamps { StartTime = DateTimeOffset.Now };
            }

            await Client.SetActivityAsync(InitialActivity);
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

                    if (window?.Project == null && old?.Project == null)
                        return;

                    if (window?.Project == null && old?.Project != null)
                        window = old;

                    var activity = GetNextActivity(window, i11n);

                    await Client.SetActivityAsync(activity).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("\n\nSET ACTIVITY ERROR: {0}\n\n", args: ex);
                }
            });
        }

        DiscordActivity GetNextActivity(Window window , GetStringFormatDelegate i11n) {

            var activity = new DiscordActivity {
                Timestamps = new DiscordActivityTimestamps(),
                Assets = new DiscordActivityAssets()
            };

            var hasProject = TryGetProjectName(window, out var project);
            var hasFile = TryGetFileName(window, out var file, out var extension);
            var hasSolution = TryGetSolutionName(out var solution);

            if (hasProject && Configuration.Discord.DisplayProject) {
                activity.State = i11n("base_working_text", project);
            }

            if (Configuration.Discord.AutoResetTimestamp) {
                OriginalStartTime = DateTimeOffset.Now;
            }

            if (Configuration.Discord.DisplayTimestamp) {
                activity.Timestamps.StartTime = OriginalStartTime;
            }

            if (hasSolution && Configuration.Discord.DisplaySolution) {
                activity.State = i11n("base_solution_text", solution);
            }

            if (hasFile) {
                activity.Details = i11n("base_editing_text", file);
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


        bool TryGetSolutionName(out string name) {
            name = "";
            if (DTE.Solution == null)
                return false;

            var solutionFile = DTE.Solution.FullName;

            if (string.IsNullOrEmpty(solutionFile))
                return false;

            try {
                var file = new FileInfo(solutionFile);
                name = Path.GetFileNameWithoutExtension(file.Name);
                return true;
            } catch {
                return false;
            }
        }

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