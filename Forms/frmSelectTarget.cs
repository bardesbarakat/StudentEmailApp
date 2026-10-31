using System;
using System.Windows.Forms;
using StudentEmailApp.Models;

namespace StudentEmailApp.Forms
{
    public partial class frmSelectTarget : Form
    {
        public frmSelectTarget()
        {
            InitializeComponent();
        }

        private void frmSelectTarget_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {UserSession.CurrentUsername}";
        }

        private void btnUndergrad_Click(object sender, EventArgs e)
        {
            // Open Form 3 targeting the Undergraduates table ("mails")
            this.Hide();
            using var generatorForm = new frmGenerator("mails", "Undergraduate Students");
            generatorForm.ShowDialog();
            this.Show();
        }

        private void btnPostgrad_Click(object sender, EventArgs e)
        {
            // Open Form 3 targeting the Postgraduates table ("PostGradMails")
            this.Hide();
            using var generatorForm = new frmGenerator("PostGradMails", "Postgraduate Students");
            generatorForm.ShowDialog();
            this.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Sign out and return to Login
            UserSession.Clear();
            this.Close();
        }
    }
}
