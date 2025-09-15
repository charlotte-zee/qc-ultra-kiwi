namespace LoginForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblLogo = new Label();
            txtPassword = new TextBox();
            btnSignIn = new Button();
            lnkForgotPassword = new LinkLabel();
            pictureBox1 = new PictureBox();
            btnMoreOptions = new Button();
            optimize = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblLogo
            // 
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(103, 29);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(64, 15);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "ZX_CODER";
            lblLogo.Click += lblLogo_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(12, 138);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(247, 23);
            txtPassword.TabIndex = 2;
            txtPassword.Text = "Password";
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // btnSignIn
            // 
            btnSignIn.Location = new Point(103, 173);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(64, 26);
            btnSignIn.TabIndex = 3;
            btnSignIn.Text = "START";
            btnSignIn.UseVisualStyleBackColor = true;
            btnSignIn.Click += btnSignIn_Click;
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.AutoSize = true;
            lnkForgotPassword.Location = new Point(12, 173);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(64, 15);
            lnkForgotPassword.TabIndex = 4;
            lnkForgotPassword.TabStop = true;
            lnkForgotPassword.Text = "Forgot Key";
            lnkForgotPassword.LinkClicked += lnkForgotPassword_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources._1a3620d7_ee25_478a_93a3_4069ad5201b8;
            pictureBox1.Location = new Point(282, 83);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(178, 266);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // btnMoreOptions
            // 
            btnMoreOptions.Location = new Point(12, 252);
            btnMoreOptions.Name = "btnMoreOptions";
            btnMoreOptions.Size = new Size(106, 26);
            btnMoreOptions.TabIndex = 7;
            btnMoreOptions.Text = "More Options";
            btnMoreOptions.UseVisualStyleBackColor = true;

            // 
            // optimize
            // 
            optimize.BackColor = Color.White;
            optimize.Location = new Point(12, 297);
            optimize.Name = "optimize";
            optimize.Size = new Size(106, 26);
            optimize.TabIndex = 8;
            optimize.Text = "Quick Optimize";
            optimize.UseVisualStyleBackColor = false;
            optimize.Click += optimize_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(467, 349);
            Controls.Add(optimize);
            Controls.Add(btnMoreOptions);
            Controls.Add(lblLogo);
            Controls.Add(pictureBox1);
            Controls.Add(lnkForgotPassword);
            Controls.Add(btnSignIn);
            Controls.Add(txtPassword);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLogo;
        private TextBox txtPassword;
        private Button btnSignIn;
        private LinkLabel lnkForgotPassword;
        private PictureBox pictureBox1;
        private Button btnMoreOptions;
        private Button optimize;
    }
}
