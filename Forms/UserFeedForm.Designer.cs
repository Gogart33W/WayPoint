using System;
using System.Drawing;
using System.Windows.Forms;

namespace WayPoint
{
    partial class UserFeedForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnMessenger = new System.Windows.Forms.Button(); // НОВА КНОПКА
            this.lblBack = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();
            this.tabControlFeed = new System.Windows.Forms.TabControl();
            this.tabMyTrips = new System.Windows.Forms.TabPage();
            this.flowMyTrips = new System.Windows.Forms.FlowLayoutPanel();
            this.tabGlobalFeed = new System.Windows.Forms.TabPage();
            this.flowGlobalFeed = new System.Windows.Forms.FlowLayoutPanel();

            this.pnlHeader.SuspendLayout();
            this.tabControlFeed.SuspendLayout();
            this.tabMyTrips.SuspendLayout();
            this.tabGlobalFeed.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.pnlHeader.Controls.Add(this.btnMessenger); // ДОДАЛИ НА ПАНЕЛЬ
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblBack);
            this.pnlHeader.Controls.Add(this.lblExit);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1080, 60);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseDown);
            this.pnlHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseMove);
            this.pnlHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseUp);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(60, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(207, 31);
            this.lblTitle.Text = "Стрічка WayPoint";

            // btnMessenger (Кнопка чату)
            this.btnMessenger.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnMessenger.ForeColor = System.Drawing.Color.White;
            this.btnMessenger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMessenger.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnMessenger.Location = new System.Drawing.Point(850, 12);
            this.btnMessenger.Size = new System.Drawing.Size(160, 35);
            this.btnMessenger.Text = "💬 Месенджер";
            this.btnMessenger.FlatAppearance.BorderSize = 0;
            this.btnMessenger.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMessenger.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMessenger.Click += new System.EventHandler(this.btnMessenger_Click);

            // lblBack
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBack.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblBack.ForeColor = System.Drawing.Color.LightGray;
            this.lblBack.Location = new System.Drawing.Point(15, 12);
            this.lblBack.Name = "lblBack";
            this.lblBack.Size = new System.Drawing.Size(42, 37);
            this.lblBack.Text = "⬅️";
            this.lblBack.Click += new System.EventHandler(this.pbBack_Click);
            this.lblBack.MouseEnter += (s, e) => this.lblBack.ForeColor = System.Drawing.Color.White;
            this.lblBack.MouseLeave += (s, e) => this.lblBack.ForeColor = System.Drawing.Color.LightGray;

            // lblExit
            this.lblExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExit.AutoSize = true;
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblExit.ForeColor = System.Drawing.Color.LightGray;
            this.lblExit.Location = new System.Drawing.Point(1030, 12);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(42, 37);
            this.lblExit.Text = "❌";
            this.lblExit.Click += new System.EventHandler(this.pbExit_Click);
            this.lblExit.MouseEnter += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.White;
            this.lblExit.MouseLeave += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.LightGray;

            // 
            // tabControlFeed
            // 
            this.tabControlFeed.Controls.Add(this.tabMyTrips);
            this.tabControlFeed.Controls.Add(this.tabGlobalFeed);
            this.tabControlFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlFeed.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.tabControlFeed.Location = new System.Drawing.Point(0, 60);
            this.tabControlFeed.Name = "tabControlFeed";
            this.tabControlFeed.Padding = new System.Drawing.Point(20, 10);
            this.tabControlFeed.SelectedIndex = 0;
            this.tabControlFeed.Size = new System.Drawing.Size(1080, 620);
            this.tabControlFeed.TabIndex = 1;

            // tabMyTrips
            this.tabMyTrips.BackColor = System.Drawing.Color.Gainsboro;
            this.tabMyTrips.Controls.Add(this.flowMyTrips);
            this.tabMyTrips.Location = new System.Drawing.Point(4, 51);
            this.tabMyTrips.Name = "tabMyTrips";
            this.tabMyTrips.Padding = new System.Windows.Forms.Padding(3);
            this.tabMyTrips.Size = new System.Drawing.Size(1072, 565);
            this.tabMyTrips.TabIndex = 0;
            this.tabMyTrips.Text = "✍️ Мої подорожі";

            // flowMyTrips
            this.flowMyTrips.AutoScroll = true;
            this.flowMyTrips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMyTrips.Location = new System.Drawing.Point(3, 3);
            this.flowMyTrips.Name = "flowMyTrips";
            this.flowMyTrips.Padding = new System.Windows.Forms.Padding(15);
            this.flowMyTrips.Size = new System.Drawing.Size(1066, 559);
            this.flowMyTrips.TabIndex = 0;

            // tabGlobalFeed
            this.tabGlobalFeed.BackColor = System.Drawing.Color.Gainsboro;
            this.tabGlobalFeed.Controls.Add(this.flowGlobalFeed);
            this.tabGlobalFeed.Location = new System.Drawing.Point(4, 51);
            this.tabGlobalFeed.Name = "tabGlobalFeed";
            this.tabGlobalFeed.Padding = new System.Windows.Forms.Padding(3);
            this.tabGlobalFeed.Size = new System.Drawing.Size(1072, 565);
            this.tabGlobalFeed.TabIndex = 1;
            this.tabGlobalFeed.Text = "🌍 Стрічка спільноти";

            // flowGlobalFeed
            this.flowGlobalFeed.AutoScroll = true;
            this.flowGlobalFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowGlobalFeed.Location = new System.Drawing.Point(3, 3);
            this.flowGlobalFeed.Name = "flowGlobalFeed";
            this.flowGlobalFeed.Padding = new System.Windows.Forms.Padding(15);
            this.flowGlobalFeed.Size = new System.Drawing.Size(1066, 559);
            this.flowGlobalFeed.TabIndex = 0;

            // UserFeedForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 680);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Controls.Add(this.tabControlFeed);
            this.Controls.Add(this.pnlHeader);
            this.Name = "UserFeedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Стрічка";
            this.Load += new System.EventHandler(this.UserFeedForm_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tabControlFeed.ResumeLayout(false);
            this.tabMyTrips.ResumeLayout(false);
            this.tabGlobalFeed.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnMessenger;
        private System.Windows.Forms.Label lblBack;
        private System.Windows.Forms.Label lblExit;
        private System.Windows.Forms.TabControl tabControlFeed;
        private System.Windows.Forms.TabPage tabMyTrips;
        private System.Windows.Forms.FlowLayoutPanel flowMyTrips;
        private System.Windows.Forms.TabPage tabGlobalFeed;
        private System.Windows.Forms.FlowLayoutPanel flowGlobalFeed;
    }
}