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
        private bool isAddingMode = false;
        private System.Windows.Forms.Timer badgeTimer;

        public MainWork()
        {
            InitializeComponent();

            SoundHelper.AttachSounds(this);
            numMarketPrice.Maximum = 1000000m;
            numBudget.Maximum = 1000000m;
            numNights.Maximum = 365m;
            numAdults.Maximum = 100m;
            numChildren.Maximum = 20m;
            numNights.Minimum = 1m;
            numAdults.Minimum = 1m;

            // Відв'язуємо стандартні якорі, щоб форма не псувала кнопки при фулскріні
            lblExit.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnOpenFeed.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnAdminReturn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnMessenger.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Додаємо власну подію зміни розміру, яка ідеально розставить кнопки
            this.Resize += MainWork_Resize;

            SetupDataTableStructure();
            BindEvents();
        }

        // === МАТЕМАТИЧНЕ ВИРІВНЮВАННЯ КНОПОК ===
        private void MainWork_Resize(object sender, EventArgs e)
        {
            if (pnlHeader != null)
            {
                lblExit.Left = pnlHeader.Width - 40;
                int currentRightEdge = lblExit.Left - 20;

                if (btnOpenFeed.Visible)
                {
                    btnOpenFeed.Left = currentRightEdge - btnOpenFeed.Width;
                    currentRightEdge = btnOpenFeed.Left - 20;
                }
                if (btnAdminReturn.Visible)
                {
                    btnAdminReturn.Left = currentRightEdge - btnAdminReturn.Width;
                    currentRightEdge = btnAdminReturn.Left - 20;
                }
                if (btnMessenger.Visible)
                {
                    btnMessenger.Left = currentRightEdge - btnMessenger.Width;
                }
            }
        }

        private void BindEvents()
        {
            txtCountry.TextChanged += (s, e) => { currentHotelPricePerNight = 0m; txtHotel.Text = "Натисніть для вибору готелю..."; txtHotel.ForeColor = Color.Gray; AutoCalculateMarketPrice(); };
            txtCity.TextChanged += (s, e) => { currentHotelPricePerNight = 0m; txtHotel.Text = "Натисніть для вибору готелю..."; txtHotel.ForeColor = Color.Gray; AutoCalculateMarketPrice(); };

            numAdults.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            numChildren.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            numNights.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            cmbBoard.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            numBudget.ValueChanged += (s, e) => AnalyzeBenefit();

            cmbTransport.SelectedIndexChanged += (s, e) => { AutoCalculateMarketPrice(); UpdateLogisticsVisibility(); };

            txtHotel.Click += (s, e) => OpenHotelDictionary();
            txtHotel.Cursor = Cursors.Hand;

            txtDepartureTime.Click += Mask_Click;
            txtTransferTime.Click += Mask_Click;

            txtDepartureStreet.Leave += FormatStreet_Leave;
            txtTransferStreet.Leave += FormatStreet_Leave;
        }

        private void Mask_Click(object sender, EventArgs e)
        {
            MaskedTextBox mask = sender as MaskedTextBox;
            if (mask.Text.Replace(":", "").Trim().Length == 0)
                mask.SelectionStart = 0;
        }

        private void FormatStreet_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(txt.Text))
            {
                string text = txt.Text.Trim();
                if (!text.StartsWith("Вул.", StringComparison.OrdinalIgnoreCase))
                {
                    if (text.Length > 0) text = char.ToUpper(text[0]) + text.Substring(1);
                    txt.Text = "Вул. " + text;
                }
            }
        }

        private void MainWork_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"WayPoint Business | {Session.Username}";

            string role = (Session.Role ?? "").Trim();
            bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            // ЛОГІКА ДОСТУПУ
            btnAdminReturn.Visible = isAdmin;
            btnMessenger.Visible = !isAdmin; // Працівник БАЧИТЬ месенджер, Адмін НІ

            // Оновлюємо дизайн кнопок після зміни видимості
            MainWork_Resize(this, EventArgs.Empty);

            if (!isAdmin)
            {
                badgeTimer = new System.Windows.Forms.Timer { Interval = 3000 };
                badgeTimer.Tick += (s, ev) => UpdateMessengerBadge();
                badgeTimer.Start();
                UpdateMessengerBadge();
            }

            dtpDepartureDate.MinDate = DateTime.Today;

            LoadUsersToCombo();
            LoadDataFromDatabase();

            isUpdatingFromGrid = true;
            dgvData.ClearSelection();
            ClearFields();
            SetInputFieldsState(false);
            isUpdatingFromGrid = false;

            AutoCalculateMarketPrice();
            UpdateLogisticsVisibility();
        }

        private void UpdateMessengerBadge()
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    string sql = "SELECT COUNT(*) FROM Messages WHERE ReceiverUsername='Support' AND IsRead=0";
                    var cmd = new SqlCommand(sql, conn);
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
            catch { }
        }

        private void UpdateLogisticsVisibility()
        {
            if (cmbTransport.SelectedItem == null) return;

            string transport = cmbTransport.Text;
            bool isPlane = transport.Contains("Літак");
            bool isTransfer = transport.Contains("Пересадка");

            txtFlightNumber.Enabled = isPlane;
            txtTerminal.Enabled = isPlane || transport.Contains("Потяг") || transport.Contains("Автобус");

            txtTransferCity.Enabled = isTransfer;
            cmbTransferTransport.Enabled = isTransfer;
            txtTransferFlightNumber.Enabled = isTransfer;
            txtTransferTime.Enabled = isTransfer;
            txtTransferTerminal.Enabled = isTransfer;
            txtTransferStreet.Enabled = isTransfer;

            if (!txtFlightNumber.Enabled) txtFlightNumber.Clear();
            if (!isTransfer)
            {
                txtTransferCity.Clear(); txtTransferFlightNumber.Clear();
                txtTransferTime.Clear(); txtTransferTerminal.Clear(); txtTransferStreet.Clear();
                if (cmbTransferTransport.Items.Count > 0) cmbTransferTransport.SelectedIndex = -1;
            }
        }

        private void SetInputFieldsState(bool enabled)
        {
            txtCountry.Enabled = enabled; txtCity.Enabled = enabled;
            txtHotel.Enabled = enabled; btnOpenMap.Enabled = enabled;
            numAdults.Enabled = enabled; numChildren.Enabled = enabled;
            numNights.Enabled = enabled; cmbBoard.Enabled = enabled;

            cmbTransport.Enabled = enabled; txtDepartureCity.Enabled = enabled;
            txtDepartureStreet.Enabled = enabled; dtpDepartureDate.Enabled = enabled;
            txtDepartureTime.Enabled = enabled;

            cmbAssignedUser.Enabled = enabled; cmbStatus.Enabled = enabled;
            numBudget.Enabled = enabled;

            if (enabled) UpdateLogisticsVisibility();
            else
            {
                txtFlightNumber.Enabled = false; txtTerminal.Enabled = false;
                txtTransferCity.Enabled = false; cmbTransferTransport.Enabled = false;
                txtTransferFlightNumber.Enabled = false; txtTransferTime.Enabled = false;
                txtTransferTerminal.Enabled = false; txtTransferStreet.Enabled = false;
            }
        }

        private void ToggleAddMode(bool enable)
        {
            isAddingMode = enable;
            if (enable)
            {
                btnAdd.Text = "✅ Підтвердити";
                btnCancelAdd.Visible = true;
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
                btnAdd.Text = "➕ Створити";
                btnCancelAdd.Visible = false;
                btnEdit.Visible = true;
                btnDelete.Visible = true;
                btnClear.Visible = true;
                lblSidebarTitle.Text = "Деталі туру";
            }
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Скасувати створення туру?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ToggleAddMode(false);
                isUpdatingFromGrid = true;
                dgvData.ClearSelection();
                SetInputFieldsState(false);
                ClearFields();
                isUpdatingFromGrid = false;
            }
        }

        private void ClearFields()
        {
            txtCountry.Clear(); txtCity.Clear();
            txtHotel.Text = "Натисніть для вибору готелю...";
            txtHotel.ForeColor = Color.Gray;
            currentHotelPricePerNight = 0m;
            numAdults.Value = 2; numChildren.Value = 0; numNights.Value = 7;
            if (cmbBoard.Items.Count > 0) cmbBoard.SelectedIndex = 4;

            txtDepartureCity.Text = "Київ"; txtDepartureStreet.Clear();
            dtpDepartureDate.Value = DateTime.Today; txtDepartureTime.Clear();
            txtFlightNumber.Clear(); txtTerminal.Clear();

            txtTransferCity.Clear(); if (cmbTransferTransport.Items.Count > 0) cmbTransferTransport.SelectedIndex = -1;
            txtTransferFlightNumber.Clear(); txtTransferTime.Clear(); txtTransferTerminal.Clear(); txtTransferStreet.Clear();

            if (cmbTransport.Items.Count > 0) cmbTransport.SelectedIndex = 0;

            numBudget.Value = 0; numMarketPrice.Value = 0;
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
            if (cmbAssignedUser.Items.Count > 0) cmbAssignedUser.SelectedIndex = -1;

            UpdateLogisticsVisibility();
        }

        private bool CheckUnsavedChanges()
        {
            if (isAddingMode)
            {
                var result = MessageBox.Show("Ви не зберегли новий тур! Всі введені дані будуть втрачені.\n\nПродовжити?", "Увага", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return result == DialogResult.Yes;
            }
            return true;
        }

        // === ВАЛІДАЦІЯ ЧАСУ (00:00 - 23:59 ТА ПЕРЕВІРКА МИНУЛОГО ЧАСУ) ===
        private bool ValidateTimeField(MaskedTextBox mask, DateTime? dateToCheck, string fieldName, TabPage errorTab)
        {
            string rawText = mask.Text.Replace(":", "").Replace("_", "").Trim();
            if (rawText.Length == 0) return true; // Поле не обов'язкове

            // TimeSpan.TryParse успішно парсить "25:00" як 1 день 1 годину, тому забороняємо >= 1 день
            if (!TimeSpan.TryParse(mask.Text, out TimeSpan parsedTime) || parsedTime.TotalDays >= 1)
            {
                MessageBox.Show($"Некоректний час '{fieldName}'! Формат має бути від 00:00 до 23:59.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabControlInfo.SelectedTab = errorTab;
                return false;
            }

            if (dateToCheck.HasValue && dateToCheck.Value.Date == DateTime.Today)
            {
                if (parsedTime < DateTime.Now.TimeOfDay)
                {
                    MessageBox.Show($"Час '{fieldName}' не може бути в минулому для сьогоднішньої дати!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabControlInfo.SelectedTab = errorTab;
                    return false;
                }
            }
            return true;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCountry.Text)) { MessageBox.Show("Вкажіть країну!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabControlInfo.SelectedTab = tabRoute; return false; }
            if (string.IsNullOrWhiteSpace(txtCity.Text)) { MessageBox.Show("Вкажіть місто!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabControlInfo.SelectedTab = tabRoute; return false; }
            if (string.IsNullOrWhiteSpace(txtHotel.Text) || txtHotel.Text.Contains("Натисніть")) { MessageBox.Show("Оберіть готель!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabControlInfo.SelectedTab = tabRoute; return false; }
            if (cmbAssignedUser.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmbAssignedUser.Text)) { MessageBox.Show("Оберіть клієнта!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabControlInfo.SelectedTab = tabFinance; return false; }
            if (numBudget.Value <= 0) { MessageBox.Show("Вкажіть вашу ціну (бюджет)!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); tabControlInfo.SelectedTab = tabFinance; return false; }

            // Перевіряємо обидва часи
            if (!ValidateTimeField(txtDepartureTime, dtpDepartureDate.Value, "відправлення", tabLogistics)) return false;
            if (!ValidateTimeField(txtTransferTime, null, "пересадки", tabLogistics)) return false;

            return true;
        }

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
                decimal children = numChildren.Value;

                decimal hotelCost = baseNight * boardMult * nights * (adults + (children * 0.5m));
                decimal totalTransportCost = transportPrice * (adults + children);
                decimal totalCost = hotelCost + totalTransportCost;

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

        private void SetupDataTableStructure()
        {
            travelTable = new DataTable();
            travelTable.Columns.Add("ID", typeof(int));
            travelTable.Columns.Add("User", typeof(string));
            travelTable.Columns.Add("Country", typeof(string));
            travelTable.Columns.Add("City", typeof(string));
            travelTable.Columns.Add("Transport", typeof(string));
            travelTable.Columns.Add("Date", typeof(string));
            travelTable.Columns.Add("Budget", typeof(decimal));
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
                            row["Transport"] = reader["TransportType"] == DBNull.Value ? "Літак (Прямий)" : reader["TransportType"];

                            string dateStr = reader["DepartureDate"] == DBNull.Value ? "" : Convert.ToDateTime(reader["DepartureDate"]).ToShortDateString();
                            row["Date"] = dateStr;

                            row["Budget"] = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
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
                else ToggleAddMode(false);
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

                                try { txtHotel.Text = reader["HotelName"]?.ToString() ?? ""; } catch { txtHotel.Text = ""; }
                                if (!string.IsNullOrEmpty(txtHotel.Text)) txtHotel.ForeColor = Color.Black;
                                currentHotelPricePerNight = GetHotelPriceFromDB(txtCity.Text, txtHotel.Text);

                                int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                                numAdults.Value = dbAdults < 1 ? 2 : Math.Min(dbAdults, 100);

                                try { numChildren.Value = reader["Children"] == DBNull.Value ? 0 : Math.Min(Convert.ToInt32(reader["Children"]), 20); } catch { numChildren.Value = 0; }

                                int dbNights = reader["Nights"] == DBNull.Value ? 7 : Convert.ToInt32(reader["Nights"]);
                                numNights.Value = dbNights < 1 ? 7 : Math.Min(dbNights, 365);

                                cmbBoard.Text = reader["BoardType"]?.ToString() ?? "AI (Все вкл.)";

                                cmbTransport.Text = reader["TransportType"]?.ToString() ?? "Літак (Прямий)";
                                txtDepartureCity.Text = reader["DepartureCity"]?.ToString() ?? "Київ";

                                try { txtDepartureStreet.Text = reader["DepartureStreet"]?.ToString() ?? ""; } catch { txtDepartureStreet.Text = ""; }
                                try { dtpDepartureDate.Value = reader["DepartureDate"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["DepartureDate"]); } catch { dtpDepartureDate.Value = DateTime.Today; }
                                try { txtDepartureTime.Text = reader["DepartureTime"]?.ToString() ?? ""; } catch { txtDepartureTime.Text = ""; }
                                try { txtFlightNumber.Text = reader["FlightNumber"]?.ToString() ?? ""; } catch { txtFlightNumber.Text = ""; }
                                try { txtTerminal.Text = reader["TerminalOrStation"]?.ToString() ?? ""; } catch { txtTerminal.Text = ""; }

                                try { txtTransferCity.Text = reader["TransferCity"]?.ToString() ?? ""; } catch { txtTransferCity.Text = ""; }
                                try { cmbTransferTransport.Text = reader["TransferTransport"]?.ToString() ?? ""; } catch { cmbTransferTransport.SelectedIndex = -1; }
                                try { txtTransferStreet.Text = reader["TransferStreet"]?.ToString() ?? ""; } catch { txtTransferStreet.Text = ""; }
                                try { txtTransferTime.Text = reader["TransferTime"]?.ToString() ?? ""; } catch { txtTransferTime.Text = ""; }
                                try { txtTransferFlightNumber.Text = reader["TransferFlightNumber"]?.ToString() ?? ""; } catch { txtTransferFlightNumber.Text = ""; }
                                try { txtTransferTerminal.Text = reader["TransferTerminal"]?.ToString() ?? ""; } catch { txtTransferTerminal.Text = ""; }

                                decimal dbMarket = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                                numMarketPrice.Value = Math.Min(dbMarket, numMarketPrice.Maximum);

                                decimal dbBudget = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                                numBudget.Value = Math.Min(dbBudget, numBudget.Maximum);

                                cmbStatus.Text = reader["Status"]?.ToString() ?? "Запит";
                                cmbAssignedUser.Text = reader["User"]?.ToString() ?? "";

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
                    UpdateLogisticsVisibility();
                    AutoCalculateMarketPrice();
                }
            }
            else
            {
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
                    var cmd = new SqlCommand("SELECT Username FROM Users WHERE Role = 'User'", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbAssignedUser.Items.Add(reader["Username"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Помилка завантаження клієнтів: " + ex.Message); }
        }

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
                        ([User],Country,City,DepartureCity,TransportType,Budget,MarketPrice,
                         [Status],Adults,Nights,BoardType,HotelName,Rating,Comment,
                         Children,DepartureStreet,DepartureDate,DepartureTime,TransferCity,FlightNumber,TerminalOrStation,
                         TransferTransport,TransferStreet,TransferTime,TransferFlightNumber,TransferTerminal)
                        VALUES
                        (@u,@co,@ci,@dep,@tr,@b,@mp,@s,@ad,@ni,@bt,@hn,1,'',
                         @ch,@ds,@dd,@dt,@tc,@fn,@tos,
                         @ttr,@tst,@ttm,@tfn,@ttos)";
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

                    string hotelToSave = txtHotel.Text.Contains("Натисніть") ? "" : txtHotel.Text;
                    cmd.Parameters.AddWithValue("@hn", hotelToSave);

                    cmd.Parameters.AddWithValue("@ch", (int)numChildren.Value);
                    cmd.Parameters.AddWithValue("@ds", txtDepartureStreet.Text);
                    cmd.Parameters.AddWithValue("@dd", dtpDepartureDate.Value.Date);
                    string savedTime = txtDepartureTime.Text.Replace(":", "").Trim().Length > 0 ? txtDepartureTime.Text : "";
                    cmd.Parameters.AddWithValue("@dt", savedTime);
                    cmd.Parameters.AddWithValue("@tc", txtTransferCity.Text);
                    cmd.Parameters.AddWithValue("@fn", txtFlightNumber.Text);
                    cmd.Parameters.AddWithValue("@tos", txtTerminal.Text);

                    cmd.Parameters.AddWithValue("@ttr", cmbTransferTransport.Text ?? "");
                    cmd.Parameters.AddWithValue("@tst", txtTransferStreet.Text);
                    string tSavedTime = txtTransferTime.Text.Replace(":", "").Trim().Length > 0 ? txtTransferTime.Text : "";
                    cmd.Parameters.AddWithValue("@ttm", tSavedTime);
                    cmd.Parameters.AddWithValue("@tfn", txtTransferFlightNumber.Text);
                    cmd.Parameters.AddWithValue("@ttos", txtTransferTerminal.Text);

                    cmd.ExecuteNonQuery();
                }

                isUpdatingFromGrid = true;
                LoadDataFromDatabase();
                ToggleAddMode(false);
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
                        Adults=@ad,Nights=@ni,BoardType=@bt,HotelName=@hn,
                        Children=@ch,DepartureStreet=@ds,DepartureDate=@dd,DepartureTime=@dt,
                        TransferCity=@tc,FlightNumber=@fn,TerminalOrStation=@tos,
                        TransferTransport=@ttr,TransferStreet=@tst,TransferTime=@ttm,
                        TransferFlightNumber=@tfn,TransferTerminal=@ttos
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

                    string hotelToSave = txtHotel.Text.Contains("Натисніть") ? "" : txtHotel.Text;
                    cmd.Parameters.AddWithValue("@hn", hotelToSave);

                    cmd.Parameters.AddWithValue("@ch", (int)numChildren.Value);
                    cmd.Parameters.AddWithValue("@ds", txtDepartureStreet.Text);
                    cmd.Parameters.AddWithValue("@dd", dtpDepartureDate.Value.Date);
                    string savedTime = txtDepartureTime.Text.Replace(":", "").Trim().Length > 0 ? txtDepartureTime.Text : "";
                    cmd.Parameters.AddWithValue("@dt", savedTime);
                    cmd.Parameters.AddWithValue("@tc", txtTransferCity.Text);
                    cmd.Parameters.AddWithValue("@fn", txtFlightNumber.Text);
                    cmd.Parameters.AddWithValue("@tos", txtTerminal.Text);

                    cmd.Parameters.AddWithValue("@ttr", cmbTransferTransport.Text ?? "");
                    cmd.Parameters.AddWithValue("@tst", txtTransferStreet.Text);
                    string tSavedTime = txtTransferTime.Text.Replace(":", "").Trim().Length > 0 ? txtTransferTime.Text : "";
                    cmd.Parameters.AddWithValue("@ttm", tSavedTime);
                    cmd.Parameters.AddWithValue("@tfn", txtTransferFlightNumber.Text);
                    cmd.Parameters.AddWithValue("@ttos", txtTransferTerminal.Text);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                isUpdatingFromGrid = true;
                LoadDataFromDatabase();
                isUpdatingFromGrid = false;

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
                isUpdatingFromGrid = true;
                LoadDataFromDatabase();
                dgvData.ClearSelection();
                ClearFields();
                SetInputFieldsState(false);
                isUpdatingFromGrid = false;
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
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
            isUpdatingFromGrid = true;
            LoadDataFromDatabase();
            isUpdatingFromGrid = false;
        }

        private void btnMessenger_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            this.Hide();
            using (var messenger = new MessengerForm()) { messenger.ShowDialog(); }
            this.Show();
            UpdateMessengerBadge();
        }

        private void btnAdminReturn_Click(object sender, EventArgs e) { if (!CheckUnsavedChanges()) return; this.Close(); }
        private void lblBack_Click(object sender, EventArgs e) { if (!CheckUnsavedChanges()) return; this.Close(); }
        private void lblExit_Click(object sender, EventArgs e) { if (!CheckUnsavedChanges()) return; Application.Exit(); }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging && this.WindowState != FormWindowState.Maximized) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (badgeTimer != null) { badgeTimer.Stop(); badgeTimer.Dispose(); }
            base.OnFormClosed(e);
        }
    }

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
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246);
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

            SoundHelper.AttachSounds(this); // ПІДКЛЮЧЕНО ЗВУК
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
            else MessageBox.Show("Оберіть готель зі списку!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}