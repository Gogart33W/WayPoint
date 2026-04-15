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
            pnlHeader = new Panel();
            lblTitle = new Label();
            pbBack = new PictureBox();
            pbExit = new PictureBox();
            tabControlFeed = new TabControl();
            tabMyTrips = new TabPage();
            flowMyTrips = new FlowLayoutPanel();
            tabGlobalFeed = new TabPage();
            flowGlobalFeed = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbBack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbExit).BeginInit();
            tabControlFeed.SuspendLayout();
            tabMyTrips.SuspendLayout();
            tabGlobalFeed.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(pbBack);
            pnlHeader.Controls.Add(pbExit);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(850, 62);
            pnlHeader.TabIndex = 0;
            pnlHeader.MouseDown += pnlHeader_MouseDown;
            pnlHeader.MouseMove += pnlHeader_MouseMove;
            pnlHeader.MouseUp += pnlHeader_MouseUp;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(55, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(207, 31);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Стрічка WayPoint";
            // 
            // pbBack
            // 
            pbBack.Cursor = Cursors.Hand;
            pbBack.Image = Properties.Resources.Home3_37171;
            pbBack.Location = new Point(12, 12);
            pbBack.Margin = new Padding(3, 4, 3, 4);
            pbBack.Name = "pbBack";
            pbBack.Size = new Size(30, 38);
            pbBack.SizeMode = PictureBoxSizeMode.Zoom;
            pbBack.TabIndex = 2;
            pbBack.TabStop = false;
            pbBack.Click += pbBack_Click;
            // 
            // pbExit
            // 
            pbExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbExit.Cursor = Cursors.Hand;
            pbExit.Image = Properties.Resources.free_icon_window_14062773;
            pbExit.Location = new Point(808, 12);
            pbExit.Margin = new Padding(3, 4, 3, 4);
            pbExit.Name = "pbExit";
            pbExit.Size = new Size(30, 38);
            pbExit.SizeMode = PictureBoxSizeMode.Zoom;
            pbExit.TabIndex = 3;
            pbExit.TabStop = false;
            pbExit.Click += pbExit_Click;
            // 
            // tabControlFeed
            // 
            tabControlFeed.Controls.Add(tabMyTrips);
            tabControlFeed.Controls.Add(tabGlobalFeed);
            tabControlFeed.Dock = DockStyle.Fill;
            tabControlFeed.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tabControlFeed.Location = new Point(0, 62);
            tabControlFeed.Margin = new Padding(3, 4, 3, 4);
            tabControlFeed.Name = "tabControlFeed";
            tabControlFeed.Padding = new Point(20, 10);
            tabControlFeed.SelectedIndex = 0;
            tabControlFeed.Size = new Size(850, 750);
            tabControlFeed.TabIndex = 1;
            // 
            // tabMyTrips
            // 
            tabMyTrips.BackColor = Color.Gainsboro;
            tabMyTrips.Controls.Add(flowMyTrips);
            tabMyTrips.Location = new Point(4, 51);
            tabMyTrips.Margin = new Padding(3, 4, 3, 4);
            tabMyTrips.Name = "tabMyTrips";
            tabMyTrips.Padding = new Padding(3, 4, 3, 4);
            tabMyTrips.Size = new Size(842, 695);
            tabMyTrips.TabIndex = 0;
            tabMyTrips.Text = "✍️ Мої подорожі";
            // 
            // flowMyTrips
            // 
            flowMyTrips.AutoScroll = true;
            flowMyTrips.Dock = DockStyle.Fill;
            flowMyTrips.Location = new Point(3, 4);
            flowMyTrips.Margin = new Padding(3, 4, 3, 4);
            flowMyTrips.Name = "flowMyTrips";
            flowMyTrips.Padding = new Padding(15, 19, 15, 19);
            flowMyTrips.Size = new Size(836, 687);
            flowMyTrips.TabIndex = 0;
            // 
            // tabGlobalFeed
            // 
            tabGlobalFeed.BackColor = Color.Gainsboro;
            tabGlobalFeed.Controls.Add(flowGlobalFeed);
            tabGlobalFeed.Location = new Point(4, 51);
            tabGlobalFeed.Margin = new Padding(3, 4, 3, 4);
            tabGlobalFeed.Name = "tabGlobalFeed";
            tabGlobalFeed.Padding = new Padding(3, 4, 3, 4);
            tabGlobalFeed.Size = new Size(842, 695);
            tabGlobalFeed.TabIndex = 1;
            tabGlobalFeed.Text = "🌍 Стрічка спільноти";
            // 
            // flowGlobalFeed
            // 
            flowGlobalFeed.AutoScroll = true;
            flowGlobalFeed.Dock = DockStyle.Fill;
            flowGlobalFeed.Location = new Point(3, 4);
            flowGlobalFeed.Margin = new Padding(3, 4, 3, 4);
            flowGlobalFeed.Name = "flowGlobalFeed";
            flowGlobalFeed.Padding = new Padding(15, 19, 15, 19);
            flowGlobalFeed.Size = new Size(836, 687);
            flowGlobalFeed.TabIndex = 0;
            // 
            // UserFeedForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(850, 812);
            Controls.Add(tabControlFeed);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "UserFeedForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Стрічка";
            Load += UserFeedForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbBack).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbExit).EndInit();
            tabControlFeed.ResumeLayout(false);
            tabMyTrips.ResumeLayout(false);
            tabGlobalFeed.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbBack;
        private System.Windows.Forms.PictureBox pbExit;
        private System.Windows.Forms.TabControl tabControlFeed;
        private System.Windows.Forms.TabPage tabMyTrips;
        private System.Windows.Forms.FlowLayoutPanel flowMyTrips;
        private System.Windows.Forms.TabPage tabGlobalFeed;
        private System.Windows.Forms.FlowLayoutPanel flowGlobalFeed;
    }
}