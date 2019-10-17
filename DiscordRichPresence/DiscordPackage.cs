using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace DiscordRichPresence
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid("a266a262-709b-4be0-a2f9-8587c845f573")]
    [ProvideService(UIContextGuids.SolutionExists)]
    [ProvideService(UIContextGuids.NoSolution)]
    public sealed class DiscordPackage : AsyncPackage
    {
        public DiscordPackage()
        {

        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }
    }
}
