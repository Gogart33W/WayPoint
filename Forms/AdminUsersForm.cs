using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;

namespace WayPoint
{
    public partial class AdminUsersForm : Form
    {
        private DataTable adminsWorkersTable;
        private DataTable usersTable;

        private enum Mode { View, Add }
        private Mode currentMode = Mode.View;

        private bool isDirty = false;
        private int previousTabIndex = 0;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private bool isUpdatingFromGrid = false;
        private bool isPrompting = false;

        private static readonly Random _rnd = new Random();

        private string searchAdmins = string.Empty;
        private string searchUsers = string.Empty;

        public AdminUsersForm()
        {
            InitializeComponent();
            SafeWireEvents();
        }

        #region Session

        private static string GetSessionUsername()
        {
            try { return Session.Username ?? string.Empty; }
            catch { return string.Empty; }
        }

        #endregion

        #region Wiring

        private void SafeWireEvents()
        {
            if (dgvAdminsWorkers != null)
            {
                dgvAdminsWorkers.SelectionChanged -= DgvAdminsWorkers_SelectionChanged;
                dgvAdminsWorkers.SelectionChanged += DgvAdminsWorkers_SelectionChanged;
                dgvAdminsWorkers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAdminsWorkers.MultiSelect = false;
                dgvAdminsWorkers.ReadOnly = true;
            }

            if (dgvUsersOnly != null)
            {
                dgvUsersOnly.SelectionChanged -= DgvUsersOnly_SelectionChanged;
                dgvUsersOnly.SelectionChanged += DgvUsersOnly_SelectionChanged;
                dgvUsersOnly.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvUsersOnly.MultiSelect = false;
                dgvUsersOnly.ReadOnly = true;
            }

            if (tabControl != null)
            {
                tabControl.SelectedIndexChanged -= TabControl_SelectedIndexChanged;
                tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
                try { previousTabIndex = tabControl.SelectedIndex; } catch { previousTabIndex = 0; }
            }

            if (txtUsername != null) { txtUsername.TextChanged -= TxtFields_Changed; txtUsername.TextChanged += TxtFields_Changed; }
            if (txtEmail != null) { txtEmail.TextChanged -= TxtFields_Changed; txtEmail.TextChanged += TxtFields_Changed; }
            if (cmbRole != null) { cmbRole.SelectedIndexChanged -= CmbRole_Changed; cmbRole.SelectedIndexChanged += CmbRole_Changed; }

            if (txtSearchAdmins != null) { txtSearchAdmins.TextChanged -= TxtSearchAdmins_Changed; txtSearchAdmins.TextChanged += TxtSearchAdmins_Changed; }
            if (txtSearchUsers != null) { txtSearchUsers.TextChanged -= TxtSearchUsers_Changed; txtSearchUsers.TextChanged += TxtSearchUsers_Changed; }

            if (btnNewAccount != null) { btnNewAccount.Click -= btnNewAccount_Click; btnNewAccount.Click += btnNewAccount_Click; }
            if (btnSave != null) { btnSave.Click -= btnSave_Click; btnSave.Click += btnSave_Click; }
            if (btnDelete != null) { btnDelete.Click -= btnDelete_Click; btnDelete.Click += btnDelete_Click; }
            if (btnOpenWork != null) { btnOpenWork.Click -= btnOpenWork_Click; btnOpenWork.Click += btnOpenWork_Click; }
            if (btnOpenFeed != null) { btnOpenFeed.Click -= btnOpenFeed_Click; btnOpenFeed.Click += btnOpenFeed_Click; }
            if (btnOpenAnalytics != null) { btnOpenAnalytics.Click -= btnOpenAnalytics_Click; btnOpenAnalytics.Click += btnOpenAnalytics_Click; }

            // КНОПКА НАЛАШТУВАНЬ
            if (btnSettings != null) { btnSettings.Click -= btnSettings_Click; btnSettings.Click += btnSettings_Click; }

            if (lblBack != null) { lblBack.Click -= btnBack_Click; lblBack.Click += btnBack_Click; }

            if (pnlHeader != null)
            {
                pnlHeader.MouseDown -= pnlHeader_MouseDown;
                pnlHeader.MouseMove -= pnlHeader_MouseMove;
                pnlHeader.MouseUp -= pnlHeader_MouseUp;
                pnlHeader.MouseDown += pnlHeader_MouseDown;
                pnlHeader.MouseMove += pnlHeader_MouseMove;
                pnlHeader.MouseUp += pnlHeader_MouseUp;
            }

            this.Load -= AdminUsersForm_Load;
            this.Load += AdminUsersForm_Load;
        }

