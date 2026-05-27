namespace WayPoint
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.lblExit = new System.Windows.Forms.Label();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.lblForgotPassword = new System.Windows.Forms.LinkLabel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblSwitchMode = new System.Windows.Forms.Label();

            this.pnlBackground.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.SuspendLayout();

            // ===== pnlBackground (Темний сучасний фон) =====
            this.pnlBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55))))); // Tailwind Gray-800
            this.pnlBackground.Controls.Add(this.lblExit);
            this.pnlBackground.Controls.Add(this.pnlCenter);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.TabIndex = 0;

            // ===== lblExit (Емоджі-хрестик) =====
            this.lblExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExit.AutoSize = true;
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblExit.ForeColor = System.Drawing.Color.LightGray;
            this.lblExit.Location = new System.Drawing.Point(1030, 15);
            this.lblExit.Name = "lblExit";
            this.lblExit.Text = "❌";
            this.lblExit.Click += new System.EventHandler(this.pbExit_Click);
            this.lblExit.MouseEnter += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.White;
            this.lblExit.MouseLeave += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.LightGray;

            // ===== pnlCenter (Біла КАРТКА) =====
            this.pnlCenter.BackColor = System.Drawing.Color.White;
            this.pnlCenter.Anchor = System.Windows.Forms.AnchorStyles.None; // Тримає по центру
            this.pnlCenter.Location = new System.Drawing.Point(340, 90);
            this.pnlCenter.Size = new System.Drawing.Size(400, 520);
            // Щоб картка виглядала гарно без тіней, ми робимо її просто білою на темному фоні
            this.pnlCenter.Controls.Add(this.lblTitle);
            this.pnlCenter.Controls.Add(this.lblUsername);
            this.pnlCenter.Controls.Add(this.txtUsername);
            this.pnlCenter.Controls.Add(this.lblEmail);
            this.pnlCenter.Controls.Add(this.txtEmail);
            this.pnlCenter.Controls.Add(this.lblPassword);
            this.pnlCenter.Controls.Add(this.txtPassword);
            this.pnlCenter.Controls.Add(this.chkShowPassword);
            this.pnlCenter.Controls.Add(this.lblForgotPassword);
            this.pnlCenter.Controls.Add(this.btnLogin);
            this.pnlCenter.Controls.Add(this.lblSwitchMode);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Black", 26F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55))))); // Темний текст
            this.lblTitle.Location = new System.Drawing.Point(90, 25);
            this.lblTitle.Text = "WayPoint";

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.Gray;
            this.lblUsername.Location = new System.Drawing.Point(46, 105);
            this.lblUsername.Text = "Логін або Пошта";

            // txtUsername (Світло-сіре поле на білому фоні)
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtUsername.ForeColor = System.Drawing.Color.Black;
            this.txtUsername.Location = new System.Drawing.Point(50, 130);
            this.txtUsername.Size = new System.Drawing.Size(300, 39);

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmail.ForeColor = System.Drawing.Color.Gray;
            this.lblEmail.Location = new System.Drawing.Point(46, 185);
            this.lblEmail.Text = "Email (тільки для реєстрації)";
            this.lblEmail.Visible = false;

            // txtEmail
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtEmail.ForeColor = System.Drawing.Color.Black;
            this.txtEmail.Location = new System.Drawing.Point(50, 210);
            this.txtEmail.Size = new System.Drawing.Size(300, 39);
            this.txtEmail.Visible = false;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.Gray;
            this.lblPassword.Location = new System.Drawing.Point(46, 265);
            this.lblPassword.Text = "Пароль";

            // txtPassword
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtPassword.ForeColor = System.Drawing.Color.Black;
            this.txtPassword.Location = new System.Drawing.Point(50, 290);
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.Size = new System.Drawing.Size(300, 39);
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);

            // chkShowPassword
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkShowPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.chkShowPassword.Location = new System.Drawing.Point(50, 335);
            this.chkShowPassword.Text = "Показати пароль";
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);

            // lblForgotPassword
            this.lblForgotPassword.AutoSize = true;
            this.lblForgotPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblForgotPassword.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246))))); // Tailwind Blue-500
            this.lblForgotPassword.Location = new System.Drawing.Point(225, 336);
            this.lblForgotPassword.Text = "Забули пароль?";
            this.lblForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblForgotPassword_LinkClicked);

            // btnLogin
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129))))); // Tailwind Emerald-500
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(50, 380);
            this.btnLogin.Size = new System.Drawing.Size(300, 50);
            this.btnLogin.Text = "УВІЙТИ";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // lblSwitchMode
            this.lblSwitchMode.AutoSize = true;
            this.lblSwitchMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSwitchMode.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblSwitchMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblSwitchMode.Location = new System.Drawing.Point(125, 450);
            this.lblSwitchMode.Text = "Створити акаунт";
            this.lblSwitchMode.Click += new System.EventHandler(this.lblSwitchMode_Click);

            // ===== FORM =====
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 680);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Controls.Add(this.pnlBackground);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WayPoint Login";
            this.Resize += new System.EventHandler(this.LoginForm_Resize);

            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.pnlCenter.ResumeLayout(false);
            this.pnlCenter.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblSwitchMode;
        private System.Windows.Forms.Label lblExit;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.LinkLabel lblForgotPassword;
    }
}