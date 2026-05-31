using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WayPoint.Services;

namespace WayPoint
{
    public partial class LoginForm : Form
    {
        private bool isLoginMode = true;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Кольори для рамок
        private Color normalBorder = Color.FromArgb(203, 213, 225); // Світло-сірий
        private Color focusedBorder = Color.FromArgb(79, 70, 229);  // Синій фокус

        public LoginForm()
        {
            InitializeComponent();
            SoundHelper.AttachSounds(this); // ПІДКЛЮЧЕНО ЗВУК
            isLoginMode = false;
            SwitchMode();
        }

        // --- ДИНАМІЧНЕ ЦЕНТРУВАННЯ ---
        private void LoginForm_Resize(object sender, EventArgs e)
        {
            if (pnlCenter != null)
            {
                pnlCenter.Left = (this.ClientSize.Width - pnlCenter.Width) / 2;
                pnlCenter.Top = (this.ClientSize.Height - pnlCenter.Height) / 2;
            }
        }

        // --- МАЛЮВАННЯ ТОНКИХ РАМОК ІНПУТІВ ---
        private void pnlInput_Paint(object sender, PaintEventArgs e)
        {
            Panel pnl = sender as Panel;
            Color borderColor = normalBorder;

            foreach (Control c in pnl.Controls)
            {
                if (c is TextBox && c.Focused)
                {
                    borderColor = focusedBorder;
                    break;
                }
            }
            ControlPaint.DrawBorder(e.Graphics, pnl.ClientRectangle, borderColor, ButtonBorderStyle.Solid);
        }

