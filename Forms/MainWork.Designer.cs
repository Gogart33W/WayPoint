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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnOpenFeed = new System.Windows.Forms.Button();
            this.btnAdminReturn = new System.Windows.Forms.Button(); // НОВА КНОПКА
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbBack = new System.Windows.Forms.PictureBox();
            this.pbExit = new System.Windows.Forms.PictureBox();

            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.lblSidebarTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.btnOpenMap = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numBudget = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.cmbAssignedUser = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();

            this.pnlData = new System.Windows.Forms.Panel();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.lblDataTitle = new System.Windows.Forms.Label();

            this.ctxGridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuDelete = new System.Windows.Forms.ToolStripMenuItem();

            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).BeginInit();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).BeginInit();
            this.pnlData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();

            // --- HEADER ---
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlHeader.Controls.Add(this.btnAdminReturn); // Додаємо кнопку
            this.pnlHeader.Controls.Add(this.btnOpenFeed);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.pbBack);
            this.pnlHeader.Controls.Add(this.pbExit);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseDown);
            this.pnlHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseMove);
            this.pnlHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseUp);

            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(60, 15);
            this.lblTitle.Text = "WayPoint | Робоча станція";

            this.pbBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbBack.Image = global::WayPoint.Properties.Resources.Home3_37171;
            this.pbBack.Location = new System.Drawing.Point(15, 12);
            this.pbBack.Size = new System.Drawing.Size(35, 35);
            this.pbBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBack.Click += new System.EventHandler(this.pbBack_Click);

            this.pbExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbExit.Image = global::WayPoint.Properties.Resources.free_icon_window_14062773;
            this.pbExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.pbExit.Location = new System.Drawing.Point(1030, 12);
            this.pbExit.Size = new System.Drawing.Size(35, 35);
            this.pbExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbExit.Click += new System.EventHandler(this.pbExit_Click);

            this.btnOpenFeed.BackColor = System.Drawing.Color.FromArgb(139, 92, 246);
            this.btnOpenFeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFeed.FlatAppearance.BorderSize = 0;
            this.btnOpenFeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFeed.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOpenFeed.ForeColor = System.Drawing.Color.White;
            this.btnOpenFeed.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnOpenFeed.Location = new System.Drawing.Point(820, 12);
            this.btnOpenFeed.Size = new System.Drawing.Size(180, 35);
            this.btnOpenFeed.Text = "🌍 Стрічка";
            this.btnOpenFeed.Click += new System.EventHandler(this.btnOpenFeed_Click);

            // Кнопка повернення в адмінку (видима тільки для Адміна)
            this.btnAdminReturn.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnAdminReturn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdminReturn.FlatAppearance.BorderSize = 0;
            this.btnAdminReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminReturn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdminReturn.ForeColor = System.Drawing.Color.White;
            this.btnAdminReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnAdminReturn.Location = new System.Drawing.Point(630, 12);
            this.btnAdminReturn.Size = new System.Drawing.Size(180, 35);
            this.btnAdminReturn.Text = "🛡️ В Адмін-панель";
            this.btnAdminReturn.Click += new System.EventHandler(this.btnAdminReturn_Click);

            // --- SIDEBAR ---
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.Location = new System.Drawing.Point(20, 80);
            this.pnlSidebar.Size = new System.Drawing.Size(320, 580);
            this.pnlSidebar.BorderStyle = BorderStyle.FixedSingle;

            this.lblSidebarTitle.Text = "Деталі туру";
            this.lblSidebarTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSidebarTitle.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.lblSidebarTitle.Location = new System.Drawing.Point(20, 20);
            this.lblSidebarTitle.AutoSize = true;

            this.label1.Text = "Країна";
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(20, 70);
            this.label1.AutoSize = true;

            this.txtCountry.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtCountry.Location = new System.Drawing.Point(20, 95);
            this.txtCountry.Size = new System.Drawing.Size(280, 32);

            this.label2.Text = "Місто";
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(20, 140);
            this.label2.AutoSize = true;

            this.txtCity.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtCity.Location = new System.Drawing.Point(20, 165);
            this.txtCity.Size = new System.Drawing.Size(230, 32);

            this.btnOpenMap.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.btnOpenMap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenMap.FlatAppearance.BorderSize = 0;
            this.btnOpenMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenMap.Font = new System.Drawing.Font("Segoe UI Emoji", 12F);
            this.btnOpenMap.Location = new System.Drawing.Point(260, 164);
            this.btnOpenMap.Size = new System.Drawing.Size(40, 34);
            this.btnOpenMap.Text = "🌍";
            this.btnOpenMap.Click += new System.EventHandler(this.btnOpenMap_Click);

            this.label3.Text = "Бюджет ($)";
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(20, 210);
            this.label3.AutoSize = true;

            this.numBudget.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.numBudget.Location = new System.Drawing.Point(20, 235);
            this.numBudget.Size = new System.Drawing.Size(280, 32);
            this.numBudget.Maximum = 100000;

            this.label5.Text = "Статус";
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(20, 280);
            this.label5.AutoSize = true;

            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbStatus.Items.AddRange(new object[] { "Запит", "Планується", "Очікує перевірки", "Завершено" });
            this.cmbStatus.Location = new System.Drawing.Point(20, 305);
            this.cmbStatus.Size = new System.Drawing.Size(280, 33);

            this.lblUser.Text = "Призначити клієнта:";
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUser.ForeColor = System.Drawing.Color.Gray;
            this.lblUser.Location = new System.Drawing.Point(20, 350);
            this.lblUser.AutoSize = true;

            this.cmbAssignedUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssignedUser.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbAssignedUser.Location = new System.Drawing.Point(20, 375);
            this.cmbAssignedUser.Size = new System.Drawing.Size(280, 33);

            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(20, 440);
            this.btnAdd.Size = new System.Drawing.Size(135, 45);
            this.btnAdd.Text = "Додати";
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEdit.Location = new System.Drawing.Point(165, 440);
            this.btnEdit.Size = new System.Drawing.Size(135, 45);
            this.btnEdit.Text = "Оновити";
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(20, 500);
            this.btnDelete.Size = new System.Drawing.Size(135, 45);
            this.btnDelete.Text = "Видалити";
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.btnClear.BackColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.Location = new System.Drawing.Point(165, 500);
            this.btnClear.Size = new System.Drawing.Size(135, 45);
            this.btnClear.Text = "Очистити";
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            this.pnlSidebar.Controls.Add(this.lblSidebarTitle);
            this.pnlSidebar.Controls.Add(this.label1);
            this.pnlSidebar.Controls.Add(this.txtCountry);
            this.pnlSidebar.Controls.Add(this.label2);
            this.pnlSidebar.Controls.Add(this.txtCity);
            this.pnlSidebar.Controls.Add(this.btnOpenMap);
            this.pnlSidebar.Controls.Add(this.label3);
            this.pnlSidebar.Controls.Add(this.numBudget);
            this.pnlSidebar.Controls.Add(this.label5);
            this.pnlSidebar.Controls.Add(this.cmbStatus);
            this.pnlSidebar.Controls.Add(this.lblUser);
            this.pnlSidebar.Controls.Add(this.cmbAssignedUser);
            this.pnlSidebar.Controls.Add(this.btnAdd);
            this.pnlSidebar.Controls.Add(this.btnEdit);
            this.pnlSidebar.Controls.Add(this.btnDelete);
            this.pnlSidebar.Controls.Add(this.btnClear);

            // --- DATA PANEL ---
            this.pnlData.BackColor = System.Drawing.Color.White;
            this.pnlData.Location = new System.Drawing.Point(360, 80);
            this.pnlData.Size = new System.Drawing.Size(700, 580);
            this.pnlData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlData.BorderStyle = BorderStyle.FixedSingle;

            this.lblDataTitle.Text = "База подорожей";
            this.lblDataTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDataTitle.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.lblDataTitle.Location = new System.Drawing.Point(20, 20);
            this.lblDataTitle.AutoSize = true;

            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvData.Location = new System.Drawing.Point(20, 70);
            this.dgvData.Size = new System.Drawing.Size(660, 490);
            this.dgvData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.pnlData.Controls.Add(this.lblDataTitle);
            this.pnlData.Controls.Add(this.dgvData);

            // --- MAIN FORM ---
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.ClientSize = new System.Drawing.Size(1080, 680);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "MainWork";
            this.Text = "WayPoint";
            this.Load += new System.EventHandler(this.MainWork_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).EndInit();
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).EndInit();
            this.pnlData.ResumeLayout(false);
            this.pnlData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbBack;
        private System.Windows.Forms.PictureBox pbExit;
        private System.Windows.Forms.Button btnOpenFeed;
        private System.Windows.Forms.Button btnAdminReturn; // Додана кнопка

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblSidebarTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCountry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Button btnOpenMap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numBudget;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox cmbAssignedUser;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.Panel pnlData;
        private System.Windows.Forms.Label lblDataTitle;
        private System.Windows.Forms.DataGridView dgvData;

        private System.Windows.Forms.ContextMenuStrip ctxGridMenu;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuEdit;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuDelete;
    }
}