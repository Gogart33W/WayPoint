using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;

namespace WayPoint
{
    public partial class MainWork : Form
    {
        private DataTable travelTable = new DataTable();
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public MainWork()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            SetupDataTableStructure();
        }

        private void MainWork_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"WayPoint | База турів | {Session.Username} ({Session.Role})";

            // МАГІЯ: Показуємо кнопку повернення тільки якщо це Адмін!
            btnAdminReturn.Visible = (Session.Role == "Admin");

            LoadDataFromDatabase();
        }

        // Обробник нової кнопки!
        private void btnAdminReturn_Click(object sender, EventArgs e)
        {
            this.Close(); // Поверне нас на форму управління користувачами
        }

        private void SetupDataTableStructure()
        {
            travelTable = new DataTable("Travels");
            travelTable.Columns.Add("ID", typeof(int));
            travelTable.Columns.Add("User", typeof(string));
            travelTable.Columns.Add("Country", typeof(string));
            travelTable.Columns.Add("City", typeof(string));
            travelTable.Columns.Add("Budget", typeof(decimal));
            travelTable.Columns.Add("Status", typeof(string));
            travelTable.Columns.Add("Rating", typeof(int));
            travelTable.Columns.Add("Comment", typeof(string));

            dgvData.DataSource = travelTable;

            if (dgvData.Columns.Contains("ID"))
            {
                dgvData.Columns["ID"].Visible = false;
                dgvData.Columns["ID"].ReadOnly = true;
            }

            dgvData.EnableHeadersVisualStyles = false;
            dgvData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgvData.ColumnHeadersDefaultCellStyle.ForeColor = Color.DimGray;
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvData.ColumnHeadersHeight = 40;

            dgvData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvData.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvData.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvData.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvData.RowTemplate.Height = 35;

            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.MultiSelect = false;
            dgvData.ReadOnly = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvData.RowHeadersVisible = false;
            dgvData.AllowUserToAddRows = false;

            dgvData.SelectionChanged += DgvData_SelectionChanged;
            dgvData.DataBindingComplete += DgvData_DataBindingComplete;
            dgvData.CellValueChanged += DgvData_CellValueChanged;
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                dgvData.CellValueChanged -= DgvData_CellValueChanged;

                travelTable.Clear();
                using (SqlConnection connection = DatabaseService.GetConnection())
                {
                    string sql = "SELECT * FROM Travels";
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            travelTable.Rows.Add(
                                reader["ID"],
                                reader["User"].ToString(),
                                reader["Country"].ToString(),
                                reader["City"].ToString(),
                                Convert.ToDecimal(reader["Budget"]),
                                reader["Status"].ToString(),
                                Convert.ToInt32(reader["Rating"]),
                                reader["Comment"].ToString()
                            );
                        }
                    }
                }

                dgvData.CellValueChanged += DgvData_CellValueChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var row = dgvData.Rows[e.RowIndex];
            if (row.Cells["ID"].Value == DBNull.Value || row.Cells["ID"].Value == null) return;

            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string columnName = dgvData.Columns[e.ColumnIndex].Name;
            object newValue = row.Cells[e.ColumnIndex].Value;

            try
            {
                using (SqlConnection connection = DatabaseService.GetConnection())
                {
                    string sql = $"UPDATE Travels SET [{columnName}] = @val WHERE ID = @id";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@val", newValue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка оновлення поля '{columnName}': {ex.Message}");
                LoadDataFromDatabase();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCountry.Text) || string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("Будь ласка, вкажіть країну та місто.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseService.GetConnection())
                {
                    string sql = "INSERT INTO Travels ([User], Country, City, Budget, [Status], Rating, Comment) VALUES (@u, @co, @ci, @b, @s, 1, '')";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    string selectedUser = cmbAssignedUser.SelectedItem != null ? cmbAssignedUser.SelectedItem.ToString() : Session.Username;
                    cmd.Parameters.AddWithValue("@u", selectedUser);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                btnClear_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            try
            {
                using (SqlConnection connection = DatabaseService.GetConnection())
                {
                    string sql = "UPDATE Travels SET [User]=@u, Country=@co, City=@ci, Budget=@b, [Status]=@s WHERE ID=@id";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    string selectedUser = cmbAssignedUser.SelectedItem != null ? cmbAssignedUser.SelectedItem.ToString() : Session.Username;

                    cmd.Parameters.AddWithValue("@u", selectedUser);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Видалити запис?", "Підтвердження", MessageBoxButtons.YesNo) == DialogResult.No) return;

            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            try
            {
                using (SqlConnection connection = DatabaseService.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Travels WHERE ID = @id", connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCountry.Clear();
            txtCity.Clear();
            numBudget.Value = 0;
            if (cmbAssignedUser.Items.Count > 0) cmbAssignedUser.SelectedIndex = 0;
            dgvData.ClearSelection();
        }

        private void btnOpenMap_Click(object sender, EventArgs e)
        {
            string query = $"{txtCity.Text} {txtCountry.Text}".Trim();

            using (MapForm mapForm = new MapForm(query))
            {
                if (mapForm.ShowDialog() == DialogResult.OK)
                {
                    txtCity.Text = mapForm.SelectedCity;
                    txtCountry.Text = mapForm.SelectedCountry;
                }
            }
        }

        private void btnOpenFeed_Click(object sender, EventArgs e)
        {
            UserFeedForm feedForm = new UserFeedForm();
            this.Hide();
            feedForm.ShowDialog();
            this.Show();
            LoadDataFromDatabase();
        }

        private void pbBack_Click(object sender, EventArgs e) => this.Close();
        private void pbExit_Click(object sender, EventArgs e) => Application.Exit();

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                var row = (DataRowView)dgvData.SelectedRows[0].DataBoundItem;
                txtCountry.Text = row["Country"].ToString();
                txtCity.Text = row["City"].ToString();

                if (decimal.TryParse(row["Budget"].ToString(), out decimal budgetValue))
                {
                    numBudget.Value = budgetValue;
                }

                string statusValue = row["Status"].ToString();
                if (!string.IsNullOrEmpty(statusValue) && cmbStatus.Items.Contains(statusValue))
                {
                    cmbStatus.Text = statusValue;
                }
            }
        }

        private void DgvData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvData.ClearSelection();

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                if (status == "Запит") row.DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199);
                else if (status == "Очікує перевірки") row.DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226);
                else if (status == "Завершено") row.DefaultCellStyle.BackColor = Color.FromArgb(209, 250, 229);
            }
        }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = System.Windows.Forms.Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging) { Point dif = Point.Subtract(System.Windows.Forms.Cursor.Position, new Size(dragCursorPoint)); this.Location = Point.Add(dragFormPoint, new Size(dif)); } }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }
}