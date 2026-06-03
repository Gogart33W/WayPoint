using System;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace WayPoint.Services
{
    public static class DatabaseService
    {
        public static SqlConnection GetConnection()
        {
            // Беремо рядок підключення з config.json
            var connection = new SqlConnection(ConfigManager.Config.DbConnectionString);

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    // Якщо підключилися успішно — просто йдемо далі
                }
            }
            catch (SqlException ex)
            {
                // Якщо помилка 40 (сервер не знайдено) або 52 (не створена база)
                MessageBox.Show($"Помилка підключення до Бази Даних:\n{ex.Message}\n\nЩоб змінити підключення, натисніть Ctrl+Shift+S на вікні логіну.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Загальна помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}