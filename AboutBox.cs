//-------------------------------------------------------------------
// Copyright © 2012 Kindel Systems, LLC
// http://www.kindel.com
// charlie@kindel.com
// 
// Published under the MIT License.
// Source control on SourceForge 
//    http://sourceforge.net/projects/mcecontroller/
//-------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows.Forms;
using MCEControl.Properties;

namespace MCEControl {
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class AboutBox : Form {
        private Label appNameLabel;
        private Button okButton;
        private LinkLabel licenseLink;
        private LinkLabel copyrightLink;
        private Label licenseSummaryLabel;
        private PictureBox logoPictureBox;
        private LinkLabel _linkLabelGuillen;
        private Label _label1;
        private LinkLabel homePageLink;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        public AboutBox() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            appNameLabel.Text = string.Format(Resources.MCE_Controller_Version_label, Resources.App_FullName, LatestVersion.CurrentVersion);

            copyrightLink.Text = Resources.Copyright;
            copyrightLink.Tag = Resources.Copyright_Link;

            licenseSummaryLabel.Text = Resources.License_Summary;

            licenseLink.Text = Resources.License_Label;
            licenseLink.Tag = Resources.License_Link;

            homePageLink.Text = Resources.HomePage_Label;
            homePageLink.Tag = Resources.HomePage_Link;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.appNameLabel = new System.Windows.Forms.Label();
            this.licenseLink = new System.Windows.Forms.LinkLabel();
            this.okButton = new System.Windows.Forms.Button();
            this.copyrightLink = new System.Windows.Forms.LinkLabel();
            this.licenseSummaryLabel = new System.Windows.Forms.Label();
            this.homePageLink = new System.Windows.Forms.LinkLabel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this._linkLabelGuillen = new System.Windows.Forms.LinkLabel();
            this._label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // appNameLabel
            // 
            this.appNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appNameLabel.Location = new System.Drawing.Point(208, 18);
            this.appNameLabel.Name = "appNameLabel";
            this.appNameLabel.Size = new System.Drawing.Size(721, 30);
            this.appNameLabel.TabIndex = 0;
            this.appNameLabel.Text = "<AppName>";
            // 
            // licenseLink
            // 
            this.licenseLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.licenseLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.licenseLink.Location = new System.Drawing.Point(209, 111);
            this.licenseLink.Name = "licenseLink";
            this.licenseLink.Size = new System.Drawing.Size(720, 23);
            this.licenseLink.TabIndex = 3;
            this.licenseLink.TabStop = true;
            this.licenseLink.Tag = "<license_link>";
            this.licenseLink.Text = "<license_label>";
            this.licenseLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelMceControllerLinkClicked);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.okButton.Location = new System.Drawing.Point(809, 212);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(120, 34);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // copyrightLink
            // 
            this.copyrightLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.copyrightLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.copyrightLink.Location = new System.Drawing.Point(208, 48);
            this.copyrightLink.Name = "copyrightLink";
            this.copyrightLink.Size = new System.Drawing.Size(721, 24);
            this.copyrightLink.TabIndex = 1;
            this.copyrightLink.TabStop = true;
            this.copyrightLink.Tag = "<copyright_link>";
            this.copyrightLink.Text = "<copyright>";
            this.copyrightLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelCharlieLinkClicked);
            // 
            // licenseSummaryLabel
            // 
            this.licenseSummaryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.licenseSummaryLabel.Location = new System.Drawing.Point(208, 72);
            this.licenseSummaryLabel.Name = "licenseSummaryLabel";
            this.licenseSummaryLabel.Size = new System.Drawing.Size(721, 39);
            this.licenseSummaryLabel.TabIndex = 2;
            this.licenseSummaryLabel.Text = "<license_summary>";
            // 
            // homePageLink
            // 
            this.homePageLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.homePageLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.homePageLink.Location = new System.Drawing.Point(208, 134);
            this.homePageLink.Name = "homePageLink";
            this.homePageLink.Size = new System.Drawing.Size(721, 24);
            this.homePageLink.TabIndex = 4;
            this.homePageLink.TabStop = true;
            this.homePageLink.Tag = "<home_page>";
            this.homePageLink.Text = "<home_page>";
            this.homePageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHomePage_LinkClicked);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.InitialImage = null;
            this.logoPictureBox.Location = new System.Drawing.Point(19, 18);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(154, 140);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 5;
            this.logoPictureBox.TabStop = false;
            // 
            // _linkLabelGuillen
            // 
            this._linkLabelGuillen.AutoSize = true;
            this._linkLabelGuillen.Location = new System.Drawing.Point(38, 187);
            this._linkLabelGuillen.Name = "_linkLabelGuillen";
            this._linkLabelGuillen.Size = new System.Drawing.Size(108, 20);
            this._linkLabelGuillen.TabIndex = 6;
            this._linkLabelGuillen.TabStop = true;
            this._linkLabelGuillen.Text = "GuillenDesign";
            this._linkLabelGuillen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGuillen_LinkClicked);
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.Location = new System.Drawing.Point(62, 162);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(60, 20);
            this._label1.TabIndex = 7;
            this._label1.Text = "Icon by";
            // 
            // AboutBox
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(941, 258);
            this.ControlBox = false;
            this.Controls.Add(this._label1);
            this.Controls.Add(this._linkLabelGuillen);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.homePageLink);
            this.Controls.Add(this.licenseSummaryLabel);
            this.Controls.Add(this.copyrightLink);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.licenseLink);
            this.Controls.Add(this.appNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "http://guillendesign.deviantart.com/";
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void ButtonOkClick(object sender, EventArgs e) {
            Close();
        }

        private void LinkLabelMceControllerLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(licenseLink.Tag.ToString());
        }

        private void LinkLabelCharlieLinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(copyrightLink.Tag.ToString());
        }

        private void linkLabelHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(homePageLink.Tag.ToString());
        }

        private void linkLabelGuillen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(_linkLabelGuillen.Tag.ToString());
        }
    }
}
