using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;
using WayPoint.Models;

namespace WayPoint
{
    public partial class AdminUsersForm : Form
    {
        private DataTable usersTable;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool isUpdatingFromGrid = false;

        public AdminUsersForm()
        {
            InitializeComponent();
            SetupGrid();

            // Підключаємо подію виділення рядка в таблиці
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;

            // Рух форми (на випадок виходу з повноекранного режиму)
            pnlHeader.MouseDown += pnlHeader_MouseDown;
            pnlHeader.MouseMove += pnlHeader_MouseMove;
            pnlHeader.MouseUp += pnlHeader_MouseUp;
        }

        private void SetupGrid()
        {
            usersTable = new DataTable();
            usersTable.Columns.Add("ID", typeof(int));
            usersTable.Columns.Add("Логін", typeof(string));
            usersTable.Columns.Add("Email", typeof(string));
            usersTable.Columns.Add("Роль", typeof(string));

            dgvUsers.DataSource = usersTable;
            if (dgvUsers.Columns.Contains("ID")) dgvUsers.Columns["ID"].Visible = false;
        }

        private void AdminUsersForm_Load(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                usersTable.Clear();
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT ID, Username, Email, Role FROM Users", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataRow row = usersTable.NewRow();
                            row["ID"] = reader["ID"];
                            row["Логін"] = reader["Username"].ToString();
                            row["Email"] = reader["Email"].ToString();
                            row["Роль"] = reader["Role"].ToString();
                            usersTable.Rows.Add(row);
                        }
                    }
                }
                dgvUsers.ClearSelection();
                ClearSidebar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження бази даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearSidebar()
        {
            txtUsername.Clear();
            txtEmail.Clear();
            cmbRole.SelectedIndex = -1;
        }

        // Автоматичне заповнення панелі при кліку на користувача
        private void DgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (isUpdatingFromGrid || dgvUsers.SelectedRows.Count == 0) return;

            isUpdatingFromGrid = true;
            txtUsername.Text = dgvUsers.SelectedRows[0].Cells["Логін"].Value.ToString();
            txtEmail.Text = dgvUsers.SelectedRows[0].Cells["Email"].Value.ToString();
            cmbRole.Text = dgvUsers.SelectedRows[0].Cells["Роль"].Value.ToString();
            isUpdatingFromGrid = false;
        }

        // ЗБЕРЕЖЕННЯ / ОНОВЛЕННЯ ПРОФІЛЮ
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть користувача зі списку для оновлення!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["ID"].Value);
            string newUsername = txtUsername.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string newRole = cmbRole.Text;

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newEmail) || string.IsNullOrEmpty(newRole))
            {
                MessageBox.Show("Всі поля повинні бути заповнені!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("UPDATE Users SET Username=@u, Email=@e, Role=@r WHERE ID=@id", conn);
                    cmd.Parameters.AddWithValue("@u", newUsername);
                    cmd.Parameters.AddWithValue("@e", newEmail);
                    cmd.Parameters.AddWithValue("@r", newRole);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Дані користувача успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка оновлення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ВИДАЛЕННЯ КОРИСТУВАЧА
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            string username = dgvUsers.SelectedRows[0].Cells["Логін"].Value.ToString();

            // Захист від видалення самого себе
            if (username == Session.Username)
            {
                MessageBox.Show("Ви не можете видалити власний акаунт!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Ви дійсно хочете назавжди видалити користувача '{username}'?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["ID"].Value);
                try
                {
                    using (var conn = DatabaseService.GetConnection())
                    {
                        var cmd = new SqlCommand("DELETE FROM Users WHERE ID=@id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    LoadDataFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка видалення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ПОВЕРНЕННЯ НАЗАД
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // МЕТОДИ РУХУ ФОРМИ
        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging && this.WindowState != FormWindowState.Maximized) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); this.Location = Point.Add(dragFormPoint, new Size(dif)); } }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }
}