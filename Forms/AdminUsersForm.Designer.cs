using System;
using System.Drawing;
using System.Windows.Forms;

namespace WayPoint
{
    partial class AdminUsersForm
    {
        private System.ComponentModel.IContainer components = null;

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
        private Button btnNewAccount;
        private Button btnOpenWork;
        private Button btnOpenFeed;
        private Button btnOpenAnalytics; // КНОПКА АНАЛІТИКИ

        private TabControl tabControl;
        private TabPage tabAdminsWorkers;
        private TabPage tabUsersOnly;

        private Label lblDataTitleAdmins;
        private TextBox txtSearchAdmins;
        private DataGridView dgvAdminsWorkers;

        private Label lblDataTitleUsers;
        private TextBox txtSearchUsers;
        private DataGridView dgvUsersOnly;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ===== HEADER =====
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblBack = new Label();
            this.lblExit = new Label();

            this.pnlHeader.SuspendLayout();
            this.pnlHeader.BackColor = Color.FromArgb(31, 41, 55);
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
            this.lblBack.MouseEnter += (s, e) => this.lblBack.ForeColor = Color.White;
            this.lblBack.MouseLeave += (s, e) => this.lblBack.ForeColor = Color.LightGray;

            this.lblExit.Text = "❌";
            this.lblExit.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblExit.ForeColor = Color.LightGray;
            this.lblExit.Location = new Point(1030, 12);
            this.lblExit.AutoSize = true;
            this.lblExit.Cursor = Cursors.Hand;
            this.lblExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblExit.MouseEnter += (s, e) => this.lblExit.ForeColor = Color.White;
            this.lblExit.MouseLeave += (s, e) => this.lblExit.ForeColor = Color.LightGray;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblBack);
            this.pnlHeader.Controls.Add(this.lblExit);

            // ===== SIDEBAR =====
            this.pnlSidebar = new Panel();
            this.pnlSidebar.BackColor = Color.White;
            this.pnlSidebar.Location = new Point(20, 80);
            this.pnlSidebar.Size = new Size(320, 620);
            this.pnlSidebar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            this.lblSidebarTitle = new Label();
            this.lblSidebarTitle.Text = "Профіль користувача";
            this.lblSidebarTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            this.lblSidebarTitle.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblSidebarTitle.Location = new Point(20, 20);
            this.lblSidebarTitle.AutoSize = true;

            AddLabel("Логін", 70, this.pnlSidebar);
            this.txtUsername = new TextBox();
            this.txtUsername.Location = new Point(20, 95);
            this.txtUsername.Size = new Size(275, 29);
            this.txtUsername.Font = new Font("Segoe UI", 12F);
            this.txtUsername.BackColor = Color.FromArgb(243, 244, 246);
            this.txtUsername.BorderStyle = BorderStyle.FixedSingle;

            AddLabel("Електронна пошта", 140, this.pnlSidebar);
            this.txtEmail = new TextBox();
            this.txtEmail.Location = new Point(20, 165);
            this.txtEmail.Size = new Size(275, 29);
            this.txtEmail.Font = new Font("Segoe UI", 12F);
            this.txtEmail.BackColor = Color.FromArgb(243, 244, 246);
            this.txtEmail.BorderStyle = BorderStyle.FixedSingle;

