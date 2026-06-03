using System;
using System.Drawing;
using System.Windows.Forms;
using WayPoint.Services;

namespace WayPoint.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Підтягуємо дані з конфігу
            txtDb.Text = ConfigManager.Config.DbConnectionString;
            txtEmail.Text = ConfigManager.Config.SmtpEmail;
            txtPass.Text = ConfigManager.Config.SmtpPassword;
            SettingsForm_Resize(this, EventArgs.Empty);
        }

        private void SettingsForm_Resize(object sender, EventArgs e)
        {
            if (pnlCenter != null)
            {
                pnlCenter.Left = (this.ClientSize.Width - pnlCenter.Width) / 2;
                pnlCenter.Top = (this.ClientSize.Height - pnlCenter.Height) / 2;
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !chkShowPass.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfigManager.Config.DbConnectionString = txtDb.Text.Trim();
            ConfigManager.Config.SmtpEmail = txtEmail.Text.Trim();
            ConfigManager.Config.SmtpPassword = txtPass.Text.Trim();
            ConfigManager.SaveConfig();

            MessageBox.Show("Налаштування успішно збережено у config.json!\nДля надійності рекомендується перезапустити програму.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}