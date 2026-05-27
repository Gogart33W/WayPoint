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
            this.btnAdminReturn = new System.Windows.Forms.Button();
            this.btnOpenFeed = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbBack = new System.Windows.Forms.PictureBox();
            this.pbExit = new System.Windows.Forms.PictureBox();

            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblSidebarTitle = new System.Windows.Forms.Label();

            // Напрямок
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.btnOpenMap = new System.Windows.Forms.Button();

            // Транспорт і відправлення
            this.cmbTransport = new System.Windows.Forms.ComboBox();
            this.txtDepartureCity = new System.Windows.Forms.TextBox();
            this.txtDepartureStreet = new System.Windows.Forms.TextBox();
            this.dtpDepartureDate = new System.Windows.Forms.DateTimePicker();
            this.txtDepartureTime = new System.Windows.Forms.TextBox();
            this.txtFlightNumber = new System.Windows.Forms.TextBox();
            this.txtTerminal = new System.Windows.Forms.TextBox();
            this.pnlTransfer = new System.Windows.Forms.Panel();
            this.txtTransferCity = new System.Windows.Forms.TextBox();

            // Пасажири
            this.numAdults = new System.Windows.Forms.NumericUpDown();
            this.numChildren = new System.Windows.Forms.NumericUpDown();
            this.numNights = new System.Windows.Forms.NumericUpDown();
            this.cmbBoard = new System.Windows.Forms.ComboBox();

            // Готель і ціни
            this.txtHotel = new System.Windows.Forms.TextBox();
            this.numMarketPrice = new System.Windows.Forms.NumericUpDown();
            this.numBudget = new System.Windows.Forms.NumericUpDown();

            // Клієнт і статус
            this.txtClientSearch = new System.Windows.Forms.TextBox();
            this.lstClientSearch = new System.Windows.Forms.ListBox();
            this.cmbAssignedUser = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();

            // Кнопки
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();

            this.pnlData = new System.Windows.Forms.Panel();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.lblDataTitle = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).BeginInit();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarketPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAdults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChildren)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNights)).BeginInit();
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
            this.btnOpenFeed.Location = new Point(820, 12);
            this.btnOpenFeed.Size = new Size(180, 35);
            this.btnOpenFeed.Text = "🌍 Стрічка";
            this.btnOpenFeed.Click += new EventHandler(this.btnOpenFeed_Click);

            this.btnAdminReturn.BackColor = Color.FromArgb(220, 38, 38);
            this.btnAdminReturn.ForeColor = Color.White;
            this.btnAdminReturn.FlatStyle = FlatStyle.Flat;
            this.btnAdminReturn.Location = new Point(630, 12);
            this.btnAdminReturn.Size = new Size(180, 35);
            this.btnAdminReturn.Text = "🛡️ Адмінка";
            this.btnAdminReturn.Click += new EventHandler(this.btnAdminReturn_Click);

            this.pbBack.Image = global::WayPoint.Properties.Resources.Home3_37171;
            this.pbBack.Location = new Point(15, 12);
            this.pbBack.Size = new Size(35, 35);
            this.pbBack.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbBack.Click += new EventHandler(this.pbBack_Click);

            this.pbExit.Image = global::WayPoint.Properties.Resources.free_icon_window_14062773;
            this.pbExit.Location = new Point(1030, 12);
            this.pbExit.Size = new Size(35, 35);
            this.pbExit.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbExit.Click += new EventHandler(this.pbExit_Click);

            // ===== SIDEBAR — скролюється =====
            this.pnlSidebar.BackColor = Color.White;
            this.pnlSidebar.Location = new Point(20, 80);
            this.pnlSidebar.Size = new Size(340, 595);
            this.pnlSidebar.BorderStyle = BorderStyle.FixedSingle;
            this.pnlSidebar.AutoScroll = true;

            this.lblSidebarTitle.Text = "Деталі туру";
            this.lblSidebarTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.lblSidebarTitle.Location = new Point(10, 8);
            this.lblSidebarTitle.AutoSize = true;
            this.pnlSidebar.Controls.Add(this.lblSidebarTitle);

            int y = 35; // поточна Y-позиція

            // ── СЕКЦІЯ: НАПРЯМОК ──
            AddSectionLabel("📍 Напрямок", ref y);

            AddFieldLabel("Країна *", y);
            this.txtCountry.Location = new Point(10, y + 18);
            this.txtCountry.Size = new Size(145, 26);
            this.txtCountry.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.txtCountry);

            AddFieldLabel("Місто *", y, 165);
            this.txtCity.Location = new Point(165, y + 18);
            this.txtCity.Size = new Size(115, 26);
            this.txtCity.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.txtCity);

            this.btnOpenMap.Location = new Point(285, y + 17);
            this.btnOpenMap.Size = new Size(35, 28);
            this.btnOpenMap.Text = "🌍";
            this.btnOpenMap.BackColor = Color.FromArgb(243, 244, 246);
            this.btnOpenMap.FlatStyle = FlatStyle.Flat;
            this.btnOpenMap.FlatAppearance.BorderSize = 0;
            this.btnOpenMap.Click += new EventHandler(this.btnOpenMap_Click);
            this.pnlSidebar.Controls.Add(this.btnOpenMap);

            y += 52;

            // ── СЕКЦІЯ: ТРАНСПОРТ ──
            AddSectionLabel("🚀 Транспорт і відправлення", ref y);

            AddFieldLabel("Тип транспорту *", y);
            this.cmbTransport.Items.AddRange(new object[] {
                "Літак (Прямий)", "Літак (Пересадка)", "Автобус", "Потяг", "Корабель", "Автомобіль"
            });
            this.cmbTransport.Location = new Point(10, y + 18);
            this.cmbTransport.Size = new Size(305, 26);
            this.cmbTransport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTransport.SelectedIndex = 0;
            this.cmbTransport.Font = new Font("Segoe UI", 10F);
            this.cmbTransport.SelectedIndexChanged += new EventHandler(this.cmbTransport_SelectedIndexChanged);
            this.pnlSidebar.Controls.Add(this.cmbTransport);
            y += 52;

            AddFieldLabel("Місто відправлення *", y);
            this.txtDepartureCity.Location = new Point(10, y + 18);
            this.txtDepartureCity.Size = new Size(305, 26);
            this.txtDepartureCity.Text = "Київ";
            this.txtDepartureCity.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.txtDepartureCity);
            y += 52;

            AddFieldLabel("Вулиця / вокзал / аеропорт", y);
            this.txtDepartureStreet.Location = new Point(10, y + 18);
            this.txtDepartureStreet.Size = new Size(305, 26);
            this.txtDepartureStreet.Font = new Font("Segoe UI", 10F);
            this.txtDepartureStreet.PlaceholderText = "напр. Бориспіль, Термінал D";
            this.pnlSidebar.Controls.Add(this.txtDepartureStreet);
            y += 52;

            AddFieldLabel("Дата відправлення", y);
            this.dtpDepartureDate.Location = new Point(10, y + 18);
            this.dtpDepartureDate.Size = new Size(185, 26);
            this.dtpDepartureDate.Font = new Font("Segoe UI", 10F);
            this.dtpDepartureDate.Format = DateTimePickerFormat.Short;
            this.dtpDepartureDate.Value = DateTime.Today;
            this.pnlSidebar.Controls.Add(this.dtpDepartureDate);

            AddFieldLabel("Час (ГГ:ХХ)", y, 205);
            this.txtDepartureTime.Location = new Point(205, y + 18);
            this.txtDepartureTime.Size = new Size(110, 26);
            this.txtDepartureTime.Font = new Font("Segoe UI", 10F);
            this.txtDepartureTime.PlaceholderText = "08:30";
            this.txtDepartureTime.MaxLength = 5;
            this.pnlSidebar.Controls.Add(this.txtDepartureTime);
            y += 52;

            AddFieldLabel("№ рейсу / потяга", y);
            this.txtFlightNumber.Location = new Point(10, y + 18);
            this.txtFlightNumber.Size = new Size(145, 26);
            this.txtFlightNumber.Font = new Font("Segoe UI", 10F);
            this.txtFlightNumber.PlaceholderText = "PS 201";
            this.pnlSidebar.Controls.Add(this.txtFlightNumber);

            AddFieldLabel("Термінал / вагон", y, 165);
            this.txtTerminal.Location = new Point(165, y + 18);
            this.txtTerminal.Size = new Size(150, 26);
            this.txtTerminal.Font = new Font("Segoe UI", 10F);
            this.txtTerminal.PlaceholderText = "Термінал B";
            this.pnlSidebar.Controls.Add(this.txtTerminal);
            y += 52;

            // Панель пересадки (видима тільки для "Літак (Пересадка)")
            this.pnlTransfer.Location = new Point(10, y);
            this.pnlTransfer.Size = new Size(305, 50);
            this.pnlTransfer.BackColor = Color.FromArgb(255, 251, 235);
            this.pnlTransfer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlTransfer.Visible = false;

            var lblTransfer = new Label
            {
                Text = "✈ Місто пересадки",
                Location = new Point(5, 3),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(146, 64, 14)
            };
            this.pnlTransfer.Controls.Add(lblTransfer);

            this.txtTransferCity.Location = new Point(5, 20);
            this.txtTransferCity.Size = new Size(290, 24);
            this.txtTransferCity.Font = new Font("Segoe UI", 10F);
            this.txtTransferCity.PlaceholderText = "напр. Варшава (WAW)";
            this.pnlTransfer.Controls.Add(this.txtTransferCity);

            this.pnlSidebar.Controls.Add(this.pnlTransfer);
            y += 58;

            // ── СЕКЦІЯ: ПАСАЖИРИ ──
            AddSectionLabel("👥 Пасажири", ref y);

            AddFieldLabel("Дорослі *", y);
            this.numAdults.Location = new Point(10, y + 18);
            this.numAdults.Size = new Size(80, 26);
            this.numAdults.Minimum = 1;
            this.numAdults.Maximum = 100;
            this.numAdults.Value = 2;
            this.numAdults.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.numAdults);

            AddFieldLabel("Діти (до 12 р.)", y, 100);
            this.numChildren.Location = new Point(100, y + 18);
            this.numChildren.Size = new Size(80, 26);
            this.numChildren.Minimum = 0;
            this.numChildren.Maximum = 20;
            this.numChildren.Value = 0;
            this.numChildren.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.numChildren);

            AddFieldLabel("Ночей *", y, 195);
            this.numNights.Location = new Point(195, y + 18);
            this.numNights.Size = new Size(75, 26);
            this.numNights.Minimum = 1;
            this.numNights.Maximum = 365;
            this.numNights.Value = 7;
            this.numNights.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.numNights);

            AddFieldLabel("🍴 Харчування", y, 280);
            this.cmbBoard.Items.AddRange(new object[] {
                "RO (Без їжі)", "BB (Сніданок)", "HB (Снід.+Веч.)", "FB (3-разове)", "AI (Все вкл.)"
            });
            // Харчування на новому рядку — ширше
            y += 52;
            AddFieldLabel("🍴 Харчування", y);
            this.cmbBoard.Location = new Point(10, y + 18);
            this.cmbBoard.Size = new Size(305, 26);
            this.cmbBoard.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbBoard.SelectedIndex = 4;
            this.cmbBoard.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.cmbBoard);
            y += 52;

            // ── СЕКЦІЯ: ГОТЕЛЬ І ЦІНА ──
            AddSectionLabel("🏨 Готель і вартість", ref y);

            AddFieldLabel("Готель (клікніть для вибору) *", y);
            this.txtHotel.Location = new Point(10, y + 18);
            this.txtHotel.Size = new Size(305, 26);
            this.txtHotel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.txtHotel.BackColor = Color.FromArgb(239, 246, 255);
            this.txtHotel.ReadOnly = true;
            this.txtHotel.Text = "Натисніть для вибору готелю...";
            this.txtHotel.ForeColor = Color.Gray;
            this.pnlSidebar.Controls.Add(this.txtHotel);
            y += 52;

            AddFieldLabel("Ринкова ціна (авто) $", y);
            this.numMarketPrice.Location = new Point(10, y + 18);
            this.numMarketPrice.Size = new Size(305, 26);
            this.numMarketPrice.Minimum = 0;
            this.numMarketPrice.Maximum = 1000000;
            this.numMarketPrice.Value = 0;
            this.numMarketPrice.ReadOnly = true;
            this.numMarketPrice.BackColor = Color.FromArgb(243, 244, 246);
            this.numMarketPrice.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.numMarketPrice);
            y += 52;

            AddFieldLabel("НАША ЦІНА ($) *", y);
            this.numBudget.Location = new Point(10, y + 18);
            this.numBudget.Size = new Size(305, 30);
            this.numBudget.Minimum = 0;
            this.numBudget.Maximum = 1000000;
            this.numBudget.Value = 0;
            this.numBudget.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.numBudget.BackColor = Color.FromArgb(220, 252, 231);
            this.pnlSidebar.Controls.Add(this.numBudget);
            y += 58;

            // ── СЕКЦІЯ: КЛІЄНТ ──
            AddSectionLabel("👤 Клієнт і статус", ref y);

            AddFieldLabel("Пошук клієнта", y);
            this.txtClientSearch.Location = new Point(10, y + 18);
            this.txtClientSearch.Size = new Size(305, 26);
            this.txtClientSearch.Font = new Font("Segoe UI", 10F);
            this.txtClientSearch.PlaceholderText = "Введіть ім'я для пошуку...";
            this.pnlSidebar.Controls.Add(this.txtClientSearch);
            y += 48;

            // Прихований listbox для результатів пошуку
            this.lstClientSearch.Location = new Point(10, y);
            this.lstClientSearch.Size = new Size(305, 80);
            this.lstClientSearch.Font = new Font("Segoe UI", 10F);
            this.lstClientSearch.Visible = false;
            this.lstClientSearch.BorderStyle = BorderStyle.FixedSingle;
            this.lstClientSearch.BackColor = Color.White;
            this.pnlSidebar.Controls.Add(this.lstClientSearch);
            // Зарезервуємо місце — lstClientSearch показується динамічно

            AddFieldLabel("Обраний клієнт *", y);
            this.cmbAssignedUser.Location = new Point(10, y + 18);
            this.cmbAssignedUser.Size = new Size(305, 26);
            this.cmbAssignedUser.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAssignedUser.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.cmbAssignedUser);
            y += 52;

            AddFieldLabel("Статус туру", y);
            this.cmbStatus.Items.AddRange(new object[] { "Запит", "Планується", "Завершено" });
            this.cmbStatus.Location = new Point(10, y + 18);
            this.cmbStatus.Size = new Size(305, 26);
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.SelectedIndex = 0;
            this.cmbStatus.Font = new Font("Segoe UI", 10F);
            this.pnlSidebar.Controls.Add(this.cmbStatus);
            y += 58;

            // ── КНОПКИ ──
            this.btnAdd.Location = new Point(10, y);
            this.btnAdd.Size = new Size(145, 44);
            this.btnAdd.Text = "➕ Створити";
            this.btnAdd.BackColor = Color.FromArgb(16, 185, 129);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.pnlSidebar.Controls.Add(this.btnAdd);

            this.btnEdit.Location = new Point(165, y);
            this.btnEdit.Size = new Size(150, 44);
            this.btnEdit.Text = "✏️ Оновити";
            this.btnEdit.BackColor = Color.FromArgb(245, 158, 11);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.pnlSidebar.Controls.Add(this.btnEdit);
            y += 52;

            this.btnDelete.Location = new Point(10, y);
            this.btnDelete.Size = new Size(145, 44);
            this.btnDelete.Text = "🗑 Видалити";
            this.btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.pnlSidebar.Controls.Add(this.btnDelete);

            this.btnClear.Location = new Point(165, y);
            this.btnClear.Size = new Size(150, 44);
            this.btnClear.Text = "Скинути";
            this.btnClear.BackColor = Color.FromArgb(229, 231, 235);
            this.btnClear.ForeColor = Color.Black;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.pnlSidebar.Controls.Add(this.btnClear);

            // ===== DATA PANEL =====
            this.pnlData.Location = new Point(380, 80);
            this.pnlData.Size = new Size(680, 595);
            this.pnlData.BackColor = Color.White;
            this.pnlData.BorderStyle = BorderStyle.FixedSingle;

            this.dgvData.Location = new Point(15, 60);
            this.dgvData.Size = new Size(650, 520);
            this.dgvData.BackgroundColor = Color.White;
            this.dgvData.BorderStyle = BorderStyle.None;

            this.lblDataTitle.Text = "База подорожей";
            this.lblDataTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblDataTitle.Location = new Point(15, 15);
            this.lblDataTitle.AutoSize = true;

            this.pnlData.Controls.Add(this.lblDataTitle);
            this.pnlData.Controls.Add(this.dgvData);

            this.ClientSize = new Size(1080, 695);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlData);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(MainWork_Load);

            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarketPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAdults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numChildren)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlData.ResumeLayout(false);
            this.pnlData.PerformLayout();
            this.ResumeLayout(false);
        }

        private void AddSectionLabel(string text, ref int y)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(10, y),
                AutoSize = false,
                Size = new Size(305, 22),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(55, 65, 81),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 0, 0)
            };
            this.pnlSidebar.Controls.Add(lbl);
            y += 26;
        }

        private void AddFieldLabel(string text, int y, int x = 10)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(107, 114, 128)
            };
            this.pnlSidebar.Controls.Add(lbl);
        }

        private Panel pnlHeader;
        private Label lblTitle;
        private PictureBox pbBack, pbExit;
        private Button btnOpenFeed, btnAdminReturn;
        private Panel pnlSidebar;
        private Label lblSidebarTitle;
        private TextBox txtCountry, txtCity, txtDepartureCity;
        private TextBox txtDepartureStreet, txtDepartureTime, txtFlightNumber, txtTerminal;
        private TextBox txtTransferCity;
        private DateTimePicker dtpDepartureDate;
        private Panel pnlTransfer;
        private Button btnOpenMap;
        private NumericUpDown numBudget, numMarketPrice, numAdults, numChildren, numNights;
        private ComboBox cmbBoard, cmbStatus, cmbAssignedUser, cmbTransport;
        private TextBox txtClientSearch, txtHotel;
        private ListBox lstClientSearch;
        private Button btnAdd, btnEdit, btnDelete, btnClear;
        private Panel pnlData;
        private DataGridView dgvData;
        private Label lblDataTitle;
    }
}