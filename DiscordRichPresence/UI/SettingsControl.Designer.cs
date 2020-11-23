
namespace DiscordRichPresence.UI
{
    partial class SettingsControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbApplicationID = new System.Windows.Forms.Label();
            this.txtApplicationID = new System.Windows.Forms.TextBox();
            this.lbInstanceID = new System.Windows.Forms.Label();
            this.txtInstanceID = new System.Windows.Forms.TextBox();
            this.cbProjectFlag = new System.Windows.Forms.CheckBox();
            this.cbSolutionFlag = new System.Windows.Forms.CheckBox();
            this.cbTimestampFlag = new System.Windows.Forms.CheckBox();
            this.cbTimestampResetFlag = new System.Windows.Forms.CheckBox();
            this.btnCommit = new System.Windows.Forms.Button();
            this.btnAssets = new System.Windows.Forms.Button();
            this.btnLocalization = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbApplicationID
            // 
            this.lbApplicationID.AutoSize = true;
            this.lbApplicationID.Location = new System.Drawing.Point(38, 15);
            this.lbApplicationID.Name = "lbApplicationID";
            this.lbApplicationID.Size = new System.Drawing.Size(84, 15);
            this.lbApplicationID.TabIndex = 0;
            this.lbApplicationID.Text = "Application Id:";
            // 
            // txtApplicationID
            // 
            this.txtApplicationID.Location = new System.Drawing.Point(128, 12);
            this.txtApplicationID.Name = "txtApplicationID";
            this.txtApplicationID.Size = new System.Drawing.Size(314, 23);
            this.txtApplicationID.TabIndex = 1;
            // 
            // lbInstanceID
            // 
            this.lbInstanceID.AutoSize = true;
            this.lbInstanceID.Location = new System.Drawing.Point(12, 44);
            this.lbInstanceID.Name = "lbInstanceID";
            this.lbInstanceID.Size = new System.Drawing.Size(110, 15);
            this.lbInstanceID.TabIndex = 2;
            this.lbInstanceID.Text = "Discord Instance Id:";
            // 
            // txtInstanceID
            // 
            this.txtInstanceID.Location = new System.Drawing.Point(128, 41);
            this.txtInstanceID.Name = "txtInstanceID";
            this.txtInstanceID.Size = new System.Drawing.Size(314, 23);
            this.txtInstanceID.TabIndex = 3;
            // 
            // cbProjectFlag
            // 
            this.cbProjectFlag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProjectFlag.Location = new System.Drawing.Point(128, 84);
            this.cbProjectFlag.Name = "cbProjectFlag";
            this.cbProjectFlag.Size = new System.Drawing.Size(314, 19);
            this.cbProjectFlag.TabIndex = 4;
            this.cbProjectFlag.Text = "Display project name";
            this.cbProjectFlag.UseVisualStyleBackColor = true;
            // 
            // cbSolutionFlag
            // 
            this.cbSolutionFlag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSolutionFlag.Location = new System.Drawing.Point(128, 111);
            this.cbSolutionFlag.Name = "cbSolutionFlag";
            this.cbSolutionFlag.Size = new System.Drawing.Size(314, 19);
            this.cbSolutionFlag.TabIndex = 5;
            this.cbSolutionFlag.Text = "Display solution name";
            this.cbSolutionFlag.UseVisualStyleBackColor = true;
            // 
            // cbTimestampFlag
            // 
            this.cbTimestampFlag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTimestampFlag.Location = new System.Drawing.Point(128, 136);
            this.cbTimestampFlag.Name = "cbTimestampFlag";
            this.cbTimestampFlag.Size = new System.Drawing.Size(314, 19);
            this.cbTimestampFlag.TabIndex = 6;
            this.cbTimestampFlag.Text = "Display elapsed time";
            this.cbTimestampFlag.UseVisualStyleBackColor = true;
            // 
            // cbTimestampResetFlag
            // 
            this.cbTimestampResetFlag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTimestampResetFlag.Location = new System.Drawing.Point(128, 161);
            this.cbTimestampResetFlag.Name = "cbTimestampResetFlag";
            this.cbTimestampResetFlag.Size = new System.Drawing.Size(314, 20);
            this.cbTimestampResetFlag.TabIndex = 7;
            this.cbTimestampResetFlag.Text = "Reset elapsed time on window change";
            this.cbTimestampResetFlag.UseVisualStyleBackColor = true;
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommit.Location = new System.Drawing.Point(356, 309);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(86, 29);
            this.btnCommit.TabIndex = 10;
            this.btnCommit.Text = "Apply";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.BtnCommit_Click);
            // 
            // btnAssets
            // 
            this.btnAssets.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAssets.Location = new System.Drawing.Point(161, 199);
            this.btnAssets.Name = "btnAssets";
            this.btnAssets.Size = new System.Drawing.Size(183, 38);
            this.btnAssets.TabIndex = 8;
            this.btnAssets.Text = "Assets";
            this.btnAssets.UseVisualStyleBackColor = true;
            // 
            // btnLocalization
            // 
            this.btnLocalization.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLocalization.Location = new System.Drawing.Point(161, 243);
            this.btnLocalization.Name = "btnLocalization";
            this.btnLocalization.Size = new System.Drawing.Size(183, 38);
            this.btnLocalization.TabIndex = 9;
            this.btnLocalization.Text = "Localization";
            this.btnLocalization.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnLocalization);
            this.Controls.Add(this.btnAssets);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.cbTimestampResetFlag);
            this.Controls.Add(this.cbTimestampFlag);
            this.Controls.Add(this.cbSolutionFlag);
            this.Controls.Add(this.cbProjectFlag);
            this.Controls.Add(this.txtInstanceID);
            this.Controls.Add(this.lbInstanceID);
            this.Controls.Add(this.txtApplicationID);
            this.Controls.Add(this.lbApplicationID);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(454, 350);
            this.Load += new System.EventHandler(this.SettingsControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbApplicationID;
        private System.Windows.Forms.TextBox txtApplicationID;
        private System.Windows.Forms.Label lbInstanceID;
        private System.Windows.Forms.TextBox txtInstanceID;
        private System.Windows.Forms.CheckBox cbProjectFlag;
        private System.Windows.Forms.CheckBox cbSolutionFlag;
        private System.Windows.Forms.CheckBox cbTimestampFlag;
        private System.Windows.Forms.CheckBox cbTimestampResetFlag;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Button btnAssets;
        private System.Windows.Forms.Button btnLocalization;
    }
}