using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DiscordRichPresence.UI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace DiscordRichPresence.Core
{
    [Guid("848D043A-EB0C-4A8E-B8B5-890BBC7EAB39")]
    public class SettingsControlProvider : DialogPage
    {
        public SettingsControlProvider()
        {

        }

        protected override IWin32Window Window
            => new SettingsControl();

        static readonly Mutex SyncMtx = new Mutex(false);

        public override void SaveSettingsToStorage()
        {
            if (SyncMtx.WaitOne())
            {
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    await ConfigurationProvider.Instance.SaveConfigurationsAsync();
                    SyncMtx.ReleaseMutex();
                });
            }
        }
    }
}
