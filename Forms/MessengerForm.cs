using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WayPoint.Services;

namespace WayPoint
{
    public partial class MessengerForm : Form
    {
        private string selectedContact = "";
        private System.Windows.Forms.Timer chatTimer;

        private bool dragging = false;
        private Point dragCursorPoint = Point.Empty;
        private Point dragFormPoint = Point.Empty;

        private Dictionary<int, Panel> renderedMessages = new Dictionary<int, Panel>();

        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public MessengerForm()
        {
            InitializeComponent();

            typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, flowChat, new object[] { true });
            typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, flowContacts, new object[] { true });

            chatTimer = new System.Windows.Forms.Timer();
            chatTimer.Interval = 3000;
            chatTimer.Tick += (s, e) => {
                if (Session.Role != "User") LoadContacts();
                LoadChatHistory();
            };
        }

        private void MessengerForm_Load(object sender, EventArgs e)
        {
            SoundHelper.AttachSounds(this); // ПІДКЛЮЧЕННЯ ЗВУКІВ

            if (Session.Role == "User")
            {
                lblContacts.Visible = false;
                flowContacts.Visible = false;
                txtSearchContacts.Visible = false;
                flowChat.Left = 20;
                flowChat.Width = this.Width - 40;
                txtMessage.Left = 20;
                txtMessage.Width = this.Width - 180;

                selectedContact = Session.Username;
                lblHeaderTitle.Text = "Підтримка WayPoint";

                LoadChatHistory();
                chatTimer.Start();
            }
            else
            {
                lblHeaderTitle.Text = $"Робоче місце: {Session.Username}";
                LoadContacts();
                chatTimer.Start();
            }
        }

        private void txtSearchContacts_TextChanged(object sender, EventArgs e)
        {
            LoadContacts(); // Перезавантажує список при кожному введеному символі
        }

        private void LoadContacts()
        {
            if (Session.Role == "User") return;

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    // ФІЛЬТР: Показує тільки тих клієнтів, які хоча б раз писали АБО яким писали, ПЛЮС працює пошук!
                    string sql = @"
                        SELECT u.Username, 
                               (SELECT COUNT(*) FROM Messages WHERE SenderUsername = u.Username AND ReceiverUsername = 'Support' AND IsRead = 0) AS Unread
                        FROM Users u
                        WHERE u.Role = 'User' 
                          AND EXISTS (SELECT 1 FROM Messages WHERE SenderUsername = u.Username OR ReceiverUsername = u.Username)
                          AND u.Username LIKE @search
                        ORDER BY Unread DESC, u.Username ASC"; // Спочатку непрочитані, потім за алфавітом

                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearchContacts.Text.Trim() + "%");

                    List<string> currentUsers = new List<string>();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string username = reader["Username"].ToString();
                            int unread = Convert.ToInt32(reader["Unread"]);
                            currentUsers.Add(username);

                            Button btn = null;
                            foreach (Control c in flowContacts.Controls)
                            {
                                if (c.Name == "btn_" + username) { btn = (Button)c; break; }
                            }

                            if (btn == null)
                            {
                                btn = new Button();
                                btn.Name = "btn_" + username;
                                btn.Width = flowContacts.Width - 25;
                                btn.Height = 45;
                                btn.FlatStyle = FlatStyle.Flat;
                                btn.FlatAppearance.BorderSize = 0;
                                btn.TextAlign = ContentAlignment.MiddleLeft;
                                btn.Cursor = Cursors.Hand;

                                btn.Click += (s, e) => {
                                    SoundHelper.PlayClick();
                                    selectedContact = username;
                                    lblHeaderTitle.Text = $"Чат з клієнтом: {username}";
                                    flowChat.Controls.Clear();
                                    renderedMessages.Clear();
                                    LoadContacts();
                                    LoadChatHistory();
                                };
                                flowContacts.Controls.Add(btn);
                            }

                            btn.Text = unread > 0 ? $"{username} ({unread})" : username;
                            btn.BackColor = (selectedContact == username) ? Color.FromArgb(79, 70, 229) : Color.FromArgb(30, 41, 59);
                            btn.ForeColor = unread > 0 ? Color.Gold : Color.White;
                            btn.Font = new Font("Segoe UI", 12, unread > 0 ? FontStyle.Bold : FontStyle.Regular);
                        }
                    }

                    // Видалення кнопок клієнтів, які не підпадають під пошук
                    List<Control> toRemove = new List<Control>();
                    foreach (Control c in flowContacts.Controls)
                    {
                        if (!currentUsers.Contains(c.Name.Substring(4))) toRemove.Add(c);
                    }
                    foreach (var c in toRemove) flowContacts.Controls.Remove(c);
                }
            }
            catch { }
        }

        private void LoadChatHistory()
        {
            if (string.IsNullOrEmpty(selectedContact)) return;

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    if (Session.Role == "User")
                        new SqlCommand($"UPDATE Messages SET IsRead=1 WHERE ReceiverUsername='{Session.Username}' AND IsRead=0", conn).ExecuteNonQuery();
                    else
                        new SqlCommand($"UPDATE Messages SET IsRead=1 WHERE SenderUsername='{selectedContact}' AND ReceiverUsername='Support' AND IsRead=0", conn).ExecuteNonQuery();

                    string sql = "";
                    if (Session.Role == "User")
                    {
                        sql = @"SELECT ID, SenderUsername, MessageText, SentAt FROM Messages 
                                WHERE (SenderUsername=@me AND ReceiverUsername='Support') 
                                   OR (ReceiverUsername=@me) 
                                ORDER BY SentAt ASC";
                    }
                    else
                    {
                        sql = @"SELECT ID, SenderUsername, MessageText, SentAt FROM Messages 
                                WHERE (SenderUsername=@them AND ReceiverUsername='Support') 
                                   OR (ReceiverUsername=@them) 
                                ORDER BY SentAt ASC";
                    }

                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@me", Session.Username);
                    cmd.Parameters.AddWithValue("@them", selectedContact);

                    List<int> currentDbIds = new List<int>();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            string sender = reader["SenderUsername"].ToString();
                            string dbText = reader["MessageText"].ToString();
                            DateTime time = Convert.ToDateTime(reader["SentAt"]);

                            currentDbIds.Add(id);

                            if (!renderedMessages.ContainsKey(id))
                            {
                                Panel bubble = CreateBubble(id, sender, dbText, time);
                                renderedMessages[id] = bubble;
                                flowChat.Controls.Add(bubble);
                                flowChat.ScrollControlIntoView(bubble);
                            }
                            else
                            {
                                Panel existingBubble = renderedMessages[id];
                                Label lblText = (Label)existingBubble.Controls.Find("lblText", true)[0];

                                bool newIsEdited = dbText.EndsWith(" (змінено)");
                                string newCleanText = newIsEdited ? dbText.Substring(0, dbText.Length - 10) : dbText;

                                if (lblText.Text != newCleanText)
                                {
                                    int index = flowChat.Controls.GetChildIndex(existingBubble);
                                    flowChat.Controls.Remove(existingBubble);

                                    Panel freshBubble = CreateBubble(id, sender, dbText, time);
                                    renderedMessages[id] = freshBubble;
                                    flowChat.Controls.Add(freshBubble);
                                    flowChat.Controls.SetChildIndex(freshBubble, index);
                                }
                            }
                        }
                    }

                    List<int> toRemove = new List<int>();
                    foreach (var id in renderedMessages.Keys) { if (!currentDbIds.Contains(id)) toRemove.Add(id); }
                    foreach (var id in toRemove) { flowChat.Controls.Remove(renderedMessages[id]); renderedMessages.Remove(id); }
                }
            }
            catch { }
        }

        private Panel CreateBubble(int msgId, string sender, string dbText, DateTime time)
        {
            bool isMine = (sender == Session.Username);

            bool isEdited = dbText.EndsWith(" (змінено)");
            string text = isEdited ? dbText.Substring(0, dbText.Length - 10) : dbText;

            string displayName = "";
            if (isMine) displayName = "Ви";
            else if (Session.Role == "User") displayName = $"Підтримка ({sender})";
            else if (sender == selectedContact) displayName = sender;
            else displayName = $"Колега ({sender})";

            Panel pnlRow = new Panel();
            pnlRow.Width = flowChat.ClientSize.Width - 25;
            pnlRow.AutoSize = true;
            pnlRow.Padding = new Padding(10, 5, 10, 5);

            Panel pnlBubble = new Panel();
            pnlBubble.AutoSize = true;
            pnlBubble.MaximumSize = new Size(pnlRow.Width - 250, 0);
            pnlBubble.BackColor = isMine ? Color.FromArgb(79, 70, 229) : Color.FromArgb(51, 65, 85);
            pnlBubble.Padding = new Padding(15, 10, 15, 10);

            pnlBubble.Resize += (s, e) => {
                pnlBubble.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, pnlBubble.Width, pnlBubble.Height, 18, 18));
            };

            Label lblName = new Label();
            lblName.Text = displayName;
            lblName.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblName.ForeColor = isMine ? Color.FromArgb(199, 210, 254) : Color.FromArgb(148, 163, 184);
            lblName.AutoSize = true;
            lblName.Location = new Point(15, 8);
            pnlBubble.Controls.Add(lblName);

            Label lblText = new Label();
            lblText.Name = "lblText";
            lblText.Text = text;
            lblText.Font = new Font("Segoe UI", 12);
            lblText.ForeColor = Color.White;
            lblText.AutoSize = true;
            lblText.MaximumSize = new Size(pnlBubble.MaximumSize.Width - 30, 0);
            lblText.Location = new Point(15, 30);
            pnlBubble.Controls.Add(lblText);

            Panel pnlBottom = new Panel();
            pnlBottom.Height = 22;
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.BackColor = Color.Transparent;

            Label lblTime = new Label();
            lblTime.Text = time.ToString("HH:mm");
            lblTime.Font = new Font("Segoe UI", 8);
            lblTime.ForeColor = Color.FromArgb(200, 200, 200);
            lblTime.AutoSize = true;
            lblTime.Dock = DockStyle.Right;
            lblTime.Padding = new Padding(0, 5, 0, 0);
            pnlBottom.Controls.Add(lblTime);

            if (isEdited)
            {
                Label lblEdited = new Label();
                lblEdited.Text = "змінено";
                lblEdited.Font = new Font("Segoe UI", 8, FontStyle.Italic);
                lblEdited.ForeColor = Color.FromArgb(160, 175, 192);
                lblEdited.AutoSize = true;
                lblEdited.Dock = DockStyle.Right;
                lblEdited.Padding = new Padding(0, 5, 8, 0);
                pnlBottom.Controls.Add(lblEdited);
            }

            pnlBubble.Controls.Add(pnlBottom);

            if (isMine)
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                var editItem = menu.Items.Add("✏️ Редагувати");
                var delItem = menu.Items.Add("🗑 Видалити");

                editItem.Click += (s, e) => EditMessage(msgId, lblText.Text);
                delItem.Click += (s, e) => DeleteMessage(msgId);

                pnlBubble.ContextMenuStrip = menu;
                lblText.ContextMenuStrip = menu;
            }

            if (isMine)
            {
                pnlRow.RightToLeft = RightToLeft.Yes;
                pnlBubble.RightToLeft = RightToLeft.No;
            }

            pnlRow.Controls.Add(pnlBubble);
            return pnlRow;
        }

        private void EditMessage(int id, string oldText)
        {
            string rawText = oldText.EndsWith(" (змінено)") ? oldText.Substring(0, oldText.Length - 10) : oldText;
            string newText = MessengerPrompt.ShowDialog("Редагувати повідомлення:", "Оновлення", rawText);

            if (!string.IsNullOrWhiteSpace(newText) && newText != rawText)
            {
                try
                {
                    using (var conn = DatabaseService.GetConnection())
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        var cmd = new SqlCommand("UPDATE Messages SET MessageText = @t WHERE ID = @id", conn);
                        cmd.Parameters.AddWithValue("@t", newText + " (змінено)");
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                    LoadChatHistory();
                }
                catch { }
            }
        }

        private void DeleteMessage(int id)
        {
            if (MessageBox.Show("Точно видалити це повідомлення?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (var conn = DatabaseService.GetConnection())
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();
                        new SqlCommand($"DELETE FROM Messages WHERE ID = {id}", conn).ExecuteNonQuery();
                    }
                    LoadChatHistory();
                }
                catch { }
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (Session.Role != "User" && string.IsNullOrEmpty(selectedContact))
            {
                MessageBox.Show("Оберіть чат для відправки!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string text = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    string receiver = Session.Role == "User" ? "Support" : selectedContact;

                    var cmd = new SqlCommand("INSERT INTO Messages (SenderUsername, ReceiverUsername, MessageText, IsRead) VALUES (@s, @r, @t, 0)", conn);
                    cmd.Parameters.AddWithValue("@s", Session.Username);
                    cmd.Parameters.AddWithValue("@r", receiver);
                    cmd.Parameters.AddWithValue("@t", text);
                    cmd.ExecuteNonQuery();
                }

                txtMessage.Clear();
                txtMessage.Focus();
                LoadChatHistory();
            }
            catch (Exception ex) { MessageBox.Show("Помилка відправки: " + ex.Message); }
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; e.SuppressKeyPress = true;
                BtnSend_Click(this, EventArgs.Empty);
            }
        }

        private void lblBack_Click(object sender, EventArgs e) => this.Close();
        private void lblExit_Click(object sender, EventArgs e) => this.Close();
        private void lblExit_MouseEnter(object sender, EventArgs e) => lblExit.ForeColor = Color.Crimson;
        private void lblExit_MouseLeave(object sender, EventArgs e) => lblExit.ForeColor = Color.LightGray;

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e) { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e) { if (dragging && this.WindowState != FormWindowState.Maximized) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (chatTimer != null) { chatTimer.Stop(); chatTimer.Dispose(); }
            base.OnFormClosed(e);
        }
    }

    public static class MessengerPrompt
    {
        public static string ShowDialog(string text, string caption, string defaultValue = "")
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.White
            };

            prompt.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, prompt.ClientRectangle, Color.FromArgb(79, 70, 229), ButtonBorderStyle.Solid);

            Label lblCaption = new Label() { Left = 30, Top = 25, Text = caption, Width = 440, Font = new Font("Segoe UI Black", 14, FontStyle.Bold), ForeColor = Color.FromArgb(15, 23, 42) };
            Label textLabel = new Label() { Left = 30, Top = 65, Text = text, Width = 440, Height = 25, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(100, 116, 139) };

            Panel pnlInput = new Panel() { Left = 30, Top = 95, Width = 440, Height = 60, BackColor = Color.White, Padding = new Padding(1) };
            TextBox textBox = new TextBox() { Location = new Point(5, 5), Width = 430, Height = 50, Multiline = true, Font = new Font("Segoe UI", 12), BackColor = Color.White, BorderStyle = BorderStyle.None, ForeColor = Color.Black, Text = defaultValue };
            pnlInput.Controls.Add(textBox);

            pnlInput.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, pnlInput.ClientRectangle, textBox.Focused ? Color.FromArgb(79, 70, 229) : Color.FromArgb(203, 213, 225), ButtonBorderStyle.Solid);
            textBox.Enter += (s, e) => pnlInput.Invalidate();
            textBox.Leave += (s, e) => pnlInput.Invalidate();

            Button confirmation = new Button() { Text = "ЗБЕРЕГТИ", Top = 175, Height = 45, DialogResult = DialogResult.OK, BackColor = Color.FromArgb(79, 70, 229), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            confirmation.FlatAppearance.BorderSize = 0; confirmation.Font = new Font("Segoe UI", 10, FontStyle.Bold); confirmation.Cursor = Cursors.Hand;

            Button cancel = new Button() { Text = "СКАСУВАТИ", Top = 175, Height = 45, DialogResult = DialogResult.Cancel, BackColor = Color.FromArgb(241, 245, 249), ForeColor = Color.FromArgb(71, 85, 105), FlatStyle = FlatStyle.Flat };
            cancel.FlatAppearance.BorderSize = 0; cancel.Font = new Font("Segoe UI", 10, FontStyle.Bold); cancel.Cursor = Cursors.Hand;

            int buttonWidth = (prompt.Width - 60 - 15) / 2;
            cancel.Width = buttonWidth; cancel.Left = 30;
            confirmation.Width = buttonWidth; confirmation.Left = cancel.Right + 15;

            SoundHelper.AttachSounds(prompt);

            prompt.Controls.Add(lblCaption); prompt.Controls.Add(pnlInput); prompt.Controls.Add(confirmation); prompt.Controls.Add(cancel); prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation; prompt.CancelButton = cancel;
            prompt.Shown += (s, e) => { textBox.Focus(); textBox.SelectionStart = textBox.Text.Length; };

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : "";
        }
    }
}