using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;

namespace WayPoint
{
    public partial class UserFeedForm : Form
    {
        private DataTable travelTable = new DataTable();
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public UserFeedForm()
        {
            InitializeComponent();

            // Якщо сесія пуста, ставимо дефолтні значення
            if (string.IsNullOrEmpty(Session.Username))
            {
                Session.Username = "Guest";
                Session.Role = "User";
            }
        }

        private void UserFeedForm_Load(object sender, EventArgs e)
        {
            // Формуємо правильний заголовок залежно від ролі
            string roleLabel = "";
            if (Session.Role == "Admin") roleLabel = "[СУПЕРАДМІН]";
            else if (Session.Role == "Moderator") roleLabel = "[ПРАЦІВНИК]";

            lblTitle.Text = $"WayPoint Feed | {Session.Username} {roleLabel}";

            // АДМІНУ ТА ПРАЦІВНИКУ не потрібна вкладка "Мої подорожі" (бо вони не клієнти)
            if (Session.Role == "Admin" || Session.Role == "Moderator")
            {
                if (tabControlFeed.TabPages.Contains(tabMyTrips))
                {
                    tabControlFeed.TabPages.Remove(tabMyTrips);
                }
            }

            RefreshData();
        }

        private void RefreshData()
        {
            LoadDataFromDatabase();
            RenderFeeds();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                travelTable = new DataTable();
                travelTable.Columns.Add("ID", typeof(int));
                travelTable.Columns.Add("User", typeof(string));
                travelTable.Columns.Add("Country", typeof(string));
                travelTable.Columns.Add("City", typeof(string));
                travelTable.Columns.Add("Budget", typeof(decimal));
                travelTable.Columns.Add("Status", typeof(string));
                travelTable.Columns.Add("Rating", typeof(int));
                travelTable.Columns.Add("Comment", typeof(string));

                using (SqlConnection connection = DatabaseService.GetConnection())
                {
                    string sql = "SELECT * FROM Travels";
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            travelTable.Rows.Add(
                                Convert.ToInt32(reader["ID"]),
                                reader["User"].ToString(),
                                reader["Country"].ToString(),
                                reader["City"].ToString(),
                                Convert.ToDecimal(reader["Budget"]),
                                reader["Status"].ToString(),
                                Convert.ToInt32(reader["Rating"]),
                                reader["Comment"].ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження стрічки: " + ex.Message);
            }
        }

        private void RenderFeeds()
        {
            // Рендеримо "Мої подорожі" ТІЛЬКИ ДЛЯ КЛІЄНТІВ
            if (Session.Role == "User")
            {
                flowMyTrips.Controls.Clear();
                DataView myView = new DataView(travelTable);
                myView.RowFilter = $"User = '{Session.Username.Replace("'", "''")}'";

                if (myView.Count == 0)
                    flowMyTrips.Controls.Add(CreateEmptyLabel("У тебе поки немає призначених подорожей 😢"));
                else
                    foreach (DataRowView row in myView) flowMyTrips.Controls.Add(CreateMyTripCard(row.Row));
            }

            flowGlobalFeed.Controls.Clear();

            // Глобальна стрічка: все, що завершено або на перевірці
            DataView globalView = new DataView(travelTable);
            globalView.RowFilter = "Status = 'Завершено' OR Status = 'Очікує перевірки'";

            if (globalView.Count == 0)
                flowGlobalFeed.Controls.Add(CreateEmptyLabel("Стрічка поки порожня."));
            else
                foreach (DataRowView row in globalView) flowGlobalFeed.Controls.Add(CreateGlobalFeedCard(row.Row));
        }

        private Label CreateEmptyLabel(string text)
        {
            return new Label { Text = text, Font = new Font("Segoe UI", 12, FontStyle.Italic), ForeColor = Color.Gray, AutoSize = true, Margin = new Padding(30) };
        }

        private Panel CreateMyTripCard(DataRow row)
        {
            int cardWidth = flowMyTrips.ClientSize.Width - 40;
            Panel pnlCard = new Panel { BackColor = Color.White, Width = cardWidth, Height = 280, Margin = new Padding(10, 10, 10, 20), BorderStyle = BorderStyle.FixedSingle };

            Label lblDestination = new Label { Text = $"📍 {row["Country"]} — {row["City"]}", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };
            Label lblBudget = new Label { Text = $"💰 Бюджет: {row["Budget"]} $", Font = new Font("Segoe UI", 10), ForeColor = Color.DimGray, Location = new Point(20, 50), AutoSize = true };

            string currentStatus = row["Status"].ToString();

            Label lblRatingText = new Label { Text = "Оцінка (1-10):", Location = new Point(20, 90), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            NumericUpDown numRating = new NumericUpDown { Minimum = 1, Maximum = 10, Value = Convert.ToInt32(row["Rating"]), Location = new Point(130, 88), Width = 60 };

            TextBox txtComment = new TextBox { Multiline = true, Font = new Font("Segoe UI", 10), Location = new Point(20, 130), Width = cardWidth - 40, Height = 60, Text = row["Comment"].ToString() };

            Button btnSave = new Button { Text = "Опублікувати звіт", BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(20, 210), Size = new Size(cardWidth - 40, 40), Cursor = Cursors.Hand };

            if (currentStatus == "Запит" || currentStatus == "Очікує перевірки")
            {
                btnSave.Enabled = false;
                btnSave.BackColor = Color.Gray;
                btnSave.Text = currentStatus == "Запит" ? "Очікуйте нарахування бюджету" : "Звіт на перевірці у адміна";
                numRating.Enabled = false;
                txtComment.Enabled = false;
            }
            else if (currentStatus == "Завершено")
            {
                btnSave.Text = "💾 Оновити відгук";
                btnSave.BackColor = Color.ForestGreen;
            }

            btnSave.Click += (s, e) =>
            {
                try
                {
                    using (SqlConnection connection = DatabaseService.GetConnection())
                    {
                        string sql = "UPDATE Travels SET Comment = @msg, Rating = @rat, [Status] = @stat WHERE ID = @id";
                        SqlCommand cmd = new SqlCommand(sql, connection);
                        cmd.Parameters.AddWithValue("@msg", txtComment.Text);
                        cmd.Parameters.AddWithValue("@rat", (int)numRating.Value);
                        cmd.Parameters.AddWithValue("@stat", currentStatus == "Планується" ? "Очікує перевірки" : currentStatus);
                        cmd.Parameters.AddWithValue("@id", row["ID"]);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Звіт збережено!");
                    RefreshData();
                }
                catch (Exception ex) { MessageBox.Show("Помилка збереження: " + ex.Message); }
            };

            pnlCard.Controls.Add(lblDestination); pnlCard.Controls.Add(lblBudget);
            pnlCard.Controls.Add(lblRatingText); pnlCard.Controls.Add(numRating);
            pnlCard.Controls.Add(txtComment); pnlCard.Controls.Add(btnSave);

            return pnlCard;
        }

        private Panel CreateGlobalFeedCard(DataRow row)
        {
            int cardWidth = flowGlobalFeed.ClientSize.Width - 40;
            Panel pnlCard = new Panel { BackColor = Color.White, Width = cardWidth, Height = 210, Margin = new Padding(10, 10, 10, 20), BorderStyle = BorderStyle.FixedSingle };

            string author = row["User"].ToString();
            int tourId = Convert.ToInt32(row["ID"]);

            Label lblUser = new Label { Text = $"👤 @{author}", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, Location = new Point(15, 15), AutoSize = true };
            Label lblDest = new Label { Text = $"🌍 {row["Country"]}, {row["City"]}", Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(15, 45), AutoSize = true };

            int rating = Convert.ToInt32(row["Rating"]);
            Label lblRating = new Label { Text = $"Оцінка: {rating}/10 {new string('⭐', rating)}", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.DarkOrange, Location = new Point(15, 75), AutoSize = true };

            Label lblComment = new Label { Text = $"\"{row["Comment"]}\"", Font = new Font("Segoe UI", 10, FontStyle.Italic), Location = new Point(15, 105), MaximumSize = new Size(cardWidth - 30, 60), AutoSize = true };

            pnlCard.Controls.Add(lblUser); pnlCard.Controls.Add(lblDest); pnlCard.Controls.Add(lblRating); pnlCard.Controls.Add(lblComment);

            // =====================================
            // РОЗДІЛЕННЯ РОЛЕЙ У СТРІЧЦІ
            // =====================================

            // 1. Якщо це КЛІЄНТ і це не його пост -> даємо кнопку "Хочу сюди"
            if (Session.Role == "User" && author != Session.Username)
            {
                Button btnOrder = new Button { Text = "🔥 Хочу сюди!", BackColor = Color.Coral, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(cardWidth - 140, 165), Size = new Size(120, 30), Cursor = Cursors.Hand };
                btnOrder.Click += (s, e) =>
                {
                    if (MessageBox.Show("Створити запит на такий тур?", "Замовлення", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection connection = DatabaseService.GetConnection())
                            {
                                string sql = "INSERT INTO Travels ([User], Country, City, Budget, [Status], Rating, Comment) VALUES (@u, @co, @ci, 0, 'Запит', 1, '')";
                                SqlCommand cmd = new SqlCommand(sql, connection);
                                cmd.Parameters.AddWithValue("@u", Session.Username);
                                cmd.Parameters.AddWithValue("@co", row["Country"]);
                                cmd.Parameters.AddWithValue("@ci", row["City"]);
                                cmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Заявку надіслано адміністратору!");
                            RefreshData();
                        }
                        catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
                    }
                };
                pnlCard.Controls.Add(btnOrder);
            }
            // 2. Якщо це АДМІН -> даємо кнопку "Видалити пост"
            else if (Session.Role == "Admin")
            {
                Button btnDelete = new Button { Text = "🗑 Видалити", BackColor = Color.Crimson, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Location = new Point(cardWidth - 140, 165), Size = new Size(120, 30), Cursor = Cursors.Hand };
                btnDelete.Click += (s, e) =>
                {
                    if (MessageBox.Show("Точно видалити цей запис зі стрічки?", "Модерація", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection connection = DatabaseService.GetConnection())
                            {
                                SqlCommand cmd = new SqlCommand("DELETE FROM Travels WHERE ID = @id", connection);
                                cmd.Parameters.AddWithValue("@id", tourId);
                                cmd.ExecuteNonQuery();
                            }
                            RefreshData();
                        }
                        catch (Exception ex) { MessageBox.Show("Помилка видалення: " + ex.Message); }
                    }
                };
                pnlCard.Controls.Add(btnDelete);
            }

            // 3. Якщо це ПРАЦІВНИК (Moderator) -> логіка просто проходить повз, кнопок не додається! Він тільки читає.

            return pnlCard;
        }

        private void pbBack_Click(object sender, EventArgs e) => this.Close();
        private void pbExit_Click(object sender, EventArgs e) => Application.Exit();

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }
}