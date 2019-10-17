using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DiscordRichPresence.Core;
using DiscordRichPresence.Pipe;
using DiscordRichPresence.Pipe.Entities;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using ConfigurationManager = DiscordRichPresence.Core.ConfigurationManager;
using System.IO;
using DiscordRichPresence.Pipe.EventArgs;

namespace DiscordRichPresence
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid("a266a262-709b-4be0-a2f9-8587c845f573")]

    //[ProvideService(UIContextGuids.SolutionExists, IsAsyncQueryable = false)]
    //[ProvideService(UIContextGuids.NoSolution, IsAsyncQueryable = false)]

    [ProvideAutoLoad(UIContextGuids.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(UIContextGuids.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class DiscordPackage : AsyncPackage
    {
        public ConfigurationManager Configuration { get; }
        public DiscordPipeClient Client { get; private set; }
        public DiscordActivity LastActivity { get; private set; }
        public DTE DTE { get; private set; }
        public Events Events { get; private set; }

        public DiscordPackage()
        {
            this.Configuration = new ConfigurationManager();
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.Configuration.InitializeAsync();
            await this.SetupPipeAsync();

            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            this.DTE = await this.GetServiceAsync(typeof(DTE)) as DTE;
            this.Events = this.DTE.Events;
            this.Events.WindowEvents.WindowActivated += this.HandleWindowActivated;
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
                this.Client.Ready -= this.HandleReadyAsync;
                this.Client.Disconnected -= this.HandleDisconnectedAsync;

                await this.Client.DisconnectAsync();

                this.Client = null;
            }

            this.Client = new DiscordPipeClient(this.Configuration.Discord.CurrentInstanceId,
                    this.Configuration.Discord.ApplicationId);

            this.Client.Ready += this.HandleReadyAsync;
            this.Client.Disconnected += this.HandleDisconnectedAsync;

            await this.Client.ConnectAsync();
        }

        void HandleWindowActivated(Window window, Window old)
        {
            //System.Diagnostics.Debugger.Break();

            _ = Task.Run(async () =>
            {
                if (!this.Configuration.Discord.DisplayTimestamp)
                    this.LastActivity.Timestamps = null;
                else
                {
                    if (this.Configuration.Discord.AutoResetTimestamp)
                        this.LastActivity.Timestamps.StartTime = DateTimeOffset.Now;
                }

                if (this.LastActivity.Assets == null)
                    this.LastActivity.Assets = new DiscordActivityAssets();

                if (window == null && old == null)
                    return;

                if (window == null && old != null)
                    window = old;

                if (!this.Configuration.Discord.DisplayProject)
                    this.LastActivity.State = null;
                else
                {
                    if (this.TryGetProjectName(window, out var project_name))
                        this.LastActivity.State = $"Working on: {project_name}";
                }

                if (!this.Configuration.Discord.DisplaySolution)
                {
                    this.LastActivity.Assets.SmallImage = null;
                    this.LastActivity.Assets.SmallText = null;
                }
                else
                {
                    if (this.DTE.Solution == null)
                    {
                        this.LastActivity.Assets.SmallImage = null;
                        this.LastActivity.Assets.SmallText = null;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.DTE.Solution.FullName))
                        {
                            var file = new FileInfo(this.DTE.Solution.FullName);
                            this.LastActivity.Assets.SmallText = $"Solution: {file.Name}";
                            this.LastActivity.Assets.SmallImage = "visualstudio_small";
                        }
                    }
                }

                if (this.TryGetFileName(window, out var file_name, out var ext))
                {
                    this.LastActivity.Details = $"Editing: {file_name}";

                    var asset = this.Configuration.Assets.FindAsset(ext);

                    if (asset == null)
                    {
                        asset = this.Configuration.Assets.DefaultAsset;

                        if (asset == null)
                        {
                            this.LastActivity.Assets.LargeImage = "visualstudio_small";
                            this.LastActivity.Assets.LargeText = "Visual Studio";
                        }
                    }
                    else
                    {
                        this.LastActivity.Assets.LargeImage = asset.Key;
                        this.LastActivity.Assets.LargeText = asset.Text;
                    }
                }

                if (string.IsNullOrEmpty(this.LastActivity.Assets.LargeImage)
                    && string.IsNullOrEmpty(this.LastActivity.Assets.LargeText)
                    && string.IsNullOrEmpty(this.LastActivity.Assets.SmallImage)
                    && string.IsNullOrEmpty(this.LastActivity.Assets.SmallText))
                    this.LastActivity.Assets = null;

                await this.Client.SetActivityAsync(this.LastActivity).ConfigureAwait(false);
            });
        }

        bool TryGetProjectName(Window window, out string name)
        {
            name = null;

            if (window.Project == null)
                return false;

            var file = new FileInfo(window.Project.FullName);
            name = Path.GetFileNameWithoutExtension(file.Name);
            return true;
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