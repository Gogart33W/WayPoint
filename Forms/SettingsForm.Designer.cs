namespace WayPoint.Forms
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBack = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.lblDb = new System.Windows.Forms.Label();
            this.txtDb = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.chkShowPass = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblBack);
            this.pnlHeader.Controls.Add(this.lblExit);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;

            // 
            // lblBack
            // 
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBack.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Bold);
            this.lblBack.ForeColor = System.Drawing.Color.LightGray;
            this.lblBack.Location = new System.Drawing.Point(15, 12);
            this.lblBack.Text = "⬅️ Назад";
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);
            this.lblBack.MouseEnter += (s, e) => this.lblBack.ForeColor = System.Drawing.Color.White;
            this.lblBack.MouseLeave += (s, e) => this.lblBack.ForeColor = System.Drawing.Color.LightGray;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(130, 14);
            this.lblTitle.Text = "⚙️ Налаштування системи (config.json)";

            // 
            // lblExit
            // 
            this.lblExit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblExit.AutoSize = true;
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblExit.ForeColor = System.Drawing.Color.LightGray;
            this.lblExit.Location = new System.Drawing.Point(1050, 18);
            this.lblExit.Text = "❌ Закрити";
            this.lblExit.Click += new System.EventHandler(this.lblExit_Click);
            this.lblExit.MouseEnter += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.Crimson;
            this.lblExit.MouseLeave += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.LightGray;

            // 
            // pnlCenter
            // 
            this.pnlCenter.BackColor = System.Drawing.Color.White;
            this.pnlCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCenter.Size = new System.Drawing.Size(650, 420);
            this.pnlCenter.Controls.Add(this.lblDb);
            this.pnlCenter.Controls.Add(this.txtDb);
            this.pnlCenter.Controls.Add(this.lblEmail);
            this.pnlCenter.Controls.Add(this.txtEmail);
            this.pnlCenter.Controls.Add(this.lblPass);
            this.pnlCenter.Controls.Add(this.txtPass);
            this.pnlCenter.Controls.Add(this.chkShowPass);
            this.pnlCenter.Controls.Add(this.btnSave);

            // lblDb
            this.lblDb.AutoSize = true;
            this.lblDb.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDb.Location = new System.Drawing.Point(40, 40);
            this.lblDb.Text = "Рядок підключення до Бази Даних (Connection String):";

            // txtDb
            this.txtDb.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtDb.Location = new System.Drawing.Point(40, 70);
            this.txtDb.Size = new System.Drawing.Size(570, 27);

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblEmail.Location = new System.Drawing.Point(40, 130);
            this.lblEmail.Text = "Smtp Email (Пошта відправника, напр. Gmail):";

            // txtEmail
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtEmail.Location = new System.Drawing.Point(40, 160);
            this.txtEmail.Size = new System.Drawing.Size(570, 27);

            // lblPass
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPass.Location = new System.Drawing.Point(40, 220);
            this.lblPass.Text = "Smtp App Password (Пароль додатка для пошти):";

            // txtPass
            this.txtPass.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPass.Location = new System.Drawing.Point(40, 250);
            this.txtPass.Size = new System.Drawing.Size(570, 27);
            this.txtPass.UseSystemPasswordChar = true;

            // chkShowPass
            this.chkShowPass.AutoSize = true;
            this.chkShowPass.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkShowPass.Location = new System.Drawing.Point(40, 290);
            this.chkShowPass.Text = "Показати пароль";
            this.chkShowPass.CheckedChanged += new System.EventHandler(this.chkShowPass_CheckedChanged);

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(40, 335);
            this.btnSave.Size = new System.Drawing.Size(570, 50);
            this.btnSave.Text = "💾 ЗБЕРЕГТИ НАЛАШТУВАННЯ";
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // 
            // SettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Resize += new System.EventHandler(this.SettingsForm_Resize);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCenter.ResumeLayout(false);
            this.pnlCenter.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBack;
        private System.Windows.Forms.Label lblExit;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.TextBox txtDb;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.CheckBox chkShowPass;
        private System.Windows.Forms.Button btnSave;
    }
}