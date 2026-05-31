using System;
using System.Windows.Forms;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace WayPoint.Services
{
    public static class EmailService
    {
        // Твій GMAIL
        private static string myEmail = "hofart33w@gmail.com";

        // Твій пароль додатка
        private static string appPassword = "fngckxswddfzgzxf";

        public static bool SendVerificationCode(string toEmail, string code)
        {
            try
            {
                var message = new MimeMessage();
                // Змінив на WayPoint Team (менше підозр для спам-фільтрів)
                message.From.Add(new MailboxAddress("WayPoint Team", myEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Код підтвердження WayPoint";

                // ВИКОРИСТОВУЄМО BODYBUILDER ДЛЯ ОБХОДУ СПАМУ
                var builder = new BodyBuilder();

                // 1. Обов'язкова текстова версія (роботи поштовиків перевіряють її)
                builder.TextBody = $"Вітаємо у WayPoint!\nЩоб завершити реєстрацію, введіть цей 4-значний код: {code}\nКод діє 15 хвилин.";

                // 2. Гарний HTML-лист
                builder.HtmlBody = $@"
                    <div style='font-family: Arial; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                        <h2 style='color: #2E8B57;'>Вітаємо у WayPoint!</h2>
                        <p>Щоб завершити реєстрацію, введіть цей 4-значний код у програмі:</p>
                        <h1 style='background: #f4f4f4; padding: 10px; width: fit-content; letter-spacing: 5px;'>{code}</h1>
                        <p style='color: gray; font-size: 12px;'>Код діє 15 хвилин.</p>
                    </div>";

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(myEmail, appPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка відправки листа: " + ex.Message);
                return false;
            }
        }

        public static bool SendPasswordReset(string toEmail, string newPassword)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("WayPoint Team", myEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Відновлення пароля WayPoint";

                var builder = new BodyBuilder();

                builder.TextBody = $"Відновлення доступу.\nВаш новий тимчасовий пароль: {newPassword}\nБудь ласка, змініть його після входу в систему.";

                builder.HtmlBody = $@"
                    <div style='font-family: Arial; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                        <h2 style='color: #E63946;'>Відновлення доступу</h2>
                        <p>Ваш новий тимчасовий пароль для входу:</p>
                        <h1 style='background: #f4f4f4; padding: 10px; width: fit-content; letter-spacing: 2px;'>{newPassword}</h1>
                        <p style='color: gray; font-size: 12px;'>Будь ласка, змініть його після входу в систему в налаштуваннях.</p>
                    </div>";

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(myEmail, appPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка відправки листа: " + ex.Message);
                return false;
            }
        }
    }
}