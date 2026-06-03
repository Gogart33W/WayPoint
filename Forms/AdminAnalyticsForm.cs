using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;

namespace WayPoint.Forms
{
    public partial class AdminAnalyticsForm : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public AdminAnalyticsForm()
        {
            InitializeComponent();

            // Налаштування перетягування вікна
            pnlHeader.MouseDown += (s, e) => { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; };
            pnlHeader.MouseMove += (s, e) => {
                if (dragging && this.WindowState != FormWindowState.Maximized)
                {
                    int dx = Cursor.Position.X - dragCursorPoint.X;
                    int dy = Cursor.Position.Y - dragCursorPoint.Y;
                    this.Location = new Point(dragFormPoint.X + dx, dragFormPoint.Y + dy);
                }
            };
            pnlHeader.MouseUp += (s, e) => dragging = false;

            // ФІКС СИНЬОГО ВИДІЛЕННЯ: прибираємо будь-яке виділення в таблиці
            dgvManagerStats.SelectionChanged += (s, e) => dgvManagerStats.ClearSelection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AdminAnalyticsForm_Load(object sender, EventArgs e)
        {
            cmbPeriod.SelectedIndex = 0; // По замовчуванню "За весь час"
            LoadAllAnalytics();
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPeriod.Text == "Обрати період...")
            {
                dtpStart.Visible = true;
                dtpEnd.Visible = true;
                btnApply.Visible = true;
            }
            else
            {
                dtpStart.Visible = false;
                dtpEnd.Visible = false;
                btnApply.Visible = false;
                LoadAllAnalytics();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            LoadAllAnalytics();
        }

        // ФІКС ДАТ: Тепер місяць і тиждень захоплюють МАЙБУТНІ дати поточного періоду
        private void GetDateRange(out DateTime startDate, out DateTime endDate)
        {
            startDate = new DateTime(2000, 1, 1);
            endDate = DateTime.MaxValue;
            DateTime today = DateTime.Today;

            switch (cmbPeriod.Text)
            {
                case "За сьогодні":
                    startDate = today;
                    endDate = today.AddDays(1).AddSeconds(-1);
                    break;
                case "Поточний тиждень":
                    int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = today.AddDays(-diff); // З понеділка
                    endDate = startDate.AddDays(7).AddSeconds(-1); // До неділі
                    break;
                case "Поточний місяць":
                    startDate = new DateTime(today.Year, today.Month, 1); // 1-ше число
                    endDate = startDate.AddMonths(1).AddSeconds(-1); // Останнє число місяця
                    break;
                case "Обрати період...":
                    startDate = dtpStart.Value.Date;
                    endDate = dtpEnd.Value.Date.AddDays(1).AddSeconds(-1);
                    break;
            }
        }

        private void LoadAllAnalytics()
        {
            GetDateRange(out DateTime startDate, out DateTime endDate);

            string dateFilterT = cmbPeriod.Text == "За весь час" ? "1=1" : "DepartureDate >= @start AND DepartureDate <= @end";
            string dateFilterM = cmbPeriod.Text == "За весь час" ? "1=1" : "SentAt >= @start AND SentAt <= @end";

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    // 1. Усього працівників
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role IN ('Manager', 'Moderator')", conn))
                    {
                        int empCount = (int)cmd.ExecuteScalar();
                        lblEmpValue.Text = empCount.ToString();
                    }

                    // 2. Всього турів за період
                    using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM Travels WHERE {dateFilterT}", conn))
                    {
                        if (cmbPeriod.Text != "За весь час") { cmd.Parameters.AddWithValue("@start", startDate); cmd.Parameters.AddWithValue("@end", endDate); }
                        int toursCount = (int)cmd.ExecuteScalar();
                        lblToursValue.Text = toursCount.ToString();
                    }

                    // 3. Загальний обіг 
                    using (var cmd = new SqlCommand($"SELECT ISNULL(SUM(Budget), 0) FROM Travels WHERE (Status LIKE N'%Оплачено%' OR Status LIKE N'%Завершено%') AND {dateFilterT}", conn))
                    {
                        if (cmbPeriod.Text != "За весь час") { cmd.Parameters.AddWithValue("@start", startDate); cmd.Parameters.AddWithValue("@end", endDate); }
                        decimal revenue = Convert.ToDecimal(cmd.ExecuteScalar());
                        lblRevValue.Text = $"${revenue.ToString("N0")}";
                    }

                    // 4. Повідомлень в чатах
                    using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM Messages WHERE SenderUsername IN (SELECT Username FROM Users WHERE Role != 'User') AND {dateFilterM}", conn))
                    {
                        if (cmbPeriod.Text != "За весь час") { cmd.Parameters.AddWithValue("@start", startDate); cmd.Parameters.AddWithValue("@end", endDate); }
                        int msgCount = (int)cmd.ExecuteScalar();
                        lblMsgValue.Text = msgCount.ToString();
                    }

                    // ГРАФІК 1: СТАТУСИ
                    chartStatuses.Series["Statuses"].Points.Clear();
                    using (var cmd = new SqlCommand($"SELECT Status, COUNT(*) as Cnt FROM Travels WHERE {dateFilterT} GROUP BY Status", conn))
                    {
                        if (cmbPeriod.Text != "За весь час") { cmd.Parameters.AddWithValue("@start", startDate); cmd.Parameters.AddWithValue("@end", endDate); }
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string status = reader["Status"].ToString();
                                int count = Convert.ToInt32(reader["Cnt"]);
                                chartStatuses.Series["Statuses"].Points.AddXY(status, count);
                            }
                        }
                    }

