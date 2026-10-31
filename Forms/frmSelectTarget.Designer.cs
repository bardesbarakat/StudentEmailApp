namespace StudentEmailApp.Forms
{
    partial class frmSelectTarget
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSub;
        private System.Windows.Forms.Button btnUndergrad;
        private System.Windows.Forms.Button btnPostgrad;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlHeader;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.btnUndergrad = new System.Windows.Forms.Button();
            this.btnPostgrad = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlHeader.Controls.Add(this.btnLogout);
            this.pnlHeader.Controls.Add(this.lblWelcome);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(700, 60);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.lblWelcome.Location = new System.Drawing.Point(24, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(167, 19);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, [Current User]";

            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(595, 14);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 32);
            this.btnLogout.TabIndex = 1;
            this.btnLogout.Text = "Sign Out";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblHeaderTitle.Location = new System.Drawing.Point(40, 95);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(273, 32);
            this.lblHeaderTitle.TabIndex = 1;
            this.lblHeaderTitle.Text = "Select Target Category";

            // 
            // lblHeaderSub
            // 
            this.lblHeaderSub.AutoSize = true;
            this.lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblHeaderSub.Location = new System.Drawing.Point(42, 135);
            this.lblHeaderSub.Name = "lblHeaderSub";
            this.lblHeaderSub.Size = new System.Drawing.Size(538, 19);
            this.lblHeaderSub.TabIndex = 2;
            this.lblHeaderSub.Text = "Choose the student cohort below to generate emails and synchronize with the database.";

            // 
            // btnUndergrad
            // 
            this.btnUndergrad.BackColor = System.Drawing.Color.White;
            this.btnUndergrad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUndergrad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnUndergrad.FlatAppearance.BorderSize = 2;
            this.btnUndergrad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(238, 242, 255);
            this.btnUndergrad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndergrad.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUndergrad.ForeColor = System.Drawing.Color.FromArgb(30, 64, 175);
            this.btnUndergrad.Location = new System.Drawing.Point(45, 185);
            this.btnUndergrad.Name = "btnUndergrad";
            this.btnUndergrad.Padding = new System.Windows.Forms.Padding(15);
            this.btnUndergrad.Size = new System.Drawing.Size(610, 85);
            this.btnUndergrad.TabIndex = 3;
            this.btnUndergrad.Text = "🎓  Create Emails for Undergraduates\n(Database Table: mails)";
            this.btnUndergrad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUndergrad.UseVisualStyleBackColor = false;
            this.btnUndergrad.Click += new System.EventHandler(this.btnUndergrad_Click);

            // 
            // btnPostgrad
            // 
            this.btnPostgrad.BackColor = System.Drawing.Color.White;
            this.btnPostgrad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPostgrad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnPostgrad.FlatAppearance.BorderSize = 2;
            this.btnPostgrad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(240, 253, 250);
            this.btnPostgrad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPostgrad.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPostgrad.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPostgrad.Location = new System.Drawing.Point(45, 290);
            this.btnPostgrad.Name = "btnPostgrad";
            this.btnPostgrad.Padding = new System.Windows.Forms.Padding(15);
            this.btnPostgrad.Size = new System.Drawing.Size(610, 85);
            this.btnPostgrad.TabIndex = 4;
            this.btnPostgrad.Text = "🔬  Create Emails for Postgraduates\n(Database Table: PostGradMails)";
            this.btnPostgrad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPostgrad.UseVisualStyleBackColor = false;
            this.btnPostgrad.Click += new System.EventHandler(this.btnPostgrad_Click);

            // 
            // frmSelectTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.ClientSize = new System.Drawing.Size(700, 420);
            this.Controls.Add(this.btnPostgrad);
            this.Controls.Add(this.btnUndergrad);
            this.Controls.Add(this.lblHeaderSub);
            this.Controls.Add(this.lblHeaderTitle);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmSelectTarget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Email System - Target Selection";
            this.Load += new System.EventHandler(this.frmSelectTarget_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
