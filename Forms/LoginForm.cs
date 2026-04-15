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

            // ФІКС: Примусово задаємо компактний вигляд (Вхід) ЩЕ ДО появи вікна на екрані
            isLoginMode = false; 
            SwitchMode();
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

                        if (user.Role == "Admin")
                        {
                            nextForm = new AdminUsersForm(); // Суперадмін 
                        }
                        else if (user.Role == "Moderator")
                        {
                            nextForm = new MainWork(); // Працівник 
                        }
                        else
                        {
                            nextForm = new UserFeedForm(); // Клієнт
                        }
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
                        // Якщо юзер зайшов по пошті, ми дістаємо його реальний логін з помилки
                        string realUsername = ex.Message.Split('|')[1];
                        MessageBox.Show("Ваша пошта не підтверджена! Будь ласка, введіть код.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ShowVerificationPrompt(realUsername);
                    }
                }
            }
            else // === РЕЖИМ РЕЄСТРАЦІЇ ===
            {
                string email = txtEmail.Text.Trim(); // Тепер беремо пошту з форми!

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

        // Вікно для введення 4-значного коду (його залишаємо спливаючим)
        private void ShowVerificationPrompt(string username)
        {
            string code = CustomPrompt.ShowDialog("Введіть 4-значний код з листа:", "Підтвердження пошти");
            if (AuthManager.VerifyCode(username, code))
            {
                MessageBox.Show("Пошту успішно підтверджено! Тепер ви можете увійти.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!isLoginMode) SwitchMode(); // Перекидаємо на вхід
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
                // === РЕЖИМ ВХОДУ (Компактний) ===
                btnLogin.Text = "УВІЙТИ";
                btnLogin.BackColor = Color.MediumSeaGreen;
                lblSwitchMode.Text = "Створити акаунт";
                lblTitle.Text = "WayPoint";
                lblUsername.Text = "Логін або Пошта";

                txtEmail.Visible = false;
                lblEmail.Visible = false;

                // Піднімаємо пароль і кнопку на місце пошти (на 80 пікселів вгору)
                lblPassword.Top = 200;
                txtPassword.Top = 226;
                btnLogin.Top = 306;
                lblSwitchMode.Top = 376;

                // Зменшуємо висоту самої форми
                this.Height = 450;
            }
            else
            {
                // === РЕЖИМ РЕЄСТРАЦІЇ (Розширений) ===
                btnLogin.Text = "ЗАРЕЄСТРУВАТИСЯ";
                btnLogin.BackColor = Color.DodgerBlue;
                lblSwitchMode.Text = "Вже є акаунт? Увійти";
                lblTitle.Text = "Реєстрація";
                lblUsername.Text = "Логін";

                txtEmail.Visible = true;
                lblEmail.Visible = true;

                // Опускаємо пароль і кнопку вниз, звільняючи місце
                lblPassword.Top = 280;
                txtPassword.Top = 306;
                btnLogin.Top = 386;
                lblSwitchMode.Top = 456;

                // Збільшуємо висоту форми
                this.Height = 550;
            }

            txtPassword.Clear();
            txtEmail.Clear();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { btnLogin_Click(this, new EventArgs()); e.Handled = true; e.SuppressKeyPress = true; }
        }

        private void pbExit_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(Color.White, 3)) { g.DrawLine(pen, 8, 8, 22, 22); g.DrawLine(pen, 22, 8, 8, 22); }
        }

        private void pbExit_Click(object sender, EventArgs e) => Application.Exit();
        private void pnlBackground_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = System.Windows.Forms.Cursor.Position; dragFormPoint = this.Location; }
        private void pnlBackground_MouseMove(object sender, MouseEventArgs e) { if (dragging) { Point dif = Point.Subtract(System.Windows.Forms.Cursor.Position, new Size(dragCursorPoint)); this.Location = Point.Add(dragFormPoint, new Size(dif)); } }
        private void pnlBackground_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }

    // Допоміжний клас для міні-вікна коду
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