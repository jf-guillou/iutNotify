namespace iutNotify
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lbl_messages = new System.Windows.Forms.Label();
            this.lbl_print_quota = new System.Windows.Forms.Label();
            this.lbl_print_quota_value = new System.Windows.Forms.Label();
            this.lbl_print_link = new System.Windows.Forms.LinkLabel();
            this.mainContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.mainContextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "iutNotify";
            this.notifyIcon.Visible = true;
            this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
            this.notifyIcon.BalloonTipClosed += new System.EventHandler(this.notifyIcon_BalloonTipClosed);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // mainContextMenuStrip
            // 
            this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItemExit});
            this.mainContextMenuStrip.Name = "contextMenuStrip";
            this.mainContextMenuStrip.Size = new System.Drawing.Size(112, 26);
            // 
            // mainToolStripMenuItemExit
            // 
            this.mainToolStripMenuItemExit.Name = "mainToolStripMenuItemExit";
            this.mainToolStripMenuItemExit.Size = new System.Drawing.Size(111, 22);
            this.mainToolStripMenuItemExit.Text = "Quitter";
            this.mainToolStripMenuItemExit.Click += new System.EventHandler(this.mainToolStripMenuItemExit_Click);
            // 
            // lbl_messages
            // 
            this.lbl_messages.AutoSize = true;
            this.lbl_messages.Location = new System.Drawing.Point(12, 65);
            this.lbl_messages.Name = "lbl_messages";
            this.lbl_messages.Size = new System.Drawing.Size(102, 13);
            this.lbl_messages.TabIndex = 4;
            this.lbl_messages.Text = "Derniers messages :";
            // 
            // lbl_print_quota
            // 
            this.lbl_print_quota.AutoSize = true;
            this.lbl_print_quota.Location = new System.Drawing.Point(12, 9);
            this.lbl_print_quota.Name = "lbl_print_quota";
            this.lbl_print_quota.Size = new System.Drawing.Size(99, 13);
            this.lbl_print_quota.TabIndex = 1;
            this.lbl_print_quota.Text = "Quota impressions :";
            // 
            // lbl_print_quota_value
            // 
            this.lbl_print_quota_value.AutoSize = true;
            this.lbl_print_quota_value.Location = new System.Drawing.Point(109, 9);
            this.lbl_print_quota_value.Name = "lbl_print_quota_value";
            this.lbl_print_quota_value.Size = new System.Drawing.Size(13, 13);
            this.lbl_print_quota_value.TabIndex = 2;
            this.lbl_print_quota_value.Text = "_";
            // 
            // lbl_print_link
            // 
            this.lbl_print_link.AutoSize = true;
            this.lbl_print_link.Location = new System.Drawing.Point(19, 31);
            this.lbl_print_link.Name = "lbl_print_link";
            this.lbl_print_link.Size = new System.Drawing.Size(131, 13);
            this.lbl_print_link.TabIndex = 3;
            this.lbl_print_link.TabStop = true;
            this.lbl_print_link.Text = "Historique des impressions";
            this.lbl_print_link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbl_print_link_LinkClicked);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(204, 90);
            this.Controls.Add(this.lbl_print_link);
            this.Controls.Add(this.lbl_messages);
            this.Controls.Add(this.lbl_print_quota_value);
            this.Controls.Add(this.lbl_print_quota);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "iutNotify";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDoubleClick);
            this.mainContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip mainContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItemExit;
        private System.Windows.Forms.Label lbl_messages;
        private System.Windows.Forms.Label lbl_print_quota;
        private System.Windows.Forms.Label lbl_print_quota_value;
        private System.Windows.Forms.LinkLabel lbl_print_link;
    }
}