            AddLabel("Системна роль", 210, this.pnlSidebar);
            this.cmbRole = new ComboBox();
            this.cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "User" });
            this.cmbRole.Location = new Point(20, 235);
            this.cmbRole.Size = new Size(275, 29);
            this.cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRole.Font = new Font("Segoe UI", 11F);
            this.cmbRole.BackColor = Color.FromArgb(243, 244, 246);

            this.btnSave = new Button();
            this.btnSave.Location = new Point(20, 285);
            this.btnSave.Size = new Size(275, 42);
            this.btnSave.Text = "💾 Зберегти зміни";
            this.btnSave.BackColor = Color.FromArgb(16, 185, 129);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnSave.Cursor = Cursors.Hand;

            this.btnDelete = new Button();
            this.btnDelete.Location = new Point(20, 337);
            this.btnDelete.Size = new Size(275, 42);
            this.btnDelete.Text = "🗑 Видалити акаунт";
            this.btnDelete.BackColor = Color.FromArgb(239, 68, 68);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnDelete.Cursor = Cursors.Hand;

            this.btnNewAccount = new Button();
            this.btnNewAccount.Location = new Point(20, 389);
            this.btnNewAccount.Size = new Size(275, 42);
            this.btnNewAccount.Text = "➕ Створити акаунт";
            this.btnNewAccount.BackColor = Color.FromArgb(59, 130, 246);
            this.btnNewAccount.ForeColor = Color.White;
            this.btnNewAccount.FlatAppearance.BorderSize = 0;
            this.btnNewAccount.FlatStyle = FlatStyle.Flat;
            this.btnNewAccount.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnNewAccount.Cursor = Cursors.Hand;

            var sep = new Label();
            sep.BorderStyle = BorderStyle.Fixed3D;
            sep.Location = new Point(20, 443);
            sep.Size = new Size(275, 2);

            AddLabel("Перегляд від імені", 450, this.pnlSidebar);

            this.btnOpenWork = new Button();
            this.btnOpenWork.Location = new Point(20, 472);
            this.btnOpenWork.Size = new Size(275, 42);
            this.btnOpenWork.Text = "🖥 Робочий простір";
            this.btnOpenWork.BackColor = Color.FromArgb(107, 114, 128);
            this.btnOpenWork.ForeColor = Color.White;
            this.btnOpenWork.FlatAppearance.BorderSize = 0;
            this.btnOpenWork.FlatStyle = FlatStyle.Flat;
            this.btnOpenWork.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnOpenWork.Cursor = Cursors.Hand;
            this.btnOpenWork.Enabled = false;

            this.btnOpenFeed = new Button();
            this.btnOpenFeed.Location = new Point(20, 520);
            this.btnOpenFeed.Size = new Size(275, 42);
            this.btnOpenFeed.Text = "📰 Стрічка користувача";
            this.btnOpenFeed.BackColor = Color.FromArgb(107, 114, 128);
            this.btnOpenFeed.ForeColor = Color.White;
            this.btnOpenFeed.FlatAppearance.BorderSize = 0;
            this.btnOpenFeed.FlatStyle = FlatStyle.Flat;
            this.btnOpenFeed.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnOpenFeed.Cursor = Cursors.Hand;
            this.btnOpenFeed.Enabled = true;

            // КНОПКА АНАЛІТИКИ
            this.btnOpenAnalytics = new Button();
            this.btnOpenAnalytics.Location = new Point(20, 568);
            this.btnOpenAnalytics.Size = new Size(275, 42);
            this.btnOpenAnalytics.Text = "📊 Аналітика";
            this.btnOpenAnalytics.BackColor = Color.FromArgb(139, 92, 246);
            this.btnOpenAnalytics.ForeColor = Color.White;
            this.btnOpenAnalytics.FlatAppearance.BorderSize = 0;
            this.btnOpenAnalytics.FlatStyle = FlatStyle.Flat;
            this.btnOpenAnalytics.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.btnOpenAnalytics.Cursor = Cursors.Hand;

            this.pnlSidebar.Controls.Add(this.lblSidebarTitle);
            this.pnlSidebar.Controls.Add(this.txtUsername);
            this.pnlSidebar.Controls.Add(this.txtEmail);
            this.pnlSidebar.Controls.Add(this.cmbRole);
            this.pnlSidebar.Controls.Add(this.btnSave);
            this.pnlSidebar.Controls.Add(this.btnDelete);
            this.pnlSidebar.Controls.Add(this.btnNewAccount);
            this.pnlSidebar.Controls.Add(sep);
            this.pnlSidebar.Controls.Add(this.btnOpenWork);
            this.pnlSidebar.Controls.Add(this.btnOpenFeed);
            this.pnlSidebar.Controls.Add(this.btnOpenAnalytics);

            // ===== TAB CONTROL =====
            this.tabControl = new TabControl();
            this.tabAdminsWorkers = new TabPage();
            this.tabUsersOnly = new TabPage();

            this.tabControl.Location = new Point(360, 80);
            this.tabControl.Size = new Size(700, 620);
            this.tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.tabAdminsWorkers.Text = "Адміни і працівники";
            this.tabUsersOnly.Text = "Користувачі";

            // --- Tab 1: Admins & Workers ---
            this.lblDataTitleAdmins = new Label();
            this.lblDataTitleAdmins.Text = "Адміни і працівники";
            this.lblDataTitleAdmins.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            this.lblDataTitleAdmins.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblDataTitleAdmins.Location = new Point(20, 15);
            this.lblDataTitleAdmins.AutoSize = true;

            this.txtSearchAdmins = new TextBox();
            this.txtSearchAdmins.Location = new Point(20, 50);
            this.txtSearchAdmins.Size = new Size(650, 28);
            this.txtSearchAdmins.Font = new Font("Segoe UI", 11F);
            this.txtSearchAdmins.BackColor = Color.FromArgb(243, 244, 246);
            this.txtSearchAdmins.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearchAdmins.ForeColor = Color.Gray;
            this.txtSearchAdmins.Text = "🔍 Пошук за ніком...";
            this.txtSearchAdmins.GotFocus += (s, e) => { if (this.txtSearchAdmins.ForeColor == Color.Gray) { this.txtSearchAdmins.Text = ""; this.txtSearchAdmins.ForeColor = Color.Black; } };
            this.txtSearchAdmins.LostFocus += (s, e) => { if (string.IsNullOrEmpty(this.txtSearchAdmins.Text)) { this.txtSearchAdmins.ForeColor = Color.Gray; this.txtSearchAdmins.Text = "🔍 Пошук за ніком..."; } };

            this.dgvAdminsWorkers = new DataGridView();
            this.dgvAdminsWorkers.AutoGenerateColumns = true;
            this.dgvAdminsWorkers.Location = new Point(20, 88);
            this.dgvAdminsWorkers.Size = new Size(650, 490);
            this.dgvAdminsWorkers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvAdminsWorkers.BackgroundColor = Color.White;
            this.dgvAdminsWorkers.BorderStyle = BorderStyle.None;
            this.dgvAdminsWorkers.EnableHeadersVisualStyles = false;
            this.dgvAdminsWorkers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            this.dgvAdminsWorkers.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.dgvAdminsWorkers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.dgvAdminsWorkers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246);
            this.dgvAdminsWorkers.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            this.dgvAdminsWorkers.RowHeadersVisible = false;
            this.dgvAdminsWorkers.AllowUserToAddRows = false;
            this.dgvAdminsWorkers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdminsWorkers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAdminsWorkers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAdminsWorkers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255);
            this.dgvAdminsWorkers.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.dgvAdminsWorkers.DefaultCellStyle.Font = new Font("Segoe UI", 10F);

            this.tabAdminsWorkers.Controls.Add(this.lblDataTitleAdmins);
            this.tabAdminsWorkers.Controls.Add(this.txtSearchAdmins);
            this.tabAdminsWorkers.Controls.Add(this.dgvAdminsWorkers);

            // --- Tab 2: Users only ---
            this.lblDataTitleUsers = new Label();
            this.lblDataTitleUsers.Text = "Користувачі";
            this.lblDataTitleUsers.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            this.lblDataTitleUsers.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblDataTitleUsers.Location = new Point(20, 15);
            this.lblDataTitleUsers.AutoSize = true;

            this.txtSearchUsers = new TextBox();
            this.txtSearchUsers.Location = new Point(20, 50);
            this.txtSearchUsers.Size = new Size(650, 28);
            this.txtSearchUsers.Font = new Font("Segoe UI", 11F);
            this.txtSearchUsers.BackColor = Color.FromArgb(243, 244, 246);
            this.txtSearchUsers.BorderStyle = BorderStyle.FixedSingle;
            this.txtSearchUsers.ForeColor = Color.Gray;
            this.txtSearchUsers.Text = "🔍 Пошук за ніком...";
            this.txtSearchUsers.GotFocus += (s, e) => { if (this.txtSearchUsers.ForeColor == Color.Gray) { this.txtSearchUsers.Text = ""; this.txtSearchUsers.ForeColor = Color.Black; } };
            this.txtSearchUsers.LostFocus += (s, e) => { if (string.IsNullOrEmpty(this.txtSearchUsers.Text)) { this.txtSearchUsers.ForeColor = Color.Gray; this.txtSearchUsers.Text = "🔍 Пошук за ніком..."; } };

            this.dgvUsersOnly = new DataGridView();
            this.dgvUsersOnly.AutoGenerateColumns = true;
            this.dgvUsersOnly.Location = new Point(20, 88);
            this.dgvUsersOnly.Size = new Size(650, 490);
            this.dgvUsersOnly.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvUsersOnly.BackgroundColor = Color.White;
            this.dgvUsersOnly.BorderStyle = BorderStyle.None;
            this.dgvUsersOnly.EnableHeadersVisualStyles = false;
            this.dgvUsersOnly.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            this.dgvUsersOnly.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            this.dgvUsersOnly.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            this.dgvUsersOnly.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 244, 246);
            this.dgvUsersOnly.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            this.dgvUsersOnly.RowHeadersVisible = false;
            this.dgvUsersOnly.AllowUserToAddRows = false;
            this.dgvUsersOnly.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsersOnly.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsersOnly.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUsersOnly.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255);
            this.dgvUsersOnly.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.dgvUsersOnly.DefaultCellStyle.Font = new Font("Segoe UI", 10F);

            this.tabUsersOnly.Controls.Add(this.lblDataTitleUsers);
            this.tabUsersOnly.Controls.Add(this.txtSearchUsers);
            this.tabUsersOnly.Controls.Add(this.dgvUsersOnly);

            this.tabControl.Controls.Add(this.tabAdminsWorkers);
            this.tabControl.Controls.Add(this.tabUsersOnly);

            // ===== FORM =====
            this.ClientSize = new Size(1080, 720);
            this.BackColor = Color.FromArgb(243, 244, 246);
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.tabControl);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
        }

        private void AddLabel(string text, int y, Control parent)
        {
            var lbl = new Label
            {
                Text = text,
                Location = new Point(20, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.Gray
            };
            parent.Controls.Add(lbl);
        }
    }
}