        private void TxtFields_Changed(object sender, EventArgs e) { if (currentMode == Mode.Add) isDirty = true; }
        private void CmbRole_Changed(object sender, EventArgs e) { if (currentMode == Mode.Add) isDirty = true; }

        private void TxtSearchAdmins_Changed(object sender, EventArgs e)
        {
            if (txtSearchAdmins?.ForeColor == Color.Gray) return;
            searchAdmins = (txtSearchAdmins?.Text ?? string.Empty).Trim();
            ApplyFilter(dgvAdminsWorkers, adminsWorkersTable, searchAdmins);
        }

        private void TxtSearchUsers_Changed(object sender, EventArgs e)
        {
            if (txtSearchUsers?.ForeColor == Color.Gray) return;
            searchUsers = (txtSearchUsers?.Text ?? string.Empty).Trim();
            ApplyFilter(dgvUsersOnly, usersTable, searchUsers);
        }

        #endregion

        #region Load

        private void AdminUsersForm_Load(object sender, EventArgs e)
        {
            try
            {
                SoundHelper.AttachSounds(this);
                LoadData();
                EnterViewMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка ініціалізації: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Data Loading

        private void LoadData()
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    if (dgvAdminsWorkers != null)
                    {
                        SuspendGridEvents(dgvAdminsWorkers);
                        var dt = new DataTable();
                        using (var da = new SqlDataAdapter("SELECT ID, Username, Email, Role FROM Users WHERE Role IN ('Admin', 'Manager')", conn))
                            da.Fill(dt);
                        adminsWorkersTable = dt;
                        ApplyFilter(dgvAdminsWorkers, adminsWorkersTable, searchAdmins);
                        dgvAdminsWorkers.ClearSelection();
                        ResumeGridEvents(dgvAdminsWorkers);
                    }

                    if (dgvUsersOnly != null)
                    {
                        SuspendGridEvents(dgvUsersOnly);
                        var dt2 = new DataTable();
                        using (var da2 = new SqlDataAdapter("SELECT ID, Username, Email FROM Users WHERE Role = 'User'", conn))
                            da2.Fill(dt2);
                        usersTable = dt2;
                        ApplyFilter(dgvUsersOnly, usersTable, searchUsers);
                        dgvUsersOnly.ClearSelection();
                        ResumeGridEvents(dgvUsersOnly);
                    }
                }

                ClearSidebar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter(DataGridView dgv, DataTable source, string search)
        {
            if (dgv == null || source == null) return;

            SuspendGridEvents(dgv);
            try
            {
                DataTable result;
                if (string.IsNullOrEmpty(search))
                {
                    result = source;
                }
                else
                {
                    var rows = source.AsEnumerable()
                        .Where(r => (r["Username"]?.ToString() ?? string.Empty)
                            .IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                    result = rows.Any() ? rows.CopyToDataTable() : source.Clone();
                }

                dgv.DataSource = result;
                dgv.Refresh();
                SetupGridColumns(dgv);
                dgv.ClearSelection();
            }
            catch { }
            finally
            {
                ResumeGridEvents(dgv);
            }
        }

        private void SetupGridColumns(DataGridView dgv)
        {
            if (dgv?.Columns == null) return;

            foreach (DataGridViewColumn c in dgv.Columns)
            {
                if (c == null) continue;

                if (string.Equals(c.Name, "ID", StringComparison.OrdinalIgnoreCase))
                {
                    c.Visible = false;
                }
                else
                {
                    c.Visible = true;
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    c.FillWeight = 100f;
                }
            }

            if (dgv.Columns.Contains("Username")) dgv.Columns["Username"].HeaderText = "Логін";
            if (dgv.Columns.Contains("Email")) dgv.Columns["Email"].HeaderText = "Електронна пошта";
            if (dgv.Columns.Contains("Role")) dgv.Columns["Role"].HeaderText = "Роль";
        }

        #endregion

        #region Grid event suspend/resume

        private void SuspendGridEvents(DataGridView dgv)
        {
            if (dgv == null) return;
            isUpdatingFromGrid = true;
            if (dgv == dgvAdminsWorkers) dgv.SelectionChanged -= DgvAdminsWorkers_SelectionChanged;
            else if (dgv == dgvUsersOnly) dgv.SelectionChanged -= DgvUsersOnly_SelectionChanged;
        }

        private void ResumeGridEvents(DataGridView dgv)
        {
            if (dgv == null) return;
            if (dgv == dgvAdminsWorkers)
            {
                dgv.SelectionChanged -= DgvAdminsWorkers_SelectionChanged;
                dgv.SelectionChanged += DgvAdminsWorkers_SelectionChanged;
            }
            else if (dgv == dgvUsersOnly)
            {
                dgv.SelectionChanged -= DgvUsersOnly_SelectionChanged;
                dgv.SelectionChanged += DgvUsersOnly_SelectionChanged;
            }
            isUpdatingFromGrid = false;
        }

        #endregion

        #region Sidebar

        private void ClearSidebar()
        {
            if (txtUsername != null) txtUsername.Text = string.Empty;
            if (txtEmail != null) txtEmail.Text = string.Empty;
            if (cmbRole != null) { cmbRole.SelectedIndex = -1; cmbRole.Enabled = false; }
            isDirty = false;
        }

        private void SetSidebarEnabled(bool enabled)
        {
            if (txtUsername != null) txtUsername.Enabled = enabled;
            if (txtEmail != null) txtEmail.Enabled = enabled;
            if (cmbRole != null) cmbRole.Enabled = enabled;
        }

        private void UpdateSidebarState()
        {
            if (currentMode == Mode.Add)
            {
                SetSidebarEnabled(true);
                if (btnSave != null) btnSave.Enabled = true;
                if (btnDelete != null) btnDelete.Enabled = true;
                return;
            }

            bool hasSelection = (dgvAdminsWorkers?.SelectedRows?.Count > 0) || (dgvUsersOnly?.SelectedRows?.Count > 0);
            SetSidebarEnabled(hasSelection);

            if (btnSave != null) btnSave.Enabled = hasSelection;
            if (btnDelete != null) btnDelete.Enabled = hasSelection;
            if (!hasSelection) ClearSidebar();
        }

        private void EnterViewMode()
        {
            currentMode = Mode.View;
            isDirty = false;
            UpdateSidebarState();
            if (btnSave != null) btnSave.Text = "💾 Зберегти зміни";
            if (btnDelete != null) btnDelete.Text = "🗑 Видалити акаунт";
            if (btnNewAccount != null) { btnNewAccount.Enabled = true; btnNewAccount.Visible = true; }
            UpdateFeedButton();
        }

        private void EnterAddMode()
        {
            if (currentMode == Mode.Add) { txtUsername?.Focus(); return; }
            currentMode = Mode.Add;
            isDirty = false;
            ClearGridSelectionSilently();
            ClearSidebar();
            SetSidebarEnabled(true);

            if (btnSave != null) btnSave.Enabled = true;
            if (btnDelete != null) btnDelete.Enabled = true;

            if (btnSave != null) btnSave.Text = "✅ Підтвердити";
            if (btnDelete != null) btnDelete.Text = "❌ Скасувати";
            if (btnNewAccount != null) { btnNewAccount.Enabled = false; btnNewAccount.Visible = false; }
            UpdateFeedButton();
            txtUsername?.Focus();
        }

        private void ExitAddMode(bool cancelled)
        {
            currentMode = Mode.View;
            isDirty = false;
            if (btnSave != null) btnSave.Text = "💾 Зберегти зміни";
            if (btnDelete != null) btnDelete.Text = "🗑 Видалити акаунт";
            if (btnNewAccount != null) { btnNewAccount.Enabled = true; btnNewAccount.Visible = true; }
            if (cancelled) ClearSidebar();
            UpdateSidebarState();
            UpdateFeedButton();
        }

        private void UpdateFeedButton()
        {
            if (btnOpenFeed != null) btnOpenFeed.Enabled = true;
            if (btnOpenWork != null) btnOpenWork.Enabled = true;
        }

        private void ClearGridSelectionSilently()
        {
            SuspendGridEvents(dgvAdminsWorkers);
            SuspendGridEvents(dgvUsersOnly);
            dgvAdminsWorkers?.ClearSelection();
            dgvUsersOnly?.ClearSelection();
            ResumeGridEvents(dgvAdminsWorkers);
            ResumeGridEvents(dgvUsersOnly);
        }

        #endregion

        #region Selection handlers

        private void HandleGridSelectionChanged(DataGridView dgv)
        {
            if (isUpdatingFromGrid || dgv == null) return;

            if (currentMode == Mode.Add)
            {
                if (isPrompting) return;
                isPrompting = true;
                var res = MessageBox.Show(
                    "Ви зараз створюєте нового користувача. Всі незбережені дані будуть втрачені. Продовжити?",
                    "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                isPrompting = false;
                if (res == DialogResult.No)
                {
                    SuspendGridEvents(dgv);
                    dgv.ClearSelection();
                    ResumeGridEvents(dgv);
                    return;
                }
                isUpdatingFromGrid = true;
                ExitAddMode(cancelled: true);
                isUpdatingFromGrid = false;
            }

            if (dgv.SelectedRows == null || dgv.SelectedRows.Count == 0 || currentMode != Mode.View)
            {
                UpdateSidebarState();
                return;
            }

            var row = dgv.SelectedRows[0];
            if (row?.DataGridView == null) { UpdateSidebarState(); return; }

            isUpdatingFromGrid = true;
            try
            {
                if (txtUsername != null) txtUsername.Text = GetCellValue(row, "Username");
                if (txtEmail != null) txtEmail.Text = GetCellValue(row, "Email");

                string role = "User";
                if (row.DataGridView.Columns.Contains("Role"))
                {
                    role = GetCellValue(row, "Role");
                }

                string selectedUsername = GetCellValue(row, "Username");
                if (cmbRole != null)
                {
                    cmbRole.Text = role;
                    cmbRole.Enabled = !string.Equals(selectedUsername, GetSessionUsername(), StringComparison.OrdinalIgnoreCase);
                }
            }
            finally { isUpdatingFromGrid = false; }

            UpdateSidebarState();
        }

        private void DgvAdminsWorkers_SelectionChanged(object sender, EventArgs e) => HandleGridSelectionChanged(dgvAdminsWorkers);
        private void DgvUsersOnly_SelectionChanged(object sender, EventArgs e) => HandleGridSelectionChanged(dgvUsersOnly);

        #endregion

        #region Tab change

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl == null) return;

            if (currentMode == Mode.Add && isDirty)
            {
                if (isPrompting) { tabControl.SelectedIndex = previousTabIndex; return; }
                isPrompting = true;
                var res = MessageBox.Show(
                    "Ви зараз створюєте нового користувача. Всі незбережені дані будуть втрачені при переході. Продовжити?",
                    "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                isPrompting = false;
                if (res == DialogResult.No) { tabControl.SelectedIndex = previousTabIndex; return; }
                isUpdatingFromGrid = true;
                ExitAddMode(cancelled: true);
                isUpdatingFromGrid = false;
            }

            ClearGridSelectionSilently();
            ClearSidebar();
            UpdateSidebarState();
            UpdateFeedButton();
            try { previousTabIndex = tabControl.SelectedIndex; } catch { previousTabIndex = 0; }
        }

        #endregion

        #region Buttons

        private void btnNewAccount_Click(object sender, EventArgs e) => EnterAddMode();

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Add) CreateNewAccount();
            else UpdateSelectedUser();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Add)
            {
                if (isDirty)
                {
                    var res = MessageBox.Show("Ви маєте незбережені дані. Скасувати створення і втратити їх?",
                        "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.No) return;
                }
                ExitAddMode(cancelled: true);
                return;
            }
            DeleteSelectedUser();
        }

