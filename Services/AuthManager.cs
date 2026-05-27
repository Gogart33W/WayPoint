using System;
using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace WayPoint.Services
{
    public static class AuthManager
    {
        public static void LoadUsers() { } // Заглушка, щоб не ламався старий код

        public static bool RegisterUser(string username, string password, string email, string role = "User")
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    // 1. Перевірка чи є такий логін або пошта
                    string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = @u OR Email = @e";
                    var checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@u", username);
                    checkCmd.Parameters.AddWithValue("@e", email);
                    if ((int)checkCmd.ExecuteScalar() > 0) return false;

                    // 2. Шифруємо пароль і генеруємо код
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                    string code = new Random().Next(1000, 9999).ToString();

                    // 3. Записуємо в базу (статус підтвердження = 0)
                    string sql = @"INSERT INTO Users (Username, Password, Role, Email, IsEmailVerified, VerificationCode, CodeExpiry) 
                                   VALUES (@u, @p, @r, @e, 0, @c, @exp)";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", hashedPassword);
                    cmd.Parameters.AddWithValue("@r", role);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@c", code);
                    cmd.Parameters.AddWithValue("@exp", DateTime.Now.AddMinutes(15));
                    cmd.ExecuteNonQuery();

                    // 4. Відправляємо код на пошту
                    EmailService.SendVerificationCode(email, code);
                    return true;
                }
            }
            catch { return false; }
        }

        public static bool VerifyCode(string username, string code)
        {
            using (var conn = DatabaseService.GetConnection())
            {
                string sql = "SELECT VerificationCode, CodeExpiry FROM Users WHERE Username=@u";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string dbCode = reader["VerificationCode"]?.ToString();
                        DateTime expiry = reader["CodeExpiry"] != DBNull.Value ? Convert.ToDateTime(reader["CodeExpiry"]) : DateTime.MinValue;

                        if (dbCode == code && DateTime.Now <= expiry)
                        {
                            reader.Close();
                            // Підтверджуємо пошту
                            var updateCmd = new SqlCommand("UPDATE Users SET IsEmailVerified=1, VerificationCode=NULL WHERE Username=@u", conn);
                            updateCmd.Parameters.AddWithValue("@u", username);
                            updateCmd.ExecuteNonQuery();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static Models.User Login(string loginOrEmail, string password)
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    // Шукаємо юзера АБО по логіну, АБО по пошті
                    string sql = "SELECT Username, Password, Role, IsEmailVerified FROM Users WHERE Username = @u OR Email = @u";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", loginOrEmail);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dbHash = reader["Password"].ToString();
                            bool isVerified = Convert.ToBoolean(reader["IsEmailVerified"]);
                            string realUsername = reader["Username"].ToString();

                            // Перевіряємо хеш пароля
                            if (BCrypt.Net.BCrypt.Verify(password, dbHash))
                            {
                                // Якщо пароль правильний, але пошта не підтверджена
                                if (!isVerified) throw new Exception("NOT_VERIFIED|" + realUsername);

                                var user = new Models.User
                                {
                                    Username = realUsername,
                                    Role = reader["Role"].ToString()
                                };
                                Session.Username = user.Username;
                                Session.Role = user.Role;
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Прокидаємо помилку верифікації на форму
                if (ex.Message.StartsWith("NOT_VERIFIED")) throw;
            }
            return null;
        }

        // НОВІ МЕТОДИ ДЛЯ ВІДНОВЛЕННЯ ПАРОЛЯ
        public static string GetEmailByUsername(string username)
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT Email FROM Users WHERE Username=@u", conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    var res = cmd.ExecuteScalar();
                    if (res != null && res != DBNull.Value) return res.ToString();
                }
            }
            catch { }
            return null;
        }

        public static void UpdatePassword(string username, string newPassword)
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    string hash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    var cmd = new SqlCommand("UPDATE Users SET Password=@p WHERE Username=@u", conn);
                    cmd.Parameters.AddWithValue("@p", hash);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }
    }
}