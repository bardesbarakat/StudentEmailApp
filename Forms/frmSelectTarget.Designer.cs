namespace StudentEmailApp.Forms
{
    partial class frmSelectTarget
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlCenterCard;
        private System.Windows.Forms.Label lblHeaderTitle;
        private System.Windows.Forms.Label lblHeaderSub;
        private System.Windows.Forms.Button btnUndergrad;
        private System.Windows.Forms.Button btnPostgrad;
        private System.Windows.Forms.Button btnManageStudents;

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
            this.pnlCenterCard = new System.Windows.Forms.Panel();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.btnUndergrad = new System.Windows.Forms.Button();
            this.btnPostgrad = new System.Windows.Forms.Button();
            this.btnManageStudents = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlCenterCard.SuspendLayout();
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
            this.pnlHeader.Size = new System.Drawing.Size(1024, 64);
            this.pnlHeader.TabIndex = 0;

            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.lblWelcome.Location = new System.Drawing.Point(28, 21);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(185, 20);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, [Current User]";

            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(900, 15);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(96, 34);
            this.btnLogout.TabIndex = 1;
            this.btnLogout.Text = "Sign Out";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // pnlCenterCard
            // 
            this.pnlCenterCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCenterCard.BackColor = System.Drawing.Color.White;
            this.pnlCenterCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCenterCard.Controls.Add(this.btnManageStudents);
            this.pnlCenterCard.Controls.Add(this.btnPostgrad);
            this.pnlCenterCard.Controls.Add(this.btnUndergrad);
            this.pnlCenterCard.Controls.Add(this.lblHeaderSub);
            this.pnlCenterCard.Controls.Add(this.lblHeaderTitle);
            this.pnlCenterCard.Location = new System.Drawing.Point(152, 95);
            this.pnlCenterCard.Name = "pnlCenterCard";
            this.pnlCenterCard.Size = new System.Drawing.Size(720, 480);
            this.pnlCenterCard.TabIndex = 1;

            // 
            // lblHeaderTitle
            // 
            this.lblHeaderTitle.AutoSize = false;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblHeaderTitle.Location = new System.Drawing.Point(40, 26);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(640, 40);
            this.lblHeaderTitle.TabIndex = 1;
            this.lblHeaderTitle.Text = "Select Target Category";
            this.lblHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblHeaderSub
            // 
            this.lblHeaderSub.AutoSize = false;
            this.lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblHeaderSub.Location = new System.Drawing.Point(40, 70);
            this.lblHeaderSub.Name = "lblHeaderSub";
            this.lblHeaderSub.Size = new System.Drawing.Size(640, 26);
            this.lblHeaderSub.TabIndex = 2;
            this.lblHeaderSub.Text = "Choose the student cohort below to generate emails or manage existing student records.";
            this.lblHeaderSub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // btnUndergrad
            // 
            this.btnUndergrad.BackColor = System.Drawing.Color.White;
            this.btnUndergrad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUndergrad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnUndergrad.FlatAppearance.BorderSize = 2;
            this.btnUndergrad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(238, 242, 255);
            this.btnUndergrad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndergrad.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUndergrad.ForeColor = System.Drawing.Color.FromArgb(30, 64, 175);
            this.btnUndergrad.Location = new System.Drawing.Point(40, 116);
            this.btnUndergrad.Name = "btnUndergrad";
            this.btnUndergrad.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.btnUndergrad.Size = new System.Drawing.Size(640, 96);
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
            this.btnPostgrad.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPostgrad.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnPostgrad.Location = new System.Drawing.Point(40, 228);
            this.btnPostgrad.Name = "btnPostgrad";
            this.btnPostgrad.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.btnPostgrad.Size = new System.Drawing.Size(640, 96);
            this.btnPostgrad.TabIndex = 4;
            this.btnPostgrad.Text = "🔬  Create Emails for Postgraduates\n(Database Table: PostGradMails)";
            this.btnPostgrad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPostgrad.UseVisualStyleBackColor = false;
            this.btnPostgrad.Click += new System.EventHandler(this.btnPostgrad_Click);

            // 
            // btnManageStudents
            // 
            this.btnManageStudents.BackColor = System.Drawing.Color.White;
            this.btnManageStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManageStudents.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.btnManageStudents.FlatAppearance.BorderSize = 2;
            this.btnManageStudents.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(245, 243, 255);
            this.btnManageStudents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageStudents.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnManageStudents.ForeColor = System.Drawing.Color.FromArgb(109, 40, 217);
            this.btnManageStudents.Location = new System.Drawing.Point(40, 340);
            this.btnManageStudents.Name = "btnManageStudents";
            this.btnManageStudents.Padding = new System.Windows.Forms.Padding(18, 12, 18, 12);
            this.btnManageStudents.Size = new System.Drawing.Size(640, 96);
            this.btnManageStudents.TabIndex = 5;
            this.btnManageStudents.Text = "🔍  إدارة بيانات الطلاب (Search & Edit Student Records)\n(بحث وتعديل آمن لبيانات الطلاب المسجلين)";
            this.btnManageStudents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManageStudents.UseVisualStyleBackColor = false;
            this.btnManageStudents.Click += new System.EventHandler(this.btnManageStudents_Click);

            // 
            // frmSelectTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.ClientSize = new System.Drawing.Size(1024, 640);
            this.Controls.Add(this.pnlCenterCard);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmSelectTarget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Email System - Target Selection";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSelectTarget_Load);
            this.Resize += new System.EventHandler(this.frmSelectTarget_Resize);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlCenterCard.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
