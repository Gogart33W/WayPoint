using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;

namespace WayPoint
{
    public partial class AdminUsersForm : Form
    {
        private DataTable usersTable;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public AdminUsersForm()
        {
            InitializeComponent();
        }

        private void AdminUsersForm_Load(object sender, EventArgs e)
        {
            // Налаштування для DataGrid, які складно зробити в дизайнері
            dgvUsers.CellValueChanged += DgvUsers_CellValueChanged;
            dgvUsers.CurrentCellDirtyStateChanged += (s, ev) => { if (dgvUsers.IsCurrentCellDirty) dgvUsers.CommitEdit(DataGridViewDataErrorContexts.Commit); };

            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                dgvUsers.CellValueChanged -= DgvUsers_CellValueChanged;

                using (var conn = DatabaseService.GetConnection())
                {
                    string sql = "SELECT ID, Username, Email, Role, IsEmailVerified FROM Users";
                    var cmd = new SqlCommand(sql, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    usersTable = new DataTable();
                    adapter.Fill(usersTable);

                    dgvUsers.Columns.Clear();
                    dgvUsers.DataSource = usersTable;

                    dgvUsers.Columns["ID"].Visible = false;
                    dgvUsers.Columns["Username"].ReadOnly = true;
                    dgvUsers.Columns["Email"].ReadOnly = true;
                    dgvUsers.Columns["IsEmailVerified"].ReadOnly = true;
                    dgvUsers.Columns["IsEmailVerified"].HeaderText = "Верифікація";

                    // Додаємо ComboBox для ролей
                    var roleColIndex = dgvUsers.Columns["Role"].Index;
                    dgvUsers.Columns.RemoveAt(roleColIndex);

                    var roleCombo = new DataGridViewComboBoxColumn
                    {
                        Name = "Role",
                        DataPropertyName = "Role",
                        HeaderText = "Роль (Клікни для зміни)",
                        FlatStyle = FlatStyle.Flat
                    };
                    roleCombo.Items.AddRange("User", "Moderator", "Admin");
                    dgvUsers.Columns.Insert(roleColIndex, roleCombo);
                }

                dgvUsers.CellValueChanged += DgvUsers_CellValueChanged;
            }
            catch (Exception ex) { MessageBox.Show("Помилка БД: " + ex.Message); }
        }

        // АВТОЗБЕРЕЖЕННЯ ЗМІНИ РОЛІ
        private void DgvUsers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dgvUsers.Columns[e.ColumnIndex].Name;
            if (columnName == "Role")
            {
                int id = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["ID"].Value);
                string newRole = dgvUsers.Rows[e.RowIndex].Cells["Role"].Value.ToString();
                string username = dgvUsers.Rows[e.RowIndex].Cells["Username"].Value.ToString();

                if (username == Session.Username)
                {
                    MessageBox.Show("Ви не можете змінити роль самому собі!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadUsers(); return;
                }

                try
                {
                    using (var conn = DatabaseService.GetConnection())
                    {
                        var cmd = new SqlCommand("UPDATE Users SET Role = @r WHERE ID = @id", conn);
                        cmd.Parameters.AddWithValue("@r", newRole);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show($"Користувачу {username} призначено роль: {newRole}");
                }
                catch (Exception ex) { MessageBox.Show("Помилка збереження: " + ex.Message); }
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) return;

            int id = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["ID"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

            if (username == Session.Username)
            {
                MessageBox.Show("Ви не можете видалити свій власний акаунт!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Ви впевнені, що хочете назавжди видалити користувача {username}?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = DatabaseService.GetConnection())
                    {
                        var cmdTravels = new SqlCommand("DELETE FROM Travels WHERE [User] = @u", conn);
                        cmdTravels.Parameters.AddWithValue("@u", username);
                        cmdTravels.ExecuteNonQuery();

                        var cmd = new SqlCommand("DELETE FROM Users WHERE ID = @id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    LoadUsers();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void btnAddAdmin_Click(object sender, EventArgs e)
        {
            using (var form = new CreateAdminDialog())
            {
                if (form.ShowDialog() == DialogResult.OK) LoadUsers();
            }
        }

        private void btnOpenTours_Click(object sender, EventArgs e)
        {
            MainWork toursForm = new MainWork();
            this.Hide();
            toursForm.ShowDialog();
            this.Show();
        }

        private void btnOpenFeed_Click(object sender, EventArgs e)
        {
            UserFeedForm feedForm = new UserFeedForm();
            this.Hide();
            feedForm.ShowDialog();
            this.Show();
        }

        private void btnBack_Click(object sender, EventArgs e) => this.Close();

        // Логіка перетягування форми
        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }

    // Клас для вікна реєстрації адміна залишаємо тут (бо це просто допоміжне віконце)
    public class CreateAdminDialog : Form
    {
        private TextBox txtUsername, txtEmail, txtPassword;

        public CreateAdminDialog()
        {
            this.Text = "Реєстрація нового Адміністратора";
            this.Size = new Size(350, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblU = new Label { Text = "Логін:", Location = new Point(20, 20), AutoSize = true };
            txtUsername = new TextBox { Location = new Point(20, 40), Width = 290, Font = new Font("Segoe UI", 10) };

            Label lblE = new Label { Text = "Email:", Location = new Point(20, 80), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(20, 100), Width = 290, Font = new Font("Segoe UI", 10) };

            Label lblP = new Label { Text = "Пароль:", Location = new Point(20, 140), AutoSize = true };
            txtPassword = new TextBox { Location = new Point(20, 160), Width = 290, Font = new Font("Segoe UI", 10), PasswordChar = '•' };

            Button btnCreate = new Button { Text = "Створити", Location = new Point(20, 210), Size = new Size(140, 35), BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnCreate.Click += BtnCreate_Click;

            this.Controls.AddRange(new Control[] { lblU, txtUsername, lblE, txtEmail, lblP, txtPassword, btnCreate });
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Заповніть коректно всі поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @u OR Email = @e", conn);
                    checkCmd.Parameters.AddWithValue("@u", txtUsername.Text);
                    checkCmd.Parameters.AddWithValue("@e", txtEmail.Text);
                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Такий логін або пошта вже існують!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                    string sql = "INSERT INTO Users (Username, Password, Role, Email, IsEmailVerified) VALUES (@u, @p, 'Admin', @e, 1)";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@p", hashedPassword);
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text);

                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Адміністратора успішно створено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Помилка БД: " + ex.Message); }
        }
    }
}