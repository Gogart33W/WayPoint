using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace WayPoint.Services
{
    public static class AuthManager
    {
        // === ТИМЧАСОВЕ СХОВИЩЕ (Щоб не засмічувати БД до підтвердження) ===
        private class PendingUser
        {
            public string Username { get; set; }
            public string PasswordHash { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public string Code { get; set; }
            public DateTime Expiry { get; set; }
        }

        private static Dictionary<string, PendingUser> _pendingUsers = new Dictionary<string, PendingUser>(StringComparer.OrdinalIgnoreCase);

        public static void LoadUsers() { } // Заглушка, щоб не ламався старий код

        public static bool RegisterUser(string username, string password, string email, string role = "User")
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                    // 1. Перевірка чи є такий логін або пошта В БАЗІ
                    string checkSql = "SELECT COUNT(*) FROM Users WHERE Username = @u OR Email = @e";
                    var checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@u", username);
                    checkCmd.Parameters.AddWithValue("@e", email);
                    if ((int)checkCmd.ExecuteScalar() > 0) return false;

                    // 2. Перевірка чи юзер вже не висить у процесі реєстрації (щоб не спамили)
                    if (_pendingUsers.ContainsKey(username)) _pendingUsers.Remove(username);

                    // 3. Шифруємо пароль і генеруємо код
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                    string code = new Random().Next(1000, 9999).ToString();

                    // 4. ЗАПИСУЄМО В ТИМЧАСОВУ ПАМ'ЯТЬ (БД залишається чистою!)
                    _pendingUsers[username] = new PendingUser
                    {
                        Username = username,
                        PasswordHash = hashedPassword,
                        Email = email,
                        Role = role,
                        Code = code,
                        Expiry = DateTime.Now.AddMinutes(15)
                    };

                    // 5. Відправляємо код на пошту
                    EmailService.SendVerificationCode(email, code);
                    return true;
                }
            }
            catch { return false; }
        }

        public static bool VerifyCode(string username, string code)
        {
            try
            {
                // 1. Шукаємо юзера в тимчасовій пам'яті
                if (_pendingUsers.TryGetValue(username, out var pending))
                {
                    // 2. Перевіряємо код і час дії
                    if (pending.Code == code && DateTime.Now <= pending.Expiry)
                    {
                        // 3. КОД ПРАВИЛЬНИЙ -> ТІЛЬКИ ТЕПЕР ЗАПИСУЄМО В БАЗУ
                        using (var conn = DatabaseService.GetConnection())
                        {
                            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                            string sql = @"INSERT INTO Users (Username, Password, Role, Email, IsEmailVerified, VerificationCode, CodeExpiry) 
                                           VALUES (@u, @p, @r, @e, 1, NULL, NULL)"; // Одразу ставимо IsEmailVerified = 1

                            var cmd = new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@u", pending.Username);
                            cmd.Parameters.AddWithValue("@p", pending.PasswordHash);
                            cmd.Parameters.AddWithValue("@r", pending.Role);
                            cmd.Parameters.AddWithValue("@e", pending.Email);
                            cmd.ExecuteNonQuery();
                        }

                        // Видаляємо з тимчасового сховища
                        _pendingUsers.Remove(username);
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        public static Models.User Login(string loginOrEmail, string password)
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != System.Data.ConnectionState.Open) conn.Open();

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
                                // Якщо пароль правильний, але пошта не підтверджена (на випадок старих акаунтів у БД)
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
                if (ex.Message.StartsWith("NOT_VERIFIED")) throw;
            }
            return null;
        }

        public static string GetEmailByUsername(string username)
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
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
                    if (conn.State != System.Data.ConnectionState.Open) conn.Open();
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