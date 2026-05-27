using System;
using System.Collections.Generic;
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
        private bool isAddingMode = false;

        // Список всіх клієнтів для пошуку
        private List<string> allClients = new List<string>();

        public MainWork()
        {
            InitializeComponent();
            SetupDataTableStructure();
            BindEvents();
        }

        private void BindEvents()
        {
            txtCountry.TextChanged += (s, e) => { ResetHotelSelection(); AutoCalculateMarketPrice(); };
            txtCity.TextChanged += (s, e) => { ResetHotelSelection(); AutoCalculateMarketPrice(); };
            numAdults.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            numChildren.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            numNights.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            cmbBoard.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            cmbTransport.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            numBudget.ValueChanged += (s, e) => AnalyzeBenefit();

            txtHotel.Click += (s, e) => OpenHotelDictionary();
            txtHotel.Cursor = Cursors.Hand;

            // Пошук клієнта
            txtClientSearch.TextChanged += TxtClientSearch_TextChanged;
            lstClientSearch.Click += LstClientSearch_Click;
            lstClientSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && lstClientSearch.SelectedItem != null)
                    SelectClientFromList();
            };

            // Якщо клік поза listbox — ховаємо його
            this.Click += (s, e) => HideClientList();
            pnlSidebar.Click += (s, e) => HideClientList();

            // Час відправлення — автоформат
            txtDepartureTime.Leave += (s, e) => FormatTime();
        }

        private void FormatTime()
        {
            string t = txtDepartureTime.Text.Trim().Replace(".", ":").Replace("-", ":");
            if (t.Length == 4 && !t.Contains(":"))
                t = t.Insert(2, ":");
            if (System.Text.RegularExpressions.Regex.IsMatch(t, @"^\d{1,2}:\d{2}$"))
                txtDepartureTime.Text = t;
        }

        private void cmbTransport_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Показуємо панель пересадки тільки для "Літак (Пересадка)"
            bool isTransfer = cmbTransport.Text == "Літак (Пересадка)";
            pnlTransfer.Visible = isTransfer;
            AutoCalculateMarketPrice();
        }

        // ══════════════════════════════════════════
        // ПОШУК КЛІЄНТА
        // ══════════════════════════════════════════
        private void TxtClientSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtClientSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(query))
            {
                HideClientList();
                return;
            }

            lstClientSearch.Items.Clear();
            foreach (var client in allClients)
            {
                if (client.ToLower().Contains(query))
                    lstClientSearch.Items.Add(client);
            }

            if (lstClientSearch.Items.Count > 0)
            {
                lstClientSearch.Visible = true;
                lstClientSearch.BringToFront();
                int height = Math.Min(lstClientSearch.Items.Count * 20 + 8, 120);
                lstClientSearch.Height = height;
            }
            else
            {
                HideClientList();
            }
        }

        private void LstClientSearch_Click(object sender, EventArgs e)
        {
            SelectClientFromList();
        }

        private void SelectClientFromList()
        {
            if (lstClientSearch.SelectedItem == null) return;
            string selected = lstClientSearch.SelectedItem.ToString();
            cmbAssignedUser.Text = selected;
            txtClientSearch.Text = selected;
            HideClientList();
        }

        private void HideClientList()
        {
            lstClientSearch.Visible = false;
        }

        private void ResetHotelSelection()
        {
            currentHotelPricePerNight = 0m;
            txtHotel.Text = "Натисніть для вибору готелю...";
            txtHotel.ForeColor = Color.Gray;
        }

        private void MainWork_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"WayPoint Business | {Session.Username}";
            btnAdminReturn.Visible = (Session.Role == "Admin");
            LoadUsersToCombo();
            LoadDataFromDatabase();

            isUpdatingFromGrid = true;
            dgvData.ClearSelection();
            ClearFields();
            SetInputFieldsState(false);
            isUpdatingFromGrid = false;

            AutoCalculateMarketPrice();
        }

        // ══════════════════════════════════════════
        // УПРАВЛІННЯ СТАНОМ ФОРМИ
        // ══════════════════════════════════════════
        private void SetInputFieldsState(bool enabled)
        {
            txtCountry.Enabled = enabled;
            txtCity.Enabled = enabled;
            txtDepartureCity.Enabled = enabled;
            txtDepartureStreet.Enabled = enabled;
            dtpDepartureDate.Enabled = enabled;
            txtDepartureTime.Enabled = enabled;
            txtFlightNumber.Enabled = enabled;
            txtTerminal.Enabled = enabled;
            txtTransferCity.Enabled = enabled;
            cmbTransport.Enabled = enabled;
            numAdults.Enabled = enabled;
            numChildren.Enabled = enabled;
            numNights.Enabled = enabled;
            cmbBoard.Enabled = enabled;
            numBudget.Enabled = enabled;
            cmbStatus.Enabled = enabled;
            cmbAssignedUser.Enabled = enabled;
            txtClientSearch.Enabled = enabled;
            txtHotel.Enabled = enabled;
            btnOpenMap.Enabled = enabled;
        }

        private void ToggleAddMode(bool enable)
        {
            isAddingMode = enable;
            if (enable)
            {
                btnAdd.Text = "✅ Підтвердити";
                btnAdd.Location = new Point(10, btnAdd.Location.Y);
                btnAdd.Size = new Size(305, 44);

                btnEdit.Visible = false;
                btnDelete.Visible = false;

                btnClear.Text = "❌ Скасувати";
                btnClear.BackColor = Color.FromArgb(239, 68, 68);
                btnClear.ForeColor = Color.White;
                btnClear.Location = new Point(10, btnClear.Location.Y);
                btnClear.Size = new Size(305, 44);

                SetInputFieldsState(true);
                ClearFields();
                lblSidebarTitle.Text = "Створення туру";
                lblSidebarTitle.ForeColor = Color.Black;

                isUpdatingFromGrid = true;
                dgvData.ClearSelection();
                isUpdatingFromGrid = false;
            }
            else
            {
                btnAdd.Text = "➕ Створити";
                btnAdd.Location = new Point(10, btnAdd.Location.Y);
                btnAdd.Size = new Size(145, 44);

                btnEdit.Visible = true;
                btnDelete.Visible = true;

                btnClear.Text = "Скинути";
                btnClear.BackColor = Color.FromArgb(229, 231, 235);
                btnClear.ForeColor = Color.Black;
                btnClear.Location = new Point(165, btnClear.Location.Y);
                btnClear.Size = new Size(150, 44);

                lblSidebarTitle.Text = "Деталі туру";
                lblSidebarTitle.ForeColor = Color.Black;
            }
        }

        private void ClearFields()
        {
            txtCountry.Clear();
            txtCity.Clear();
            txtDepartureCity.Text = "Київ";
            txtDepartureStreet.Clear();
            dtpDepartureDate.Value = DateTime.Today;
            txtDepartureTime.Clear();
            txtFlightNumber.Clear();
            txtTerminal.Clear();
            txtTransferCity.Clear();
            pnlTransfer.Visible = false;

            ResetHotelSelection();

            if (cmbTransport.Items.Count > 0) cmbTransport.SelectedIndex = 0;
            numBudget.Value = 0;
            numMarketPrice.Value = 0;
            numAdults.Value = 2;
            numChildren.Value = 0;
            numNights.Value = 7;
            if (cmbBoard.Items.Count > 0) cmbBoard.SelectedIndex = 4;
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;

            txtClientSearch.Clear();
            HideClientList();
        }

        private bool CheckUnsavedChanges()
        {
            if (!isAddingMode) return true;
            return MessageBox.Show(
                "Ви не зберегли тур!\nВсі незбережені дані будуть втрачені.\n\nПродовжити?",
                "Попередження",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCountry.Text))
            { MessageBox.Show("Вкажіть країну призначення!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (string.IsNullOrWhiteSpace(txtCity.Text))
            { MessageBox.Show("Вкажіть місто призначення!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (string.IsNullOrWhiteSpace(txtDepartureCity.Text))
            { MessageBox.Show("Вкажіть місто відправлення!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (cmbTransport.Text == "Літак (Пересадка)" && string.IsNullOrWhiteSpace(txtTransferCity.Text))
            { MessageBox.Show("Вкажіть місто пересадки!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (!string.IsNullOrWhiteSpace(txtDepartureTime.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtDepartureTime.Text.Trim(), @"^\d{1,2}:\d{2}$"))
                { MessageBox.Show("Час відправлення має бути у форматі ГГ:ХХ (напр. 08:30)!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            }

            if (string.IsNullOrWhiteSpace(txtHotel.Text) || txtHotel.Text.Contains("Натисніть"))
            { MessageBox.Show("Оберіть готель з довідника!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (numBudget.Value <= 0)
            { MessageBox.Show("Вкажіть нашу ціну (більше 0)!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            if (cmbAssignedUser.SelectedIndex < 0 && string.IsNullOrEmpty(cmbAssignedUser.Text))
            { MessageBox.Show("Оберіть клієнта!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }

            return true;
        }

        // ══════════════════════════════════════════
        // КАЛЬКУЛЯТОР
        // ══════════════════════════════════════════
        private void AutoCalculateMarketPrice()
        {
            if (isUpdatingFromGrid) return;

            // БАГ-ФІХ: якщо не вибрано країну і місто — ціна завжди 0
            if (string.IsNullOrWhiteSpace(txtCountry.Text) && string.IsNullOrWhiteSpace(txtCity.Text))
            {
                numMarketPrice.Value = 0;
                AnalyzeBenefit();
                return;
            }

            try
            {
                string loc = (txtCountry.Text + " " + txtCity.Text).Trim().ToLower();

                decimal baseNight = currentHotelPricePerNight > 0 ? currentHotelPricePerNight : 40m;
                if (currentHotelPricePerNight == 0m)
                {
                    if (loc.Contains("єгипет") || loc.Contains("египет")) baseNight = 45m;
                    else if (loc.Contains("туреччина") || loc.Contains("турция")) baseNight = 60m;
                    else if (loc.Contains("франція") || loc.Contains("італія") || loc.Contains("іспанія")) baseNight = 100m;
                    else if (loc.Contains("оае") || loc.Contains("дубай") || loc.Contains("мальдіви")) baseNight = 200m;
                    else if (loc.Contains("україна") || loc.Contains("київ") || loc.Contains("львів") || loc.Contains("одеса")) baseNight = 20m;
                    else if (loc.Contains("польща") || loc.Contains("варшава")) baseNight = 35m;
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
                else if (transportText.Contains("Літак (Пересадка)")) transportPrice = 320m; // дорожче через пересадку
                else if (transportText.Contains("Автобус")) transportPrice = 70m;
                else if (transportText.Contains("Потяг")) transportPrice = 50m;
                else if (transportText.Contains("Корабель")) transportPrice = 400m;

                if (loc.Contains("україна") || loc.Contains("київ") || loc.Contains("львів") || loc.Contains("одеса"))
                {
                    if (transportText.Contains("Літак")) transportPrice = 50m;
                    else if (transportText.Contains("Автобус")) transportPrice = 15m;
                    else if (transportText.Contains("Потяг")) transportPrice = 20m;
                }

                decimal nights = numNights.Value < 1 ? 1 : numNights.Value;
                decimal adults = numAdults.Value < 1 ? 1 : numAdults.Value;
                decimal children = numChildren.Value;

                // Діти — 60% від дорослої ціни готелю, але повна ціна транспорту (якщо > 2 роки)
                decimal hotelCost = baseNight * boardMult * nights * (adults + children * 0.6m);
                decimal transportCost = transportPrice * (adults + children * 0.7m);
                decimal totalCost = hotelCost + transportCost;

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

        // ══════════════════════════════════════════
        // ДОВІДНИК ГОТЕЛІВ
        // ══════════════════════════════════════════
        private void OpenHotelDictionary()
        {
            string country = txtCountry.Text.Trim();
            string city = txtCity.Text.Trim();

            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(city))
            {
                MessageBox.Show(
                    "Спочатку введіть країну та місто!\nДовідник готелів прив'язаний до конкретного міста.",
                    "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        // БД
        // ══════════════════════════════════════════
        private void SetupDataTableStructure()
        {
            travelTable = new DataTable();
            travelTable.Columns.Add("ID", typeof(int));
            travelTable.Columns.Add("User", typeof(string));
            travelTable.Columns.Add("Country", typeof(string));
            travelTable.Columns.Add("City", typeof(string));
            travelTable.Columns.Add("Hotel", typeof(string));
            travelTable.Columns.Add("Transport", typeof(string));
            travelTable.Columns.Add("Departure", typeof(string));
            travelTable.Columns.Add("DepartureDate", typeof(string));
            travelTable.Columns.Add("Adults", typeof(int));
            travelTable.Columns.Add("Children", typeof(int));
            travelTable.Columns.Add("Nights", typeof(int));
            travelTable.Columns.Add("Budget", typeof(decimal));
            travelTable.Columns.Add("MarketPrice", typeof(decimal));
            travelTable.Columns.Add("Status", typeof(string));

            dgvData.DataSource = travelTable;
            if (dgvData.Columns.Contains("ID")) dgvData.Columns["ID"].Visible = false;

            dgvData.EnableHeadersVisualStyles = false;
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
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
                            row["Country"] = reader["Country"] == DBNull.Value ? "" : reader["Country"];
                            row["City"] = reader["City"] == DBNull.Value ? "" : reader["City"];
                            try { row["Hotel"] = reader["HotelName"] == DBNull.Value ? "" : reader["HotelName"]; } catch { row["Hotel"] = ""; }
                            row["Transport"] = reader["TransportType"] == DBNull.Value ? "" : reader["TransportType"];
                            row["Departure"] = reader["DepartureCity"] == DBNull.Value ? "" : reader["DepartureCity"];
                            try
                            {
                                row["DepartureDate"] = reader["DepartureDate"] == DBNull.Value ? "" :
                                    Convert.ToDateTime(reader["DepartureDate"]).ToString("dd.MM.yyyy");
                            }
                            catch { row["DepartureDate"] = ""; }

                            int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                            row["Adults"] = dbAdults < 1 ? 2 : dbAdults;

                            try
                            {
                                int dbChildren = reader["Children"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Children"]);
                                row["Children"] = dbChildren < 0 ? 0 : dbChildren;
                            }
                            catch { row["Children"] = 0; }

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
            catch (Exception ex) { MessageBox.Show("Помилка завантаження БД: " + ex.Message); }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (isUpdatingFromGrid) return;

            if (isAddingMode && dgvData.SelectedRows.Count > 0)
            {
                if (!CheckUnsavedChanges())
                {
                    isUpdatingFromGrid = true;
                    dgvData.ClearSelection();
                    isUpdatingFromGrid = false;
                    return;
                }
                ToggleAddMode(false);
            }

            if (dgvData.SelectedRows.Count > 0)
            {
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
                                txtClientSearch.Text = reader["User"]?.ToString() ?? "";

                                // Нові поля відправлення
                                try { txtDepartureStreet.Text = reader["DepartureStreet"]?.ToString() ?? ""; } catch { }
                                try
                                {
                                    if (reader["DepartureDate"] != DBNull.Value)
                                        dtpDepartureDate.Value = Convert.ToDateTime(reader["DepartureDate"]);
                                }
                                catch { dtpDepartureDate.Value = DateTime.Today; }
                                try { txtDepartureTime.Text = reader["DepartureTime"]?.ToString() ?? ""; } catch { }
                                try { txtFlightNumber.Text = reader["FlightNumber"]?.ToString() ?? ""; } catch { }
                                try { txtTerminal.Text = reader["TerminalOrStation"]?.ToString() ?? ""; } catch { }
                                try
                                {
                                    txtTransferCity.Text = reader["TransferCity"]?.ToString() ?? "";
                                    pnlTransfer.Visible = cmbTransport.Text == "Літак (Пересадка)";
                                }
                                catch { }

                                string hotelName = "";
                                try { hotelName = reader["HotelName"]?.ToString() ?? ""; } catch { }
                                if (!string.IsNullOrEmpty(hotelName))
                                {
                                    txtHotel.Text = hotelName;
                                    txtHotel.ForeColor = Color.Black;
                                    currentHotelPricePerNight = GetHotelPriceFromDB(txtCity.Text, hotelName);
                                }
                                else
                                {
                                    ResetHotelSelection();
                                }

                                decimal dbBudget = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                                numBudget.Value = Math.Min(dbBudget, numBudget.Maximum);

                                decimal dbMarket = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                                numMarketPrice.Value = Math.Min(dbMarket, numMarketPrice.Maximum);

                                int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                                numAdults.Value = dbAdults < 1 ? 2 : Math.Min(dbAdults, 100);

                                try
                                {
                                    int dbChildren = reader["Children"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Children"]);
                                    numChildren.Value = Math.Max(0, Math.Min(dbChildren, 20));
                                }
                                catch { numChildren.Value = 0; }

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
                if (!isAddingMode)
                {
                    SetInputFieldsState(false);
                    ClearFields();
                    lblSidebarTitle.Text = "Деталі туру";
                    lblSidebarTitle.ForeColor = Color.Black;
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
                    var cmd = new SqlCommand(
                        "SELECT TOP 1 PricePerNight FROM HotelsDictionary WHERE City=@c AND Name=@n", conn);
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
                allClients.Clear();
                cmbAssignedUser.Items.Clear();
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand(
                        "SELECT Username FROM Users WHERE Role = 'User' ORDER BY Username", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string username = reader["Username"].ToString();
                            allClients.Add(username);
                            cmbAssignedUser.Items.Add(username);
                        }
                    }
                }
                if (cmbAssignedUser.Items.Count > 0) cmbAssignedUser.SelectedIndex = 0;
            }
            catch { }
        }

        private SqlCommand BuildTravelCommand(SqlConnection conn, string sql, bool includeId = false, int id = 0)
        {
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", cmbAssignedUser.Text);
            cmd.Parameters.AddWithValue("@co", txtCountry.Text.Trim());
            cmd.Parameters.AddWithValue("@ci", txtCity.Text.Trim());
            cmd.Parameters.AddWithValue("@dep", txtDepartureCity.Text.Trim());
            cmd.Parameters.AddWithValue("@depStreet", txtDepartureStreet.Text.Trim());
            cmd.Parameters.AddWithValue("@depDate", dtpDepartureDate.Value.Date);
            cmd.Parameters.AddWithValue("@depTime", txtDepartureTime.Text.Trim());
            cmd.Parameters.AddWithValue("@flightNum", txtFlightNumber.Text.Trim());
            cmd.Parameters.AddWithValue("@terminal", txtTerminal.Text.Trim());
            cmd.Parameters.AddWithValue("@transfer", txtTransferCity.Text.Trim());
            cmd.Parameters.AddWithValue("@tr", cmbTransport.Text);
            cmd.Parameters.AddWithValue("@b", numBudget.Value);
            cmd.Parameters.AddWithValue("@mp", numMarketPrice.Value);
            cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
            cmd.Parameters.AddWithValue("@ad", (int)numAdults.Value);
            cmd.Parameters.AddWithValue("@ch", (int)numChildren.Value);
            cmd.Parameters.AddWithValue("@ni", (int)numNights.Value);
            cmd.Parameters.AddWithValue("@bt", cmbBoard.Text);
            cmd.Parameters.AddWithValue("@hn", txtHotel.Text);
            if (includeId) cmd.Parameters.AddWithValue("@id", id);
            return cmd;
        }

        // ══════════════════════════════════════════
        // КНОПКИ
        // ══════════════════════════════════════════
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!isAddingMode)
            {
                ToggleAddMode(true);
                return;
            }
            if (!ValidateInputs()) return;

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"INSERT INTO Travels 
                        ([User],Country,City,DepartureCity,DepartureStreet,DepartureDate,DepartureTime,
                         FlightNumber,TerminalOrStation,TransferCity,TransportType,
                         Budget,MarketPrice,[Status],Adults,Children,Nights,BoardType,HotelName,Rating,Comment)
                        VALUES
                        (@u,@co,@ci,@dep,@depStreet,@depDate,@depTime,
                         @flightNum,@terminal,@transfer,@tr,
                         @b,@mp,@s,@ad,@ch,@ni,@bt,@hn,1,'')";
                    BuildTravelCommand(conn, sql).ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                ToggleAddMode(false);

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
            if (dgvData.SelectedRows.Count == 0)
            { MessageBox.Show("Оберіть тур для оновлення!", "Перевірка"); return; }
            if (!ValidateInputs()) return;

            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"UPDATE Travels SET
                        [User]=@u,Country=@co,City=@ci,DepartureCity=@dep,DepartureStreet=@depStreet,
                        DepartureDate=@depDate,DepartureTime=@depTime,FlightNumber=@flightNum,
                        TerminalOrStation=@terminal,TransferCity=@transfer,TransportType=@tr,
                        Budget=@b,MarketPrice=@mp,[Status]=@s,
                        Adults=@ad,Children=@ch,Nights=@ni,BoardType=@bt,HotelName=@hn
                        WHERE ID=@id";
                    BuildTravelCommand(conn, sql, true, id).ExecuteNonQuery();
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
            if (MessageBox.Show("Видалити тур?", "Підтвердження",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    new SqlCommand($"DELETE FROM Travels WHERE ID={id}", conn).ExecuteNonQuery();
                }
                LoadDataFromDatabase();
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
            if (isAddingMode)
            {
                ToggleAddMode(false);
                isUpdatingFromGrid = true;
                dgvData.ClearSelection();
                ClearFields();
                SetInputFieldsState(false);
                isUpdatingFromGrid = false;
                return;
            }
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

        private void btnOpenFeed_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            this.Hide();
            using (var feed = new UserFeedForm()) { feed.ShowDialog(); }
            this.Show();
            LoadDataFromDatabase();
        }

        private void btnAdminReturn_Click(object sender, EventArgs e)
        { if (!CheckUnsavedChanges()) return; this.Close(); }

        private void pbBack_Click(object sender, EventArgs e)
        { if (!CheckUnsavedChanges()) return; this.Close(); }

        private void pbExit_Click(object sender, EventArgs e)
        { if (!CheckUnsavedChanges()) return; Application.Exit(); }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e)
        { if (dragging) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }

    // HotelDictionaryForm залишається без змін — вона вже ідеальна
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
            this.Text = $"Довідник готелів — {city}, {country}";
            this.Size = new Size(860, 530);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;
            BuildUI();
            LoadHotels();
        }

        private void BuildUI()
        {
            var pnlLeft = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(255, 475),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlLeft.Controls.Add(new Label
            {
                Text = "Реєстрація готелю",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            });

            pnlLeft.Controls.Add(new Label { Text = "Назва готелю *", Location = new Point(10, 45), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9) });
            txtName = new TextBox { Location = new Point(10, 63), Size = new Size(225, 25), Font = new Font("Segoe UI", 10), MaxLength = 200 };
            pnlLeft.Controls.Add(txtName);

            pnlLeft.Controls.Add(new Label { Text = "Адреса / район", Location = new Point(10, 100), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9) });
            txtAddress = new TextBox { Location = new Point(10, 118), Size = new Size(225, 25), Font = new Font("Segoe UI", 10), MaxLength = 300 };
            pnlLeft.Controls.Add(txtAddress);

            pnlLeft.Controls.Add(new Label { Text = "Зірки (1–5)", Location = new Point(10, 155), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9) });
            numStars = new NumericUpDown { Location = new Point(10, 173), Size = new Size(100, 25), Minimum = 1, Maximum = 5, Value = 4, Font = new Font("Segoe UI", 10) };
            pnlLeft.Controls.Add(numStars);

            pnlLeft.Controls.Add(new Label { Text = "Ціна за 1 ніч ($) *", Location = new Point(10, 210), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 9) });
            numPrice = new NumericUpDown { Location = new Point(10, 228), Size = new Size(225, 25), Minimum = 1, Maximum = 100000, Value = 50, Font = new Font("Segoe UI", 10) };
            pnlLeft.Controls.Add(numPrice);

            var btnSave = new Button
            {
                Text = "💾 Зберегти готель",
                Location = new Point(10, 270),
                Size = new Size(225, 40),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;
            pnlLeft.Controls.Add(btnSave);

            var btnNew = new Button
            {
                Text = "✚ Новий готель",
                Location = new Point(10, 320),
                Size = new Size(225, 35),
                BackColor = Color.FromArgb(243, 244, 246),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnNew.Click += (s, e) =>
            {
                txtName.Clear(); txtAddress.Clear();
                numStars.Value = 4; numPrice.Value = 50;
                dgv.ClearSelection(); txtName.Focus();
            };
            pnlLeft.Controls.Add(btnNew);

            var btnDelete = new Button
            {
                Text = "🗑 Видалити обраний",
                Location = new Point(10, 365),
                Size = new Size(225, 35),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnDelete.Click += BtnDelete_Click;
            pnlLeft.Controls.Add(btnDelete);

            pnlLeft.Controls.Add(new Label
            {
                Text = $"📍 {city}, {country}",
                Location = new Point(10, 420),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8)
            });

            this.Controls.Add(pnlLeft);

            dgv = new DataGridView
            {
                Location = new Point(275, 10),
                Size = new Size(565, 420),
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
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.SelectionChanged += Dgv_SelectionChanged;
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnSelect_Click(s, e); };

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ID", Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Готель", FillWeight = 38 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stars", HeaderText = "⭐", FillWeight = 12 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "$/Ніч", FillWeight = 15 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Address", HeaderText = "Адреса", FillWeight = 35 });

            this.Controls.Add(dgv);

            var btnSelect = new Button
            {
                Text = "✅ Обрати для туру",
                Location = new Point(275, 440),
                Size = new Size(270, 48),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnSelect.Click += BtnSelect_Click;
            this.Controls.Add(btnSelect);

            var btnCancel = new Button
            {
                Text = "Закрити",
                Location = new Point(555, 440),
                Size = new Size(285, 48),
                BackColor = Color.FromArgb(229, 231, 235),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11)
            };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnCancel);

            this.Controls.Add(new Label
            {
                Text = "💡 Подвійний клік — обрати готель",
                Location = new Point(275, 495),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8)
            });
        }

        private void LoadHotels()
        {
            dgv.Rows.Clear();
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand(
                        "SELECT * FROM HotelsDictionary WHERE Country=@co AND City=@ci ORDER BY Stars DESC, Name", conn);
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
                                $"${reader["PricePerNight"]}",
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
            if (dgv.SelectedRows.Count == 0) return;
            txtName.Text = dgv.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "";
            txtAddress.Text = dgv.SelectedRows[0].Cells["Address"].Value?.ToString() ?? "";
            string starsStr = dgv.SelectedRows[0].Cells["Stars"].Value?.ToString() ?? "★★★★";
            numStars.Value = Math.Max(1, Math.Min(5, starsStr.Length));
            string priceStr = dgv.SelectedRows[0].Cells["Price"].Value?.ToString().Replace("$", "") ?? "50";
            if (decimal.TryParse(priceStr, out decimal price))
                numPrice.Value = Math.Min(price, numPrice.Maximum);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            { MessageBox.Show("Введіть назву готелю!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (numPrice.Value <= 0)
            { MessageBox.Show("Ціна має бути більше 0!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var checkCmd = new SqlCommand(
                        "SELECT ID FROM HotelsDictionary WHERE Country=@co AND City=@ci AND Name=@n", conn);
                    checkCmd.Parameters.AddWithValue("@co", country);
                    checkCmd.Parameters.AddWithValue("@ci", city);
                    checkCmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                    var existingId = checkCmd.ExecuteScalar();

                    if (existingId != null)
                    {
                        var upd = new SqlCommand(
                            "UPDATE HotelsDictionary SET Address=@a, Stars=@s, PricePerNight=@p WHERE ID=@id", conn);
                        upd.Parameters.AddWithValue("@a", txtAddress.Text.Trim());
                        upd.Parameters.AddWithValue("@s", (int)numStars.Value);
                        upd.Parameters.AddWithValue("@p", numPrice.Value);
                        upd.Parameters.AddWithValue("@id", existingId);
                        upd.ExecuteNonQuery();
                        MessageBox.Show("Готель оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        var ins = new SqlCommand(
                            "INSERT INTO HotelsDictionary (Country,City,Name,Address,Stars,PricePerNight) VALUES (@co,@ci,@n,@a,@s,@p)", conn);
                        ins.Parameters.AddWithValue("@co", country);
                        ins.Parameters.AddWithValue("@ci", city);
                        ins.Parameters.AddWithValue("@n", txtName.Text.Trim());
                        ins.Parameters.AddWithValue("@a", txtAddress.Text.Trim());
                        ins.Parameters.AddWithValue("@s", (int)numStars.Value);
                        ins.Parameters.AddWithValue("@p", numPrice.Value);
                        ins.ExecuteNonQuery();
                        MessageBox.Show("Готель додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                LoadHotels();
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            { MessageBox.Show("Оберіть готель для видалення!"); return; }
            string name = dgv.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "";
            if (MessageBox.Show($"Видалити «{name}»?", "Підтвердження",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            try
            {
                int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["ID"].Value);
                using (var conn = DatabaseService.GetConnection())
                    new SqlCommand($"DELETE FROM HotelsDictionary WHERE ID={id}", conn).ExecuteNonQuery();
                LoadHotels();
                txtName.Clear(); txtAddress.Clear();
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            { MessageBox.Show("Оберіть готель зі списку!", "Перевірка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            SelectedHotelName = dgv.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "";
            string priceStr = dgv.SelectedRows[0].Cells["Price"].Value?.ToString().Replace("$", "") ?? "0";
            decimal.TryParse(priceStr, out decimal price);
            SelectedPricePerNight = price;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}