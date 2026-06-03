namespace WayPoint.Forms
{
    partial class AdminAnalyticsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBack = new System.Windows.Forms.Label();
            this.lblExit = new System.Windows.Forms.Label();

            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.btnApply = new System.Windows.Forms.Button();

            this.pnlSummary = new System.Windows.Forms.FlowLayoutPanel();
            this.cardEmployees = new System.Windows.Forms.Panel();
            this.lblEmpTitle = new System.Windows.Forms.Label();
            this.lblEmpValue = new System.Windows.Forms.Label();
            this.cardTours = new System.Windows.Forms.Panel();
            this.lblToursTitle = new System.Windows.Forms.Label();
            this.lblToursValue = new System.Windows.Forms.Label();
            this.cardRevenue = new System.Windows.Forms.Panel();
            this.lblRevTitle = new System.Windows.Forms.Label();
            this.lblRevValue = new System.Windows.Forms.Label();
            this.cardMessages = new System.Windows.Forms.Panel();
            this.lblMsgTitle = new System.Windows.Forms.Label();
            this.lblMsgValue = new System.Windows.Forms.Label();

            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.chartStatuses = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartDynamics = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvManagerStats = new System.Windows.Forms.DataGridView();

            this.pnlHeader.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.cardEmployees.SuspendLayout();
            this.cardTours.SuspendLayout();
            this.cardRevenue.SuspendLayout();
            this.cardMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatuses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDynamics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManagerStats)).BeginInit();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.pnlHeader.Controls.Add(this.lblBack);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblExit);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;

            // lblBack
            this.lblBack.AutoSize = true;
            this.lblBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBack.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBack.ForeColor = System.Drawing.Color.LightGray;
            this.lblBack.Location = new System.Drawing.Point(15, 18);
            this.lblBack.Text = "⬅️ Назад";
            this.lblBack.Click += new System.EventHandler(this.btnClose_Click);
            this.lblBack.MouseEnter += (s, e) => this.lblBack.ForeColor = System.Drawing.Color.White;
            this.lblBack.MouseLeave += (s, e) => this.lblBack.ForeColor = System.Drawing.Color.LightGray;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(130, 14);
            this.lblTitle.Text = "📊 Аналітичний Дашборд";

            // lblExit
            this.lblExit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblExit.AutoSize = true;
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblExit.ForeColor = System.Drawing.Color.LightGray;
            this.lblExit.Location = new System.Drawing.Point(1050, 18);
            this.lblExit.Text = "❌ Закрити";
            this.lblExit.Click += new System.EventHandler(this.btnExitApp_Click);
            this.lblExit.MouseEnter += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.Crimson;
            this.lblExit.MouseLeave += (s, e) => this.lblExit.ForeColor = System.Drawing.Color.LightGray;

            // 
            // pnlFilters
            // 
            this.pnlFilters.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlFilters.Controls.Add(this.lblFilter);
            this.pnlFilters.Controls.Add(this.cmbPeriod);
            this.pnlFilters.Controls.Add(this.dtpStart);
            this.pnlFilters.Controls.Add(this.dtpEnd);
            this.pnlFilters.Controls.Add(this.btnApply);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Height = 50;
            this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFilter.Location = new System.Drawing.Point(20, 15);
            this.lblFilter.Text = "Період:";

            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.Font = new System.Drawing.Font("Segoe UI", 10F);
            // ФІКС НАЗВ ПЕРІОДІВ
            this.cmbPeriod.Items.AddRange(new object[] { "За весь час", "За сьогодні", "Поточний тиждень", "Поточний місяць", "Обрати період..." });
            this.cmbPeriod.Location = new System.Drawing.Point(85, 12);
            this.cmbPeriod.Width = 160;
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);

            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(255, 12);
            this.dtpStart.Width = 100;
            this.dtpStart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpStart.Visible = false;

            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(365, 12);
            this.dtpEnd.Width = 100;
            this.dtpEnd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpEnd.Visible = false;

            this.btnApply.BackColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnApply.Location = new System.Drawing.Point(475, 11);
            this.btnApply.Size = new System.Drawing.Size(120, 27);
            this.btnApply.Text = "Застосувати";
            this.btnApply.Visible = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);

            // 
            // pnlSummary
            // 
            this.pnlSummary.BackColor = System.Drawing.Color.White;
            this.pnlSummary.Controls.Add(this.cardEmployees);
            this.pnlSummary.Controls.Add(this.cardTours);
            this.pnlSummary.Controls.Add(this.cardRevenue);
            this.pnlSummary.Controls.Add(this.cardMessages);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSummary.Height = 100;
            this.pnlSummary.Padding = new System.Windows.Forms.Padding(15, 15, 0, 0);

            // cardEmployees
            this.cardEmployees.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.cardEmployees.Size = new System.Drawing.Size(250, 70);
            this.cardEmployees.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblEmpTitle.AutoSize = true; this.lblEmpTitle.Location = new System.Drawing.Point(10, 10); this.lblEmpTitle.Text = "Активних працівників"; this.lblEmpTitle.ForeColor = System.Drawing.Color.Gray; this.lblEmpTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEmpValue.AutoSize = true; this.lblEmpValue.Location = new System.Drawing.Point(10, 30); this.lblEmpValue.Text = "0"; this.lblEmpValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold); this.lblEmpValue.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.cardEmployees.Controls.Add(this.lblEmpTitle); this.cardEmployees.Controls.Add(this.lblEmpValue);

            // cardTours
            this.cardTours.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.cardTours.Size = new System.Drawing.Size(250, 70);
            this.cardTours.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblToursTitle.AutoSize = true; this.lblToursTitle.Location = new System.Drawing.Point(10, 10); this.lblToursTitle.Text = "Оформлено турів"; this.lblToursTitle.ForeColor = System.Drawing.Color.Gray; this.lblToursTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblToursValue.AutoSize = true; this.lblToursValue.Location = new System.Drawing.Point(10, 30); this.lblToursValue.Text = "0"; this.lblToursValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold); this.lblToursValue.ForeColor = System.Drawing.Color.FromArgb(16, 185, 129);
            this.cardTours.Controls.Add(this.lblToursTitle); this.cardTours.Controls.Add(this.lblToursValue);

            // cardRevenue
            this.cardRevenue.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.cardRevenue.Size = new System.Drawing.Size(250, 70);
            this.cardRevenue.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblRevTitle.AutoSize = true; this.lblRevTitle.Location = new System.Drawing.Point(10, 10); this.lblRevTitle.Text = "Загальний обіг ($)"; this.lblRevTitle.ForeColor = System.Drawing.Color.Gray; this.lblRevTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRevValue.AutoSize = true; this.lblRevValue.Location = new System.Drawing.Point(10, 30); this.lblRevValue.Text = "$0"; this.lblRevValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold); this.lblRevValue.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.cardRevenue.Controls.Add(this.lblRevTitle); this.cardRevenue.Controls.Add(this.lblRevValue);

            // cardMessages
            this.cardMessages.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.cardMessages.Size = new System.Drawing.Size(250, 70);
            this.cardMessages.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblMsgTitle.AutoSize = true; this.lblMsgTitle.Location = new System.Drawing.Point(10, 10); this.lblMsgTitle.Text = "Відповідей підтримки"; this.lblMsgTitle.ForeColor = System.Drawing.Color.Gray; this.lblMsgTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMsgValue.AutoSize = true; this.lblMsgValue.Location = new System.Drawing.Point(10, 30); this.lblMsgValue.Text = "0"; this.lblMsgValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold); this.lblMsgValue.ForeColor = System.Drawing.Color.FromArgb(139, 92, 246);
            this.cardMessages.Controls.Add(this.lblMsgTitle); this.cardMessages.Controls.Add(this.lblMsgValue);

            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer.Height = 350;
            this.splitContainer.SplitterDistance = 400;
            this.splitContainer.Panel1.Controls.Add(this.chartStatuses);
            this.splitContainer.Panel2.Controls.Add(this.chartDynamics);
            this.splitContainer.IsSplitterFixed = true;

            // 
            // chartStatuses
            // 
            chartArea1.Name = "ChartArea1";
            this.chartStatuses.ChartAreas.Add(chartArea1);
            this.chartStatuses.Dock = System.Windows.Forms.DockStyle.Fill;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.IsValueShownAsLabel = true;
            series1.Name = "Statuses";
            this.chartStatuses.Series.Add(series1);
            title1.Text = "Розподіл часток за статусами";
            title1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.chartStatuses.Titles.Add(title1);

            // 
            // chartDynamics
            // 
            chartArea2.Name = "ChartArea1";
            this.chartDynamics.ChartAreas.Add(chartArea2);
            this.chartDynamics.Dock = System.Windows.Forms.DockStyle.Fill;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            series2.Color = System.Drawing.Color.FromArgb(100, 16, 185, 129);
            series2.BorderWidth = 3;
            series2.BorderColor = System.Drawing.Color.FromArgb(16, 185, 129);
            series2.IsValueShownAsLabel = true;
            series2.Name = "Revenue";
            this.chartDynamics.Series.Add(series2);
            title2.Text = "Динаміка прибутку по датах ($)";
            title2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.chartDynamics.Titles.Add(title2);

            // 
            // dgvManagerStats
            // 
            this.dgvManagerStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvManagerStats.BackgroundColor = System.Drawing.Color.White;
            this.dgvManagerStats.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvManagerStats.ReadOnly = true;
            this.dgvManagerStats.AllowUserToAddRows = false;
            this.dgvManagerStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvManagerStats.EnableHeadersVisualStyles = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.dgvManagerStats.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvManagerStats.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);

            // ФІКС КОЛЬОРУ ВИДІЛЕННЯ ТАБЛИЦІ (ТЕПЕР БІЛЕ/ЧОРНЕ ЯК ФОН)
            this.dgvManagerStats.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvManagerStats.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvManagerStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // 
            // AdminAnalyticsForm
            // 
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Controls.Add(this.dgvManagerStats);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlHeader);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;
            this.Load += new System.EventHandler(this.AdminAnalyticsForm_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pnlSummary.ResumeLayout(false);
            this.cardEmployees.ResumeLayout(false);
            this.cardEmployees.PerformLayout();
            this.cardTours.ResumeLayout(false);
            this.cardTours.PerformLayout();
            this.cardRevenue.ResumeLayout(false);
            this.cardRevenue.PerformLayout();
            this.cardMessages.ResumeLayout(false);
            this.cardMessages.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartStatuses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDynamics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManagerStats)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBack;
        private System.Windows.Forms.Label lblExit;

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnApply;

        private System.Windows.Forms.FlowLayoutPanel pnlSummary;
        private System.Windows.Forms.Panel cardEmployees, cardTours, cardRevenue, cardMessages;
        private System.Windows.Forms.Label lblEmpTitle, lblEmpValue, lblToursTitle, lblToursValue, lblRevTitle, lblRevValue, lblMsgTitle, lblMsgValue;

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStatuses;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDynamics;

        private System.Windows.Forms.DataGridView dgvManagerStats;
    }
}