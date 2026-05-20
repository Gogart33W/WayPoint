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
            this.txtAgency = new System.Windows.Forms.TextBox();
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

            // HEADER
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
            this.pbBack.Click += (s, e) => this.Close();

            this.pbExit.Image = global::WayPoint.Properties.Resources.free_icon_window_14062773;
            this.pbExit.Location = new Point(1030, 12);
            this.pbExit.Size = new Size(35, 35);
            this.pbExit.SizeMode = PictureBoxSizeMode.Zoom;
            this.pbExit.Click += (s, e) => Application.Exit();

            // SIDEBAR
            this.pnlSidebar.BackColor = Color.White;
            this.pnlSidebar.Location = new Point(20, 80);
            this.pnlSidebar.Size = new Size(320, 580);
            this.pnlSidebar.BorderStyle = BorderStyle.FixedSingle;

            this.lblSidebarTitle.Text = "Деталі туру";
            this.lblSidebarTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.lblSidebarTitle.Location = new Point(20, 10);
            this.lblSidebarTitle.AutoSize = true;

            AddLabel("Куди (Країна)", 45, 20);
            txtCountry.Location = new Point(20, 65); txtCountry.Size = new Size(130, 30); txtCountry.Font = new Font("Segoe UI", 10F);

            AddLabel("Місто", 45, 160);
            txtCity.Location = new Point(160, 65); txtCity.Size = new Size(100, 30); txtCity.Font = new Font("Segoe UI", 10F);

            btnOpenMap.Location = new Point(265, 64); btnOpenMap.Size = new Size(35, 32); btnOpenMap.Text = "🌍";
            btnOpenMap.BackColor = Color.FromArgb(243, 244, 246); btnOpenMap.FlatStyle = FlatStyle.Flat; btnOpenMap.FlatAppearance.BorderSize = 0;
            btnOpenMap.Click += new EventHandler(this.btnOpenMap_Click);

            AddLabel("Звідки (Місто)", 100, 20);
            txtDepartureCity.Location = new Point(20, 120); txtDepartureCity.Size = new Size(130, 30); txtDepartureCity.Text = "Київ"; txtDepartureCity.Font = new Font("Segoe UI", 10F);

            AddLabel("Транспорт", 100, 160);
            cmbTransport.Items.AddRange(new object[] { "Літак (Прямий)", "Літак (Пересадка)", "Автобус", "Потяг" });
            cmbTransport.Location = new Point(160, 120); cmbTransport.Size = new Size(140, 30); cmbTransport.DropDownStyle = ComboBoxStyle.DropDownList; cmbTransport.SelectedIndex = 0; cmbTransport.Font = new Font("Segoe UI", 10F);

            AddLabel("👥 Дорослі", 155, 20);
            numAdults.Location = new Point(20, 175); numAdults.Size = new Size(60, 30); numAdults.Value = 2; numAdults.Font = new Font("Segoe UI", 10F);

            AddLabel("🌙 Ночей", 155, 95);
            numNights.Location = new Point(95, 175); numNights.Size = new Size(60, 30); numNights.Value = 7; numNights.Font = new Font("Segoe UI", 10F);

            AddLabel("🍴 Харчування", 155, 170);
            cmbBoard.Items.AddRange(new object[] { "RO (Без їжі)", "BB (Сніданок)", "HB (Снід.+Веч.)", "FB (3-разове)", "AI (Все вкл.)" });
            cmbBoard.Location = new Point(170, 175); cmbBoard.Size = new Size(130, 31); cmbBoard.DropDownStyle = ComboBoxStyle.DropDownList; cmbBoard.SelectedIndex = 4; cmbBoard.Font = new Font("Segoe UI", 10F);

            AddLabel("Ринкова ціна ($)", 215, 20);
            numMarketPrice.Location = new Point(20, 235); numMarketPrice.Size = new Size(130, 30); numMarketPrice.Maximum = 100000; numMarketPrice.ReadOnly = true; numMarketPrice.BackColor = Color.FromArgb(243, 244, 246); numMarketPrice.Font = new Font("Segoe UI", 10F);

            AddLabel("Конкурент (Авто)", 215, 160);
            txtAgency.Location = new Point(160, 235); txtAgency.Size = new Size(140, 30); txtAgency.ReadOnly = true; txtAgency.BackColor = Color.FromArgb(243, 244, 246); txtAgency.Font = new Font("Segoe UI", 10F);

            AddLabel("НАША ЦІНА ($)", 275, 20);
            numBudget.Location = new Point(20, 295); numBudget.Size = new Size(280, 30); numBudget.Maximum = 100000; numBudget.Font = new Font("Segoe UI", 12, FontStyle.Bold); numBudget.BackColor = Color.FromArgb(220, 252, 231);

            AddLabel("Статус", 335, 20);
            cmbStatus.Items.AddRange(new object[] { "Запит", "Планується", "Завершено" });
            cmbStatus.Location = new Point(20, 355); cmbStatus.Size = new Size(130, 31); cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList; cmbStatus.SelectedIndex = 0; cmbStatus.Font = new Font("Segoe UI", 10F);

            AddLabel("Клієнт", 335, 160);
            cmbAssignedUser.Location = new Point(160, 355); cmbAssignedUser.Size = new Size(140, 31); cmbAssignedUser.DropDownStyle = ComboBoxStyle.DropDownList; cmbAssignedUser.Font = new Font("Segoe UI", 10F);

            this.btnAdd.Location = new Point(20, 420); btnAdd.Size = new Size(135, 45); btnAdd.Text = "Додати"; btnAdd.BackColor = Color.FromArgb(16, 185, 129); btnAdd.ForeColor = Color.White; btnAdd.FlatStyle = FlatStyle.Flat; btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.Click += new EventHandler(this.btnAdd_Click);

            this.btnEdit.Location = new Point(165, 420); btnEdit.Size = new Size(135, 45); btnEdit.Text = "Оновити"; btnEdit.BackColor = Color.FromArgb(245, 158, 11); btnEdit.ForeColor = Color.White; btnEdit.FlatStyle = FlatStyle.Flat; btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnEdit.Click += new EventHandler(this.btnEdit_Click);

            this.btnDelete.Location = new Point(20, 475); btnDelete.Size = new Size(135, 45); btnDelete.Text = "Видалити"; btnDelete.BackColor = Color.FromArgb(239, 68, 68); btnDelete.ForeColor = Color.White; btnDelete.FlatStyle = FlatStyle.Flat; btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDelete.Click += new EventHandler(this.btnDelete_Click);

            this.btnClear.Location = new Point(165, 475); btnClear.Size = new Size(135, 45); btnClear.Text = "Очистити"; btnClear.BackColor = Color.FromArgb(229, 231, 235); btnClear.FlatStyle = FlatStyle.Flat; btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClear.Click += new EventHandler(this.btnClear_Click);

            this.pnlSidebar.Controls.Add(btnAdd); this.pnlSidebar.Controls.Add(btnEdit); this.pnlSidebar.Controls.Add(btnDelete); this.pnlSidebar.Controls.Add(btnClear);
            this.pnlSidebar.Controls.Add(txtCountry); this.pnlSidebar.Controls.Add(txtCity); this.pnlSidebar.Controls.Add(btnOpenMap);
            this.pnlSidebar.Controls.Add(txtDepartureCity); this.pnlSidebar.Controls.Add(cmbTransport);
            this.pnlSidebar.Controls.Add(numAdults); this.pnlSidebar.Controls.Add(numNights); this.pnlSidebar.Controls.Add(cmbBoard);
            this.pnlSidebar.Controls.Add(numBudget); this.pnlSidebar.Controls.Add(numMarketPrice); this.pnlSidebar.Controls.Add(txtAgency);
            this.pnlSidebar.Controls.Add(cmbStatus); this.pnlSidebar.Controls.Add(cmbAssignedUser);

            // DATA PANEL
            this.pnlData.Location = new Point(360, 80); this.pnlData.Size = new Size(700, 580); this.pnlData.BackColor = Color.White; this.pnlData.BorderStyle = BorderStyle.FixedSingle;
            this.dgvData.Location = new Point(20, 70); this.dgvData.Size = new Size(660, 490); this.dgvData.BackgroundColor = Color.White; this.dgvData.BorderStyle = BorderStyle.None;
            this.lblDataTitle.Text = "База подорожей"; this.lblDataTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold); this.lblDataTitle.Location = new Point(20, 20);
            this.pnlData.Controls.Add(lblDataTitle); this.pnlData.Controls.Add(dgvData);

            this.ClientSize = new Size(1080, 680);
            this.Controls.Add(pnlHeader); this.Controls.Add(pnlSidebar); this.Controls.Add(pnlData);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(MainWork_Load);

            this.pnlHeader.ResumeLayout(false); this.pnlHeader.PerformLayout();
            this.pnlSidebar.ResumeLayout(false); this.pnlSidebar.PerformLayout();
            this.pnlData.ResumeLayout(false); this.pnlData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
        }

        private void AddLabel(string text, int y, int x = 20)
        {
            Label lbl = new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.Gray };
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
        private TextBox txtAgency;
        private ComboBox cmbBoard, cmbStatus, cmbAssignedUser, cmbTransport;
        private Button btnAdd, btnEdit, btnDelete, btnClear;
        private Panel pnlData;
        private DataGridView dgvData;
        private Label lblDataTitle;
    }
}