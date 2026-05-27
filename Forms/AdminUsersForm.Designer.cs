using System;
using System.Drawing;
using System.Windows.Forms;

namespace WayPoint
{
    partial class AdminUsersForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBack = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();

            this.pnlSidebar = new System.Windows.Forms.Panel(); // КАРТКА ЛІВОРАУЧ
            this.lblSidebarTitle = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();

            this.pnlData = new System.Windows.Forms.Panel(); // КАРТКА ПРАВОРУЧ
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.lblDataTitle = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.pnlData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();

            // ===== HEADER =====
            this.pnlHeader.BackColor = Color.FromArgb(31, 41, 55);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblBack);
            this.pnlHeader.Controls.Add(this.lblExit);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 60;

            this.lblTitle.Text = "WayPoint | Панель Адміністратора";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblTitle.Location = new Point(60, 15);
            this.lblTitle.AutoSize = true;

            this.lblBack.Text = "⬅️";
            this.lblBack.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblBack.ForeColor = Color.LightGray;
            this.lblBack.Location = new Point(15, 12);
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = Cursors.Hand;
            this.lblBack.Click += new EventHandler(this.btnBack_Click);
            this.lblBack.MouseEnter += (s, e) => this.lblBack.ForeColor = Color.White;
            this.lblBack.MouseLeave += (s, e) => this.lblBack.ForeColor = Color.LightGray;

            this.lblExit.Text = "❌";
            this.lblExit.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblExit.ForeColor = Color.LightGray;
            this.lblExit.Location = new Point(1030, 12);
            this.lblExit.AutoSize = true;
            this.lblExit.Cursor = Cursors.Hand;
            this.lblExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblExit.Click += (s, e) => Application.Exit();
            this.lblExit.MouseEnter += (s, e) => this.lblExit.ForeColor = Color.White;
            this.lblExit.MouseLeave += (s, e) => this.lblExit.ForeColor = Color.LightGray;

            // ===== SIDEBAR (БІЛА КАРТКА БЕЗ РАМКИ) =====
            this.pnlSidebar.BackColor = Color.White;
            this.pnlSidebar.Location = new Point(20, 80);
            this.pnlSidebar.Size = new Size(320, 580);
            this.pnlSidebar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            this.lblSidebarTitle.Text = "Профіль користувача";
            this.lblSidebarTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblSidebarTitle.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblSidebarTitle.Location = new Point(20, 20);
            this.lblSidebarTitle.AutoSize = true;

            AddLabel("Логін", 70, this.pnlSidebar);
            this.txtUsername.Location = new Point(20, 95);
            this.txtUsername.Size = new Size(275, 29);
            this.txtUsername.Font = new Font("Segoe UI", 12F);
            this.txtUsername.BackColor = Color.FromArgb(243, 244, 246);
            this.txtUsername.BorderStyle = BorderStyle.FixedSingle;
            this.pnlSidebar.Controls.Add(this.txtUsername);

            AddLabel("Електронна пошта", 140, this.pnlSidebar);
            this.txtEmail.Location = new Point(20, 165);
            this.txtEmail.Size = new Size(275, 29);
            this.txtEmail.Font = new Font("Segoe UI", 12F);
            this.txtEmail.BackColor = Color.FromArgb(243, 244, 246);
            this.txtEmail.BorderStyle = BorderStyle.FixedSingle;
            this.pnlSidebar.Controls.Add(this.txtEmail);

            AddLabel("Системна роль", 210, this.pnlSidebar);
            this.cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "User" });
            this.cmbRole.Location = new Point(20, 235);
            this.cmbRole.Size = new Size(275, 29);
            this.cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRole.Font = new Font("Segoe UI", 11F);
            this.cmbRole.BackColor = Color.FromArgb(243, 244, 246);
            this.pnlSidebar.Controls.Add(this.cmbRole);

            this.btnSave.Location = new Point(20, 300);
            this.btnSave.Size = new Size(275, 45);
            this.btnSave.Text = "💾 Зберегти зміни";
            this.btnSave.BackColor = Color.FromArgb(16, 185, 129); // Emerald
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.pnlSidebar.Controls.Add(this.btnSave);

            this.btnDelete.Location = new Point(20, 360);
            this.btnDelete.Size = new Size(275, 45);
            this.btnDelete.Text = "🗑 Видалити акаунт";
            this.btnDelete.BackColor = Color.FromArgb(239, 68, 68); // Red
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.pnlSidebar.Controls.Add(this.btnDelete);

            // ===== DATA PANEL (БІЛА КАРТКА ПРАВОРУЧ) =====
            this.pnlData.BackColor = Color.White;
            this.pnlData.Location = new Point(360, 80);
            this.pnlData.Size = new Size(700, 580);
            this.pnlData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.lblDataTitle.Text = "Список зареєстрованих користувачів";
            this.lblDataTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblDataTitle.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblDataTitle.Location = new Point(20, 20);
            this.lblDataTitle.AutoSize = true;

            // Красива таблиця
            this.dgvUsers.Location = new Point(20, 70);
            this.dgvUsers.Size = new Size(660, 490);
            this.dgvUsers.BackgroundColor = Color.White;
            this.dgvUsers.BorderStyle = BorderStyle.None;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            this.dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255); // Ніжно синій при виділенні
            this.dgvUsers.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.pnlData.Controls.Add(this.lblDataTitle);
            this.pnlData.Controls.Add(this.dgvUsers);

            // ===== FORM CONFIG =====
            this.ClientSize = new Size(1080, 680);
            this.BackColor = Color.FromArgb(243, 244, 246); // СВІТЛО-СІРИЙ ФОН МІЖ КАРТКАМИ
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlData);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(this.AdminUsersForm_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlData.ResumeLayout(false);
            this.pnlData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }

        private void AddLabel(string text, int y, Control parent)
        {
            Label lbl = new Label { Text = text, Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = Color.Gray };
            parent.Controls.Add(lbl);
        }

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblBack;
        private Label lblExit;
        private Panel pnlSidebar;
        private Label lblSidebarTitle;
        private TextBox txtUsername;
        private TextBox txtEmail;
        private ComboBox cmbRole;
        private Button btnSave;
        private Button btnDelete;
        private Panel pnlData;
        private DataGridView dgvUsers;
        private Label lblDataTitle;
    }
}