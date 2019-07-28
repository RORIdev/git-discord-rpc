using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.LiveShare;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Task = System.Threading.Tasks.Task;

namespace DiscordRichPresence
{
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid("9cdb6758-5259-441f-9d55-842fce0d87e2")]
    public sealed partial class DiscordPackage : AsyncPackage
    {
        private DiscordConfigManager config;
        private DiscordManager discord;

        public DiscordPackage()
        {
            config = new DiscordConfigManager();
            config.Initialize();

            discord = new DiscordManager(config.Discord.ApplicationId, config.Discord.DiscordInstanceId);
            discord.Initialize();
        }

        protected override async Task InitializeAsync(CancellationToken ct, IProgress<ServiceProgressData> p)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(ct);

            _dte = await GetServiceAsync(typeof(DTE)) as DTE;
            _events = _dte.Events;
            _events.WindowEvents.WindowActivated += NotifyWindowActivated;
        }
    }
}
