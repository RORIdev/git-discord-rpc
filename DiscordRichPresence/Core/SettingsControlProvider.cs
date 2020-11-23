using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DiscordRichPresence.UI;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

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

        static readonly SemaphoreSlim WriteSemaphore = new SemaphoreSlim(1, 1);

        public override void SaveSettingsToStorage()
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await WriteSemaphore.WaitAsync();
                    await ConfigurationProvider.Instance.SaveConfigurationsAsync();
                }
                finally
                {
                    WriteSemaphore.Release();
                }
            });
        }
    }
}
