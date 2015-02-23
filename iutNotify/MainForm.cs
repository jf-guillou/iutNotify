using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace iutNotify
{
    public partial class MainForm : Form
    {
        #region Var
        private static string PRINTQUOTA_URL = "http://intranet.iutsb.local/printquota/";
        private static int BASE_HEIGHT = 124;
        private static int BASE_WIDTH = 220;
        private static int WIDTH_INCREMENT = 22;
        private bool isFrameReady = false;

        private DataPush push = null;

        private List<Bubble> bubbles;
        private int displayingBubble = -1;
        private bool isBubbleVisible = false;
        private bool isBubbleReShow = false;
        private int historyLength = 0;
        #endregion

        #region Frame controls
        public MainForm()
        {
            InitializeComponent();
            SetPosition();
            this.Opacity = 0;
            this.Hide();
            bubbles = new List<Bubble>();

            string login = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            push = new DataPush(this, login.Contains("\\") ? login.Split(new string[] { "\\" }, StringSplitOptions.None)[1] : login);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                push.RequestStop();
            }
        }

        private void SetPosition()
        {
            this.Size = new Size(BASE_WIDTH, BASE_HEIGHT + historyLength * WIDTH_INCREMENT);
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(screen.X + screen.Width - this.Width - 4, screen.Y + screen.Height - this.Height - 4);
        }

        private void ForceShow()
        {
            if (!isFrameReady)
                return;

            if (this.Opacity == 0)
                this.Opacity = 1.0;
            if (!this.TopMost)
                this.TopMost = true;

            this.Show();
            SetPosition();
        }

        private void AppendMessageHist(int i, string text)
        {
            LinkLabel tmpLbl = new LinkLabel();
            tmpLbl.AutoSize = true;
            tmpLbl.Text = text;
            tmpLbl.TabIndex = 5 + i;
            tmpLbl.Name = String.Format("lbl_message_hist_{0}", historyLength);
            tmpLbl.Location = new System.Drawing.Point(19, 87 + WIDTH_INCREMENT * historyLength);
            tmpLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lbl_messages_value_LinkClicked);
            this.Controls.Add(tmpLbl);

            ++historyLength;
        }

        private void lbl_messages_value_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ReShowBubble(((LinkLabel)sender).TabIndex - 5);
        }

        private void lbl_print_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(PRINTQUOTA_URL);
        }

        public bool Exists()
        {
            return !this.Disposing && this.Created;
        }

        public void SetPrintQuota(bool hasQuota = true)
        {
            SetControlPropertyThreadSafe(lbl_print_quota_value, "Text", hasQuota ? "aucun" : "illimité");
        }

        public void SetPrintQuota(int quota, int maxQuota)
        {
            SetControlPropertyThreadSafe(lbl_print_quota_value, "Text", quota + "/" + maxQuota + " page" + (quota != 1 ? "s" : ""));
        }

        public void FrameFilled()
        {
            if (!isFrameReady)
            {
                isFrameReady = true;
                if (System.Diagnostics.Debugger.IsAttached)
                    this.Invoke(new MethodInvoker(delegate { ForceShow(); }));
            }
        }
        #endregion

        #region Thread safe controls
        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        #endregion

        #region toolStrip
        private void mainToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region notifyIcon
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Opacity > 0 && this.Visible)
                this.Hide();
            else
                ForceShow();
        }
        #endregion

        #region Bubble
        public void EnqueueBubble(string title, string text, DateTime addedAt, string url, bool isNotif)
        {
            bubbles.Add(new Bubble(title, text, addedAt, url));
            if(!isNotif)
                this.Invoke(new MethodInvoker(delegate { AppendMessageHist(bubbles.Count - 1, title); }));

            if (historyLength > 0)
                this.Invoke(new MethodInvoker(delegate { SetPosition(); }));
            NextBubbleIfPossible();
        }

        private void NextBubbleIfPossible()
        {
            if (!isBubbleVisible && displayingBubble < bubbles.Count - 1)
            {
                bubbles.ElementAt(++displayingBubble).ShowBubble(notifyIcon);
                isBubbleVisible = true;
            }
        }

        private void ReShowBubble(int i)
        {
            displayingBubble = i;
            bubbles.ElementAt(i).ShowBubble(notifyIcon);
            isBubbleVisible = true;
            isBubbleReShow = true;
        }

        private void notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            isBubbleVisible = false;
            if (isBubbleReShow)
            {
                isBubbleReShow = false;
                displayingBubble = bubbles.Count - 1;
            }
            else
                NextBubbleIfPossible();
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            bubbles.ElementAt(displayingBubble).OnClick();
            notifyIcon_BalloonTipClosed(sender, e);
        }
        #endregion

        #region Debug
        private void MainForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                Application.Exit();
        }
        #endregion

    }

    class Bubble
    {
        private string title;
        private string text;
        private int timeout;
        private ToolTipIcon icon;
        private string url;

        public Bubble(string title, string message, DateTime addedAt, string url)
        {
            this.title = title;
            this.timeout = 30000;
            this.url = url;

            string lTitle = title.ToLower();
            if (lTitle.Contains("erreur") || lTitle.Contains("impossible"))
                icon = ToolTipIcon.Error;
            else if (lTitle.Contains("attention") || lTitle.Contains("!"))
                icon = ToolTipIcon.Warning;
            else
                icon = ToolTipIcon.Info;

            text = "";
            if (addedAt != null)
                text += addedAt.ToString() + "\n";
            text += message;
            if (url != null && url != "")
                text += "\n\nCliquez ici pour en savoir plus.";
        }

        public void ShowBubble(NotifyIcon notifyIcon)
        {
            notifyIcon.ShowBalloonTip(timeout, title, text, icon);
        }

        public void OnClick()
        {
            if(url != null && url != "")
                System.Diagnostics.Process.Start(url);
        }
    }
}
