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
        private DataTable travelTable = new DataTable();
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool isUpdatingFromGrid = false;

        private decimal currentHotelPricePerNight = 0m;
        private bool isAddingMode = false;
        private System.Windows.Forms.Timer badgeTimer = new System.Windows.Forms.Timer();
        private CheckBox chkMyTours = new CheckBox();

        public MainWork()
        {
            InitializeComponent();

            this.Load += MainWork_Load;
            this.Resize += MainWork_Resize;

            if (lblExit != null)
            {
                lblExit.Click += lblExit_Click;
                lblExit.MouseEnter += (s, e) => lblExit.ForeColor = Color.Crimson;
                lblExit.MouseLeave += (s, e) => lblExit.ForeColor = Color.LightGray;
            }
            if (lblBack != null) lblBack.Click += lblBack_Click;
            if (btnAdminReturn != null) btnAdminReturn.Click += btnAdminReturn_Click;
            if (btnOpenFeed != null) btnOpenFeed.Click += btnOpenFeed_Click;
            if (btnMessenger != null) btnMessenger.Click += btnMessenger_Click;

            if (pnlHeader != null)
            {
                pnlHeader.MouseDown += pnlHeader_MouseDown;
                pnlHeader.MouseMove += pnlHeader_MouseMove;
                pnlHeader.MouseUp += pnlHeader_MouseUp;
            }

            if (btnAdd != null) btnAdd.Click += btnAdd_Click;
            if (btnEdit != null) btnEdit.Click += btnEdit_Click;
            if (btnDelete != null) btnDelete.Click += btnDelete_Click;
            if (btnClear != null) btnClear.Click += btnClear_Click;
            if (btnCancelAdd != null) btnCancelAdd.Click += btnCancelAdd_Click;
            if (btnOpenMap != null) btnOpenMap.Click += btnOpenMap_Click;

            SoundHelper.AttachSounds(this);

            if (numMarketPrice != null) numMarketPrice.Maximum = 1000000m;
            if (numBudget != null) numBudget.Maximum = 1000000m;
            if (numNights != null) { numNights.Maximum = 365m; numNights.Minimum = 1m; }
            if (numAdults != null) { numAdults.Maximum = 100m; numAdults.Minimum = 1m; }
            if (numChildren != null) numChildren.Maximum = 20m;

            if (lblExit != null) lblExit.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            if (btnOpenFeed != null) btnOpenFeed.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            if (btnAdminReturn != null) btnAdminReturn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            if (btnMessenger != null) btnMessenger.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            SetupDataTableStructure();
            SetupFilters();
            BindEvents();
        }

        private void SetupFilters()
        {
            if (cmbFilterTours != null)
            {
                if (cmbFilterTours.Items.Count == 0)
                {
                    cmbFilterTours.Items.AddRange(new string[] { "Активні", "Запити", "Закінчені / Відмінені", "Всі тури" });
                }
                if (cmbFilterTours.SelectedIndex == -1) cmbFilterTours.SelectedIndex = 0;
                cmbFilterTours.SelectedIndexChanged += (s, e) => ApplyFilters();
            }

            if (txtSearch != null)
            {
                txtSearch.TextChanged += (s, e) => ApplyFilters();
            }

            chkMyTours.Text = "Тільки мої клієнти";
            chkMyTours.AutoSize = true;
            chkMyTours.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            chkMyTours.Location = new Point(565, 8);
            chkMyTours.Cursor = Cursors.Hand;
            chkMyTours.CheckedChanged += (s, e) => ApplyFilters();

            if (btnContactClient != null)
            {
                btnContactClient.Click += BtnContactClient_Click;

                if (btnContactClient.Parent != null)
                {
                    btnContactClient.Parent.Controls.Add(chkMyTours);
                    btnContactClient.Left = chkMyTours.Right + 15;
                }
            }
        }

        private void MainWork_Resize(object sender, EventArgs e)
        {
            if (pnlHeader != null)
            {
                if (lblExit != null)
                {
                    lblExit.Left = pnlHeader.Width - 40;
                    int currentRightEdge = lblExit.Left - 20;

                    if (btnOpenFeed != null && btnOpenFeed.Visible)
                    { btnOpenFeed.Left = currentRightEdge - btnOpenFeed.Width; currentRightEdge = btnOpenFeed.Left - 20; }

                    if (btnAdminReturn != null && btnAdminReturn.Visible)
                    { btnAdminReturn.Left = currentRightEdge - btnAdminReturn.Width; currentRightEdge = btnAdminReturn.Left - 20; }

                    if (btnMessenger != null && btnMessenger.Visible)
                    { btnMessenger.Left = currentRightEdge - btnMessenger.Width; }
                }
            }
        }

        private void BindEvents()
        {
            if (txtCountry != null)
            {
                txtCountry.TextChanged += (s, e) =>
                {
                    currentHotelPricePerNight = 0m;
                    if (txtHotel != null) { txtHotel.Text = "Натисніть для вибору готелю..."; txtHotel.ForeColor = Color.Gray; }
                    AutoCalculateMarketPrice();
                };
            }

            if (txtCity != null)
            {
                txtCity.TextChanged += (s, e) =>
                {
                    currentHotelPricePerNight = 0m;
                    if (txtHotel != null) { txtHotel.Text = "Натисніть для вибору готелю..."; txtHotel.ForeColor = Color.Gray; }
                    AutoCalculateMarketPrice();
                };
            }

            if (numAdults != null) numAdults.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            if (numChildren != null) numChildren.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            if (numNights != null) numNights.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            if (cmbBoard != null) cmbBoard.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            if (numBudget != null) numBudget.ValueChanged += (s, e) => AnalyzeBenefit();

            if (cmbTransport != null)
            {
                cmbTransport.SelectedIndexChanged += (s, e) => { AutoCalculateMarketPrice(); UpdateLogisticsVisibility(); };
            }

            if (txtHotel != null)
            {
                txtHotel.Click += (s, e) => OpenHotelDictionary();
                txtHotel.Cursor = Cursors.Hand;
            }

            if (txtDepartureTime != null) txtDepartureTime.Click += Mask_Click;
            if (txtTransferTime != null) txtTransferTime.Click += Mask_Click;

            if (txtDepartureStreet != null) txtDepartureStreet.Leave += FormatStreet_Leave;
            if (txtTransferStreet != null) txtTransferStreet.Leave += FormatStreet_Leave;
        }

        private void Mask_Click(object sender, EventArgs e)
        {
            MaskedTextBox mask = sender as MaskedTextBox;
            if (mask != null && mask.Text.Replace(":", "").Trim().Length == 0) mask.SelectionStart = 0;
        }

        private void FormatStreet_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null && !string.IsNullOrWhiteSpace(txt.Text))
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
            if (lblTitle != null) lblTitle.Text = $"WayPoint Business | {Session.Username}";

            string role = (Session.Role ?? "").Trim();
            bool isAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            if (btnAdminReturn != null) btnAdminReturn.Visible = isAdmin;
            if (btnMessenger != null) btnMessenger.Visible = !isAdmin;

            MainWork_Resize(this, EventArgs.Empty);

            if (!isAdmin)
            {
                badgeTimer.Interval = 3000;
                badgeTimer.Tick += (s, ev) => UpdateMessengerBadge();
                badgeTimer.Start();
                UpdateMessengerBadge();
            }

            LoadUsersToCombo();
            LoadDataFromDatabase();

            isUpdatingFromGrid = true;
            if (dgvData != null) dgvData.ClearSelection();
            ClearFields();
            SetInputFieldsState(false);
            isUpdatingFromGrid = false;

            AutoCalculateMarketPrice();
            UpdateLogisticsVisibility();
            AnalyzeBenefit();
        }

        private void ApplyFilters()
        {
            if (travelTable == null || travelTable.Rows.Count == 0 || cmbFilterTours == null) return;

            string filter = "";

            if (cmbFilterTours.Text == "Активні") filter = "[Status] NOT IN ('Запит', 'Завершено', 'Скасовано')";
            else if (cmbFilterTours.Text == "Запити") filter = "[Status] = 'Запит'";
            else if (cmbFilterTours.Text == "Закінчені / Відмінені") filter = "[Status] IN ('Завершено', 'Скасовано')";

            if (chkMyTours.Checked)
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Manager = '{Session.Username.Replace("'", "''")}'";
            }

            string search = txtSearch?.Text?.Trim().Replace("'", "''") ?? "";
            if (!string.IsNullOrEmpty(search))
            {
                string searchFilter = $"([Country] LIKE '%{search}%' OR [City] LIKE '%{search}%' OR [User] LIKE '%{search}%')";
                if (!string.IsNullOrEmpty(filter)) filter += $" AND {searchFilter}";
                else filter = searchFilter;
            }

            travelTable.DefaultView.RowFilter = filter;
        }

        private void BtnContactClient_Click(object sender, EventArgs e)
        {
            if (dgvData == null || dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть тур клієнта у таблиці, щоб почати чат!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string clientName = dgvData.SelectedRows[0].Cells["User"].Value?.ToString() ?? "";
            if (string.IsNullOrEmpty(clientName))
            {
                MessageBox.Show("Цей тур ще не закріплено за конкретним клієнтом.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();
            using (var messenger = new MessengerForm(clientName))
            {
                messenger.ShowDialog();
            }
            this.Show();
            UpdateMessengerBadge();
        }

        private void UpdateMessengerBadge()
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    string sql = "SELECT COUNT(*) FROM Messages WHERE ReceiverUsername='Support' AND IsRead=0";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (btnMessenger != null)
                        {
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
            }
            catch { }
        }

        private void UpdateLogisticsVisibility()
        {
            if (cmbTransport == null || cmbTransport.SelectedItem == null) return;

            string transport = cmbTransport.Text;
            bool isPlane = transport.Contains("Літак");
            bool isTransfer = transport.Contains("Пересадка");

            if (txtFlightNumber != null) txtFlightNumber.Enabled = isPlane;
            if (txtTerminal != null) txtTerminal.Enabled = isPlane || transport.Contains("Потяг") || transport.Contains("Автобус");

            if (txtTransferCity != null) txtTransferCity.Enabled = isTransfer;
            if (cmbTransferTransport != null) cmbTransferTransport.Enabled = isTransfer;
            if (txtTransferFlightNumber != null) txtTransferFlightNumber.Enabled = isTransfer;
            if (txtTransferTime != null) txtTransferTime.Enabled = isTransfer;
            if (txtTransferTerminal != null) txtTransferTerminal.Enabled = isTransfer;
            if (txtTransferStreet != null) txtTransferStreet.Enabled = isTransfer;

            if (txtFlightNumber != null && !txtFlightNumber.Enabled) txtFlightNumber.Clear();

            if (!isTransfer)
            {
                if (txtTransferCity != null) txtTransferCity.Clear();
                if (txtTransferFlightNumber != null) txtTransferFlightNumber.Clear();
                if (txtTransferTime != null) txtTransferTime.Clear();
                if (txtTransferTerminal != null) txtTransferTerminal.Clear();
                if (txtTransferStreet != null) txtTransferStreet.Clear();
                if (cmbTransferTransport != null && cmbTransferTransport.Items.Count > 0) cmbTransferTransport.SelectedIndex = -1;
            }
        }

        private void SetInputFieldsState(bool enabled)
        {
            if (txtCountry != null) txtCountry.Enabled = enabled;
            if (txtCity != null) txtCity.Enabled = enabled;
            if (txtHotel != null) txtHotel.Enabled = enabled;
            if (btnOpenMap != null) btnOpenMap.Enabled = enabled;
            if (numAdults != null) numAdults.Enabled = enabled;
            if (numChildren != null) numChildren.Enabled = enabled;
            if (numNights != null) numNights.Enabled = enabled;
            if (cmbBoard != null) cmbBoard.Enabled = enabled;

            if (cmbTransport != null) cmbTransport.Enabled = enabled;
            if (txtDepartureCity != null) txtDepartureCity.Enabled = enabled;
            if (txtDepartureStreet != null) txtDepartureStreet.Enabled = enabled;
            if (dtpDepartureDate != null) dtpDepartureDate.Enabled = enabled;
            if (txtDepartureTime != null) txtDepartureTime.Enabled = enabled;

            if (cmbAssignedUser != null) cmbAssignedUser.Enabled = enabled;
            if (cmbStatus != null) cmbStatus.Enabled = enabled;
            if (numBudget != null) numBudget.Enabled = enabled;

            if (enabled) UpdateLogisticsVisibility();
            else
            {
                if (txtFlightNumber != null) txtFlightNumber.Enabled = false;
                if (txtTerminal != null) txtTerminal.Enabled = false;
                if (txtTransferCity != null) txtTransferCity.Enabled = false;
                if (cmbTransferTransport != null) cmbTransferTransport.Enabled = false;
                if (txtTransferFlightNumber != null) txtTransferFlightNumber.Enabled = false;
                if (txtTransferTime != null) txtTransferTime.Enabled = false;
                if (txtTransferTerminal != null) txtTransferTerminal.Enabled = false;
                if (txtTransferStreet != null) txtTransferStreet.Enabled = false;
            }
        }

        private void ToggleAddMode(bool enable)
        {
            isAddingMode = enable;
            if (enable)
            {
                if (btnAdd != null) btnAdd.Text = "✅ Підтвердити";
                if (btnCancelAdd != null) btnCancelAdd.Visible = true;
                if (btnEdit != null) btnEdit.Visible = false;
                if (btnDelete != null) btnDelete.Visible = false;
                if (btnClear != null) btnClear.Visible = false;

                SetInputFieldsState(true);
                ClearFields();
                if (lblSidebarTitle != null) lblSidebarTitle.Text = "Створення туру";

                isUpdatingFromGrid = true;
                if (dgvData != null) dgvData.ClearSelection();
                isUpdatingFromGrid = false;
            }
            else
            {
                if (btnAdd != null) btnAdd.Text = "➕ Створити";
                if (btnCancelAdd != null) btnCancelAdd.Visible = false;
                if (btnEdit != null) btnEdit.Visible = true;
                if (btnDelete != null) btnDelete.Visible = true;
                if (btnClear != null) btnClear.Visible = true;
                if (lblSidebarTitle != null) lblSidebarTitle.Text = "Деталі туру";
            }
            AnalyzeBenefit();
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Скасувати створення туру?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ToggleAddMode(false);
                isUpdatingFromGrid = true;
                if (dgvData != null) dgvData.ClearSelection();
                SetInputFieldsState(false);
                ClearFields();
                isUpdatingFromGrid = false;
            }
        }

        private void ClearFields()
        {
            if (txtCountry != null) txtCountry.Clear();
            if (txtCity != null) txtCity.Clear();
            if (txtHotel != null) { txtHotel.Text = "Натисніть для вибору готелю..."; txtHotel.ForeColor = Color.Gray; }
            currentHotelPricePerNight = 0m;

            if (numAdults != null) numAdults.Value = 2;
            if (numChildren != null) numChildren.Value = 0;
            if (numNights != null) numNights.Value = 7;

            if (cmbBoard != null && cmbBoard.Items.Count > 0) cmbBoard.SelectedIndex = 4;

            if (txtDepartureCity != null) txtDepartureCity.Text = "Київ";
            if (txtDepartureStreet != null) txtDepartureStreet.Clear();
            if (dtpDepartureDate != null) dtpDepartureDate.Value = DateTime.Today;
            if (txtDepartureTime != null) txtDepartureTime.Clear();
            if (txtFlightNumber != null) txtFlightNumber.Clear();
            if (txtTerminal != null) txtTerminal.Clear();

            if (txtTransferCity != null) txtTransferCity.Clear();
            if (cmbTransferTransport != null && cmbTransferTransport.Items.Count > 0) cmbTransferTransport.SelectedIndex = -1;
            if (txtTransferFlightNumber != null) txtTransferFlightNumber.Clear();
            if (txtTransferTime != null) txtTransferTime.Clear();
            if (txtTransferTerminal != null) txtTransferTerminal.Clear();
            if (txtTransferStreet != null) txtTransferStreet.Clear();

            if (cmbTransport != null && cmbTransport.Items.Count > 0) cmbTransport.SelectedIndex = 0;

            // ФІКС ЦІНИ ПРИ СКИДАННІ
            if (numBudget != null) numBudget.Value = 0;
            if (numMarketPrice != null) numMarketPrice.Value = 0;

            if (cmbStatus != null && cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
            if (cmbAssignedUser != null && cmbAssignedUser.Items.Count > 0) cmbAssignedUser.SelectedIndex = -1;

            UpdateLogisticsVisibility();

            // ФІКС ЗАГОЛОВКА
            if (lblSidebarTitle != null)
            {
                lblSidebarTitle.Text = isAddingMode ? "Створення туру" : "Оберіть тур або створіть новий";
                lblSidebarTitle.ForeColor = Color.Black;
            }
            if (pbBenefitIcon != null) pbBenefitIcon.Visible = false;
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

        private bool ValidateTimeField(MaskedTextBox mask, DateTime? dateToCheck, string fieldName, TabPage errorTab, bool checkPastTime)
        {
            if (mask == null) return true;
            string rawText = mask.Text.Replace(":", "").Replace("_", "").Trim();
            if (rawText.Length == 0) return true;

            if (!TimeSpan.TryParse(mask.Text, out TimeSpan parsedTime) || parsedTime.TotalDays >= 1)
            {
                MessageBox.Show($"Некоректний час '{fieldName}'! Формат має бути від 00:00 до 23:59.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (tabControlInfo != null) tabControlInfo.SelectedTab = errorTab;
                return false;
            }

            if (checkPastTime && dateToCheck.HasValue && dateToCheck.Value.Date == DateTime.Today)
            {
                if (parsedTime < DateTime.Now.TimeOfDay)
                {
                    MessageBox.Show($"Час '{fieldName}' не може бути в минулому для сьогоднішньої дати при створенні туру!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (tabControlInfo != null) tabControlInfo.SelectedTab = errorTab;
                    return false;
                }
            }
            return true;
        }

        private bool ValidateInputs()
        {
            if (txtCountry == null || string.IsNullOrWhiteSpace(txtCountry.Text)) { MessageBox.Show("Вкажіть країну!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); if (tabControlInfo != null) tabControlInfo.SelectedTab = tabRoute; return false; }
            if (txtCity == null || string.IsNullOrWhiteSpace(txtCity.Text)) { MessageBox.Show("Вкажіть місто!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); if (tabControlInfo != null) tabControlInfo.SelectedTab = tabRoute; return false; }
            if (txtHotel == null || string.IsNullOrWhiteSpace(txtHotel.Text) || txtHotel.Text.Contains("Натисніть")) { MessageBox.Show("Оберіть готель!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); if (tabControlInfo != null) tabControlInfo.SelectedTab = tabRoute; return false; }
            if (cmbAssignedUser == null || cmbAssignedUser.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmbAssignedUser.Text)) { MessageBox.Show("Оберіть клієнта!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); if (tabControlInfo != null) tabControlInfo.SelectedTab = tabFinance; return false; }
            if (numBudget == null || numBudget.Value <= 0) { MessageBox.Show("Вкажіть вашу ціну (бюджет)!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); if (tabControlInfo != null) tabControlInfo.SelectedTab = tabFinance; return false; }

            if (isAddingMode && dtpDepartureDate != null && dtpDepartureDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Дата відправлення не може бути в минулому при створенні нового туру!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (tabControlInfo != null) tabControlInfo.SelectedTab = tabLogistics;
                return false;
            }

            if (!ValidateTimeField(txtDepartureTime, dtpDepartureDate?.Value, "відправлення", tabLogistics, isAddingMode)) return false;
            if (!ValidateTimeField(txtTransferTime, null, "пересадки", tabLogistics, isAddingMode)) return false;

            return true;
        }

        private void AutoCalculateMarketPrice()
        {
            if (isUpdatingFromGrid) return;

            // ФІКС ЦІНИ ПРИ ПОРОЖНІХ ПОЛЯХ
            if (string.IsNullOrWhiteSpace(txtCountry?.Text) || string.IsNullOrWhiteSpace(txtCity?.Text))
            {
                if (numMarketPrice != null) numMarketPrice.Value = 0;
                AnalyzeBenefit();
                return;
            }

            try
            {
                string loc = (txtCountry?.Text + " " + txtCity?.Text).Trim().ToLower();

                decimal baseNight = currentHotelPricePerNight > 0 ? currentHotelPricePerNight : 40m;
                if (currentHotelPricePerNight == 0m)
                {
                    if (loc.Contains("єгипет")) baseNight = 45m;
                    else if (loc.Contains("туреччина")) baseNight = 60m;
                    else if (loc.Contains("оае") || loc.Contains("дубай")) baseNight = 200m;
                    else if (loc.Contains("україна") || loc.Contains("київ")) baseNight = 20m;
                }

                decimal boardMult = 1.0m;
                string boardText = cmbBoard?.Text ?? "";
                if (boardText.Contains("BB")) boardMult = 1.1m;
                else if (boardText.Contains("HB")) boardMult = 1.3m;
                else if (boardText.Contains("FB")) boardMult = 1.5m;
                else if (boardText.Contains("AI")) boardMult = 1.8m;

                decimal transportPrice = 0m;
                string transportText = cmbTransport?.Text ?? "";
                if (transportText.Contains("Літак (Прямий)")) transportPrice = 250m;
                else if (transportText.Contains("Літак (Пересадка)")) transportPrice = 180m;
                else if (transportText.Contains("Автобус")) transportPrice = 70m;
                else if (transportText.Contains("Потяг")) transportPrice = 50m;

                decimal nights = numNights?.Value < 1 ? 1 : numNights?.Value ?? 7;
                decimal adults = numAdults?.Value < 1 ? 1 : numAdults?.Value ?? 2;
                decimal children = numChildren?.Value ?? 0;

                decimal hotelCost = baseNight * boardMult * nights * (adults + (children * 0.5m));
                decimal totalTransportCost = transportPrice * (adults + children);
                decimal totalCost = hotelCost + totalTransportCost;

                decimal finalMarketPrice = Math.Round(totalCost * 1.10m);
                if (numMarketPrice != null)
                {
                    if (finalMarketPrice > numMarketPrice.Maximum) finalMarketPrice = numMarketPrice.Maximum;
                    numMarketPrice.Value = finalMarketPrice;
                }

                AnalyzeBenefit();
            }
            catch { }
        }

        private void AnalyzeBenefit()
        {
            if (isUpdatingFromGrid || numBudget == null || numMarketPrice == null || lblSidebarTitle == null || pbBenefitIcon == null) return;

            decimal our = numBudget.Value;
            decimal market = numMarketPrice.Value;

            if (market > 0 || our > 0)
            {
                decimal diff = market - our;
                if (diff >= 0)
                {
                    lblSidebarTitle.Text = $"Вигода: +${diff}";
                    lblSidebarTitle.ForeColor = Color.Green;
                    pbBenefitIcon.Image = Properties.Resources.profit_up;
                    pbBenefitIcon.Visible = true;
                }
                else
                {
                    lblSidebarTitle.Text = $"Переплата: ${Math.Abs(diff)}";
                    lblSidebarTitle.ForeColor = Color.Red;
                    pbBenefitIcon.Image = Properties.Resources.profit_down;
                    pbBenefitIcon.Visible = true;
                }
            }
            else
            {
                lblSidebarTitle.Text = isAddingMode ? "Створення туру" : "Оберіть тур або створіть новий";
                lblSidebarTitle.ForeColor = Color.Black;
                pbBenefitIcon.Visible = false;
            }

            lblSidebarTitle.Refresh();
            pbBenefitIcon.Location = new Point(lblSidebarTitle.Right + 10, lblSidebarTitle.Top - 5);
        }

        private void OpenHotelDictionary()
        {
            if (txtCountry == null || txtCity == null) return;

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
                    if (txtHotel != null)
                    {
                        txtHotel.Text = form.SelectedHotelName;
                        txtHotel.ForeColor = Color.Black;
                    }
                    currentHotelPricePerNight = form.SelectedPricePerNight;
                    AutoCalculateMarketPrice();
                }
            }
        }

        private void SetupDataTableStructure()
        {
            if (dgvData == null) return;

            travelTable = new DataTable();
            travelTable.Columns.Add("ID", typeof(int));
            travelTable.Columns.Add("Manager", typeof(string));
            travelTable.Columns.Add("User", typeof(string));
            travelTable.Columns.Add("Country", typeof(string));
            travelTable.Columns.Add("City", typeof(string));
            travelTable.Columns.Add("Transport", typeof(string));
            travelTable.Columns.Add("Date", typeof(string));
            travelTable.Columns.Add("Budget", typeof(decimal));
            travelTable.Columns.Add("Status", typeof(string));

            dgvData.DataSource = travelTable;

            if (dgvData.Columns.Contains("ID"))
            {
                dgvData.Columns["ID"].Visible = false;
            }

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
                    using (var cmd = new SqlCommand("SELECT * FROM Travels", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = travelTable.NewRow();
                                row["ID"] = reader["ID"];
                                row["Manager"] = reader["Manager"] == DBNull.Value ? "" : reader["Manager"].ToString();
                                row["User"] = reader["User"] == DBNull.Value ? "" : reader["User"].ToString();
                                row["Country"] = reader["Country"] == DBNull.Value ? "" : reader["Country"].ToString();
                                row["City"] = reader["City"] == DBNull.Value ? "" : reader["City"].ToString();
                                row["Transport"] = reader["TransportType"] == DBNull.Value ? "Літак (Прямий)" : reader["TransportType"].ToString();

                                string dateStr = reader["DepartureDate"] == DBNull.Value ? "" : Convert.ToDateTime(reader["DepartureDate"]).ToShortDateString();
                                row["Date"] = dateStr;

                                row["Budget"] = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                                row["Status"] = reader["Status"] == DBNull.Value ? "Запит" : reader["Status"].ToString();

                                travelTable.Rows.Add(row);
                            }
                        }
                    }
                }

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження даних: " + ex.Message);
            }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (isUpdatingFromGrid || dgvData == null) return;

            if (isAddingMode && dgvData.SelectedRows.Count > 0)
            {
                if (!CheckUnsavedChanges())
                {
                    isUpdatingFromGrid = true;
                    dgvData.ClearSelection();
                    isUpdatingFromGrid = false;
                    return;
                }
                else
                {
                    ToggleAddMode(false);
                }
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
                        using (var cmd = new SqlCommand("SELECT * FROM Travels WHERE ID=@id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (txtCountry != null) txtCountry.Text = reader["Country"]?.ToString() ?? "";
                                    if (txtCity != null) txtCity.Text = reader["City"]?.ToString() ?? "";

                                    try { if (txtHotel != null) txtHotel.Text = reader["HotelName"]?.ToString() ?? ""; } catch { if (txtHotel != null) txtHotel.Text = ""; }
                                    if (txtHotel != null && !string.IsNullOrEmpty(txtHotel.Text)) txtHotel.ForeColor = Color.Black;

                                    currentHotelPricePerNight = GetHotelPriceFromDB(txtCity?.Text, txtHotel?.Text);

                                    int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                                    if (numAdults != null) numAdults.Value = dbAdults < 1 ? 2 : Math.Min(dbAdults, 100);

                                    try { if (numChildren != null) numChildren.Value = reader["Children"] == DBNull.Value ? 0 : Math.Min(Convert.ToInt32(reader["Children"]), 20); } catch { if (numChildren != null) numChildren.Value = 0; }

                                    int dbNights = reader["Nights"] == DBNull.Value ? 7 : Convert.ToInt32(reader["Nights"]);
                                    if (numNights != null) numNights.Value = dbNights < 1 ? 7 : Math.Min(dbNights, 365);

                                    if (cmbBoard != null) cmbBoard.Text = reader["BoardType"]?.ToString() ?? "AI (Все вкл.)";
                                    if (cmbTransport != null) cmbTransport.Text = reader["TransportType"]?.ToString() ?? "Літак (Прямий)";
                                    if (txtDepartureCity != null) txtDepartureCity.Text = reader["DepartureCity"]?.ToString() ?? "Київ";

                                    try { if (txtDepartureStreet != null) txtDepartureStreet.Text = reader["DepartureStreet"]?.ToString() ?? ""; } catch { if (txtDepartureStreet != null) txtDepartureStreet.Text = ""; }
                                    try { if (dtpDepartureDate != null) dtpDepartureDate.Value = reader["DepartureDate"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["DepartureDate"]); } catch { if (dtpDepartureDate != null) dtpDepartureDate.Value = DateTime.Today; }
                                    try { if (txtDepartureTime != null) txtDepartureTime.Text = reader["DepartureTime"]?.ToString() ?? ""; } catch { if (txtDepartureTime != null) txtDepartureTime.Text = ""; }
                                    try { if (txtFlightNumber != null) txtFlightNumber.Text = reader["FlightNumber"]?.ToString() ?? ""; } catch { if (txtFlightNumber != null) txtFlightNumber.Text = ""; }
                                    try { if (txtTerminal != null) txtTerminal.Text = reader["TerminalOrStation"]?.ToString() ?? ""; } catch { if (txtTerminal != null) txtTerminal.Text = ""; }

                                    try { if (txtTransferCity != null) txtTransferCity.Text = reader["TransferCity"]?.ToString() ?? ""; } catch { if (txtTransferCity != null) txtTransferCity.Text = ""; }
                                    try { if (cmbTransferTransport != null) cmbTransferTransport.Text = reader["TransferTransport"]?.ToString() ?? ""; } catch { if (cmbTransferTransport != null) cmbTransferTransport.SelectedIndex = -1; }
                                    try { if (txtTransferStreet != null) txtTransferStreet.Text = reader["TransferStreet"]?.ToString() ?? ""; } catch { if (txtTransferStreet != null) txtTransferStreet.Text = ""; }
                                    try { if (txtTransferTime != null) txtTransferTime.Text = reader["TransferTime"]?.ToString() ?? ""; } catch { if (txtTransferTime != null) txtTransferTime.Text = ""; }
                                    try { if (txtTransferFlightNumber != null) txtTransferFlightNumber.Text = reader["TransferFlightNumber"]?.ToString() ?? ""; } catch { if (txtTransferFlightNumber != null) txtTransferFlightNumber.Text = ""; }
                                    try { if (txtTransferTerminal != null) txtTransferTerminal.Text = reader["TransferTerminal"]?.ToString() ?? ""; } catch { if (txtTransferTerminal != null) txtTransferTerminal.Text = ""; }

                                    decimal dbMarket = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                                    if (numMarketPrice != null) numMarketPrice.Value = Math.Min(dbMarket, numMarketPrice.Maximum);

                                    decimal dbBudget = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                                    if (numBudget != null) numBudget.Value = Math.Min(dbBudget, numBudget.Maximum);

                                    if (cmbStatus != null) cmbStatus.Text = reader["Status"]?.ToString() ?? "Запит";
                                    if (cmbAssignedUser != null) cmbAssignedUser.Text = reader["User"]?.ToString() ?? "";

                                    if (lblSidebarTitle != null)
                                    {
                                        lblSidebarTitle.Text = "Деталі туру";
                                        lblSidebarTitle.ForeColor = Color.Black;
                                    }
                                }
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
                    AnalyzeBenefit();
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
                    using (var cmd = new SqlCommand("SELECT TOP 1 PricePerNight FROM HotelsDictionary WHERE City=@c AND Name=@n", conn))
                    {
                        cmd.Parameters.AddWithValue("@c", city);
                        cmd.Parameters.AddWithValue("@n", hotelName);

                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch { }
            return 0m;
        }

        private void LoadUsersToCombo()
        {
            if (cmbAssignedUser == null) return;

            try
            {
                cmbAssignedUser.Items.Clear();

                using (var conn = DatabaseService.GetConnection())
                {
                    using (var cmd = new SqlCommand("SELECT Username FROM Users WHERE Role = 'User'", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbAssignedUser.Items.Add(reader["Username"].ToString());
                            }
                        }
                    }
                }
            }
            catch { }
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
                        (Manager,[User],Country,City,DepartureCity,TransportType,Budget,MarketPrice,
                         [Status],Adults,Nights,BoardType,HotelName,Rating,Comment,
                         Children,DepartureStreet,DepartureDate,DepartureTime,TransferCity,FlightNumber,TerminalOrStation,
                         TransferTransport,TransferStreet,TransferTime,TransferFlightNumber,TransferTerminal)
                        VALUES
                        (@mgr,@u,@co,@ci,@dep,@tr,@b,@mp,@s,@ad,@ni,@bt,@hn,1,'',
                         @ch,@ds,@dd,@dt,@tc,@fn,@tos,
                         @ttr,@tst,@ttm,@tfn,@ttos)";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mgr", Session.Username);
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
                }

                isUpdatingFromGrid = true;
                LoadDataFromDatabase();
                ToggleAddMode(false);
                if (dgvData != null) dgvData.ClearSelection();
                SetInputFieldsState(false);
                ClearFields();
                isUpdatingFromGrid = false;

                MessageBox.Show("Тур успішно створено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка додавання: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData == null || dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть тур для оновлення!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;

            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"UPDATE Travels SET
                        Manager=@mgr, [User]=@u,Country=@co,City=@ci,DepartureCity=@dep,TransportType=@tr,
                        Budget=@b,MarketPrice=@mp,[Status]=@s,
                        Adults=@ad,Nights=@ni,BoardType=@bt,HotelName=@hn,
                        Children=@ch,DepartureStreet=@ds,DepartureDate=@dd,DepartureTime=@dt,
                        TransferCity=@tc,FlightNumber=@fn,TerminalOrStation=@tos,
                        TransferTransport=@ttr,TransferStreet=@tst,TransferTime=@ttm,
                        TransferFlightNumber=@tfn,TransferTerminal=@ttos
                        WHERE ID=@id";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@mgr", Session.Username);
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
                }

                isUpdatingFromGrid = true;
                LoadDataFromDatabase();
                isUpdatingFromGrid = false;

                MessageBox.Show("Дані туру оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка оновлення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData == null || dgvData.SelectedRows.Count == 0) return;

            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;

            if (MessageBox.Show("Видалити тур?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    using (var cmd = new SqlCommand($"DELETE FROM Travels WHERE ID={id}", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                isUpdatingFromGrid = true;
                LoadDataFromDatabase();
                if (dgvData != null) dgvData.ClearSelection();
                ClearFields();
                SetInputFieldsState(false);
                isUpdatingFromGrid = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            isUpdatingFromGrid = true;
            if (dgvData != null) dgvData.ClearSelection();
            ClearFields();
            SetInputFieldsState(false);
            isUpdatingFromGrid = false;
        }

        private void btnOpenMap_Click(object sender, EventArgs e)
        {
            if (txtCity == null || txtCountry == null) return;

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
            using (var feed = new UserFeedForm())
            {
                feed.ShowDialog();
            }
            this.Show();

            isUpdatingFromGrid = true;
            LoadDataFromDatabase();
            isUpdatingFromGrid = false;
        }

        private void btnMessenger_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;

            this.Hide();
            using (var messenger = new MessengerForm())
            {
                messenger.ShowDialog();
            }
            this.Show();

            UpdateMessengerBadge();
        }

        private void btnAdminReturn_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            this.Close();
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
            this.Close();
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            if (!CheckUnsavedChanges()) return;
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

    public class HotelDictionaryForm : Form
    {
        // Цей код не змінювався, але залишений для повноти файлу
        public string SelectedHotelName { get; private set; } = string.Empty;
        public decimal SelectedPricePerNight { get; private set; }
        private string country; private string city;
        private DataGridView dgv = new DataGridView(); private TextBox txtName = new TextBox(); private TextBox txtAddress = new TextBox();
        private NumericUpDown numStars = new NumericUpDown(); private NumericUpDown numPrice = new NumericUpDown();

        public HotelDictionaryForm(string country, string city)
        {
            this.country = country; this.city = city; this.Text = $"CRM Довідник Готелів — {city}, {country}"; this.Size = new Size(850, 500);
            this.StartPosition = FormStartPosition.CenterParent; this.FormBorderStyle = FormBorderStyle.FixedDialog; this.MaximizeBox = false; this.BackColor = Color.WhiteSmoke;
            BuildUI(); LoadHotels();
        }

        private void BuildUI()
        {
            Panel pnlLeft = new Panel() { Location = new Point(10, 10), Size = new Size(250, 440), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            pnlLeft.Controls.Add(new Label() { Text = "Реєстрація готелю", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true });
            pnlLeft.Controls.Add(new Label() { Text = "Назва готелю:", Location = new Point(10, 45), AutoSize = true, ForeColor = Color.Gray });
            txtName.Location = new Point(10, 65); txtName.Size = new Size(220, 25); txtName.Font = new Font("Segoe UI", 10); pnlLeft.Controls.Add(txtName);
            pnlLeft.Controls.Add(new Label() { Text = "Адреса (вулиця/район):", Location = new Point(10, 105), AutoSize = true, ForeColor = Color.Gray });
            txtAddress.Location = new Point(10, 125); txtAddress.Size = new Size(220, 25); txtAddress.Font = new Font("Segoe UI", 10); pnlLeft.Controls.Add(txtAddress);
            pnlLeft.Controls.Add(new Label() { Text = "Кількість зірок (1-5):", Location = new Point(10, 165), AutoSize = true, ForeColor = Color.Gray });
            numStars.Location = new Point(10, 185); numStars.Size = new Size(220, 25); numStars.Minimum = 1; numStars.Maximum = 5; numStars.Value = 4; numStars.Font = new Font("Segoe UI", 10); pnlLeft.Controls.Add(numStars);
            pnlLeft.Controls.Add(new Label() { Text = "Ціна за 1 ніч ($):", Location = new Point(10, 225), AutoSize = true, ForeColor = Color.Gray });
            numPrice.Location = new Point(10, 245); numPrice.Size = new Size(220, 25); numPrice.Minimum = 1; numPrice.Maximum = 100000; numPrice.Value = 50; numPrice.Font = new Font("Segoe UI", 10); pnlLeft.Controls.Add(numPrice);
            Button btnAdd = new Button() { Text = "💾 Додати / Оновити", Location = new Point(10, 290), Size = new Size(220, 40), BackColor = Color.FromArgb(59, 130, 246), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) }; btnAdd.Click += BtnAdd_Click; pnlLeft.Controls.Add(btnAdd);
            Button btnDelete = new Button() { Text = "🗑 Видалити обраний", Location = new Point(10, 340), Size = new Size(220, 40), BackColor = Color.FromArgb(239, 68, 68), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) }; btnDelete.Click += BtnDelete_Click; pnlLeft.Controls.Add(btnDelete);
            this.Controls.Add(pnlLeft);

            dgv.Location = new Point(270, 10); dgv.Size = new Size(550, 380); dgv.ReadOnly = true; dgv.AllowUserToAddRows = false; dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgv.BackgroundColor = Color.White; dgv.BorderStyle = BorderStyle.None; dgv.RowHeadersVisible = false; dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.EnableHeadersVisualStyles = false; dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246); dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246); dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black; dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold); dgv.SelectionChanged += Dgv_SelectionChanged;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ID", Visible = false }); dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Готель", FillWeight = 40 }); dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stars", HeaderText = "Зірки", FillWeight = 15 }); dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "$/Ніч", FillWeight = 20 }); dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Address", HeaderText = "Адреса", FillWeight = 25 });
            this.Controls.Add(dgv);

            Button btnSelect = new Button() { Text = "✅ Обрати для туру", Location = new Point(270, 400), Size = new Size(250, 50), BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12, FontStyle.Bold) }; btnSelect.Click += BtnSelect_Click; this.Controls.Add(btnSelect);
            SoundHelper.AttachSounds(this);
        }

        private void LoadHotels()
        {
            dgv.Rows.Clear();
            try { using (var conn = DatabaseService.GetConnection()) { using (var cmd = new SqlCommand("SELECT * FROM HotelsDictionary WHERE Country=@co AND City=@ci", conn)) { cmd.Parameters.AddWithValue("@co", country); cmd.Parameters.AddWithValue("@ci", city); using (var reader = cmd.ExecuteReader()) { while (reader.Read()) { dgv.Rows.Add(reader["ID"], reader["Name"], new string('★', Convert.ToInt32(reader["Stars"])), reader["PricePerNight"], reader["Address"]); } } } } dgv.ClearSelection(); } catch { }
        }

        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0) { txtName.Text = dgv.SelectedRows[0].Cells["Name"].Value?.ToString() ?? ""; txtAddress.Text = dgv.SelectedRows[0].Cells["Address"].Value?.ToString() ?? ""; numStars.Value = dgv.SelectedRows[0].Cells["Stars"].Value?.ToString()?.Length ?? 1; numPrice.Value = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Price"].Value); }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Введіть назву готелю!"); return; }
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    using (var checkCmd = new SqlCommand("SELECT ID FROM HotelsDictionary WHERE Country=@co AND City=@ci AND Name=@n", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@co", country); checkCmd.Parameters.AddWithValue("@ci", city); checkCmd.Parameters.AddWithValue("@n", txtName.Text); var existingId = checkCmd.ExecuteScalar();
                        if (existingId != null) { using (var update = new SqlCommand("UPDATE HotelsDictionary SET Address=@a, Stars=@s, PricePerNight=@p WHERE ID=@id", conn)) { update.Parameters.AddWithValue("@a", txtAddress.Text); update.Parameters.AddWithValue("@s", numStars.Value); update.Parameters.AddWithValue("@p", numPrice.Value); update.Parameters.AddWithValue("@id", existingId); update.ExecuteNonQuery(); } }
                        else { using (var insert = new SqlCommand("INSERT INTO HotelsDictionary (Country, City, Name, Address, Stars, PricePerNight) VALUES (@co, @ci, @n, @a, @s, @p)", conn)) { insert.Parameters.AddWithValue("@co", country); insert.Parameters.AddWithValue("@ci", city); insert.Parameters.AddWithValue("@n", txtName.Text); insert.Parameters.AddWithValue("@a", txtAddress.Text); insert.Parameters.AddWithValue("@s", numStars.Value); insert.Parameters.AddWithValue("@p", numPrice.Value); insert.ExecuteNonQuery(); } }
                    }
                }
                LoadHotels(); MessageBox.Show("Готель збережено в довіднику!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Видалити готель з бази?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.No) return;
            try { int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["ID"].Value); using (var conn = DatabaseService.GetConnection()) { using (var cmd = new SqlCommand($"DELETE FROM HotelsDictionary WHERE ID={id}", conn)) { cmd.ExecuteNonQuery(); } } LoadHotels(); txtName.Clear(); txtAddress.Clear(); } catch { }
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count > 0) { SelectedHotelName = dgv.SelectedRows[0].Cells["Name"].Value?.ToString() ?? ""; SelectedPricePerNight = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Price"].Value); this.DialogResult = DialogResult.OK; this.Close(); } else { MessageBox.Show("Оберіть готель зі списку!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}