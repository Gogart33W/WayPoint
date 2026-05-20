using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;

namespace WayPoint
{
    public partial class MainWork : Form
    {
        private DataTable travelTable;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool isUpdatingFromGrid = false; // ЗАПОБІЖНИК ВІД БАГІВ

        public MainWork()
        {
            InitializeComponent();
            SetupDataTableStructure();
            BindEvents(); // Підключаємо авто-калькулятор
        }

        private void BindEvents()
        {
            // Якщо щось міняється - перераховуємо ринок
            txtCountry.TextChanged += (s, e) => AutoCalculateMarketPrice();
            txtCity.TextChanged += (s, e) => AutoCalculateMarketPrice();
            numAdults.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            numNights.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            cmbBoard.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            cmbTransport.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();

            // Якщо міняється наша ціна - перевіряємо вигоду
            numBudget.ValueChanged += (s, e) => AnalyzeBenefit();
        }

        private void MainWork_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"WayPoint Business | {Session.Username}";
            btnAdminReturn.Visible = (Session.Role == "Admin");

            LoadUsersToCombo();
            LoadDataFromDatabase();
            AutoCalculateMarketPrice(); // Старт калькулятора
        }

        // --- ІНТЕЛЕКТУАЛЬНИЙ ПРАЙС-РУШІЙ ---
        private void AutoCalculateMarketPrice()
        {
            // Якщо ми зараз клацаємо по базі даних - не перебиваємо цифри!
            if (isUpdatingFromGrid) return;

            string loc = (txtCountry.Text + " " + txtCity.Text).Trim().ToLower();
            if (string.IsNullOrEmpty(loc)) return; // Нічого не введено

            // 1. Ціна готелю за ніч на 1 людину
            decimal baseNight = 40m; // База (Європа і тд)
            if (loc.Contains("єгипет") || loc.Contains("египет")) baseNight = 45m;
            else if (loc.Contains("туреччина") || loc.Contains("турция")) baseNight = 60m;
            else if (loc.Contains("франція") || loc.Contains("італія") || loc.Contains("іспанія") || loc.Contains("париж")) baseNight = 100m;
            else if (loc.Contains("оае") || loc.Contains("дубай") || loc.Contains("мальдіви")) baseNight = 200m;
            else if (loc.Contains("україна") || loc.Contains("украина") || loc.Contains("київ") || loc.Contains("вінниця") || loc.Contains("львів") || loc.Contains("одеса")) baseNight = 20m;
            else if (loc.Contains("польща") || loc.Contains("варшава")) baseNight = 35m;

            // 2. Харчування
            decimal boardMult = 1.0m;
            if (cmbBoard.Text.Contains("BB")) boardMult = 1.1m;
            else if (cmbBoard.Text.Contains("HB")) boardMult = 1.3m;
            else if (cmbBoard.Text.Contains("FB")) boardMult = 1.5m;
            else if (cmbBoard.Text.Contains("AI")) boardMult = 1.8m;

            // 3. Транспорт
            decimal transportPrice = 0m;
            if (cmbTransport.Text.Contains("Літак (Прямий)")) transportPrice = 250m;
            else if (cmbTransport.Text.Contains("Літак (Пересадка)")) transportPrice = 180m;
            else if (cmbTransport.Text.Contains("Автобус")) transportPrice = 70m;
            else if (cmbTransport.Text.Contains("Потяг")) transportPrice = 50m;

            // Логіка внутрішнього транспорту (якщо це тур по Україні)
            if (baseNight == 20m)
            {
                if (cmbTransport.Text.Contains("Літак")) transportPrice = 50m;
                else if (cmbTransport.Text.Contains("Автобус")) transportPrice = 15m;
                else if (cmbTransport.Text.Contains("Потяг")) transportPrice = 20m;
            }

            // Математика: (Готель * Харчування * Ночі * Люди) + (Транспорт * Люди)
            decimal totalCost = (baseNight * boardMult * numNights.Value * numAdults.Value) + (transportPrice * numAdults.Value);

            // 4. Вибираємо випадкове агентство для ілюзії справжнього ринку
            string[] agencies = { "Booking.com", "Join UP!", "Coral Travel", "TUI", "Oasis Travel" };
            int hash = Math.Abs((loc + numNights.Value + numAdults.Value).GetHashCode());
            txtAgency.Text = agencies[hash % agencies.Length];

            // 5. Ринкова маржа (від 5% до 15% націнки конкурента)
            decimal margin = 1.0m + ((5m + (hash % 10)) / 100m);
            decimal finalMarketPrice = Math.Round(totalCost * margin);

            if (finalMarketPrice > numMarketPrice.Maximum) finalMarketPrice = numMarketPrice.Maximum;
            numMarketPrice.Value = finalMarketPrice;

            AnalyzeBenefit();
        }

