using System;
using System.Drawing;
using System.Windows.Forms;
using WayPoint.Services;

namespace WayPoint
{
    public partial class LoginForm : Form
    {
        private bool isLoginMode = true;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public LoginForm()
        {
            InitializeComponent();

            isLoginMode = false;
            SwitchMode();
        }

        // Автоматичне центрування панелі логіну при зміні розміру вікна
        private void LoginForm_Resize(object sender, EventArgs e)
        {
            if (pnlCenter != null)
            {
                pnlCenter.Left = (this.ClientSize.Width - pnlCenter.Width) / 2;
                pnlCenter.Top = (this.ClientSize.Height - pnlCenter.Height) / 2;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isLoginMode) // === РЕЖИМ ВХОДУ ===
            {
                try
                {
                    var user = AuthManager.Login(usernameOrEmail, password);
                    if (user != null)
                    {
                        this.Hide();
                        Form nextForm;

                        if (user.Role == "Admin") nextForm = new AdminUsersForm();
                        else if (user.Role == "Moderator" || user.Role == "Manager") nextForm = new MainWork();
                        else nextForm = new UserFeedForm();

                        nextForm.ShowDialog();

                        if (!this.IsDisposed)
                        {
                            txtPassword.Clear();
                            this.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неправильний логін/пошта або пароль!", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("NOT_VERIFIED"))
                    {
                        string realUsername = ex.Message.Split('|')[1];
                        MessageBox.Show("Ваша пошта не підтверджена! Будь ласка, введіть код.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ShowVerificationPrompt(realUsername);
                    }
                }
            }
            else // === РЕЖИМ РЕЄСТРАЦІЇ ===
            {
                string email = txtEmail.Text.Trim();

                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                {
                    MessageBox.Show("Введіть коректну електронну пошту!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (AuthManager.RegisterUser(usernameOrEmail, password, email))
                {
                    MessageBox.Show("Лист з кодом відправлено на вашу пошту!", "Підтвердження", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowVerificationPrompt(usernameOrEmail);
                }
                else
                {
                    MessageBox.Show("Користувач з таким логіном або поштою вже існує!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ShowVerificationPrompt(string username)
        {
            string code = CustomPrompt.ShowDialog("Введіть 4-значний код з листа:", "Підтвердження пошти");
            if (AuthManager.VerifyCode(username, code))
            {
                MessageBox.Show("Пошту успішно підтверджено! Тепер ви можете увійти.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!isLoginMode) SwitchMode();
            }
            else
            {
                MessageBox.Show("Неправильний код або час вийшов.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblSwitchMode_Click(object sender, EventArgs e) => SwitchMode();

        private void SwitchMode()
        {
            isLoginMode = !isLoginMode;
            if (isLoginMode)
            {
                // === РЕЖИМ ВХОДУ ===
                btnLogin.Text = "УВІЙТИ";
                btnLogin.BackColor = Color.MediumSeaGreen;
                lblSwitchMode.Text = "Створити акаунт";
                lblTitle.Text = "WayPoint";
                lblUsername.Text = "Логін або Пошта";

                txtEmail.Visible = false;
                lblEmail.Visible = false;

                lblPassword.Top = 180;
                txtPassword.Top = 206;

                chkShowPassword.Top = 252;
                chkShowPassword.Visible = true;

                lblForgotPassword.Top = 253;
                lblForgotPassword.Visible = true;

                btnLogin.Top = 300;
                lblSwitchMode.Top = 370;
            }
            else
            {
                // === РЕЖИМ РЕЄСТРАЦІЇ ===
                btnLogin.Text = "ЗАРЕЄСТРУВАТИСЯ";
                btnLogin.BackColor = Color.DodgerBlue;
                lblSwitchMode.Text = "Вже є акаунт? Увійти";
                lblTitle.Text = "Реєстрація";
                lblUsername.Text = "Логін";

                txtEmail.Visible = true;
                lblEmail.Visible = true;

                lblPassword.Top = 260;
                txtPassword.Top = 286;

                chkShowPassword.Top = 332;
                chkShowPassword.Visible = true;

                lblForgotPassword.Visible = false; // При реєстрації не треба

                btnLogin.Top = 380;
                lblSwitchMode.Top = 450;
            }

            txtPassword.Clear();
            txtEmail.Clear();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '•';
        }

        private void lblForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string username = CustomPrompt.ShowDialog("Введіть ваш логін:", "Відновлення пароля");
            if (string.IsNullOrEmpty(username)) return;

            string email = AuthManager.GetEmailByUsername(username);
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Користувача з таким логіном не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Генеруємо тимчасовий пароль 
            string tempPass = "WP" + new Random().Next(1000, 9999).ToString();

            // Відправляємо лист
            if (EmailService.SendPasswordReset(email, tempPass))
            {
                AuthManager.UpdatePassword(username, tempPass);
                MessageBox.Show($"Новий пароль відправлено на вашу пошту: {email}", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { btnLogin_Click(this, new EventArgs()); e.Handled = true; e.SuppressKeyPress = true; }
        }

        private void pbExit_Click(object sender, EventArgs e) => Application.Exit();

        // Рух форми (хоча в повноекранному режимі це рідко треба, залишаємо для безпеки)
        private void pnlBackground_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = System.Windows.Forms.Cursor.Position; dragFormPoint = this.Location; }
        private void pnlBackground_MouseMove(object sender, MouseEventArgs e) { if (dragging && this.WindowState != FormWindowState.Maximized) { Point dif = Point.Subtract(System.Windows.Forms.Cursor.Position, new Size(dragCursorPoint)); this.Location = Point.Add(dragFormPoint, new Size(dif)); } }
        private void pnlBackground_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }

    public static class CustomPrompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form() { Width = 350, Height = 160, FormBorderStyle = FormBorderStyle.FixedDialog, Text = caption, StartPosition = FormStartPosition.CenterScreen, ControlBox = false };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, Width = 300, Font = new Font("Segoe UI", 10) };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 300, Font = new Font("Segoe UI", 12) };
            Button confirmation = new Button() { Text = "ОК", Left = 220, Width = 100, Top = 85, DialogResult = DialogResult.OK, BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            confirmation.FlatAppearance.BorderSize = 0;

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : "";
        }
    }
}