                    // ГРАФІК 2: ДИНАМІКА
                    chartDynamics.Series["Revenue"].Points.Clear();
                    string dynSql = $@"
                        SELECT CAST(DepartureDate AS DATE) as Dt, SUM(Budget) as Total 
                        FROM Travels 
                        WHERE (Status LIKE N'%Оплачено%' OR Status LIKE N'%Завершено%') 
                          AND DepartureDate IS NOT NULL 
                          AND {dateFilterT}
                        GROUP BY CAST(DepartureDate AS DATE) 
                        ORDER BY Dt";

                    using (var cmd = new SqlCommand(dynSql, conn))
                    {
                        if (cmbPeriod.Text != "За весь час") { cmd.Parameters.AddWithValue("@start", startDate); cmd.Parameters.AddWithValue("@end", endDate); }
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime dt = Convert.ToDateTime(reader["Dt"]);
                                decimal total = Convert.ToDecimal(reader["Total"]);
                                chartDynamics.Series["Revenue"].Points.AddXY(dt.ToShortDateString(), total);
                            }
                        }
                    }

                    // ТАБЛИЦЯ: ПРОДУКТИВНІСТЬ
                    string statsSql = $@"
                        SELECT 
                            u.Username AS [Працівник],
                            COUNT(t.ID) AS [Усього турів],
                            SUM(CASE WHEN t.Status NOT LIKE N'%Завершено%' AND t.Status NOT LIKE N'%Скасовано%' THEN 1 ELSE 0 END) AS [В роботі (Активні)],
                            SUM(CASE WHEN t.Status LIKE N'%Завершено%' THEN 1 ELSE 0 END) AS [Успішно завершено],
                            SUM(CASE WHEN t.Status LIKE N'%Скасовано%' THEN 1 ELSE 0 END) AS [Скасовано клієнтом],
                            ISNULL(SUM(CASE WHEN t.Status LIKE N'%Оплачено%' OR t.Status LIKE N'%Завершено%' THEN t.Budget ELSE 0 END), 0) AS [Прибуток ($)],
                            ISNULL((SELECT COUNT(*) FROM Messages m WHERE m.SenderUsername = u.Username AND ({dateFilterM})), 0) AS [Відправлено повідомлень]
                        FROM Users u
                        LEFT JOIN Travels t ON u.Username = t.Manager AND ({dateFilterT})
                        WHERE u.Role IN ('Manager', 'Moderator')
                        GROUP BY u.Username
                        ORDER BY [Усього турів] DESC, [Відправлено повідомлень] DESC";

                    using (var cmd = new SqlCommand(statsSql, conn))
                    {
                        if (cmbPeriod.Text != "За весь час") { cmd.Parameters.AddWithValue("@start", startDate); cmd.Parameters.AddWithValue("@end", endDate); }

                        DataTable dtStats = new DataTable();
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtStats);
                        }
                        dgvManagerStats.DataSource = dtStats;
                        dgvManagerStats.ClearSelection(); // Знімаємо виділення після завантаження
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження аналітики: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}