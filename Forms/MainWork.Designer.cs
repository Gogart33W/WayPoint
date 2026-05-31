namespace WayPoint
{
    partial class MainWork
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlData = new System.Windows.Forms.Panel();

            this.btnAdminReturn = new System.Windows.Forms.Button();
            this.btnOpenFeed = new System.Windows.Forms.Button();
            this.btnMessenger = new System.Windows.Forms.Button(); // НОВА КНОПКА МЕСЕНДЖЕРА
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBack = new System.Windows.Forms.Label(); // ЗАМІНА PICTUREBOX
            this.lblExit = new System.Windows.Forms.Label(); // ЗАМІНА PICTUREBOX

            this.lblSidebarTitle = new System.Windows.Forms.Label();
            this.tabControlInfo = new System.Windows.Forms.TabControl();
            this.tabRoute = new System.Windows.Forms.TabPage();
            this.tabLogistics = new System.Windows.Forms.TabPage();
            this.tabFinance = new System.Windows.Forms.TabPage();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancelAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();

            this.dgvData = new System.Windows.Forms.DataGridView();
            this.lblDataTitle = new System.Windows.Forms.Label();

            // Поля Вкладка 1
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.btnOpenMap = new System.Windows.Forms.Button();
            this.txtHotel = new System.Windows.Forms.TextBox();
            this.numAdults = new System.Windows.Forms.NumericUpDown();
            this.numChildren = new System.Windows.Forms.NumericUpDown();
            this.numNights = new System.Windows.Forms.NumericUpDown();
            this.cmbBoard = new System.Windows.Forms.ComboBox();

            // Поля Вкладка 2
            this.cmbTransport = new System.Windows.Forms.ComboBox();
            this.txtDepartureCity = new System.Windows.Forms.TextBox();
            this.txtDepartureStreet = new System.Windows.Forms.TextBox();
            this.dtpDepartureDate = new System.Windows.Forms.DateTimePicker();
            this.txtDepartureTime = new System.Windows.Forms.MaskedTextBox();
            this.txtFlightNumber = new System.Windows.Forms.TextBox();
            this.txtTerminal = new System.Windows.Forms.TextBox();
            this.txtTransferCity = new System.Windows.Forms.TextBox();
            this.cmbTransferTransport = new System.Windows.Forms.ComboBox();
            this.txtTransferStreet = new System.Windows.Forms.TextBox();
            this.txtTransferTime = new System.Windows.Forms.MaskedTextBox();
            this.txtTransferFlightNumber = new System.Windows.Forms.TextBox();
            this.txtTransferTerminal = new System.Windows.Forms.TextBox();

            // Поля Вкладка 3
            this.numMarketPrice = new System.Windows.Forms.NumericUpDown();
            this.numBudget = new System.Windows.Forms.NumericUpDown();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.cmbAssignedUser = new System.Windows.Forms.ComboBox();

            this.pnlHeader.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.tabControlInfo.SuspendLayout();
            this.tabRoute.SuspendLayout();
            this.tabLogistics.SuspendLayout();
            this.tabFinance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAdults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChildren)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarketPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).BeginInit();
            this.pnlData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();

            // ===== HEADER =====
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlHeader.Controls.Add(this.btnMessenger);
            this.pnlHeader.Controls.Add(this.btnAdminReturn);
            this.pnlHeader.Controls.Add(this.btnOpenFeed);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblBack);
            this.pnlHeader.Controls.Add(this.lblExit);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;

            this.lblTitle.Text = "WayPoint Business";
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(60, 15);
            this.lblTitle.AutoSize = true;

            // Кнопка Месенджер
            this.btnMessenger.BackColor = System.Drawing.Color.FromArgb(59, 130, 246); // Синя
            this.btnMessenger.ForeColor = System.Drawing.Color.White;
            this.btnMessenger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMessenger.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnMessenger.Location = new System.Drawing.Point(740, 12);
            this.btnMessenger.Size = new System.Drawing.Size(160, 35);
            this.btnMessenger.Text = "💬 Месенджер";
            this.btnMessenger.FlatAppearance.BorderSize = 0;
            this.btnMessenger.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnMessenger.Click += new System.EventHandler(this.btnMessenger_Click);

            // Кнопка Адмінка
            this.btnAdminReturn.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnAdminReturn.ForeColor = System.Drawing.Color.White;
            this.btnAdminReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminReturn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnAdminReturn.Location = new System.Drawing.Point(910, 12);
            this.btnAdminReturn.Size = new System.Drawing.Size(160, 35);
            this.btnAdminReturn.Text = "🛡️ Адмінка";
            this.btnAdminReturn.FlatAppearance.BorderSize = 0;
            this.btnAdminReturn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdminReturn.Click += new System.EventHandler(this.btnAdminReturn_Click);

            // Кнопка Стрічка
            this.btnOpenFeed.BackColor = System.Drawing.Color.FromArgb(139, 92, 246);
            this.btnOpenFeed.ForeColor = System.Drawing.Color.White;
            this.btnOpenFeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnOpenFeed.Location = new System.Drawing.Point(1080, 12);
            this.btnOpenFeed.Size = new System.Drawing.Size(160, 35);
            this.btnOpenFeed.Text = "🌍 Стрічка";
            this.btnOpenFeed.FlatAppearance.BorderSize = 0;
            this.btnOpenFeed.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOpenFeed.Click += new System.EventHandler(this.btnOpenFeed_Click);

            // lblBack (Заміна pbBack)
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBack.Font = new System.Drawing.Font("Segoe UI Emoji", 16F, System.Drawing.FontStyle.Bold);
            this.lblBack.ForeColor = System.Drawing.Color.LightGray;
            this.lblBack.Location = new System.Drawing.Point(15, 12);
            this.lblBack.Text = "🔙";
            this.lblBack.Click += new System.EventHandler(this.lblBack_Click);

            // lblExit (Заміна pbExit)
            this.lblExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExit.AutoSize = true;
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblExit.ForeColor = System.Drawing.Color.LightGray;
            this.lblExit.Location = new System.Drawing.Point(1300, 12);
            this.lblExit.Text = "✕";
            this.lblExit.Click += new System.EventHandler(this.lblExit_Click);
            this.lblExit.MouseEnter += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.Crimson;
            this.lblExit.MouseLeave += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.LightGray;

            // ===== SIDEBAR =====
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.Location = new System.Drawing.Point(20, 80);
            this.pnlSidebar.Size = new System.Drawing.Size(340, 600);
            this.pnlSidebar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSidebar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            this.lblSidebarTitle.Text = "Новий тур";
            this.lblSidebarTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSidebarTitle.Location = new System.Drawing.Point(15, 10);
            this.lblSidebarTitle.AutoSize = true;

            // --- TAB CONTROL ---
            this.tabControlInfo.Location = new System.Drawing.Point(10, 45);
            this.tabControlInfo.Size = new System.Drawing.Size(320, 430);
            this.tabControlInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.tabControlInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this.tabRoute.Text = "🌍 Маршрут";
            this.tabRoute.BackColor = System.Drawing.Color.White;
            this.tabLogistics.Text = "✈️ Логістика";
            this.tabLogistics.BackColor = System.Drawing.Color.White;
            this.tabFinance.Text = "💰 Фінанси";
            this.tabFinance.BackColor = System.Drawing.Color.White;

            this.tabControlInfo.Controls.Add(this.tabRoute);
            this.tabControlInfo.Controls.Add(this.tabLogistics);
            this.tabControlInfo.Controls.Add(this.tabFinance);
            this.pnlSidebar.Controls.Add(this.tabControlInfo);

            // ----------------------------------------------------
            // ВКЛАДКА 1: МАРШРУТ (УЩІЛЬНЕНО)
            // ----------------------------------------------------
            AddLabel("Країна", 10, 10, tabRoute, System.Drawing.FontStyle.Regular);
            this.txtCountry.Location = new System.Drawing.Point(10, 28);
            this.txtCountry.Size = new System.Drawing.Size(130, 25);
            this.txtCountry.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabRoute.Controls.Add(this.txtCountry);

            AddLabel("Місто", 10, 150, tabRoute, System.Drawing.FontStyle.Regular);
            this.txtCity.Location = new System.Drawing.Point(150, 28);
            this.txtCity.Size = new System.Drawing.Size(110, 25);
            this.txtCity.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabRoute.Controls.Add(this.txtCity);

            this.btnOpenMap.Location = new System.Drawing.Point(265, 27);
            this.btnOpenMap.Size = new System.Drawing.Size(35, 27);
            this.btnOpenMap.Text = "🗺";
            this.btnOpenMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenMap.Click += new System.EventHandler(this.btnOpenMap_Click);
            this.tabRoute.Controls.Add(this.btnOpenMap);

            AddLabel("🏨 Готель (Довідник CRM)", 65, 10, tabRoute, System.Drawing.FontStyle.Bold);
            this.txtHotel.Location = new System.Drawing.Point(10, 85);
            this.txtHotel.Size = new System.Drawing.Size(290, 25);
            this.txtHotel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtHotel.ReadOnly = true;
            this.tabRoute.Controls.Add(this.txtHotel);

            AddLabel("Дорослі", 125, 10, tabRoute, System.Drawing.FontStyle.Regular);
            this.numAdults.Location = new System.Drawing.Point(10, 145);
            this.numAdults.Size = new System.Drawing.Size(70, 25);
            this.numAdults.Minimum = 1; this.numAdults.Value = 2;
            this.tabRoute.Controls.Add(this.numAdults);

            AddLabel("Діти", 125, 90, tabRoute, System.Drawing.FontStyle.Regular);
            this.numChildren.Location = new System.Drawing.Point(90, 145);
            this.numChildren.Size = new System.Drawing.Size(70, 25);
            this.tabRoute.Controls.Add(this.numChildren);

            AddLabel("Ночей", 125, 170, tabRoute, System.Drawing.FontStyle.Regular);
            this.numNights.Location = new System.Drawing.Point(170, 145);
            this.numNights.Size = new System.Drawing.Size(70, 25);
            this.numNights.Minimum = 1; this.numNights.Value = 7;
            this.tabRoute.Controls.Add(this.numNights);

            AddLabel("🍴 Харчування", 185, 10, tabRoute, System.Drawing.FontStyle.Regular);
            this.cmbBoard.Items.AddRange(new object[] { "RO (Без їжі)", "BB (Сніданок)", "HB (Снід.+Веч.)", "FB (3-разове)", "AI (Все вкл.)" });
            this.cmbBoard.Location = new System.Drawing.Point(10, 205);
            this.cmbBoard.Size = new System.Drawing.Size(290, 25);
            this.cmbBoard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tabRoute.Controls.Add(this.cmbBoard);

            // ----------------------------------------------------
            // ВКЛАДКА 2: ЛОГІСТИКА (УЩІЛЬНЕНО)
            // ----------------------------------------------------
            AddLabel("— ОСНОВНИЙ РЕЙС / ВІДПРАВЛЕННЯ —", 5, 10, tabLogistics, System.Drawing.FontStyle.Bold);

            AddLabel("Транспорт", 25, 10, tabLogistics, System.Drawing.FontStyle.Regular);
            this.cmbTransport.Items.AddRange(new object[] { "Літак (Прямий)", "Літак (Пересадка)", "Автобус", "Потяг" });
            this.cmbTransport.Location = new System.Drawing.Point(10, 45);
            this.cmbTransport.Size = new System.Drawing.Size(290, 25);
            this.cmbTransport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tabLogistics.Controls.Add(this.cmbTransport);

            AddLabel("Звідки (Місто)", 75, 10, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtDepartureCity.Location = new System.Drawing.Point(10, 95);
            this.txtDepartureCity.Size = new System.Drawing.Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtDepartureCity);

            AddLabel("Вулиця / Локація", 75, 150, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtDepartureStreet.Location = new System.Drawing.Point(150, 95);
            this.txtDepartureStreet.Size = new System.Drawing.Size(150, 25);
            this.tabLogistics.Controls.Add(this.txtDepartureStreet);

            AddLabel("Дата виїзду", 125, 10, tabLogistics, System.Drawing.FontStyle.Regular);
            this.dtpDepartureDate.Location = new System.Drawing.Point(10, 145);
            this.dtpDepartureDate.Size = new System.Drawing.Size(130, 25);
            this.dtpDepartureDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tabLogistics.Controls.Add(this.dtpDepartureDate);

            AddLabel("Час (00:00)", 125, 150, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtDepartureTime.Mask = "00:00";
            this.txtDepartureTime.ValidatingType = typeof(System.DateTime);
            this.txtDepartureTime.Location = new System.Drawing.Point(150, 145);
            this.txtDepartureTime.Size = new System.Drawing.Size(150, 25);
            this.txtDepartureTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabLogistics.Controls.Add(this.txtDepartureTime);

            AddLabel("Рейс (Або Автобус №)", 175, 10, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtFlightNumber.Location = new System.Drawing.Point(10, 195);
            this.txtFlightNumber.Size = new System.Drawing.Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtFlightNumber);

            AddLabel("Термінал / Вокзал", 175, 150, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtTerminal.Location = new System.Drawing.Point(150, 195);
            this.txtTerminal.Size = new System.Drawing.Size(150, 25);
            this.tabLogistics.Controls.Add(this.txtTerminal);

            AddLabel("— ІНФОРМАЦІЯ ПРО ПЕРЕСАДКУ —", 230, 10, tabLogistics, System.Drawing.FontStyle.Bold);

            AddLabel("Місто пересадки", 250, 10, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtTransferCity.Location = new System.Drawing.Point(10, 270);
            this.txtTransferCity.Size = new System.Drawing.Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtTransferCity);

            AddLabel("Транспорт", 250, 150, tabLogistics, System.Drawing.FontStyle.Regular);
            this.cmbTransferTransport.Items.AddRange(new object[] { "Літак", "Автобус", "Потяг" });
            this.cmbTransferTransport.Location = new System.Drawing.Point(150, 270);
            this.cmbTransferTransport.Size = new System.Drawing.Size(150, 25);
            this.cmbTransferTransport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tabLogistics.Controls.Add(this.cmbTransferTransport);

            AddLabel("Рейс пересадки", 300, 10, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtTransferFlightNumber.Location = new System.Drawing.Point(10, 320);
            this.txtTransferFlightNumber.Size = new System.Drawing.Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtTransferFlightNumber);

            AddLabel("Час виїзду", 300, 150, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtTransferTime.Mask = "00:00";
            this.txtTransferTime.ValidatingType = typeof(System.DateTime);
            this.txtTransferTime.Location = new System.Drawing.Point(150, 320);
            this.txtTransferTime.Size = new System.Drawing.Size(150, 25);
            this.txtTransferTime.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabLogistics.Controls.Add(this.txtTransferTime);

            AddLabel("Термінал / Локація", 350, 10, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtTransferTerminal.Location = new System.Drawing.Point(10, 370);
            this.txtTransferTerminal.Size = new System.Drawing.Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtTransferTerminal);

            AddLabel("Вулиця (якщо є)", 350, 150, tabLogistics, System.Drawing.FontStyle.Regular);
            this.txtTransferStreet.Location = new System.Drawing.Point(150, 370);
            this.txtTransferStreet.Size = new System.Drawing.Size(150, 25);
            this.tabLogistics.Controls.Add(this.txtTransferStreet);

            // ----------------------------------------------------
            // ВКЛАДКА 3: ФІНАНСИ
            // ----------------------------------------------------
            AddLabel("Клієнт (Пошук: почніть вводити ім'я)", 10, 10, tabFinance, System.Drawing.FontStyle.Bold);
            this.cmbAssignedUser.Location = new System.Drawing.Point(10, 30);
            this.cmbAssignedUser.Size = new System.Drawing.Size(290, 25);
            this.cmbAssignedUser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAssignedUser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tabFinance.Controls.Add(this.cmbAssignedUser);

            AddLabel("Статус туру", 70, 10, tabFinance, System.Drawing.FontStyle.Regular);
            this.cmbStatus.Items.AddRange(new object[] { "Запит", "Планується", "Оплачено", "Завершено" });
            this.cmbStatus.Location = new System.Drawing.Point(10, 90);
            this.cmbStatus.Size = new System.Drawing.Size(290, 25);
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tabFinance.Controls.Add(this.cmbStatus);

            AddLabel("Ринкова ціна (Авторозрахунок)", 130, 10, tabFinance, System.Drawing.FontStyle.Regular);
            this.numMarketPrice.Location = new System.Drawing.Point(10, 150);
            this.numMarketPrice.Size = new System.Drawing.Size(290, 25);
            this.numMarketPrice.ReadOnly = true;
            this.numMarketPrice.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.tabFinance.Controls.Add(this.numMarketPrice);

            AddLabel("НАША ЦІНА БЮДЖЕТ ($)", 190, 10, tabFinance, System.Drawing.FontStyle.Bold);
            this.numBudget.Location = new System.Drawing.Point(10, 210);
            this.numBudget.Size = new System.Drawing.Size(290, 30);
            this.numBudget.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.numBudget.BackColor = System.Drawing.Color.FromArgb(220, 252, 231);
            this.tabFinance.Controls.Add(this.numBudget);

            // ----------------------------------------------------
            // КНОПКИ УПРАВЛІННЯ (Внизу, завжди видимі)
            // ----------------------------------------------------
            this.btnAdd.Location = new System.Drawing.Point(10, 485);
            this.btnAdd.Size = new System.Drawing.Size(155, 45);
            this.btnAdd.Text = "➕ Створити";
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            this.pnlSidebar.Controls.Add(this.btnAdd);

            this.btnCancelAdd.Location = new System.Drawing.Point(175, 485);
            this.btnCancelAdd.Size = new System.Drawing.Size(145, 45);
            this.btnCancelAdd.Text = "Скасувати";
            this.btnCancelAdd.BackColor = System.Drawing.Color.FromArgb(107, 114, 128); // Сірий
            this.btnCancelAdd.ForeColor = System.Drawing.Color.White;
            this.btnCancelAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnCancelAdd.Visible = false; // Прихована по замовчуванню
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);
            this.pnlSidebar.Controls.Add(this.btnCancelAdd);

            this.btnEdit.Location = new System.Drawing.Point(175, 485);
            this.btnEdit.Size = new System.Drawing.Size(145, 45);
            this.btnEdit.Text = "Оновити";
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.pnlSidebar.Controls.Add(this.btnEdit);

            this.btnDelete.Location = new System.Drawing.Point(10, 540);
            this.btnDelete.Size = new System.Drawing.Size(155, 45);
            this.btnDelete.Text = "Видалити";
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.pnlSidebar.Controls.Add(this.btnDelete);

            this.btnClear.Location = new System.Drawing.Point(175, 540);
            this.btnClear.Size = new System.Drawing.Size(145, 45);
            this.btnClear.Text = "Скинути";
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.pnlSidebar.Controls.Add(this.btnClear);

            // ===== DATA PANEL =====
            this.pnlData.Location = new System.Drawing.Point(380, 80);
            this.pnlData.Size = new System.Drawing.Size(950, 600); // Початковий розмір для фулскріну
            this.pnlData.BackColor = System.Drawing.Color.White;
            this.pnlData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this.dgvData.Location = new System.Drawing.Point(20, 70);
            this.dgvData.Size = new System.Drawing.Size(910, 510);
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            this.lblDataTitle.Text = "База подорожей";
            this.lblDataTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDataTitle.Location = new System.Drawing.Point(20, 20);
            this.lblDataTitle.AutoSize = true;

            this.pnlData.Controls.Add(this.lblDataTitle);
            this.pnlData.Controls.Add(this.dgvData);

            // ===== ФОРМА =====
            this.ClientSize = new System.Drawing.Size(1350, 700);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(MainWork_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.tabControlInfo.ResumeLayout(false);
            this.tabRoute.ResumeLayout(false);
            this.tabRoute.PerformLayout();
            this.tabLogistics.ResumeLayout(false);
            this.tabLogistics.PerformLayout();
            this.tabFinance.ResumeLayout(false);
            this.tabFinance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAdults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChildren)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarketPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).EndInit();
            this.pnlData.ResumeLayout(false);
            this.pnlData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
        }

        private void AddLabel(string text, int y, int x, System.Windows.Forms.Control parent, System.Drawing.FontStyle style)
        {
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F, style),
                ForeColor = style == System.Drawing.FontStyle.Bold ? System.Drawing.Color.Black : System.Drawing.Color.Gray
            };
            parent.Controls.Add(lbl);
        }

        private System.Windows.Forms.Panel pnlHeader, pnlSidebar, pnlData;
        private System.Windows.Forms.Label lblTitle, lblSidebarTitle, lblDataTitle;
        private System.Windows.Forms.Label lblBack, lblExit;
        private System.Windows.Forms.Button btnOpenFeed, btnAdminReturn, btnMessenger, btnOpenMap, btnAdd, btnCancelAdd, btnEdit, btnDelete, btnClear;
        private System.Windows.Forms.TabControl tabControlInfo;
        private System.Windows.Forms.TabPage tabRoute, tabLogistics, tabFinance;
        private System.Windows.Forms.TextBox txtCountry, txtCity, txtDepartureCity, txtHotel;
        private System.Windows.Forms.NumericUpDown numBudget, numMarketPrice, numAdults, numChildren, numNights;
        private System.Windows.Forms.ComboBox cmbBoard, cmbStatus, cmbAssignedUser, cmbTransport;
        private System.Windows.Forms.TextBox txtDepartureStreet, txtFlightNumber, txtTerminal;
        private System.Windows.Forms.TextBox txtTransferCity, txtTransferFlightNumber, txtTransferTerminal, txtTransferStreet;
        private System.Windows.Forms.ComboBox cmbTransferTransport;
        private System.Windows.Forms.MaskedTextBox txtDepartureTime, txtTransferTime;
        private System.Windows.Forms.DateTimePicker dtpDepartureDate;
        private System.Windows.Forms.DataGridView dgvData;
    }
}