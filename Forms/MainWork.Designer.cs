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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbBack = new System.Windows.Forms.PictureBox();
            this.pbExit = new System.Windows.Forms.PictureBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbFilterMode = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.chkOnlyRequests = new System.Windows.Forms.CheckBox();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.cmbAssignedUser = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblAverageRating = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numBudget = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.btnOpenMap = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExportXML = new System.Windows.Forms.Button();
            this.btnImportXML = new System.Windows.Forms.Button();
            this.pbPlane = new System.Windows.Forms.PictureBox();
            this.btnOpenFeed = new System.Windows.Forms.Button();
            this.ctxGridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.grpInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlane)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.pbBack);
            this.pnlHeader.Controls.Add(this.pbExit);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(980, 56);
            this.pnlHeader.TabIndex = 6;
            this.pnlHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseDown);
            this.pnlHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseMove);
            this.pnlHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlHeader_MouseUp);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(50, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(294, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "WayPoint (Адмін-панель) ✈️";
            // 
            // pbBack
            // 
            this.pbBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbBack.Image = global::WayPoint.Properties.Resources.Home3_37171;
            this.pbBack.Location = new System.Drawing.Point(10, 9);
            this.pbBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbBack.Name = "pbBack";
            this.pbBack.Size = new System.Drawing.Size(30, 38);
            this.pbBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBack.TabIndex = 1;
            this.pbBack.TabStop = false;
            this.pbBack.Click += new System.EventHandler(this.pbBack_Click);
            // 
            // pbExit
            // 
            this.pbExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbExit.Image = global::WayPoint.Properties.Resources.free_icon_window_14062773;
            this.pbExit.Location = new System.Drawing.Point(940, 9);
            this.pbExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbExit.Name = "pbExit";
            this.pbExit.Size = new System.Drawing.Size(30, 38);
            this.pbExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbExit.TabIndex = 2;
            this.pbExit.TabStop = false;
            this.pbExit.Click += new System.EventHandler(this.pbExit_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.txtSearch.Location = new System.Drawing.Point(200, 75);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 30);
            this.txtSearch.TabIndex = 5;
            // 
            // cmbFilterMode
            // 
            this.cmbFilterMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterMode.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.cmbFilterMode.Location = new System.Drawing.Point(85, 75);
            this.cmbFilterMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbFilterMode.Name = "cmbFilterMode";
            this.cmbFilterMode.Size = new System.Drawing.Size(110, 31);
            this.cmbFilterMode.TabIndex = 4;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblSearch.Location = new System.Drawing.Point(15, 79);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(73, 23);
            this.lblSearch.TabIndex = 3;
            this.lblSearch.Text = "Пошук:";
            // 
            // chkOnlyRequests
            // 
            this.chkOnlyRequests.AutoSize = true;
            this.chkOnlyRequests.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chkOnlyRequests.ForeColor = System.Drawing.Color.DarkRed;
            this.chkOnlyRequests.Location = new System.Drawing.Point(435, 79);
            this.chkOnlyRequests.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkOnlyRequests.Name = "chkOnlyRequests";
            this.chkOnlyRequests.Size = new System.Drawing.Size(265, 24);
            this.chkOnlyRequests.TabIndex = 20;
            this.chkOnlyRequests.Text = "🚨 Тільки запити / На перевірці";
            this.chkOnlyRequests.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.BackgroundColor = System.Drawing.Color.White;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(340, 125);
            this.dgvData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersWidth = 51;
            this.dgvData.Size = new System.Drawing.Size(620, 300);
            this.dgvData.TabIndex = 0;
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.lblUser);
            this.grpInput.Controls.Add(this.cmbAssignedUser);
            this.grpInput.Controls.Add(this.label5);
            this.grpInput.Controls.Add(this.cmbStatus);
            this.grpInput.Controls.Add(this.lblAverageRating);
            this.grpInput.Controls.Add(this.label3);
            this.grpInput.Controls.Add(this.numBudget);
            this.grpInput.Controls.Add(this.label2);
            this.grpInput.Controls.Add(this.txtCity);
            this.grpInput.Controls.Add(this.label1);
            this.grpInput.Controls.Add(this.txtCountry);
            this.grpInput.Controls.Add(this.btnOpenMap);
            this.grpInput.Controls.Add(this.btnAdd);
            this.grpInput.Controls.Add(this.btnEdit);
            this.grpInput.Controls.Add(this.btnDelete);
            this.grpInput.Controls.Add(this.btnClear);
            this.grpInput.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.grpInput.Location = new System.Drawing.Point(15, 125);
            this.grpInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpInput.Name = "grpInput";
            this.grpInput.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpInput.Size = new System.Drawing.Size(310, 488);
            this.grpInput.TabIndex = 1;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Управління поїздкою";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUser.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblUser.Location = new System.Drawing.Point(15, 294);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(56, 20);
            this.lblUser.TabIndex = 16;
            this.lblUser.Text = "Клієнт";
            // 
            // cmbAssignedUser
            // 
            this.cmbAssignedUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssignedUser.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.cmbAssignedUser.Location = new System.Drawing.Point(110, 288);
            this.cmbAssignedUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbAssignedUser.Name = "cmbAssignedUser";
            this.cmbAssignedUser.Size = new System.Drawing.Size(180, 31);
            this.cmbAssignedUser.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.Location = new System.Drawing.Point(15, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Статус";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.cmbStatus.Items.AddRange(new object[] {
            "Запит",
            "Планується",
            "Очікує перевірки",
            "Завершено"});
            this.cmbStatus.Location = new System.Drawing.Point(110, 238);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(180, 31);
            this.cmbStatus.TabIndex = 8;
            // 
            // lblAverageRating
            // 
            this.lblAverageRating.AutoSize = true;
            this.lblAverageRating.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblAverageRating.ForeColor = System.Drawing.Color.DimGray;
            this.lblAverageRating.Location = new System.Drawing.Point(18, 198);
            this.lblAverageRating.MaximumSize = new System.Drawing.Size(270, 0);
            this.lblAverageRating.Name = "lblAverageRating";
            this.lblAverageRating.Size = new System.Drawing.Size(147, 20);
            this.lblAverageRating.TabIndex = 18;
            this.lblAverageRating.Text = "⭐ Рейтинг міста: ...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.Location = new System.Drawing.Point(15, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Бюджет ($)";
            // 
            // numBudget
            // 
            this.numBudget.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.numBudget.Location = new System.Drawing.Point(110, 144);
            this.numBudget.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numBudget.Name = "numBudget";
            this.numBudget.Size = new System.Drawing.Size(180, 30);
            this.numBudget.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(15, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Місто";
            // 
            // txtCity
            // 
            this.txtCity.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.txtCity.Location = new System.Drawing.Point(110, 94);
            this.txtCity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(180, 30);
            this.txtCity.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(15, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Країна";
            // 
            // txtCountry
            // 
            this.txtCountry.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.txtCountry.Location = new System.Drawing.Point(110, 44);
            this.txtCountry.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(140, 30);
            this.txtCountry.TabIndex = 0;
            // 
            // btnOpenMap
            // 
            this.btnOpenMap.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenMap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenMap.FlatAppearance.BorderSize = 0;
            this.btnOpenMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenMap.Font = new System.Drawing.Font("Segoe UI Emoji", 13F);
            this.btnOpenMap.Location = new System.Drawing.Point(255, 41);
            this.btnOpenMap.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenMap.Name = "btnOpenMap";
            this.btnOpenMap.Size = new System.Drawing.Size(35, 42);
            this.btnOpenMap.TabIndex = 10;
            this.btnOpenMap.Text = "🌍";
            this.btnOpenMap.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(15, 350);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(130, 50);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "➕ ДОДАТИ";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Goldenrod;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(160, 350);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(130, 50);
            this.btnEdit.TabIndex = 12;
            this.btnEdit.Text = "📝 ЗМІНИТИ";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.IndianRed;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(15, 412);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(130, 50);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "🗑 ВИДАЛИТИ";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightGray;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(160, 412);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(130, 50);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Очистити";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExportXML
            // 
            this.btnExportXML.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnExportXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportXML.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnExportXML.Location = new System.Drawing.Point(15, 625);
            this.btnExportXML.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExportXML.Name = "btnExportXML";
            this.btnExportXML.Size = new System.Drawing.Size(310, 45);
            this.btnExportXML.TabIndex = 10;
            this.btnExportXML.Text = "💾 Зберегти в XML";
            this.btnExportXML.UseVisualStyleBackColor = false;
            this.btnExportXML.Click += new System.EventHandler(this.btnExportXML_Click);
            // 
            // btnImportXML
            // 
            this.btnImportXML.BackColor = System.Drawing.Color.NavajoWhite;
            this.btnImportXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportXML.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnImportXML.Location = new System.Drawing.Point(15, 675);
            this.btnImportXML.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImportXML.Name = "btnImportXML";
            this.btnImportXML.Size = new System.Drawing.Size(310, 45);
            this.btnImportXML.TabIndex = 11;
            this.btnImportXML.Text = "📂 Завантажити з XML";
            this.btnImportXML.UseVisualStyleBackColor = false;
            this.btnImportXML.Click += new System.EventHandler(this.btnImportXML_Click);
            // 
            // pbPlane
            // 
            this.pbPlane.BackColor = System.Drawing.Color.LightSkyBlue;
            this.pbPlane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPlane.Location = new System.Drawing.Point(820, 438);
            this.pbPlane.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbPlane.Name = "pbPlane";
            this.pbPlane.Size = new System.Drawing.Size(140, 174);
            this.pbPlane.TabIndex = 2;
            this.pbPlane.TabStop = false;
            // 
            // btnOpenFeed
            // 
            this.btnOpenFeed.BackColor = System.Drawing.Color.DarkOrchid;
            this.btnOpenFeed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFeed.FlatAppearance.BorderSize = 0;
            this.btnOpenFeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFeed.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnOpenFeed.ForeColor = System.Drawing.Color.White;
            this.btnOpenFeed.Location = new System.Drawing.Point(730, 72);
            this.btnOpenFeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenFeed.Name = "btnOpenFeed";
            this.btnOpenFeed.Size = new System.Drawing.Size(200, 40);
            this.btnOpenFeed.TabIndex = 12;
            this.btnOpenFeed.Text = "🌍 Стрічка (Модерація)";
            this.btnOpenFeed.UseVisualStyleBackColor = false;
            this.btnOpenFeed.Click += new System.EventHandler(this.btnOpenFeed_Click);
            // 
            // ctxGridMenu
            // 
            this.ctxGridMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxGridMenu.Name = "ctxGridMenu";
            this.ctxGridMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // ctxMenuEdit
            // 
            this.ctxMenuEdit.Name = "ctxMenuEdit";
            this.ctxMenuEdit.Size = new System.Drawing.Size(32, 19);
            // 
            // ctxMenuDelete
            // 
            this.ctxMenuDelete.Name = "ctxMenuDelete";
            this.ctxMenuDelete.Size = new System.Drawing.Size(32, 19);
            // 
            // MainWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(980, 740);
            this.Controls.Add(this.chkOnlyRequests);
            this.Controls.Add(this.btnOpenFeed);
            this.Controls.Add(this.btnImportXML);
            this.Controls.Add(this.btnExportXML);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cmbFilterMode);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.pbPlane);
            this.Controls.Add(this.dgvData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainWork";
            this.Text = "WayPoint";
            this.Load += new System.EventHandler(this.MainWork_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBudget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlane)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTravel;
        private System.Windows.Forms.PictureBox pbPlane;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblAverageRating;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numBudget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCountry;
        private System.Windows.Forms.Button btnOpenMap;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExportXML;
        private System.Windows.Forms.Button btnImportXML;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pbBack;
        private System.Windows.Forms.PictureBox pbExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilterMode;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.CheckBox chkOnlyRequests;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox cmbAssignedUser;
        private System.Windows.Forms.Button btnOpenFeed;
        private System.Windows.Forms.ContextMenuStrip ctxGridMenu;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuEdit;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuDelete;
    }
}