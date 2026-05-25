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
        private bool isUpdatingFromGrid = false;

        private decimal currentHotelPricePerNight = 0m;

        // ЗМІННА СТАНУ ПРОГРАМИ (Режим створення)
        private bool isAddingMode = false;

        public MainWork()
        {
            InitializeComponent();

            numMarketPrice.Maximum = 1000000m;
            numBudget.Maximum = 1000000m;
            numNights.Maximum = 365m;
            numAdults.Maximum = 100m;
            numNights.Minimum = 1m;
            numAdults.Minimum = 1m;

            SetupDataTableStructure();
            BindEvents();
        }

        private void BindEvents()
        {
            txtCountry.TextChanged += (s, e) => { currentHotelPricePerNight = 0m; txtHotel.Text = "Натисніть для вибору готелю..."; txtHotel.ForeColor = Color.Gray; AutoCalculateMarketPrice(); };
            txtCity.TextChanged += (s, e) => { currentHotelPricePerNight = 0m; txtHotel.Text = "Натисніть для вибору готелю..."; txtHotel.ForeColor = Color.Gray; AutoCalculateMarketPrice(); };

            numAdults.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            numNights.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            cmbBoard.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            cmbTransport.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            numBudget.ValueChanged += (s, e) => AnalyzeBenefit();

            txtHotel.Click += (s, e) => OpenHotelDictionary();
            txtHotel.Cursor = Cursors.Hand;
        }

        private void MainWork_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"WayPoint Business | {Session.Username}";
            btnAdminReturn.Visible = (Session.Role == "Admin");

            LoadUsersToCombo();
            LoadDataFromDatabase();

            // ПРИ ЗАПУСКУ РОБИМО ФОРМУ ПУСТОЮ ТА ЗАБЛОКОВАНОЮ
            isUpdatingFromGrid = true;
            dgvData.ClearSelection();
            ClearFields();
            SetInputFieldsState(false);
            isUpdatingFromGrid = false;

            AutoCalculateMarketPrice();
        }

        // ══════════════════════════════════════════
        // ЛОГІКА РЕЖИМІВ (СТВОРЕННЯ / РЕДАГУВАННЯ / БЛОКУВАННЯ)
        // ══════════════════════════════════════════
        private void SetInputFieldsState(bool enabled)
        {
            txtCountry.Enabled = enabled;
            txtCity.Enabled = enabled;
            txtDepartureCity.Enabled = enabled;
            cmbTransport.Enabled = enabled;
            numAdults.Enabled = enabled;
            numNights.Enabled = enabled;
            cmbBoard.Enabled = enabled;
            numBudget.Enabled = enabled;
            cmbStatus.Enabled = enabled;
            cmbAssignedUser.Enabled = enabled;
            txtHotel.Enabled = enabled;
            btnOpenMap.Enabled = enabled;
        }

        private void ToggleAddMode(bool enable)
        {
            isAddingMode = enable;
            if (enable)
            {
                // УВІМКНЕНО РЕЖИМ СТВОРЕННЯ: Залишаємо лише кнопку "Підтвердити"
                btnAdd.Text = "✅ Підтвердити";
                btnAdd.Width = 280; // Розтягуємо на всю ширину для краси

                btnEdit.Visible = false;
                btnDelete.Visible = false;
                btnClear.Visible = false;

                SetInputFieldsState(true);
                ClearFields();
                lblSidebarTitle.Text = "Створення туру";

                isUpdatingFromGrid = true;
                dgvData.ClearSelection();
                isUpdatingFromGrid = false;
            }
            else
            {
                // ВИМКНЕНО РЕЖИМ СТВОРЕННЯ: Повертаємо стандартний вигляд
                btnAdd.Text = "➕ Створити";
                btnAdd.Width = 135; // Повертаємо стандартну ширину

                btnEdit.Visible = true;
                btnDelete.Visible = true;
                btnClear.Visible = true;

                lblSidebarTitle.Text = "Деталі туру";
            }
        }

        private void ClearFields()
        {
            txtCountry.Clear();
            txtCity.Clear();
            txtDepartureCity.Text = "Київ";

            txtHotel.Text = "Натисніть для вибору готелю...";
            txtHotel.ForeColor = Color.Gray;
            currentHotelPricePerNight = 0m;

            if (cmbTransport.Items.Count > 0) cmbTransport.SelectedIndex = 0;
            numBudget.Value = 0;
            numMarketPrice.Value = 0;
            numAdults.Value = 2;
            numNights.Value = 7;
            if (cmbBoard.Items.Count > 0) cmbBoard.SelectedIndex = 4;
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
        }

        private bool CheckUnsavedChanges()
        {
            if (isAddingMode)
            {
                var result = MessageBox.Show("Ви не додали тур! Всі незбережені дані будуть втрачені.\n\nБажаєте продовжити і втратити дані?", "Попередження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return result == DialogResult.Yes; // Повертає true якщо користувач погодився втратити дані
            }
            return true;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCountry.Text)) { MessageBox.Show("Вкажіть країну!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtCity.Text)) { MessageBox.Show("Вкажіть місто!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtHotel.Text) || txtHotel.Text.Contains("Натисніть")) { MessageBox.Show("Оберіть готель!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (numBudget.Value <= 0) { MessageBox.Show("Вкажіть вашу ціну (бюджет)!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        // ══════════════════════════════════════════
        // ПРАЙС-РУШІЙ ТА АНАЛІЗ ВИГОДИ
        // ══════════════════════════════════════════
        private void AutoCalculateMarketPrice()
        {
            if (isUpdatingFromGrid) return;
            try
            {
                string loc = (txtCountry.Text + " " + txtCity.Text).Trim().ToLower();

                decimal baseNight = currentHotelPricePerNight > 0 ? currentHotelPricePerNight : 40m;
                if (currentHotelPricePerNight == 0m)
                {
                    if (loc.Contains("єгипет")) baseNight = 45m;
                    else if (loc.Contains("туреччина")) baseNight = 60m;
                    else if (loc.Contains("оае") || loc.Contains("дубай")) baseNight = 200m;
                    else if (loc.Contains("україна") || loc.Contains("київ")) baseNight = 20m;
                }

                decimal boardMult = 1.0m;
                string boardText = cmbBoard.Text ?? "";
                if (boardText.Contains("BB")) boardMult = 1.1m;
                else if (boardText.Contains("HB")) boardMult = 1.3m;
                else if (boardText.Contains("FB")) boardMult = 1.5m;
                else if (boardText.Contains("AI")) boardMult = 1.8m;

                decimal transportPrice = 0m;
                string transportText = cmbTransport.Text ?? "";
                if (transportText.Contains("Літак (Прямий)")) transportPrice = 250m;
                else if (transportText.Contains("Літак (Пересадка)")) transportPrice = 180m;
                else if (transportText.Contains("Автобус")) transportPrice = 70m;
                else if (transportText.Contains("Потяг")) transportPrice = 50m;

                decimal nights = numNights.Value < 1 ? 1 : numNights.Value;
                decimal adults = numAdults.Value < 1 ? 1 : numAdults.Value;

                decimal totalCost = (baseNight * boardMult * nights * adults) + (transportPrice * adults);
                decimal finalMarketPrice = Math.Round(totalCost * 1.10m);

                if (finalMarketPrice > numMarketPrice.Maximum) finalMarketPrice = numMarketPrice.Maximum;
                numMarketPrice.Value = finalMarketPrice;

                AnalyzeBenefit();
            }
            catch { }
        }

        private void AnalyzeBenefit()
        {
            if (isUpdatingFromGrid) return;
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
                    lblSidebarTitle.Text = $"Вище ринку на: ${Math.Abs(diff)}";
                    lblSidebarTitle.ForeColor = Color.Red;
                }
            }
            else
            {
                lblSidebarTitle.Text = isAddingMode ? "Створення туру" : "Деталі туру";
                lblSidebarTitle.ForeColor = Color.Black;
            }
        }

        private void OpenHotelDictionary()
        {
            string country = txtCountry.Text.Trim();
            string city = txtCity.Text.Trim();

            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Спочатку введіть країну та місто, щоб відкрити базу готелів для цього регіону!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new HotelDictionaryForm(country, city))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    txtHotel.Text = form.SelectedHotelName;
                    txtHotel.ForeColor = Color.Black;
                    currentHotelPricePerNight = form.SelectedPricePerNight;
                    AutoCalculateMarketPrice();
                }
            }
        }

        // ══════════════════════════════════════════
        // БД — ЗАВАНТАЖЕННЯ / ЗБЕРІГАННЯ
        // ══════════════════════════════════════════
        private void SetupDataTableStructure()
        {
            travelTable = new DataTable();
            travelTable.Columns.Add("ID", typeof(int));
            travelTable.Columns.Add("User", typeof(string));
            travelTable.Columns.Add("Departure", typeof(string));
            travelTable.Columns.Add("Country", typeof(string));
            travelTable.Columns.Add("City", typeof(string));
            travelTable.Columns.Add("Hotel", typeof(string));
            travelTable.Columns.Add("Transport", typeof(string));
            travelTable.Columns.Add("Adults", typeof(int));
            travelTable.Columns.Add("Nights", typeof(int));
            travelTable.Columns.Add("Budget", typeof(decimal));
            travelTable.Columns.Add("MarketPrice", typeof(decimal));
            travelTable.Columns.Add("Status", typeof(string));

            dgvData.DataSource = travelTable;
            if (dgvData.Columns.Contains("ID")) dgvData.Columns["ID"].Visible = false;

            dgvData.EnableHeadersVisualStyles = false;
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);

            // ФІКС ВІЗУАЛЬНОГО БАГУ (Заголовок більше не світиться синім)
            dgvData.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246);
            dgvData.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

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
                            try { row["Hotel"] = reader["HotelName"] == DBNull.Value ? "" : reader["HotelName"]; } catch { row["Hotel"] = ""; }
                            row["Transport"] = reader["TransportType"] == DBNull.Value ? "Літак (Прямий)" : reader["TransportType"];

                            int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                            row["Adults"] = dbAdults < 1 ? 2 : dbAdults;

                            int dbNights = reader["Nights"] == DBNull.Value ? 7 : Convert.ToInt32(reader["Nights"]);
                            row["Nights"] = dbNights < 1 ? 7 : dbNights;

                            row["Budget"] = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                            row["MarketPrice"] = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                            row["Status"] = reader["Status"] == DBNull.Value ? "Запит" : reader["Status"];
                            travelTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch { }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (isUpdatingFromGrid) return; // Запобіжник

            // ЯКЩО МИ В РЕЖИМІ СТВОРЕННЯ І КЛІКНУЛИ НА ІНШИЙ РЯДОК
            if (isAddingMode && dgvData.SelectedRows.Count > 0)
            {
                if (!CheckUnsavedChanges())
                {
                    // Якщо користувач відмовився втрачати дані - повертаємо виділення назад
                    isUpdatingFromGrid = true;
                    dgvData.ClearSelection();
                    isUpdatingFromGrid = false;
                    return;
                }
                else
                {
                    // Якщо погодився - вимикаємо режим створення і дозволяємо виділити рядок
                    ToggleAddMode(false);
                }
            }

            if (dgvData.SelectedRows.Count > 0)
            {
                // РОЗБЛОКОВУЄМО ПОЛЯ, БО РЯДОК ВИБРАНО
                SetInputFieldsState(true);

                isUpdatingFromGrid = true;
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
                                cmbBoard.Text = reader["BoardType"]?.ToString() ?? "AI (Все вкл.)";
                                cmbStatus.Text = reader["Status"]?.ToString() ?? "Запит";
                                cmbAssignedUser.Text = reader["User"]?.ToString() ?? "";

                                try { txtHotel.Text = reader["HotelName"]?.ToString() ?? ""; } catch { txtHotel.Text = ""; }
                                if (!string.IsNullOrEmpty(txtHotel.Text)) txtHotel.ForeColor = Color.Black;

                                currentHotelPricePerNight = GetHotelPriceFromDB(txtCity.Text, txtHotel.Text);

                                decimal dbBudget = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                                numBudget.Value = Math.Min(dbBudget, numBudget.Maximum);

                                decimal dbMarket = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                                numMarketPrice.Value = Math.Min(dbMarket, numMarketPrice.Maximum);

                                int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                                numAdults.Value = dbAdults < 1 ? 2 : Math.Min(dbAdults, 100);

                                int dbNights = reader["Nights"] == DBNull.Value ? 7 : Convert.ToInt32(reader["Nights"]);
                                numNights.Value = dbNights < 1 ? 7 : Math.Min(dbNights, 365);

                                lblSidebarTitle.Text = "Деталі туру";
                                lblSidebarTitle.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                catch { }
                finally
                {
                    isUpdatingFromGrid = false;
                    AutoCalculateMarketPrice();
                }
            }
            else
            {
                // ЯКЩО НІЧОГО НЕ ВИБРАНО І МИ НЕ В РЕЖИМІ СТВОРЕННЯ - БЛОКУЄМО ПОЛЯ
                if (!isAddingMode)
                {
                    SetInputFieldsState(false);
                    ClearFields();
                }
            }
        }

        private decimal GetHotelPriceFromDB(string city, string hotelName)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(hotelName)) return 0m;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT TOP 1 PricePerNight FROM HotelsDictionary WHERE City=@c AND Name=@n", conn);
                    cmd.Parameters.AddWithValue("@c", city);
                    cmd.Parameters.AddWithValue("@n", hotelName);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value) return Convert.ToDecimal(result);
                }
            }
            catch { }
            return 0m;
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

        // ══════════════════════════════════════════
        // КНОПКИ УПРАВЛІННЯ
        // ══════════════════════════════════════════
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Якщо ми ще не в режимі створення - вмикаємо його!
            if (!isAddingMode)
            {
                ToggleAddMode(true);
                return;
            }

            // Якщо ми вже в режимі створення і натиснули "Підтвердити"
            if (!ValidateInputs()) return;

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"INSERT INTO Travels 
                        ([User],Country,City,DepartureCity,TransportType,Budget,MarketPrice,
                         [Status],Adults,Nights,BoardType,HotelName,Rating,Comment)
                        VALUES
                        (@u,@co,@ci,@dep,@tr,@b,@mp,@s,@ad,@ni,@bt,@hn,1,'')";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", cmbAssignedUser.Text);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@dep", txtDepartureCity.Text);
                    cmd.Parameters.AddWithValue("@tr", cmbTransport.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@mp", numMarketPrice.Value);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@ad", (int)numAdults.Value);
                    cmd.Parameters.AddWithValue("@ni", (int)numNights.Value);
                    cmd.Parameters.AddWithValue("@bt", cmbBoard.Text);
                    cmd.Parameters.AddWithValue("@hn", txtHotel.Text);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                ToggleAddMode(false); // Вимикаємо режим створення

                // Знімаємо виділення, щоб поля заблокувалися
                isUpdatingFromGrid = true;
                dgvData.ClearSelection();
                SetInputFieldsState(false);
                ClearFields();
                isUpdatingFromGrid = false;

                MessageBox.Show("Тур успішно створено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Помилка додавання: " + ex.Message); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) { MessageBox.Show("Оберіть тур для оновлення!"); return; }
            if (!ValidateInputs()) return;

            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"UPDATE Travels SET
                        [User]=@u,Country=@co,City=@ci,DepartureCity=@dep,TransportType=@tr,
                        Budget=@b,MarketPrice=@mp,[Status]=@s,
                        Adults=@ad,Nights=@ni,BoardType=@bt,HotelName=@hn
                        WHERE ID=@id";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", cmbAssignedUser.Text);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@dep", txtDepartureCity.Text);
                    cmd.Parameters.AddWithValue("@tr", cmbTransport.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@mp", numMarketPrice.Value);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@ad", (int)numAdults.Value);
                    cmd.Parameters.AddWithValue("@ni", (int)numNights.Value);
                    cmd.Parameters.AddWithValue("@bt", cmbBoard.Text);
                    cmd.Parameters.AddWithValue("@hn", txtHotel.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                MessageBox.Show("Дані туру оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Помилка оновлення: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            if (MessageBox.Show("Видалити тур?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    new SqlCommand($"DELETE FROM Travels WHERE ID={id}", conn).ExecuteNonQuery();
                }
                LoadDataFromDatabase();

                // Скидаємо вибір після видалення
                isUpdatingFromGrid = true;
                dgvData.ClearSelection();
                ClearFields();
                SetInputFieldsState(false);
                isUpdatingFromGrid = false;
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Кнопка "Скинути" тепер просто знімає виділення і блокує поля
            isUpdatingFromGrid = true;
            dgvData.ClearSelection();
            ClearFields();
            SetInputFieldsState(false);
            isUpdatingFromGrid = false;
        }

        private void btnOpenMap_Click(object sender, EventArgs e)
        {
            using (var mf = new MapForm($"{txtCity.Text} {txtCountry.Text}"))
            {
                if (mf.ShowDialog() == DialogResult.OK)
                {
                    txtCity.Text = mf.SelectedCity;
                    txtCountry.Text = mf.SelectedCountry;
                }
            }
        }

        // ══════════════════════════════════════════
        // ЗАХИСТ ВІД ВИХОДУ БЕЗ ЗБЕРЕЖЕННЯ
        // ══════════════════════════════════════════
        private void btnOpenFeed_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            this.Hide();
            using (var feed = new UserFeedForm()) { feed.ShowDialog(); }
            this.Show();
            LoadDataFromDatabase();
        }

        private void btnAdminReturn_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            this.Close();
        }

        private void pbBack_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            this.Close();
        }

        private void pbExit_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            Application.Exit();
        }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }

    // Довідник готелів залишається без змін (його логіка ідеальна)
    public class HotelDictionaryForm : Form
    {
        public string SelectedHotelName { get; private set; }
        public decimal SelectedPricePerNight { get; private set; }

        private string country, city;
        private DataGridView dgv;
        private TextBox txtName, txtAddress;
        private NumericUpDown numStars, numPrice;

        public HotelDictionaryForm(string country, string city)
        {
            this.country = country;
            this.city = city;

            this.Text = $"CRM Довідник Готелів — {city}, {country}";
            this.Size = new Size(850, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            BuildUI();
            LoadHotels();
        }

        private void BuildUI()
        {
            var pnlLeft = new Panel { Location = new Point(10, 10), Size = new Size(250, 440), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            pnlLeft.Controls.Add(new Label { Text = "Реєстрація готелю", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true });

            pnlLeft.Controls.Add(new Label { Text = "Назва готелю:", Location = new Point(10, 45), AutoSize = true, ForeColor = Color.Gray });
            txtName = new TextBox { Location = new Point(10, 65), Size = new Size(220, 25), Font = new Font("Segoe UI", 10) };
            pnlLeft.Controls.Add(txtName);

            pnlLeft.Controls.Add(new Label { Text = "Адреса (вулиця/район):", Location = new Point(10, 105), AutoSize = true, ForeColor = Color.Gray });
            txtAddress = new TextBox { Location = new Point(10, 125), Size = new Size(220, 25), Font = new Font("Segoe UI", 10) };
            pnlLeft.Controls.Add(txtAddress);

            pnlLeft.Controls.Add(new Label { Text = "Кількість зірок (1-5):", Location = new Point(10, 165), AutoSize = true, ForeColor = Color.Gray });
            numStars = new NumericUpDown { Location = new Point(10, 185), Size = new Size(220, 25), Minimum = 1, Maximum = 5, Value = 4, Font = new Font("Segoe UI", 10) };
            pnlLeft.Controls.Add(numStars);

            pnlLeft.Controls.Add(new Label { Text = "Ціна за 1 ніч ($):", Location = new Point(10, 225), AutoSize = true, ForeColor = Color.Gray });
            numPrice = new NumericUpDown { Location = new Point(10, 245), Size = new Size(220, 25), Minimum = 1, Maximum = 100000, Value = 50, Font = new Font("Segoe UI", 10) };
            pnlLeft.Controls.Add(numPrice);

            var btnAdd = new Button { Text = "💾 Додати / Оновити", Location = new Point(10, 290), Size = new Size(220, 40), BackColor = Color.FromArgb(59, 130, 246), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnAdd.Click += BtnAdd_Click;
            pnlLeft.Controls.Add(btnAdd);

            var btnDelete = new Button { Text = "🗑 Видалити обраний", Location = new Point(10, 340), Size = new Size(220, 40), BackColor = Color.FromArgb(239, 68, 68), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnDelete.Click += BtnDelete_Click;
            pnlLeft.Controls.Add(btnDelete);

            this.Controls.Add(pnlLeft);

            dgv = new DataGridView
            {
                Location = new Point(270, 10),
                Size = new Size(550, 380),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246); // Фікс синього
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.SelectionChanged += Dgv_SelectionChanged;

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ID", Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Готель", FillWeight = 40 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stars", HeaderText = "Зірки", FillWeight = 15 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "$/Ніч", FillWeight = 20 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Address", HeaderText = "Адреса", FillWeight = 25 });

            this.Controls.Add(dgv);

            var btnSelect = new Button { Text = "✅ Обрати для туру", Location = new Point(270, 400), Size = new Size(250, 50), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            btnSelect.Click += BtnSelect_Click;
            this.Controls.Add(btnSelect);
        }

        private void LoadHotels()
        {
            dgv.Rows.Clear();
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT * FROM HotelsDictionary WHERE Country=@co AND City=@ci", conn);
                    cmd.Parameters.AddWithValue("@co", country);
                    cmd.Parameters.AddWithValue("@ci", city);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgv.Rows.Add(
                                reader["ID"],
                                reader["Name"],
                                new string('★', Convert.ToInt32(reader["Stars"])),
                                reader["PricePerNight"],
                                reader["Address"]
                            );
                        }
                    }
                }
                dgv.ClearSelection();
            }
            catch { }
        }

        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                txtName.Text = dgv.SelectedRows[0].Cells["Name"].Value.ToString();
                txtAddress.Text = dgv.SelectedRows[0].Cells["Address"].Value.ToString();
                numStars.Value = dgv.SelectedRows[0].Cells["Stars"].Value.ToString().Length;
                numPrice.Value = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Price"].Value);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Введіть назву готелю!"); return; }

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var checkCmd = new SqlCommand("SELECT ID FROM HotelsDictionary WHERE Country=@co AND City=@ci AND Name=@n", conn);
                    checkCmd.Parameters.AddWithValue("@co", country);
                    checkCmd.Parameters.AddWithValue("@ci", city);
                    checkCmd.Parameters.AddWithValue("@n", txtName.Text);
                    var existingId = checkCmd.ExecuteScalar();

                    if (existingId != null)
                    {
                        var update = new SqlCommand("UPDATE HotelsDictionary SET Address=@a, Stars=@s, PricePerNight=@p WHERE ID=@id", conn);
                        update.Parameters.AddWithValue("@a", txtAddress.Text);
                        update.Parameters.AddWithValue("@s", numStars.Value);
                        update.Parameters.AddWithValue("@p", numPrice.Value);
                        update.Parameters.AddWithValue("@id", existingId);
                        update.ExecuteNonQuery();
                    }
                    else
                    {
                        var insert = new SqlCommand("INSERT INTO HotelsDictionary (Country, City, Name, Address, Stars, PricePerNight) VALUES (@co, @ci, @n, @a, @s, @p)", conn);
                        insert.Parameters.AddWithValue("@co", country);
                        insert.Parameters.AddWithValue("@ci", city);
                        insert.Parameters.AddWithValue("@n", txtName.Text);
                        insert.Parameters.AddWithValue("@a", txtAddress.Text);
                        insert.Parameters.AddWithValue("@s", numStars.Value);
                        insert.Parameters.AddWithValue("@p", numPrice.Value);
                        insert.ExecuteNonQuery();
                    }
                }
                LoadHotels();
                MessageBox.Show("Готель збережено в довіднику!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Видалити готель з бази?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.No) return;
            try
            {
                int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["ID"].Value);
                using (var conn = DatabaseService.GetConnection())
                {
                    new SqlCommand($"DELETE FROM HotelsDictionary WHERE ID={id}", conn).ExecuteNonQuery();
                }
                LoadHotels();
                txtName.Clear(); txtAddress.Clear();
            }
            catch { }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0)
            {
                SelectedHotelName = dgv.SelectedRows[0].Cells["Name"].Value.ToString();
                SelectedPricePerNight = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Price"].Value);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Оберіть готель зі списку!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}