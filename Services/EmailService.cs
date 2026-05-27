using System;
using System.Windows.Forms;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace WayPoint.Services
{
    public static class EmailService
    {
        // ВПИШИ СЮДИ СВІЙ GMAIL, з якого будуть відправлятися листи
        private static string myEmail = "hofart33w@gmail.com";

        // Твій пароль додатка
        private static string appPassword = "cjymnzhvswbkjhbv";

        public static bool SendVerificationCode(string toEmail, string code)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("WayPoint Security", myEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Код підтвердження WayPoint";

                // Гарний HTML-лист
                message.Body = new TextPart("html")
                {
                    Text = $@"
                        <div style='font-family: Arial; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                            <h2 style='color: #2E8B57;'>Вітаємо у WayPoint!</h2>
                            <p>Щоб завершити реєстрацію, введіть цей 4-значний код у програмі:</p>
                            <h1 style='background: #f4f4f4; padding: 10px; width: fit-content; letter-spacing: 5px;'>{code}</h1>
                            <p style='color: gray; font-size: 12px;'>Код діє 15 хвилин.</p>
                        </div>"
                };

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

        // НОВИЙ МЕТОД: ВІДНОВЛЕННЯ ПАРОЛЯ
        public static bool SendPasswordReset(string toEmail, string newPassword)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("WayPoint Security", myEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Відновлення пароля WayPoint";

                message.Body = new TextPart("html")
                {
                    Text = $@"
                        <div style='font-family: Arial; padding: 20px; border: 1px solid #ddd; border-radius: 8px;'>
                            <h2 style='color: #E63946;'>Відновлення доступу</h2>
                            <p>Ваш новий тимчасовий пароль для входу:</p>
                            <h1 style='background: #f4f4f4; padding: 10px; width: fit-content; letter-spacing: 2px;'>{newPassword}</h1>
                            <p style='color: gray; font-size: 12px;'>Будь ласка, змініть його після входу в систему в налаштуваннях.</p>
                        </div>"
                };

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