        private void btnOpenWork_Click(object sender, EventArgs e)
        {
            string username = GetSelectedUsername();
            string role = GetSelectedRole();

            if (string.IsNullOrEmpty(username))
            {
                username = GetSessionUsername();
                role = Session.Role ?? "Admin";
            }

            string savedUsername = Session.Username;
            string savedRole = Session.Role;
            try
            {
                Session.Username = username;
                Session.Role = role;
                this.Hide();
                using (var form = new MainWork())
                    form.ShowDialog();
                this.Show();
            }
            finally
            {
                Session.Username = savedUsername;
                Session.Role = savedRole;
            }
        }

        private void btnOpenFeed_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var form = new UserFeedForm())
                form.ShowDialog();
            this.Show();
            isUpdatingFromGrid = true;
            LoadData();
            isUpdatingFromGrid = false;
        }

        private void btnOpenAnalytics_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var form = new Forms.AdminAnalyticsForm())
            {
                form.ShowDialog();
            }
            this.Show();
        }

        // ОБРОБНИК КНОПКИ НАЛАШТУВАНЬ
        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var sf = new Forms.SettingsForm())
            {
                sf.ShowDialog();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Add && isDirty)
            {
                var res = MessageBox.Show("Ви маєте незбережені дані. Вийти і втратити їх?",
                    "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No) return;
            }
            this.Close();
        }

        #endregion

        #region CRUD

        private void CreateNewAccount()
        {
            string newUsername = (txtUsername?.Text ?? string.Empty).Trim();
            string newEmail = (txtEmail?.Text ?? string.Empty).Trim();
            string newRole = (cmbRole?.Text ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newEmail) || string.IsNullOrEmpty(newRole))
            {
                MessageBox.Show("Заповніть Логін, Email та Роль для створення акаунта.",
                    "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    using (var checkCmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Username=@u OR Email=@e", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@u", newUsername);
                        checkCmd.Parameters.AddWithValue("@e", newEmail);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Користувач з таким логіном або email вже існує.",
                                "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string tempPassword = "WP" + GenerateVerificationCode();
                    string hash = BCrypt.Net.BCrypt.HashPassword(tempPassword);

                    using (var cmd = new SqlCommand(
                        "INSERT INTO Users (Username, PasswordHash, Role, Email, IsEmailVerified, VerificationCode, CodeExpiry) " +
                        "VALUES (@u, @p, @r, @e, 1, NULL, NULL)", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", newUsername);
                        cmd.Parameters.AddWithValue("@p", hash);
                        cmd.Parameters.AddWithValue("@r", newRole);
                        cmd.Parameters.AddWithValue("@e", newEmail);
                        cmd.ExecuteNonQuery();
                    }

                    EmailService.SendPasswordReset(newEmail, tempPassword);
                }

                isDirty = false;
                ExitAddMode(cancelled: false);
                MessageBox.Show("Новий акаунт створено. Пароль відправлено на пошту.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка створення акаунта: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSelectedUser()
        {
            DataGridViewRow selectedRow = null;
            bool isAdminWorker = false;

            if (dgvAdminsWorkers?.SelectedRows?.Count > 0) { selectedRow = dgvAdminsWorkers.SelectedRows[0]; isAdminWorker = true; }
            else if (dgvUsersOnly?.SelectedRows?.Count > 0) selectedRow = dgvUsersOnly.SelectedRows[0];

            if (selectedRow == null) { MessageBox.Show("Оберіть користувача для оновлення.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (selectedRow.DataGridView == null) { MessageBox.Show("Не вдалося отримати дані рядка. Спробуйте знову.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int id = GetRowId(selectedRow);
            if (id <= 0) { MessageBox.Show("Не вдалося визначити ID користувача.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            string newUsername = (txtUsername?.Text ?? string.Empty).Trim();
            string newEmail = (txtEmail?.Text ?? string.Empty).Trim();
            string newRole = (cmbRole?.Text ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newEmail))
            {
                MessageBox.Show("Логін та Email обов'язкові.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedUsername = GetCellValue(selectedRow, "Username");
            string oldRole = "User";
            if (selectedRow.DataGridView.Columns.Contains("Role"))
            {
                oldRole = GetCellValue(selectedRow, "Role");
            }

            if (string.Equals(selectedUsername, GetSessionUsername(), StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals(oldRole, newRole, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Ви не можете змінити роль власного акаунта.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (cmbRole != null) cmbRole.Text = oldRole;
                    return;
                }
            }

            if (!isAdminWorker && string.IsNullOrEmpty(newRole)) newRole = "User";

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    using (var checkCmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE (Username=@u OR Email=@e) AND ID<>@id", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@u", newUsername);
                        checkCmd.Parameters.AddWithValue("@e", newEmail);
                        checkCmd.Parameters.AddWithValue("@id", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Користувач з таким логіном або email вже існує.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    using (var cmd = new SqlCommand("UPDATE Users SET Username=@u, Email=@e, Role=@r WHERE ID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@u", newUsername);
                        cmd.Parameters.AddWithValue("@e", newEmail);
                        cmd.Parameters.AddWithValue("@r", newRole);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Дані оновлено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка оновлення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteSelectedUser()
        {
            DataGridViewRow selectedRow = null;
            if (dgvAdminsWorkers?.SelectedRows?.Count > 0) selectedRow = dgvAdminsWorkers.SelectedRows[0];
            else if (dgvUsersOnly?.SelectedRows?.Count > 0) selectedRow = dgvUsersOnly.SelectedRows[0];

            if (selectedRow?.DataGridView == null) return;

            string username = GetCellValue(selectedRow, "Username");
            if (string.Equals(username, GetSessionUsername(), StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Ви не можете видалити власний акаунт!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Ви дійсно хочете назавжди видалити користувача '{username}'?",
                    "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            int id = GetRowId(selectedRow);
            if (id <= 0) { MessageBox.Show("Не вдалося визначити ID користувача.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    using (var cmd = new SqlCommand("DELETE FROM Users WHERE ID=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка видалення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helpers

        private string GetSelectedUsername()
        {
            if (dgvAdminsWorkers?.SelectedRows?.Count > 0) return GetCellValue(dgvAdminsWorkers.SelectedRows[0], "Username");
            if (dgvUsersOnly?.SelectedRows?.Count > 0) return GetCellValue(dgvUsersOnly.SelectedRows[0], "Username");
            return string.Empty;
        }

        private string GetSelectedRole()
        {
            if (dgvAdminsWorkers?.SelectedRows?.Count > 0)
            {
                var row = dgvAdminsWorkers.SelectedRows[0];
                if (row.DataGridView.Columns.Contains("Role")) return GetCellValue(row, "Role");
            }
            return "User";
        }

        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            if (row?.DataGridView?.Columns == null) return string.Empty;
            if (!row.DataGridView.Columns.Contains(columnName)) return string.Empty;
            var cell = row.Cells[row.DataGridView.Columns[columnName].Index];
            return cell?.Value?.ToString() ?? string.Empty;
        }

        private int GetRowId(DataGridViewRow row)
        {
            if (row == null) return 0;
            if (row.DataBoundItem is DataRowView drv)
            {
                var dr = drv.Row;
                if (dr.Table.Columns.Contains("ID") && dr["ID"] != DBNull.Value)
                    if (int.TryParse(dr["ID"].ToString(), out int id) && id > 0) return id;
            }
            if (row.DataGridView?.Columns != null && row.DataGridView.Columns.Contains("ID"))
            {
                var cell = row.Cells[row.DataGridView.Columns["ID"].Index];
                if (cell?.Value != null && int.TryParse(cell.Value.ToString(), out int id2) && id2 > 0) return id2;
            }
            return 0;
        }

        private string GenerateVerificationCode() => _rnd.Next(100000, 999999).ToString();

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }

        private void pnlHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && this.WindowState != FormWindowState.Maximized)
            {
                this.Location = new Point(
                    dragFormPoint.X + Cursor.Position.X - dragCursorPoint.X,
                    dragFormPoint.Y + Cursor.Position.Y - dragCursorPoint.Y
                );
            }
        }

        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;

        #endregion
    }
}