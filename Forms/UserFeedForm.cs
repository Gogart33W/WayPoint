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
        private System.Windows.Forms.Timer badgeTimer;

        public UserFeedForm()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Session.Username))
            {
                Session.Username = "Guest";
                Session.Role = "User";
            }

            lblExit.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnMessenger.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.Resize += UserFeedForm_Resize;
        }

        private void UserFeedForm_Resize(object sender, EventArgs e)
        {
            if (pnlHeader != null)
            {
                lblExit.Left = pnlHeader.Width - 40;
                if (btnMessenger.Visible)
                {
                    btnMessenger.Left = lblExit.Left - btnMessenger.Width - 20;
                }
            }
        }

        private void UserFeedForm_Load(object sender, EventArgs e)
        {
            SoundHelper.AttachSounds(this);

            string role = (Session.Role ?? "").Trim();
            bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            bool isModerator = role.Equals("Moderator", StringComparison.OrdinalIgnoreCase) || role.Equals("Manager", StringComparison.OrdinalIgnoreCase);
            bool isUser = role.Equals("User", StringComparison.OrdinalIgnoreCase);

            string roleLabel = "";
            if (isAdmin) roleLabel = "[СУПЕРАДМІН]";
            else if (isModerator) roleLabel = "[ПРАЦІВНИК]";

            lblTitle.Text = $"WayPoint Feed | {Session.Username} {roleLabel}";

            btnMessenger.Visible = isUser;
            UserFeedForm_Resize(this, EventArgs.Empty);

            if (isAdmin || isModerator)
            {
                if (tabControlFeed.TabPages.Contains(tabMyTrips))
                {
                    tabControlFeed.TabPages.Remove(tabMyTrips);
                }
            }

            if (isUser)
            {
                badgeTimer = new System.Windows.Forms.Timer();
                badgeTimer.Interval = 3000;
                badgeTimer.Tick += (s, ev) => UpdateMessengerBadge();
                badgeTimer.Start();
                UpdateMessengerBadge();
            }

            RefreshData();
        }

        private void UpdateMessengerBadge()
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    string sql = "SELECT COUNT(*) FROM Messages WHERE ReceiverUsername=@u AND IsRead=0";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", Session.Username);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            btnMessenger.Text = $"💬 Месенджер ({count})";
                            btnMessenger.BackColor = Color.Crimson;
                        }
                        else
                        {
                            btnMessenger.Text = "💬 Месенджер";
                            btnMessenger.BackColor = Color.FromArgb(59, 130, 246);
                        }
                    }
                }
            }
            catch { }
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
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                travelTable.Rows.Add(
                                    Convert.ToInt32(reader["ID"]),
                                    reader["User"] == DBNull.Value ? "" : reader["User"].ToString(),
                                    reader["Country"] == DBNull.Value ? "" : reader["Country"].ToString(),
                                    reader["City"] == DBNull.Value ? "" : reader["City"].ToString(),
                                    reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]),
                                    reader["Status"] == DBNull.Value ? "Запит" : reader["Status"].ToString(),
                                    reader["Rating"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Rating"]),
                                    reader["Comment"] == DBNull.Value ? "" : reader["Comment"].ToString()
                                );
                            }
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
            if (Session.Role == "User")
            {
                flowMyTrips.Controls.Clear();
                DataView myView = new DataView(travelTable);
                myView.RowFilter = $"User = '{Session.Username.Replace("'", "''")}'";

                if (myView.Count == 0)
                {
                    flowMyTrips.Controls.Add(CreateEmptyLabel("У тебе поки немає призначених подорожей 😢"));
                }
                else
                {
                    foreach (DataRowView row in myView)
                    {
                        flowMyTrips.Controls.Add(CreateMyTripCard(row.Row));
                    }
                }
            }

            flowGlobalFeed.Controls.Clear();

            DataView globalView = new DataView(travelTable);
            // Тепер у нас всі відгуки одразу в "Завершено", очікування перевірки ми прибрали.
            globalView.RowFilter = "Status = 'Завершено'";

            if (globalView.Count == 0)
            {
                flowGlobalFeed.Controls.Add(CreateEmptyLabel("Стрічка поки порожня."));
            }
            else
            {
                foreach (DataRowView row in globalView)
                {
                    flowGlobalFeed.Controls.Add(CreateGlobalFeedCard(row.Row));
                }
            }
        }

        private Label CreateEmptyLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Margin = new Padding(30)
            };
        }

        private Panel CreateMyTripCard(DataRow row)
        {
            int cardWidth = flowMyTrips.ClientSize.Width - 40;

            Panel pnlCard = new Panel
            {
                BackColor = Color.White,
                Width = cardWidth,
                Height = 280,
                Margin = new Padding(10, 10, 10, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblDestination = new Label
            {
                Text = $"📍 {row["Country"]} — {row["City"]}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label lblBudget = new Label
            {
                Text = $"💰 Бюджет: {row["Budget"]} $",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Location = new Point(20, 50),
                AutoSize = true
            };

            string currentStatus = row["Status"].ToString();
            bool isCompleted = currentStatus == "Завершено";
            bool isCancelled = currentStatus == "Скасовано";
            bool canReview = currentStatus == "Завершено";

            Label lblRatingText = new Label
            {
                Text = "Оцінка (1-10):",
                Location = new Point(20, 90),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            int dbRating = row["Rating"] == DBNull.Value ? 1 : Convert.ToInt32(row["Rating"]);
            int safeRating = Math.Max(1, Math.Min(10, dbRating == 0 ? 1 : dbRating));

            NumericUpDown numRating = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 10,
                Value = safeRating,
                Location = new Point(130, 88),
                Width = 60
            };

            TextBox txtComment = new TextBox
            {
                Multiline = true,
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 130),
                Width = cardWidth - 40,
                Height = 60,
                Text = row["Comment"].ToString()
            };

            Button btnSave = new Button
            {
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 210),
                Size = new Size(cardWidth - 40, 40),
                Cursor = Cursors.Hand
            };

            Button btnDeleteReview = new Button
            {
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Text = "🗑 Видалити відгук",
                Visible = false
            };

            Button btnCancel = new Button
            {
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(cardWidth - 140, 15),
                Size = new Size(120, 30),
                Cursor = Cursors.Hand,
                Text = "❌ Скасувати тур"
            };

            // Кнопка скасування видна тільки якщо тур активний (не завершений і не скасований)
            btnCancel.Visible = (currentStatus == "Запит" || currentStatus == "Планується" || currentStatus == "Оплачено");

            btnCancel.Click += (s, e) =>
            {
                if (MessageBox.Show("Ви впевнені, що хочете скасувати цей тур?", "Скасування", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = DatabaseService.GetConnection())
                        {
                            // ФІКС 1: Параметри для скасування, щоб не було ??????
                            string sql = "UPDATE Travels SET [Status] = @stat WHERE ID = @id";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@stat", "Скасовано");
                                cmd.Parameters.AddWithValue("@id", row["ID"]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Тур успішно скасовано.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                    catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };
            pnlCard.Controls.Add(btnCancel);

            bool hasReview = !string.IsNullOrWhiteSpace(row["Comment"].ToString()) || dbRating > 0;

            if (isCancelled)
            {
                btnSave.Text = "🔄 Відновити тур (відправити як запит)";
                btnSave.BackColor = Color.DarkOrange;
                btnSave.ForeColor = Color.White;
                btnSave.Enabled = true;

                numRating.Enabled = false;
                txtComment.Enabled = false;

                btnSave.Click += (s, e) =>
                {
                    if (MessageBox.Show("Відновити цей тур і відправити менеджеру як новий запит?", "Відновлення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection connection = DatabaseService.GetConnection())
                            {
                                // ФІКС 2: Параметри для відновлення (Запит), щоб не було ??????
                                string sql = "UPDATE Travels SET [Status] = @stat WHERE ID = @id";
                                using (SqlCommand cmd = new SqlCommand(sql, connection))
                                {
                                    cmd.Parameters.AddWithValue("@stat", "Запит");
                                    cmd.Parameters.AddWithValue("@id", row["ID"]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            MessageBox.Show("Тур успішно відновлено! Тепер він у статусі 'Запит'.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshData();
                        }
                        catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                };
            }
            else if (canReview)
            {
                btnSave.Text = "💾 Опублікувати / Оновити відгук";
                btnSave.BackColor = Color.ForestGreen;
                btnSave.ForeColor = Color.White;
                btnSave.Enabled = true;

                numRating.Enabled = true;
                txtComment.Enabled = true;

                if (hasReview)
                {
                    btnDeleteReview.Visible = true;
                    btnSave.Size = new Size(cardWidth - 180, 40);
                    btnDeleteReview.Location = new Point(btnSave.Right + 10, 210);
                    btnDeleteReview.Size = new Size(130, 40);
                }

                btnSave.Click += (s, e) =>
                {
                    try
                    {
                        using (SqlConnection connection = DatabaseService.GetConnection())
                        {
                            // ФІКС 3: Зберігаємо статус "Завершено", а не "Очікує перевірки"
                            string sql = "UPDATE Travels SET Comment = @msg, Rating = @rat, [Status] = @stat WHERE ID = @id";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@msg", txtComment.Text);
                                cmd.Parameters.AddWithValue("@rat", (int)numRating.Value);
                                cmd.Parameters.AddWithValue("@stat", "Завершено");
                                cmd.Parameters.AddWithValue("@id", row["ID"]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Звіт збережено та опубліковано у загальній стрічці!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                    catch (Exception ex) { MessageBox.Show("Помилка збереження: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
            }
            else
            {
                btnSave.Text = $"Тур у статусі: {currentStatus}. Відгуки доступні після завершення.";
                btnSave.BackColor = Color.Gray;
                btnSave.ForeColor = Color.White;
                btnSave.Enabled = false;

                numRating.Enabled = false;
                txtComment.Enabled = false;
            }

            btnDeleteReview.Click += (s, e) =>
            {
                if (MessageBox.Show("Ви впевнені, що хочете видалити свій відгук?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = DatabaseService.GetConnection())
                        {
                            string sql = "UPDATE Travels SET Comment = '', Rating = 0, [Status] = @stat WHERE ID = @id";
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.AddWithValue("@stat", "Завершено");
                                cmd.Parameters.AddWithValue("@id", row["ID"]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Відгук успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshData();
                    }
                    catch (Exception ex) { MessageBox.Show("Помилка видалення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            };

            SoundHelper.AttachSounds(pnlCard);

            pnlCard.Controls.Add(lblDestination);
            pnlCard.Controls.Add(lblBudget);
            pnlCard.Controls.Add(lblRatingText);
            pnlCard.Controls.Add(numRating);
            pnlCard.Controls.Add(txtComment);
            pnlCard.Controls.Add(btnSave);
            pnlCard.Controls.Add(btnDeleteReview);

            return pnlCard;
        }

        private Panel CreateGlobalFeedCard(DataRow row)
        {
            int cardWidth = flowGlobalFeed.ClientSize.Width - 40;
            Panel pnlCard = new Panel
            {
                BackColor = Color.White,
                Width = cardWidth,
                Height = 210,
                Margin = new Padding(10, 10, 10, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            string author = row["User"].ToString();
            int tourId = Convert.ToInt32(row["ID"]);

            Label lblUser = new Label
            {
                Text = $"👤 @{author}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label lblDest = new Label
            {
                Text = $"🌍 {row["Country"]}, {row["City"]}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 45),
                AutoSize = true
            };

            int rating = row["Rating"] == DBNull.Value ? 0 : Convert.ToInt32(row["Rating"]);
            int safeStars = Math.Max(0, Math.Min(10, rating));

            Label lblRating = new Label
            {
                Text = $"Оцінка: {safeStars}/10 {new string('⭐', safeStars)}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.DarkOrange,
                Location = new Point(15, 75),
                AutoSize = true
            };

            string rawComment = row["Comment"]?.ToString() ?? "";
            bool isEmpty = string.IsNullOrWhiteSpace(rawComment);

            Label lblComment = new Label
            {
                Text = isEmpty ? "🚫 Користувач не залишив коментаря до цього туру." : $"\"{rawComment}\"",
                Font = new Font("Segoe UI", 10, isEmpty ? FontStyle.Italic : FontStyle.Regular),
                ForeColor = isEmpty ? Color.Gray : Color.Black,
                Location = new Point(15, 105),
                MaximumSize = new Size(cardWidth - 30, 60),
                AutoSize = true
            };

            pnlCard.Controls.Add(lblUser);
            pnlCard.Controls.Add(lblDest);
            pnlCard.Controls.Add(lblRating);
            pnlCard.Controls.Add(lblComment);

            string role = (Session.Role ?? "").Trim();
            bool isAdminOrModerator = role.Equals("Admin", StringComparison.OrdinalIgnoreCase) || role.Equals("Moderator", StringComparison.OrdinalIgnoreCase) || role.Equals("Manager", StringComparison.OrdinalIgnoreCase);

            if (Session.Role == "User" && author != Session.Username)
            {
                Button btnOrder = new Button
                {
                    Text = "🔥 Хочу сюди!",
                    BackColor = Color.Coral,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(cardWidth - 140, 165),
                    Size = new Size(120, 30),
                    Cursor = Cursors.Hand
                };

                btnOrder.Click += (s, e) =>
                {
                    if (MessageBox.Show("Створити запит на такий тур?", "Замовлення", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection connection = DatabaseService.GetConnection())
                            {
                                // ФІКС 4: Додаємо через параметри, щоб статус "Запит" зберігся без ??????
                                string sql = "INSERT INTO Travels ([User], Country, City, Budget, [Status], Rating, Comment) VALUES (@u, @co, @ci, 0, @stat, 0, '')";
                                using (SqlCommand cmd = new SqlCommand(sql, connection))
                                {
                                    cmd.Parameters.AddWithValue("@u", Session.Username);
                                    cmd.Parameters.AddWithValue("@co", row["Country"]);
                                    cmd.Parameters.AddWithValue("@ci", row["City"]);
                                    cmd.Parameters.AddWithValue("@stat", "Запит");
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            MessageBox.Show("Заявку надіслано адміністратору!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshData();
                        }
                        catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                };
                pnlCard.Controls.Add(btnOrder);
            }
            else if (isAdminOrModerator)
            {
                Button btnDelete = new Button
                {
                    Text = "🗑 Видалити відгук",
                    BackColor = Color.Crimson,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(cardWidth - 160, 165),
                    Size = new Size(140, 30),
                    Cursor = Cursors.Hand
                };

                btnDelete.Click += (s, e) =>
                {
                    if (MessageBox.Show("Точно видалити відгук клієнта? Тур залишиться в базі.", "Модерація", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection connection = DatabaseService.GetConnection())
                            {
                                string sql = "UPDATE Travels SET Comment = '', Rating = 0, [Status] = @stat WHERE ID = @id";
                                using (SqlCommand cmd = new SqlCommand(sql, connection))
                                {
                                    cmd.Parameters.AddWithValue("@stat", "Завершено");
                                    cmd.Parameters.AddWithValue("@id", tourId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            RefreshData();
                        }
                        catch (Exception ex) { MessageBox.Show("Помилка видалення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                };
                pnlCard.Controls.Add(btnDelete);
            }

            SoundHelper.AttachSounds(pnlCard);
            return pnlCard;
        }

        private void btnMessenger_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var messenger = new MessengerForm())
            {
                messenger.ShowDialog();
            }
            this.Show();
            UpdateMessengerBadge();
        }

        private void pbBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void pnlHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && this.WindowState != FormWindowState.Maximized)
            {
                int dx = Cursor.Position.X - dragCursorPoint.X;
                int dy = Cursor.Position.Y - dragCursorPoint.Y;
                this.Location = new Point(dragFormPoint.X + dx, dragFormPoint.Y + dy);
            }
        }

        private void pnlHeader_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (badgeTimer != null)
            {
                badgeTimer.Stop();
                badgeTimer.Dispose();
            }
            base.OnFormClosed(e);
        }
    }
}