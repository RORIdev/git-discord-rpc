using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using DiscordRichPresence.Core;
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
            this.Configuration = new ConfigurationProvider();
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.Configuration.InitializeAsync();
            await this.SetupPipeAsync();
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            this.DTE = (await this.GetServiceAsync(typeof(DTE)).ConfigureAwait(false)) as DTE;

            if (this.DTE != null)
            {
                this.Events = this.DTE.Events;
                this.Events.WindowEvents.WindowActivated += this.HandleWindowActivated;
            }
        }

        async Task HandleReadyAsync(ReadyEventArgs e)
        {
            if (this.LastActivity == null)
            {
                this.LastActivity = new DiscordActivity
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

                if (this.Configuration.Discord.DisplayTimestamp)
                    this.LastActivity.Timestamps = new DiscordActivityTimestamps { StartTime = DateTimeOffset.Now };
            }

            await this.Client.SetActivityAsync(this.LastActivity);
        }

        async Task HandleDisconnectedAsync()
        {
            await Task.Delay(5000);
            await this.SetupPipeAsync();
        }

        async Task SetupPipeAsync()
        {
            if (this.Client != null)
            {
                await this.Client.DisconnectAsync();

                this.Client.Ready -= this.HandleReadyAsync;
                this.Client.Disconnected -= this.HandleDisconnectedAsync;
                this.Client = null;
            }

            this.Client = new DiscordPipeClient(this.Configuration.Discord.PipeInstanceId,
                    this.Configuration.Discord.ApplicationId);

            this.Client.Ready += this.HandleReadyAsync;
            this.Client.Disconnected += this.HandleDisconnectedAsync;

            await this.Client.ConnectAsync();
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
                    var fn = this.Configuration.Localization.GetFormatDelegate();

                    if (this.LastActivity.Assets == null)
                        this.LastActivity.Assets = new DiscordActivityAssets();

                    if (window?.Project == null && old?.Project == null)
                        return;

                    if (window?.Project == null && old?.Project != null)
                        window = old;

                    if (!this.Configuration.Discord.DisplayTimestamp)
                        this.LastActivity.Timestamps = null;
                    else
                    {
                        if (this.LastActivity.Timestamps == null)
                            this.LastActivity.Timestamps = new DiscordActivityTimestamps { StartTime = this.OriginalStartTime };

                        if (this.Configuration.Discord.AutoResetTimestamp)
                            this.LastActivity.Timestamps.StartTime = DateTimeOffset.Now;
                    }

                    if (!this.Configuration.Discord.DisplayProject)
                        this.LastActivity.State = null;
                    else
                    {
                        if (this.TryGetProjectName(window, out var project_name))
                            this.LastActivity.State = fn("base_working_text", project_name);
                        else
                            this.LastActivity.State = null;
                    }

                    if (!this.Configuration.Discord.DisplaySolution)
                    {
                        this.LastActivity.Assets.SmallImage = null;
                        this.LastActivity.Assets.SmallText = null;
                    }
                    else
                    {
                        if (this.DTE.Solution != null)
                        {
                            if (!string.IsNullOrEmpty(this.DTE.Solution.FullName))
                            {
                                try
                                {
                                    var file = new FileInfo(this.DTE.Solution.FullName);
                                    this.LastActivity.Assets.SmallText = fn("base_solution_text", Path.GetFileNameWithoutExtension(file.Name));
                                    this.LastActivity.Assets.SmallImage = "visualstudio_small";
                                }
                                catch
                                {
                                    this.LastActivity.Assets.SmallImage = null;
                                    this.LastActivity.Assets.SmallText = null;
                                }
                            }
                        }
                    }

                    if (this.TryGetFileName(window, out var file_name, out var ext))
                    {
                        this.LastActivity.Details = fn("base_editing_text", file_name);

                        if (this.Configuration.Assets.FindAsset(ext, out var asset))
                        {
                            var text = string.Empty;
                            var localized_key = asset.Text.StartsWith("@") ? asset.Text.Substring(1) : null;

                            if (!string.IsNullOrEmpty(localized_key))
                                text = fn(localized_key);
                            else
                                text = asset.Text;

                            this.LastActivity.Assets.LargeImage = asset.Key;
                            this.LastActivity.Assets.LargeText = text;
                        }
                        else
                        {
                            asset = this.Configuration.Assets.DefaultAsset;

                            if (asset == null)
                            {
                                this.LastActivity.Assets.LargeImage = "visualstudio_small";
                                this.LastActivity.Assets.LargeText = "Visual Studio";
                            }
                            else
                            {
                                this.LastActivity.Assets.LargeImage = asset.Key;

                                if (asset.Key == "default_file")
                                    this.LastActivity.Assets.LargeText = fn("base_unknown_file_text");
                                else
                                    this.LastActivity.Assets.LargeText = asset.Text;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(this.LastActivity.Assets.LargeImage)
                        && string.IsNullOrEmpty(this.LastActivity.Assets.LargeText)
                        && string.IsNullOrEmpty(this.LastActivity.Assets.SmallImage)
                        && string.IsNullOrEmpty(this.LastActivity.Assets.SmallText))
                        this.LastActivity.Assets = null;

                    await this.Client.SetActivityAsync(this.LastActivity).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("\n\nSET ACTIVITY ERROR: {0}\n\n", args: ex);
                }
            });
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