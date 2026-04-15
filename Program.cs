using System;
using System.Windows.Forms;

namespace WayPoint
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Ініціалізуємо БД перед запуском вікон
            WayPoint.Services.DatabaseService.InitializeDatabase();

            Application.Run(new LoginForm());
        }
    }
}