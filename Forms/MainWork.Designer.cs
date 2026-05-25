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
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.btnOpenMap = new System.Windows.Forms.Button();
            this.txtDepartureCity = new System.Windows.Forms.TextBox();
            this.cmbTransport = new System.Windows.Forms.ComboBox();
            this.numBudget = new System.Windows.Forms.NumericUpDown();
            this.numMarketPrice = new System.Windows.Forms.NumericUpDown();
            this.txtHotel = new System.Windows.Forms.TextBox();
            this.numAdults = new System.Windows.Forms.NumericUpDown();
            this.numNights = new System.Windows.Forms.NumericUpDown();
            this.cmbBoard = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.cmbAssignedUser = new System.Windows.Forms.ComboBox();
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

            // ===== SIDEBAR =====
            this.pnlSidebar.BackColor = Color.White;
            this.pnlSidebar.Location = new Point(20, 80);
            this.pnlSidebar.Size = new Size(320, 590); // ЗБІЛЬШЕНО ВИСОТУ ДЛЯ РАМОК
            this.pnlSidebar.BorderStyle = BorderStyle.FixedSingle;

            this.lblSidebarTitle.Text = "Новий тур";
            this.lblSidebarTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.lblSidebarTitle.Location = new Point(20, 10);
            this.lblSidebarTitle.AutoSize = true;

            // Країна + Місто
            AddLabel("Куди (Країна)", 45, 20);
            this.txtCountry.Location = new Point(20, 65);
            this.txtCountry.Size = new Size(130, 30);
            this.txtCountry.Font = new Font("Segoe UI", 10F);

            AddLabel("Місто", 45, 160);
            this.txtCity.Location = new Point(160, 65);
            this.txtCity.Size = new Size(100, 30);
            this.txtCity.Font = new Font("Segoe UI", 10F);

            this.btnOpenMap.Location = new Point(265, 64);
            this.btnOpenMap.Size = new Size(35, 32);
            this.btnOpenMap.Text = "🌍";
            this.btnOpenMap.BackColor = Color.FromArgb(243, 244, 246);
            this.btnOpenMap.FlatStyle = FlatStyle.Flat;
            this.btnOpenMap.FlatAppearance.BorderSize = 0;
            this.btnOpenMap.Click += new EventHandler(this.btnOpenMap_Click);

            // Звідки + Транспорт
            AddLabel("Звідки (Місто)", 100, 20);
            this.txtDepartureCity.Location = new Point(20, 120);
            this.txtDepartureCity.Size = new Size(130, 30);
            this.txtDepartureCity.Text = "Київ";
            this.txtDepartureCity.Font = new Font("Segoe UI", 10F);

            AddLabel("Транспорт", 100, 160);
            this.cmbTransport.Items.AddRange(new object[] { "Літак (Прямий)", "Літак (Пересадка)", "Автобус", "Потяг" });
            this.cmbTransport.Location = new Point(160, 120);
            this.cmbTransport.Size = new Size(140, 30);
            this.cmbTransport.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTransport.SelectedIndex = 0;
            this.cmbTransport.Font = new Font("Segoe UI", 10F);

            // Дорослі + Ночі
            AddLabel("👥 Дорослі", 155, 20);
            this.numAdults.Location = new Point(20, 175);
            this.numAdults.Size = new Size(60, 30);
            this.numAdults.Minimum = 1;
            this.numAdults.Maximum = 100;
            this.numAdults.Value = 2;
            this.numAdults.Font = new Font("Segoe UI", 10F);

            AddLabel("🌙 Ночей", 155, 95);
            this.numNights.Location = new Point(95, 175);
            this.numNights.Size = new Size(60, 30);
            this.numNights.Minimum = 1;
            this.numNights.Maximum = 365;
            this.numNights.Value = 7;
            this.numNights.Font = new Font("Segoe UI", 10F);

            // Харчування
            AddLabel("🍴 Харчування", 155, 170);
            this.cmbBoard.Items.AddRange(new object[] { "RO (Без їжі)", "BB (Сніданок)", "HB (Снід.+Веч.)", "FB (3-разове)", "AI (Все вкл.)" });
            this.cmbBoard.Location = new Point(170, 175);
            this.cmbBoard.Size = new Size(130, 31);
            this.cmbBoard.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbBoard.SelectedIndex = 4;
            this.cmbBoard.Font = new Font("Segoe UI", 10F);

            // Готель
            AddLabel("🏨 Готель (Довідник)", 215, 20);
            this.txtHotel.Location = new Point(20, 235);
            this.txtHotel.Size = new Size(280, 30);
            this.txtHotel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.txtHotel.BackColor = Color.FromArgb(239, 246, 255);
            this.txtHotel.ReadOnly = true;
            this.txtHotel.Text = "Натисніть для вибору готелю...";
            this.txtHotel.ForeColor = Color.Gray;

            // Ринкова ціна
            AddLabel("Ринкова ціна (Авто) $", 275, 20);
            this.numMarketPrice.Location = new Point(20, 295);
            this.numMarketPrice.Size = new Size(280, 30);
            this.numMarketPrice.Minimum = 0;
            this.numMarketPrice.Maximum = 1000000;
            this.numMarketPrice.Value = 0;
            this.numMarketPrice.ReadOnly = true;
            this.numMarketPrice.BackColor = Color.FromArgb(243, 244, 246);
            this.numMarketPrice.Font = new Font("Segoe UI", 10F);

            // НАША ЦІНА
            AddLabel("НАША ЦІНА ($)", 335, 20);
            this.numBudget.Location = new Point(20, 355);
            this.numBudget.Size = new Size(280, 30);
            this.numBudget.Minimum = 0;
            this.numBudget.Maximum = 1000000;
            this.numBudget.Value = 0;
            this.numBudget.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.numBudget.BackColor = Color.FromArgb(220, 252, 231);

            // Статус + Клієнт
            AddLabel("Статус", 395, 20);
            this.cmbStatus.Items.AddRange(new object[] { "Запит", "Планується", "Завершено" });
            this.cmbStatus.Location = new Point(20, 415);
            this.cmbStatus.Size = new Size(130, 31);
            this.cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbStatus.SelectedIndex = 0;
            this.cmbStatus.Font = new Font("Segoe UI", 10F);

            AddLabel("Клієнт", 395, 160);
            this.cmbAssignedUser.Location = new Point(160, 415);
            this.cmbAssignedUser.Size = new Size(140, 31);
            this.cmbAssignedUser.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAssignedUser.Font = new Font("Segoe UI", 10F);

            // Кнопки управління
            this.btnAdd.Location = new Point(20, 480);
            this.btnAdd.Size = new Size(135, 45);
            this.btnAdd.Text = "➕ Створити"; // Перейменовано для базового стану
            this.btnAdd.BackColor = Color.FromArgb(16, 185, 129);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnEdit.Location = new Point(165, 480);
            this.btnEdit.Size = new Size(135, 45);
            this.btnEdit.Text = "Оновити";
            this.btnEdit.BackColor = Color.FromArgb(245, 158, 11);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

            this.btnDelete.Location = new Point(20, 535);
            this.btnDelete.Size = new Size(135, 45);
            this.btnDelete.Text = "Видалити";
            this.btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.btnClear.Location = new Point(165, 535);
            this.btnClear.Size = new Size(135, 45);
            this.btnClear.Text = "Скинути";
            this.btnClear.BackColor = Color.FromArgb(229, 231, 235);
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            // Додаємо контроли на sidebar
            this.pnlSidebar.Controls.Add(this.lblSidebarTitle);
            this.pnlSidebar.Controls.Add(this.btnAdd);
            this.pnlSidebar.Controls.Add(this.btnEdit);
            this.pnlSidebar.Controls.Add(this.btnDelete);
            this.pnlSidebar.Controls.Add(this.btnClear);
            this.pnlSidebar.Controls.Add(this.txtCountry);
            this.pnlSidebar.Controls.Add(this.txtCity);
            this.pnlSidebar.Controls.Add(this.btnOpenMap);
            this.pnlSidebar.Controls.Add(this.txtDepartureCity);
            this.pnlSidebar.Controls.Add(this.cmbTransport);
            this.pnlSidebar.Controls.Add(this.numAdults);
            this.pnlSidebar.Controls.Add(this.numNights);
            this.pnlSidebar.Controls.Add(this.cmbBoard);
            this.pnlSidebar.Controls.Add(this.numBudget);
            this.pnlSidebar.Controls.Add(this.numMarketPrice);
            this.pnlSidebar.Controls.Add(this.txtHotel);
            this.pnlSidebar.Controls.Add(this.cmbStatus);
            this.pnlSidebar.Controls.Add(this.cmbAssignedUser);

            // ===== DATA PANEL =====
            this.pnlData.Location = new Point(360, 80);
            this.pnlData.Size = new Size(700, 590); // ЗБІЛЬШЕНО ВИСОТУ ДЛЯ РАМОК
            this.pnlData.BackColor = Color.White;
            this.pnlData.BorderStyle = BorderStyle.FixedSingle;

            this.dgvData.Location = new Point(20, 70);
            this.dgvData.Size = new Size(660, 500); // Трішки розтягнуто
            this.dgvData.BackgroundColor = Color.White;
            this.dgvData.BorderStyle = BorderStyle.None;

            this.lblDataTitle.Text = "База подорожей";
            this.lblDataTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblDataTitle.Location = new Point(20, 20);
            this.lblDataTitle.AutoSize = true;

            this.pnlData.Controls.Add(this.lblDataTitle);
            this.pnlData.Controls.Add(this.dgvData);

            // ===== ФОРМА =====
            this.ClientSize = new Size(1080, 680);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlData);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(MainWork_Load);

            // ResumeLayout
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMarketPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAdults)).EndInit();
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

        private void AddLabel(string text, int y, int x = 20)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Gray
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
        private Button btnOpenMap;
        private NumericUpDown numBudget, numMarketPrice, numAdults, numNights;
        private ComboBox cmbBoard, cmbStatus, cmbAssignedUser, cmbTransport;
        private Button btnAdd, btnEdit, btnDelete, btnClear;
        private Panel pnlData;
        private DataGridView dgvData;
        private Label lblDataTitle;
        private TextBox txtHotel;
    }
}