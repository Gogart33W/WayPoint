using System;
using System.Drawing;
using System.Windows.Forms;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbBack = new System.Windows.Forms.PictureBox();
            this.pbExit = new System.Windows.Forms.PictureBox();

            this.lblSidebarTitle = new System.Windows.Forms.Label();
            this.tabControlInfo = new System.Windows.Forms.TabControl();
            this.tabRoute = new System.Windows.Forms.TabPage();
            this.tabLogistics = new System.Windows.Forms.TabPage();
            this.tabFinance = new System.Windows.Forms.TabPage();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancelAdd = new System.Windows.Forms.Button(); // НОВА КНОПКА СКАСУВАТИ
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
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).BeginInit();
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
            this.pnlHeader.BackColor = Color.FromArgb(31, 41, 55);
            this.pnlHeader.Controls.Add(this.btnAdminReturn);
            this.pnlHeader.Controls.Add(this.btnOpenFeed);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.pbBack);
            this.pnlHeader.Controls.Add(this.pbExit);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 60;

            this.lblTitle.Text = "WayPoint Business";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblTitle.Location = new Point(60, 15);
            this.lblTitle.AutoSize = true;

            this.btnOpenFeed.BackColor = Color.FromArgb(139, 92, 246);
            this.btnOpenFeed.ForeColor = Color.White;
            this.btnOpenFeed.FlatStyle = FlatStyle.Flat;
            this.btnOpenFeed.Anchor = AnchorStyles.Top | AnchorStyles.Right; // ГУМОВИЙ
            this.btnOpenFeed.Location = new Point(1080, 12); // Відступ для фулскріну
            this.btnOpenFeed.Size = new Size(180, 35);
            this.btnOpenFeed.Text = "🌍 Стрічка";
            this.btnOpenFeed.Click += new EventHandler(this.btnOpenFeed_Click);

            this.btnAdminReturn.BackColor = Color.FromArgb(220, 38, 38);
            this.btnAdminReturn.ForeColor = Color.White;
            this.btnAdminReturn.FlatStyle = FlatStyle.Flat;
            this.btnAdminReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right; // ГУМОВИЙ
            this.btnAdminReturn.Location = new Point(890, 12);
            this.btnAdminReturn.Size = new Size(180, 35);
            this.btnAdminReturn.Text = "🛡️ Адмінка";
            this.btnAdminReturn.Click += new EventHandler(this.btnAdminReturn_Click);

            this.pbBack.Image = global::WayPoint.Properties.Resources.Home3_37171;
            this.pbBack.Location = new Point(15, 12);
            this.pbBack.Size = new Size(35, 35);
            this.pbBack.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbBack.Click += new EventHandler(this.pbBack_Click);

            this.pbExit.Image = global::WayPoint.Properties.Resources.free_icon_window_14062773;
            this.pbExit.Anchor = AnchorStyles.Top | AnchorStyles.Right; // ГУМОВИЙ
            this.pbExit.Location = new Point(1280, 12);
            this.pbExit.Size = new Size(35, 35);
            this.pbExit.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbExit.Click += new EventHandler(this.pbExit_Click);

            // ===== SIDEBAR =====
            this.pnlSidebar.BackColor = Color.White;
            this.pnlSidebar.Location = new Point(20, 80);
            this.pnlSidebar.Size = new Size(340, 600);
            this.pnlSidebar.BorderStyle = BorderStyle.FixedSingle;
            this.pnlSidebar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left; // ГУМОВИЙ

            this.lblSidebarTitle.Text = "Новий тур";
            this.lblSidebarTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.lblSidebarTitle.Location = new Point(15, 10);
            this.lblSidebarTitle.AutoSize = true;

            // --- TAB CONTROL ---
            this.tabControlInfo.Location = new Point(10, 45);
            this.tabControlInfo.Size = new Size(320, 430);
            this.tabControlInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.tabControlInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // ГУМОВИЙ

            this.tabRoute.Text = "🌍 Маршрут";
            this.tabRoute.BackColor = Color.White;
            this.tabLogistics.Text = "✈️ Логістика";
            this.tabLogistics.BackColor = Color.White;
            this.tabFinance.Text = "💰 Фінанси";
            this.tabFinance.BackColor = Color.White;

            this.tabControlInfo.Controls.Add(this.tabRoute);
            this.tabControlInfo.Controls.Add(this.tabLogistics);
            this.tabControlInfo.Controls.Add(this.tabFinance);
            this.pnlSidebar.Controls.Add(this.tabControlInfo);

            // ----------------------------------------------------
            // ВКЛАДКА 1: МАРШРУТ (УЩІЛЬНЕНО)
            // ----------------------------------------------------
            AddLabel("Країна", 10, 10, tabRoute, FontStyle.Regular);
            this.txtCountry.Location = new Point(10, 28);
            this.txtCountry.Size = new Size(130, 25);
            this.txtCountry.Font = new Font("Segoe UI", 10F);
            this.tabRoute.Controls.Add(this.txtCountry);

            AddLabel("Місто", 10, 150, tabRoute, FontStyle.Regular);
            this.txtCity.Location = new Point(150, 28);
            this.txtCity.Size = new Size(110, 25);
            this.txtCity.Font = new Font("Segoe UI", 10F);
            this.tabRoute.Controls.Add(this.txtCity);

            this.btnOpenMap.Location = new Point(265, 27);
            this.btnOpenMap.Size = new Size(35, 27);
            this.btnOpenMap.Text = "🗺";
            this.btnOpenMap.FlatStyle = FlatStyle.Flat;
            this.btnOpenMap.Click += new EventHandler(this.btnOpenMap_Click);
            this.tabRoute.Controls.Add(this.btnOpenMap);

            AddLabel("🏨 Готель (Довідник CRM)", 65, 10, tabRoute, FontStyle.Bold);
            this.txtHotel.Location = new Point(10, 85);
            this.txtHotel.Size = new Size(290, 25);
            this.txtHotel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.txtHotel.ReadOnly = true;
            this.tabRoute.Controls.Add(this.txtHotel);

            AddLabel("Дорослі", 125, 10, tabRoute, FontStyle.Regular);
            this.numAdults.Location = new Point(10, 145);
            this.numAdults.Size = new Size(70, 25);
            this.numAdults.Minimum = 1; this.numAdults.Value = 2;
            this.tabRoute.Controls.Add(this.numAdults);

            AddLabel("Діти", 125, 90, tabRoute, FontStyle.Regular);
            this.numChildren.Location = new Point(90, 145);
            this.numChildren.Size = new Size(70, 25);
            this.tabRoute.Controls.Add(this.numChildren);

            AddLabel("Ночей", 125, 170, tabRoute, FontStyle.Regular);
            this.numNights.Location = new Point(170, 145);
            this.numNights.Size = new Size(70, 25);
            this.numNights.Minimum = 1; this.numNights.Value = 7;
            this.tabRoute.Controls.Add(this.numNights);

            AddLabel("🍴 Харчування", 185, 10, tabRoute, FontStyle.Regular);
            this.cmbBoard.Items.AddRange(new object[] { "RO (Без їжі)", "BB (Сніданок)", "HB (Снід.+Веч.)", "FB (3-разове)", "AI (Все вкл.)" });
            this.cmbBoard.Location = new Point(10, 205);
            this.cmbBoard.Size = new Size(290, 25);
            this.cmbBoard.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabRoute.Controls.Add(this.cmbBoard);

            // ----------------------------------------------------
            // ВКЛАДКА 2: ЛОГІСТИКА (УЩІЛЬНЕНО)
            // ----------------------------------------------------
            AddLabel("— ОСНОВНИЙ РЕЙС / ВІДПРАВЛЕННЯ —", 5, 10, tabLogistics, FontStyle.Bold);

            AddLabel("Транспорт", 25, 10, tabLogistics, FontStyle.Regular);
            this.cmbTransport.Items.AddRange(new object[] { "Літак (Прямий)", "Літак (Пересадка)", "Автобус", "Потяг" });
            this.cmbTransport.Location = new Point(10, 45);
            this.cmbTransport.Size = new Size(290, 25);
            this.cmbTransport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabLogistics.Controls.Add(this.cmbTransport);

            AddLabel("Звідки (Місто)", 75, 10, tabLogistics, FontStyle.Regular);
            this.txtDepartureCity.Location = new Point(10, 95);
            this.txtDepartureCity.Size = new Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtDepartureCity);

            AddLabel("Вулиця / Локація", 75, 150, tabLogistics, FontStyle.Regular);
            this.txtDepartureStreet.Location = new Point(150, 95);
            this.txtDepartureStreet.Size = new Size(150, 25);
            this.tabLogistics.Controls.Add(this.txtDepartureStreet);

            AddLabel("Дата виїзду", 125, 10, tabLogistics, FontStyle.Regular);
            this.dtpDepartureDate.Location = new Point(10, 145);
            this.dtpDepartureDate.Size = new Size(130, 25);
            this.dtpDepartureDate.Format = DateTimePickerFormat.Short;
            this.tabLogistics.Controls.Add(this.dtpDepartureDate);

            AddLabel("Час (00:00)", 125, 150, tabLogistics, FontStyle.Regular);
            this.txtDepartureTime.Mask = "00:00";
            this.txtDepartureTime.ValidatingType = typeof(System.DateTime);
            this.txtDepartureTime.Location = new Point(150, 145);
            this.txtDepartureTime.Size = new Size(150, 25);
            this.txtDepartureTime.Font = new Font("Segoe UI", 10F);
            this.tabLogistics.Controls.Add(this.txtDepartureTime);

            AddLabel("Рейс (Або Автобус №)", 175, 10, tabLogistics, FontStyle.Regular);
            this.txtFlightNumber.Location = new Point(10, 195);
            this.txtFlightNumber.Size = new Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtFlightNumber);

            AddLabel("Термінал / Вокзал", 175, 150, tabLogistics, FontStyle.Regular);
            this.txtTerminal.Location = new Point(150, 195);
            this.txtTerminal.Size = new Size(150, 25);
            this.tabLogistics.Controls.Add(this.txtTerminal);

            AddLabel("— ІНФОРМАЦІЯ ПРО ПЕРЕСАДКУ —", 230, 10, tabLogistics, FontStyle.Bold);

            AddLabel("Місто пересадки", 250, 10, tabLogistics, FontStyle.Regular);
            this.txtTransferCity.Location = new Point(10, 270);
            this.txtTransferCity.Size = new Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtTransferCity);

            AddLabel("Транспорт", 250, 150, tabLogistics, FontStyle.Regular);
            this.cmbTransferTransport.Items.AddRange(new object[] { "Літак", "Автобус", "Потяг" });
            this.cmbTransferTransport.Location = new Point(150, 270);
            this.cmbTransferTransport.Size = new Size(150, 25);
            this.cmbTransferTransport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabLogistics.Controls.Add(this.cmbTransferTransport);

            AddLabel("Рейс пересадки", 300, 10, tabLogistics, FontStyle.Regular);
            this.txtTransferFlightNumber.Location = new Point(10, 320);
            this.txtTransferFlightNumber.Size = new Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtTransferFlightNumber);

            AddLabel("Час виїзду", 300, 150, tabLogistics, FontStyle.Regular);
            this.txtTransferTime.Mask = "00:00";
            this.txtTransferTime.ValidatingType = typeof(System.DateTime);
            this.txtTransferTime.Location = new Point(150, 320);
            this.txtTransferTime.Size = new Size(150, 25);
            this.txtTransferTime.Font = new Font("Segoe UI", 10F);
            this.tabLogistics.Controls.Add(this.txtTransferTime);

            AddLabel("Термінал / Локація", 350, 10, tabLogistics, FontStyle.Regular);
            this.txtTransferTerminal.Location = new Point(10, 370);
            this.txtTransferTerminal.Size = new Size(130, 25);
            this.tabLogistics.Controls.Add(this.txtTransferTerminal);

            AddLabel("Вулиця (якщо є)", 350, 150, tabLogistics, FontStyle.Regular);
            this.txtTransferStreet.Location = new Point(150, 370);
            this.txtTransferStreet.Size = new Size(150, 25);
            this.tabLogistics.Controls.Add(this.txtTransferStreet);

            // ----------------------------------------------------
            // ВКЛАДКА 3: ФІНАНСИ
            // ----------------------------------------------------
            AddLabel("Клієнт (Пошук: почніть вводити ім'я)", 10, 10, tabFinance, FontStyle.Bold);
            this.cmbAssignedUser.Location = new Point(10, 30);
            this.cmbAssignedUser.Size = new Size(290, 25);
            this.cmbAssignedUser.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cmbAssignedUser.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.tabFinance.Controls.Add(this.cmbAssignedUser);

            AddLabel("Статус туру", 70, 10, tabFinance, FontStyle.Regular);
            this.cmbStatus.Items.AddRange(new object[] { "Запит", "Планується", "Оплачено", "Завершено" });
            this.cmbStatus.Location = new Point(10, 90);
            this.cmbStatus.Size = new Size(290, 25);
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabFinance.Controls.Add(this.cmbStatus);

            AddLabel("Ринкова ціна (Авторозрахунок)", 130, 10, tabFinance, FontStyle.Regular);
            this.numMarketPrice.Location = new Point(10, 150);
            this.numMarketPrice.Size = new Size(290, 25);
            this.numMarketPrice.ReadOnly = true;
            this.numMarketPrice.BackColor = Color.FromArgb(243, 244, 246);
            this.tabFinance.Controls.Add(this.numMarketPrice);

            AddLabel("НАША ЦІНА БЮДЖЕТ ($)", 190, 10, tabFinance, FontStyle.Bold);
            this.numBudget.Location = new Point(10, 210);
            this.numBudget.Size = new Size(290, 30);
            this.numBudget.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.numBudget.BackColor = Color.FromArgb(220, 252, 231);
            this.tabFinance.Controls.Add(this.numBudget);

            // ----------------------------------------------------
            // КНОПКИ УПРАВЛІННЯ (Внизу, завжди видимі)
            // ----------------------------------------------------
            this.btnAdd.Location = new Point(10, 485);
            this.btnAdd.Size = new Size(155, 45);
            this.btnAdd.Text = "➕ Створити";
            this.btnAdd.BackColor = Color.FromArgb(16, 185, 129);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.pnlSidebar.Controls.Add(this.btnAdd);

            this.btnCancelAdd.Location = new Point(175, 485);
            this.btnCancelAdd.Size = new Size(145, 45);
            this.btnCancelAdd.Text = "Скасувати";
            this.btnCancelAdd.BackColor = Color.FromArgb(107, 114, 128); // Сірий
            this.btnCancelAdd.ForeColor = Color.White;
            this.btnCancelAdd.FlatStyle = FlatStyle.Flat;
            this.btnCancelAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnCancelAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnCancelAdd.Visible = false; // Прихована по замовчуванню
            this.btnCancelAdd.Click += new EventHandler(this.btnCancelAdd_Click);
            this.pnlSidebar.Controls.Add(this.btnCancelAdd);

            this.btnEdit.Location = new Point(175, 485);
            this.btnEdit.Size = new Size(145, 45);
            this.btnEdit.Text = "Оновити";
            this.btnEdit.BackColor = Color.FromArgb(245, 158, 11);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.pnlSidebar.Controls.Add(this.btnEdit);

            this.btnDelete.Location = new Point(10, 540);
            this.btnDelete.Size = new Size(155, 45);
            this.btnDelete.Text = "Видалити";
            this.btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.pnlSidebar.Controls.Add(this.btnDelete);

            this.btnClear.Location = new Point(175, 540);
            this.btnClear.Size = new Size(145, 45);
            this.btnClear.Text = "Скинути";
            this.btnClear.BackColor = Color.FromArgb(229, 231, 235);
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.pnlSidebar.Controls.Add(this.btnClear);

            // ===== DATA PANEL =====
            this.pnlData.Location = new Point(380, 80);
            this.pnlData.Size = new Size(950, 600); // Початковий розмір для фулскріну
            this.pnlData.BackColor = Color.White;
            this.pnlData.BorderStyle = BorderStyle.FixedSingle;
            this.pnlData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // ГУМОВИЙ

            this.dgvData.Location = new Point(20, 70);
            this.dgvData.Size = new Size(910, 510);
            this.dgvData.BackgroundColor = Color.White;
            this.dgvData.BorderStyle = BorderStyle.None;
            this.dgvData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // ГУМОВИЙ

            this.lblDataTitle.Text = "База подорожей";
            this.lblDataTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblDataTitle.Location = new Point(20, 20);
            this.lblDataTitle.AutoSize = true;

            this.pnlData.Controls.Add(this.lblDataTitle);
            this.pnlData.Controls.Add(this.dgvData);

            // ===== ФОРМА =====
            this.ClientSize = new Size(1350, 700); // Початковий розмір
            this.WindowState = FormWindowState.Maximized; // ПОВНИЙ ЕКРАН
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlData);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(MainWork_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).EndInit();
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

        private void AddLabel(string text, int y, int x, Control parent, FontStyle style)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, style),
                ForeColor = style == FontStyle.Bold ? Color.Black : Color.Gray
            };
            parent.Controls.Add(lbl);
        }

        private Panel pnlHeader, pnlSidebar, pnlData;
        private Label lblTitle, lblSidebarTitle, lblDataTitle;
        private PictureBox pbBack, pbExit;
        private Button btnOpenFeed, btnAdminReturn, btnOpenMap, btnAdd, btnCancelAdd, btnEdit, btnDelete, btnClear;
        private TabControl tabControlInfo;
        private TabPage tabRoute, tabLogistics, tabFinance;
        private TextBox txtCountry, txtCity, txtDepartureCity, txtHotel;
        private NumericUpDown numBudget, numMarketPrice, numAdults, numChildren, numNights;
        private ComboBox cmbBoard, cmbStatus, cmbAssignedUser, cmbTransport;
        private TextBox txtDepartureStreet, txtFlightNumber, txtTerminal;
        private TextBox txtTransferCity, txtTransferFlightNumber, txtTransferTerminal, txtTransferStreet;
        private ComboBox cmbTransferTransport;
        private MaskedTextBox txtDepartureTime, txtTransferTime;
        private DateTimePicker dtpDepartureDate;
        private DataGridView dgvData;
    }
}