using System;
using System.Windows.Forms;
using DiscordRichPresence.Core;

namespace DiscordRichPresence.UI
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            this.InitializeComponent();
        }

        private void SettingsControl_Load(object sender, EventArgs e)
        {
            var discord = ConfigurationProvider.Instance.Discord;
            this.txtApplicationID.Text = discord.ApplicationId.ToString();
            this.txtInstanceID.Text = discord.PipeInstanceId.ToString();
            this.cbProjectFlag.Checked = discord.DisplayProject;
            this.cbSolutionFlag.Checked = discord.DisplaySolution;
            this.cbTimestampFlag.Checked = discord.DisplayTimestamp;
            this.cbTimestampResetFlag.Checked = discord.AutoResetTimestamp;
        }

        private void BtnCommit_Click(object sender, EventArgs e)
        {
            var discord = ConfigurationProvider.Instance.Discord;

            if (!ulong.TryParse(this.txtApplicationID.Text, out var appid))
            {
                MessageBox.Show("Invalid application id provided.");
                return;
            }

            if (!byte.TryParse(this.txtInstanceID.Text, out var instanceId))
            {
                MessageBox.Show("Invalid instance id provided.");
                return;
            }

            discord.ApplicationId = appid;
            discord.PipeInstanceId = instanceId;
            discord.DisplayProject = this.cbProjectFlag.Checked;
            discord.DisplaySolution = this.cbSolutionFlag.Checked;
            discord.DisplayTimestamp = this.cbTimestampFlag.Checked;
            discord.AutoResetTimestamp = this.cbTimestampResetFlag.Checked;
        }
    }
}