        private void pnlCenter_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, pnlCenter.ClientRectangle, Color.FromArgb(226, 232, 240), ButtonBorderStyle.Solid);
        }

        private void Input_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            txt.Parent.Invalidate();
        }

        private void Input_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            txt.Parent.Invalidate();
        }

        // --- ПЕРЕВІРКА НА ТИМЧАСОВИЙ ПАРОЛЬ ---
        private bool IsTemporaryPassword(string password)
        {
            // Перевіряємо чи пароль починається на "WP" і далі йдуть рівно 4 цифри
            return Regex.IsMatch(password, @"^WP\d{4}$");
        }

        // --- ЛОГІКА АВТОРИЗАЦІЇ ---
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isLoginMode)
            {
                try
                {
                    var user = AuthManager.Login(usernameOrEmail, password);
                    if (user != null)
                    {
                        // ПЕРЕХОПЛЕННЯ ТИМЧАСОВОГО ПАРОЛЯ
                        if (IsTemporaryPassword(password))
                        {
                            string newPass = ChangePasswordPrompt.ShowDialog(
                                "Оновлення безпеки",
                                "Ви увійшли за допомогою тимчасового пароля.\nЗ міркувань безпеки, створіть новий пароль.");

                            if (string.IsNullOrEmpty(newPass))
                            {
                                MessageBox.Show("Ви повинні змінити пароль, щоб увійти в систему.", "Вхід скасовано", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Блокуємо вхід, якщо юзер натиснув "Скасувати"
                            }

                            // Зберігаємо новий пароль у базу
                            AuthManager.UpdatePassword(user.Username, newPass);
                            MessageBox.Show("Ваш пароль успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        // Якщо все ок - пускаємо в систему
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
            else
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
            string code = CustomPrompt.ShowDialog("Введіть 4-значний код підтвердження:", "Безпека акаунта");
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

            int startY = 120;
            int gap = 15;

            if (isLoginMode)
            {
                // === РЕЖИМ ВХОДУ ===
                btnLogin.Text = "УВІЙТИ";
                btnLogin.BackColor = Color.FromArgb(16, 185, 129); // Emerald-500
                lblSubtitle.Text = "Вхід у систему";
                lblUsername.Text = "Логін або Пошта";

                txtEmail.Visible = false;
                lblEmail.Visible = false;
                pnlEmailBorder.Visible = false;

                lblUsername.Top = startY;
                pnlUserBorder.Top = lblUsername.Bottom + 5;

                lblPassword.Top = pnlUserBorder.Bottom + gap;
                pnlPassBorder.Top = lblPassword.Bottom + 5;

                chkShowPassword.Top = pnlPassBorder.Bottom + 10;
                chkShowPassword.Visible = true;

                lblForgotPassword.Top = chkShowPassword.Top;
                lblForgotPassword.Visible = true;
                lblForgotPassword.Left = pnlPassBorder.Right - lblForgotPassword.Width;

                btnLogin.Top = chkShowPassword.Bottom + 25;

                lblNoAccount.Text = "Немає акаунта?";
                lblSwitchMode.Text = "Створити акаунт";
            }
            else
            {
                // === РЕЖИМ РЕЄСТРАЦІЇ ===
                btnLogin.Text = "ЗАРЕЄСТРУВАТИСЯ";
                btnLogin.BackColor = Color.FromArgb(79, 70, 229); // Indigo-600
                lblSubtitle.Text = "Створення нового акаунта";
                lblUsername.Text = "Придумайте логін";

                txtEmail.Visible = true;
                lblEmail.Visible = true;
                pnlEmailBorder.Visible = true;

                lblUsername.Top = startY;
                pnlUserBorder.Top = lblUsername.Bottom + 5;

                lblEmail.Top = pnlUserBorder.Bottom + gap;
                pnlEmailBorder.Top = lblEmail.Bottom + 5;

                lblPassword.Top = pnlEmailBorder.Bottom + gap;
                pnlPassBorder.Top = lblPassword.Bottom + 5;

                chkShowPassword.Top = pnlPassBorder.Bottom + 10;
                chkShowPassword.Visible = true;

                lblForgotPassword.Visible = false;

                btnLogin.Top = chkShowPassword.Bottom + 25;

                lblNoAccount.Text = "Вже є акаунт?";
                lblSwitchMode.Text = "Увійти";
            }

            lblTitle.Left = (pnlCenter.Width - lblTitle.Width) / 2;
            lblSubtitle.Left = (pnlCenter.Width - lblSubtitle.Width) / 2;

            lblNoAccount.Top = btnLogin.Bottom + 20;
            lblSwitchMode.Top = lblNoAccount.Top;

            int totalFooterWidth = lblNoAccount.Width + 5 + lblSwitchMode.Width;
            int startX = (pnlCenter.Width - totalFooterWidth) / 2;
            lblNoAccount.Left = startX;
            lblSwitchMode.Left = startX + lblNoAccount.Width + 5;

            txtPassword.Clear();
            txtEmail.Clear();
            txtUsername.Focus();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void lblForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string username = CustomPrompt.ShowDialog("Введіть ваш логін у CRM:", "Відновлення пароля");
            if (string.IsNullOrEmpty(username)) return;

            string email = AuthManager.GetEmailByUsername(username);
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Користувача з таким логіном не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tempPass = "WP" + new Random().Next(1000, 9999).ToString();

            if (EmailService.SendPasswordReset(email, tempPass))
            {
                AuthManager.UpdatePassword(username, tempPass);
                MessageBox.Show($"Новий тимчасовий пароль відправлено на вашу пошту: {email}", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { btnLogin_Click(this, new EventArgs()); e.Handled = true; e.SuppressKeyPress = true; }
        }

        private void btnExit_Click(object sender, EventArgs e) => Application.Exit();

        private void pnlBackground_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = System.Windows.Forms.Cursor.Position; dragFormPoint = this.Location; }
        private void pnlBackground_MouseMove(object sender, MouseEventArgs e) { if (dragging && this.WindowState != FormWindowState.Maximized) { Point dif = Point.Subtract(System.Windows.Forms.Cursor.Position, new Size(dragCursorPoint)); this.Location = Point.Add(dragFormPoint, new Size(dif)); } }
        private void pnlBackground_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }

    // ===== ПРОМПТ ДЛЯ ЗВИЧАЙНОГО ВВЕДЕННЯ (Код / Логін) =====
    public static class CustomPrompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 420,
                Height = 240,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.White
            };

            prompt.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, prompt.ClientRectangle, Color.FromArgb(79, 70, 229), ButtonBorderStyle.Solid);
            };

            Label lblCaption = new Label() { Left = 30, Top = 25, Text = caption, Width = 360, Font = new Font("Segoe UI Black", 14, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42) };
            Label textLabel = new Label() { Left = 30, Top = 65, Text = text, Width = 360, Height = 25, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };

            Panel pnlInput = new Panel() { Left = 30, Top = 95, Width = 360, Height = 42, BackColor = Color.White, Padding = new Padding(1) };
            TextBox textBox = new TextBox() { Location = new Point(5, 5), Width = 350, Font = new Font("Segoe UI", 13), BackColor = Color.White, BorderStyle = BorderStyle.None, ForeColor = Color.Black };
            pnlInput.Controls.Add(textBox);

            pnlInput.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, pnlInput.ClientRectangle, textBox.Focused ? Color.FromArgb(79, 70, 229) : Color.FromArgb(203, 213, 225), ButtonBorderStyle.Solid);
            };
            textBox.Enter += (s, e) => pnlInput.Invalidate();
            textBox.Leave += (s, e) => pnlInput.Invalidate();

            Button confirmation = new Button() { Text = "ПІДТВЕРДИТИ", Top = 165, Height = 45, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(79, 70, 229), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            confirmation.FlatAppearance.BorderSize = 0;
            confirmation.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            confirmation.Cursor = Cursors.Hand;

            Button cancel = new Button() { Text = "СКАСУВАТИ", Top = 165, Height = 45, DialogResult = DialogResult.Cancel, BackColor = Color.FromArgb(241, 245, 249), ForeColor = Color.FromArgb(71, 85, 105), FlatStyle = FlatStyle.Flat };
            cancel.FlatAppearance.BorderSize = 0;
            cancel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            cancel.Cursor = Cursors.Hand;

            int buttonWidth = (prompt.Width - 60 - 15) / 2;
            cancel.Width = buttonWidth;
            cancel.Left = 30;
            confirmation.Width = buttonWidth;
            confirmation.Left = cancel.Right + 15;

            prompt.Controls.Add(lblCaption);
            prompt.Controls.Add(pnlInput);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(textLabel);

            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            SoundHelper.AttachSounds(prompt); // ПІДКЛЮЧЕНО ЗВУК ДО ВІКНА

            prompt.Shown += (s, e) => textBox.Focus();

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : "";
        }
    }

    // ===== НОВИЙ ПРОМПТ ДЛЯ ЗМІНИ ТИМЧАСОВОГО ПАРОЛЯ =====
    public static class ChangePasswordPrompt
    {
        public static string ShowDialog(string caption, string subtitle)
        {
            Form prompt = new Form()
            {
                Width = 420,
                Height = 350,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.White
            };

            prompt.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, prompt.ClientRectangle, Color.FromArgb(79, 70, 229), ButtonBorderStyle.Solid);
            };

            Label lblCaption = new Label() { Left = 30, Top = 20, Text = caption, Width = 360, Font = new Font("Segoe UI Black", 14, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42) };
            Label lblSubtitle = new Label() { Left = 30, Top = 50, Text = subtitle, Width = 360, Height = 40, Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(100, 116, 139) };

            // Новий пароль
            Label lblPass1 = new Label() { Left = 30, Top = 100, Text = "Новий пароль:", Width = 360, Height = 20, Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            Panel pnlInput1 = new Panel() { Left = 30, Top = 125, Width = 360, Height = 42, BackColor = Color.White, Padding = new Padding(1) };
            TextBox txtPass1 = new TextBox() { Location = new Point(5, 5), Width = 350, Font = new Font("Segoe UI", 13), BackColor = Color.White, BorderStyle = BorderStyle.None, ForeColor = Color.Black, UseSystemPasswordChar = true };
            pnlInput1.Controls.Add(txtPass1);
            pnlInput1.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, pnlInput1.ClientRectangle, txtPass1.Focused ? Color.FromArgb(79, 70, 229) : Color.FromArgb(203, 213, 225), ButtonBorderStyle.Solid);
            txtPass1.Enter += (s, e) => pnlInput1.Invalidate();
            txtPass1.Leave += (s, e) => pnlInput1.Invalidate();

            // Повтор пароля
            Label lblPass2 = new Label() { Left = 30, Top = 175, Text = "Повторіть пароль:", Width = 360, Height = 20, Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold), ForeColor = Color.FromArgb(71, 85, 105) };
            Panel pnlInput2 = new Panel() { Left = 30, Top = 200, Width = 360, Height = 42, BackColor = Color.White, Padding = new Padding(1) };
            TextBox txtPass2 = new TextBox() { Location = new Point(5, 5), Width = 350, Font = new Font("Segoe UI", 13), BackColor = Color.White, BorderStyle = BorderStyle.None, ForeColor = Color.Black, UseSystemPasswordChar = true };
            pnlInput2.Controls.Add(txtPass2);
            pnlInput2.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, pnlInput2.ClientRectangle, txtPass2.Focused ? Color.FromArgb(79, 70, 229) : Color.FromArgb(203, 213, 225), ButtonBorderStyle.Solid);
            txtPass2.Enter += (s, e) => pnlInput2.Invalidate();
            txtPass2.Leave += (s, e) => pnlInput2.Invalidate();

            Button confirmation = new Button() { Text = "ЗБЕРЕГТИ", Top = 270, Height = 45, DialogResult = DialogResult.None, BackColor = Color.FromArgb(79, 70, 229), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            confirmation.FlatAppearance.BorderSize = 0;
            confirmation.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            confirmation.Cursor = Cursors.Hand;

            Button cancel = new Button() { Text = "СКАСУВАТИ", Top = 270, Height = 45, DialogResult = DialogResult.Cancel, BackColor = Color.FromArgb(241, 245, 249), ForeColor = Color.FromArgb(71, 85, 105), FlatStyle = FlatStyle.Flat };
            cancel.FlatAppearance.BorderSize = 0;
            cancel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            cancel.Cursor = Cursors.Hand;

            int buttonWidth = (prompt.Width - 60 - 15) / 2;
            cancel.Width = buttonWidth;
            cancel.Left = 30;
            confirmation.Width = buttonWidth;
            confirmation.Left = cancel.Right + 15;

            // Валідація
            confirmation.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtPass1.Text))
                {
                    MessageBox.Show("Пароль не може бути порожнім!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtPass1.Text != txtPass2.Text)
                {
                    MessageBox.Show("Паролі не співпадають!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                prompt.DialogResult = DialogResult.OK;
            };

            prompt.Controls.Add(lblCaption);
            prompt.Controls.Add(lblSubtitle);
            prompt.Controls.Add(lblPass1);
            prompt.Controls.Add(pnlInput1);
            prompt.Controls.Add(lblPass2);
            prompt.Controls.Add(pnlInput2);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);

            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            SoundHelper.AttachSounds(prompt); // ПІДКЛЮЧЕНО ЗВУК ДО ВІКНА

            prompt.Shown += (s, e) => txtPass1.Focus();

            return prompt.ShowDialog() == DialogResult.OK ? txtPass1.Text.Trim() : "";
        }
    }
}