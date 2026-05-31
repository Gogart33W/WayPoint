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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlTopAccent = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.lblUsername = new System.Windows.Forms.Label();
            this.pnlUserBorder = new System.Windows.Forms.Panel();
            this.txtUsername = new System.Windows.Forms.TextBox();

            this.lblEmail = new System.Windows.Forms.Label();
            this.pnlEmailBorder = new System.Windows.Forms.Panel();
            this.txtEmail = new System.Windows.Forms.TextBox();

            this.lblPassword = new System.Windows.Forms.Label();
            this.pnlPassBorder = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();

            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.lblForgotPassword = new System.Windows.Forms.LinkLabel();
            this.btnLogin = new System.Windows.Forms.Button();

            this.lblNoAccount = new System.Windows.Forms.Label();
            this.lblSwitchMode = new System.Windows.Forms.Label();

            this.pnlBackground.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.pnlUserBorder.SuspendLayout();
            this.pnlEmailBorder.SuspendLayout();
            this.pnlPassBorder.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlBackground
            // 
            this.pnlBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlBackground.Controls.Add(this.btnExit);
            this.pnlBackground.Controls.Add(this.pnlCenter);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(1280, 720);
            this.pnlBackground.TabIndex = 0;
            this.pnlBackground.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlBackground_MouseDown);
            this.pnlBackground.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlBackground_MouseMove);
            this.pnlBackground.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlBackground_MouseUp);

            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnExit.Location = new System.Drawing.Point(1230, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(50, 40);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "✕";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.MouseEnter += (s, e) => this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.MouseLeave += (s, e) => this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));

            // 
            // pnlCenter
            // 
            this.pnlCenter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCenter.BackColor = System.Drawing.Color.White;
            this.pnlCenter.Controls.Add(this.pnlTopAccent);
            this.pnlCenter.Controls.Add(this.lblTitle);
            this.pnlCenter.Controls.Add(this.lblSubtitle);
            this.pnlCenter.Controls.Add(this.lblUsername);
            this.pnlCenter.Controls.Add(this.pnlUserBorder);
            this.pnlCenter.Controls.Add(this.lblEmail);
            this.pnlCenter.Controls.Add(this.pnlEmailBorder);
            this.pnlCenter.Controls.Add(this.lblPassword);
            this.pnlCenter.Controls.Add(this.pnlPassBorder);
            this.pnlCenter.Controls.Add(this.chkShowPassword);
            this.pnlCenter.Controls.Add(this.lblForgotPassword);
            this.pnlCenter.Controls.Add(this.btnLogin);
            this.pnlCenter.Controls.Add(this.lblNoAccount);
            this.pnlCenter.Controls.Add(this.lblSwitchMode);
            this.pnlCenter.Location = new System.Drawing.Point(415, 85);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(450, 560);
            this.pnlCenter.TabIndex = 0;
            this.pnlCenter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCenter_Paint);

            // 
            // pnlTopAccent
            // 
            this.pnlTopAccent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.pnlTopAccent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopAccent.Location = new System.Drawing.Point(0, 0);
            this.pnlTopAccent.Name = "pnlTopAccent";
            this.pnlTopAccent.Size = new System.Drawing.Size(450, 4);
            this.pnlTopAccent.TabIndex = 11;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Black", 28F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(105, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 62);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "WayPoint";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubtitle.Location = new System.Drawing.Point(145, 95);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(155, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Вхід у систему";

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblUsername.Location = new System.Drawing.Point(46, 145);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(142, 21);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Логін або Пошта";

            // pnlUserBorder
            this.pnlUserBorder.BackColor = System.Drawing.Color.White;
            this.pnlUserBorder.Controls.Add(this.txtUsername);
            this.pnlUserBorder.Location = new System.Drawing.Point(50, 170);
            this.pnlUserBorder.Name = "pnlUserBorder";
            this.pnlUserBorder.Size = new System.Drawing.Size(350, 44);
            this.pnlUserBorder.TabIndex = 3;
            this.pnlUserBorder.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInput_Paint);

            // txtUsername
            this.txtUsername.BackColor = System.Drawing.Color.White;
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtUsername.Location = new System.Drawing.Point(10, 7);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(330, 29);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Enter += new System.EventHandler(this.Input_Enter);
            this.txtUsername.Leave += new System.EventHandler(this.Input_Leave);

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblEmail.Location = new System.Drawing.Point(46, 225);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(155, 21);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Електронна пошта";
            this.lblEmail.Visible = false;

            // pnlEmailBorder
            this.pnlEmailBorder.BackColor = System.Drawing.Color.White;
            this.pnlEmailBorder.Controls.Add(this.txtEmail);
            this.pnlEmailBorder.Location = new System.Drawing.Point(50, 250);
            this.pnlEmailBorder.Name = "pnlEmailBorder";
            this.pnlEmailBorder.Size = new System.Drawing.Size(350, 44);
            this.pnlEmailBorder.TabIndex = 5;
            this.pnlEmailBorder.Visible = false;
            this.pnlEmailBorder.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInput_Paint);

            // txtEmail
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtEmail.Location = new System.Drawing.Point(10, 7);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(330, 29);
            this.txtEmail.TabIndex = 0;
            this.txtEmail.Enter += new System.EventHandler(this.Input_Enter);
            this.txtEmail.Leave += new System.EventHandler(this.Input_Leave);

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblPassword.Location = new System.Drawing.Point(46, 225);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(67, 21);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Пароль";

            // pnlPassBorder
            this.pnlPassBorder.BackColor = System.Drawing.Color.White;
            this.pnlPassBorder.Controls.Add(this.txtPassword);
            this.pnlPassBorder.Location = new System.Drawing.Point(50, 250);
            this.pnlPassBorder.Name = "pnlPassBorder";
            this.pnlPassBorder.Size = new System.Drawing.Size(350, 44);
            this.pnlPassBorder.TabIndex = 7;
            this.pnlPassBorder.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInput_Paint);

            // txtPassword
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtPassword.Location = new System.Drawing.Point(10, 7);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(330, 29);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.UseSystemPasswordChar = true; // СИСТЕМНІ КРАПКИ
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            this.txtPassword.Enter += new System.EventHandler(this.Input_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.Input_Leave);

            // chkShowPassword
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkShowPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkShowPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.chkShowPassword.Location = new System.Drawing.Point(50, 305);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(161, 25);
            this.chkShowPassword.TabIndex = 8;
            this.chkShowPassword.Text = "Показати пароль";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);

            // lblForgotPassword
            this.lblForgotPassword.AutoSize = true;
            this.lblForgotPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblForgotPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblForgotPassword.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.lblForgotPassword.Location = new System.Drawing.Point(265, 306);
            this.lblForgotPassword.Name = "lblForgotPassword";
            this.lblForgotPassword.Size = new System.Drawing.Size(134, 21);
            this.lblForgotPassword.TabIndex = 9;
            this.lblForgotPassword.TabStop = true;
            this.lblForgotPassword.Text = "Забули пароль?";
            this.lblForgotPassword.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
            this.lblForgotPassword.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblForgotPassword_LinkClicked);

            // btnLogin
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(50, 360);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(350, 48);
            this.btnLogin.TabIndex = 10;
            this.btnLogin.Text = "УВІЙТИ";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // lblNoAccount
            this.lblNoAccount.AutoSize = true;
            this.lblNoAccount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNoAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblNoAccount.Location = new System.Drawing.Point(100, 440);
            this.lblNoAccount.Name = "lblNoAccount";
            this.lblNoAccount.Size = new System.Drawing.Size(133, 23);
            this.lblNoAccount.TabIndex = 13;
            this.lblNoAccount.Text = "Немає акаунта?";

            // lblSwitchMode
            this.lblSwitchMode.AutoSize = true;
            this.lblSwitchMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSwitchMode.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblSwitchMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.lblSwitchMode.Location = new System.Drawing.Point(235, 440);
            this.lblSwitchMode.Name = "lblSwitchMode";
            this.lblSwitchMode.Size = new System.Drawing.Size(139, 23);
            this.lblSwitchMode.TabIndex = 14;
            this.lblSwitchMode.Text = "Зареєструватись";
            this.lblSwitchMode.Click += new System.EventHandler(this.lblSwitchMode_Click);

            // LoginForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WayPoint Login";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.LoginForm_Resize);

            this.pnlBackground.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.pnlCenter.PerformLayout();
            this.pnlUserBorder.ResumeLayout(false);
            this.pnlUserBorder.PerformLayout();
            this.pnlEmailBorder.ResumeLayout(false);
            this.pnlEmailBorder.PerformLayout();
            this.pnlPassBorder.ResumeLayout(false);
            this.pnlPassBorder.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlTopAccent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Panel pnlUserBorder;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Panel pnlEmailBorder;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Panel pnlPassBorder;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.LinkLabel lblForgotPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblNoAccount;
        private System.Windows.Forms.Label lblSwitchMode;
        private System.Windows.Forms.Button btnExit;
    }
}