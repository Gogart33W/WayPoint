using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace WayPoint.Services
{
    public class AppConfig
    {
        public string DbConnectionString { get; set; } = @"Server=(localdb)\MSSQLLocalDB;Database=WayPointDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";
        public string SmtpEmail { get; set; } = "твоя_пошта@gmail.com";
        public string SmtpPassword { get; set; } = "твій_пароль_додатка";
    }

    public static class ConfigManager
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        private static AppConfig _config;

        public static AppConfig Config
        {
            get
            {
                if (_config == null) LoadConfig();
                return _config;
            }
        }

        public static void LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    _config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
                else
                {
                    _config = new AppConfig();
                    SaveConfig(); // Створюємо дефолтний файл, якщо його немає
                }
            }
            catch
            {
                _config = new AppConfig();
            }
        }

        public static void SaveConfig()
        {
            try
            {
                string json = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка збереження config.json: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}