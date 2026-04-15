using System;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace WayPoint.Services
{
    public static class DatabaseService
    {
        // Рядок підключення саме для твого сервера (localdb)\MSSQLLocalDB
        private static string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=WayPointDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
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
                MessageBox.Show($"Помилка підключення до (localdb):\n{ex.Message}\n\nПеревір, чи створена база 'WayPointDB' у SSMS.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Загальна помилка: " + ex.Message);
            }
        }
    }
}