        private void AnalyzeBenefit()
        {
            if (isUpdatingFromGrid) return; // Не блимаємо екраном під час завантаження

            decimal our = numBudget.Value;
            decimal market = numMarketPrice.Value;

            if (market > 0 && our > 0)
            {
                decimal diff = market - our;
                if (diff > 0)
                {
                    lblSidebarTitle.Text = $"Вигода клієнта: +${diff}";
                    lblSidebarTitle.ForeColor = Color.Green;
                }
                else
                {
                    lblSidebarTitle.Text = $"Дорожче за {txtAgency.Text} на ${Math.Abs(diff)}";
                    lblSidebarTitle.ForeColor = Color.Red;
                }
            }
            else
            {
                lblSidebarTitle.Text = "Деталі туру";
                lblSidebarTitle.ForeColor = Color.Black;
            }
        }

        private void SetupDataTableStructure()
        {
            travelTable = new DataTable();
            travelTable.Columns.Add("ID", typeof(int));
            travelTable.Columns.Add("User", typeof(string));
            travelTable.Columns.Add("Departure", typeof(string));
            travelTable.Columns.Add("Country", typeof(string));
            travelTable.Columns.Add("City", typeof(string));
            travelTable.Columns.Add("Transport", typeof(string));
            travelTable.Columns.Add("Budget", typeof(decimal));
            travelTable.Columns.Add("MarketPrice", typeof(decimal));
            travelTable.Columns.Add("Status", typeof(string));

            dgvData.DataSource = travelTable;

            if (dgvData.Columns.Contains("ID")) dgvData.Columns["ID"].Visible = false;

            dgvData.EnableHeadersVisualStyles = false;
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.ReadOnly = true;
            dgvData.AllowUserToAddRows = false;

            dgvData.SelectionChanged += DgvData_SelectionChanged;
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                travelTable.Clear();
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT * FROM Travels", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataRow row = travelTable.NewRow();
                            row["ID"] = reader["ID"];
                            row["User"] = reader["User"] == DBNull.Value ? "" : reader["User"];
                            row["Departure"] = reader["DepartureCity"] == DBNull.Value ? "Київ" : reader["DepartureCity"];
                            row["Country"] = reader["Country"] == DBNull.Value ? "" : reader["Country"];
                            row["City"] = reader["City"] == DBNull.Value ? "" : reader["City"];
                            row["Transport"] = reader["TransportType"] == DBNull.Value ? "Літак (Прямий)" : reader["TransportType"];
                            row["Budget"] = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                            row["MarketPrice"] = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                            row["Status"] = reader["Status"] == DBNull.Value ? "Запит" : reader["Status"];
                            travelTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Помилка завантаження БД: " + ex.Message); }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                isUpdatingFromGrid = true; // УВІМКНУЛИ ЗАПОБІЖНИК
                try
                {
                    int id = Convert.ToInt32(dgvData.SelectedRows[0].Cells["ID"].Value);
                    using (var conn = DatabaseService.GetConnection())
                    {
                        var cmd = new SqlCommand("SELECT * FROM Travels WHERE ID=@id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtCountry.Text = reader["Country"]?.ToString() ?? "";
                                txtCity.Text = reader["City"]?.ToString() ?? "";
                                txtDepartureCity.Text = reader["DepartureCity"]?.ToString() ?? "Київ";
                                cmbTransport.Text = reader["TransportType"]?.ToString() ?? "Літак (Прямий)";
                                numBudget.Value = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                                numMarketPrice.Value = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                                txtAgency.Text = reader["AgencyName"]?.ToString() ?? "Booking.com";
                                cmbStatus.Text = reader["Status"]?.ToString() ?? "Запит";
                                cmbAssignedUser.Text = reader["User"]?.ToString() ?? "";
                                numAdults.Value = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                                numNights.Value = reader["Nights"] == DBNull.Value ? 7 : Convert.ToInt32(reader["Nights"]);
                                cmbBoard.Text = reader["BoardType"]?.ToString() ?? "AI (Все вкл.)";

                                lblSidebarTitle.Text = "Деталі туру";
                                lblSidebarTitle.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                catch { }
                finally
                {
                    isUpdatingFromGrid = false; // ВИМКНУЛИ ЗАПОБІЖНИК
                }
            }
        }

        private void LoadUsersToCombo()
        {
            try
            {
                cmbAssignedUser.Items.Clear();
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT Username FROM Users", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) cmbAssignedUser.Items.Add(reader["Username"].ToString());
                    }
                }
                if (cmbAssignedUser.Items.Count > 0) cmbAssignedUser.SelectedIndex = 0;
            }
            catch { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"INSERT INTO Travels ([User], Country, City, DepartureCity, TransportType, Budget, MarketPrice, AgencyName, [Status], Adults, Nights, BoardType, Rating, Comment) 
                                VALUES (@u, @co, @ci, @dep, @tr, @b, @mp, @an, @s, @ad, @ni, @bt, 1, '')";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", cmbAssignedUser.Text);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@dep", txtDepartureCity.Text);
                    cmd.Parameters.AddWithValue("@tr", cmbTransport.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@mp", numMarketPrice.Value);
                    cmd.Parameters.AddWithValue("@an", txtAgency.Text);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@ad", (int)numAdults.Value);
                    cmd.Parameters.AddWithValue("@ni", (int)numNights.Value);
                    cmd.Parameters.AddWithValue("@bt", cmbBoard.Text);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                btnClear_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show("Помилка додавання: " + ex.Message); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"UPDATE Travels SET [User]=@u, Country=@co, City=@ci, DepartureCity=@dep, TransportType=@tr, 
                                Budget=@b, MarketPrice=@mp, AgencyName=@an, [Status]=@s, Adults=@ad, Nights=@ni, BoardType=@bt WHERE ID=@id";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", cmbAssignedUser.Text);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@dep", txtDepartureCity.Text);
                    cmd.Parameters.AddWithValue("@tr", cmbTransport.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@mp", numMarketPrice.Value);
                    cmd.Parameters.AddWithValue("@an", txtAgency.Text);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@ad", (int)numAdults.Value);
                    cmd.Parameters.AddWithValue("@ni", (int)numNights.Value);
                    cmd.Parameters.AddWithValue("@bt", cmbBoard.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
            }
            catch (Exception ex) { MessageBox.Show("Помилка оновлення: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            if (MessageBox.Show("Видалити тур?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.No) return;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    new SqlCommand($"DELETE FROM Travels WHERE ID={id}", conn).ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                btnClear_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            isUpdatingFromGrid = true;
            txtCountry.Clear(); txtCity.Clear();
            txtDepartureCity.Text = "Київ";
            cmbTransport.SelectedIndex = 0;
            numBudget.Value = 0; numMarketPrice.Value = 0; txtAgency.Clear();
            numAdults.Value = 2; numNights.Value = 7;
            if (cmbBoard.Items.Count > 0) cmbBoard.SelectedIndex = 4;
            dgvData.ClearSelection();
            lblSidebarTitle.Text = "Деталі туру";
            lblSidebarTitle.ForeColor = Color.Black;
            isUpdatingFromGrid = false;
        }

        private void btnOpenMap_Click(object sender, EventArgs e)
        {
            using (MapForm mf = new MapForm($"{txtCity.Text} {txtCountry.Text}"))
            {
                if (mf.ShowDialog() == DialogResult.OK)
                {
                    txtCity.Text = mf.SelectedCity;
                    txtCountry.Text = mf.SelectedCountry;
                }
            }
        }

        private void btnOpenFeed_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (UserFeedForm feed = new UserFeedForm()) { feed.ShowDialog(); }
            this.Show();
            LoadDataFromDatabase();
        }

        private void btnAdminReturn_Click(object sender, EventArgs e) => this.Close();
        private void pbBack_Click(object sender, EventArgs e) => this.Close();
        private void pbExit_Click(object sender, EventArgs e) => Application.Exit();

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }
}