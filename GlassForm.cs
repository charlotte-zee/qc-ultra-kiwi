using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace LoginForm
{
    public class GlassForm : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;                                  // drag message [12]
        private PictureBox pictureBox1;
        private PictureBox pictureBox4;
        private PictureBox pictureBox2;
        private Label labelWebsite;
        private const int HTCAPTION = 0x2;                                          // treat as title bar [12]
        [DllImport("user32.dll")] private static extern bool ReleaseCapture();      // native drag helper [12]
        [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam); // native drag helper [12]

        private Button btnClose;
        private Label labelTitle;
        private Label labelSeller;
        private Label labelGithub;

        public GlassForm()
        {
            InitializeComponent();                                                  // create and add controls [13]

            // Core look (glassy, borderless); do not override Designer size
            FormBorderStyle = FormBorderStyle.None;                                 // borderless window [14]
            StartPosition = FormStartPosition.CenterParent;                       // center over owner [15]
            BackColor = Color.Black;                                          // dark background [11]
            Opacity = 0.88;                                                 // translucent glass effect [11]
            DoubleBuffered = true;                                                 // smoother repaint [12]

            // Shared label styling
            void StyleLabel(Label lbl)
            {
                lbl.AutoSize = true;                                               // size to content [10]
                lbl.Anchor = AnchorStyles.None;                                  // keep programmatic position; no edge pinning [2]
                lbl.Margin = new Padding(0);                                     // no layout offset [10]
                lbl.BackColor = Color.Transparent;                                  // blends with glass [10]
                lbl.ForeColor = Color.White;                                        // readable on dark [10]
                lbl.Font = new Font("Segoe UI", 14f, FontStyle.Bold);          // consistent typography [10]
            }
            StyleLabel(labelTitle);
            StyleLabel(labelSeller);
            StyleLabel(labelWebsite);
            StyleLabel(labelGithub);

            // Horizontal layout with spacing
            int leftStart = 24;                                                     // left margin [2]
            int topLine = 40;                                                     // top margin [2]
            int gap = 36;                                                     // gap between labels [2]

            using (var g = CreateGraphics())
            {
                // Title
                SizeF s1 = g.MeasureString(labelTitle.Text, labelTitle.Font);       // measure width [10]
                labelTitle.Location = new Point(leftStart, topLine);

                // Seller
                int x2 = leftStart + (int)Math.Ceiling(s1.Width) + gap;
                labelSeller.Location = new Point(x2, topLine);

                // Website
                SizeF s2 = g.MeasureString(labelSeller.Text, labelSeller.Font);
                int x3 = x2 + (int)Math.Ceiling(s2.Width) + gap;
                labelWebsite.Location = new Point(x3, topLine);

                // Github
                SizeF s3 = g.MeasureString(labelWebsite.Text, labelWebsite.Font);
                int x4 = x3 + (int)Math.Ceiling(s3.Width) + gap;
                labelGithub.Location = new Point(x4, topLine);
            }

            // Drag anywhere on empty surface
            MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            // Close button (X)
            btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Size = new Size(36, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right                      // pin to top-right [2]
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(65, 65, 65);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 90);
            btnClose.Click += (_, __) => Close();

            Controls.Add(btnClose);
            Shown += (_, __) => PositionCloseButton();
            Resize += (_, __) => PositionCloseButton();
        }

        private void InitializeComponent()
        {
            labelTitle = new Label();
            labelSeller = new Label();
            labelGithub = new Label();
            pictureBox1 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox2 = new PictureBox();
            labelWebsite = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(12, 34);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(66, 15);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Kiwi ( Dev )";
            // 
            // labelSeller
            // 
            labelSeller.AutoSize = true;
            labelSeller.Location = new Point(116, 34);
            labelSeller.Name = "labelSeller";
            labelSeller.Size = new Size(35, 15);
            labelSeller.TabIndex = 1;
            labelSeller.Text = "Seller";
            // 
            // labelGithub
            // 
            labelGithub.AutoSize = true;
            labelGithub.Location = new Point(405, 34);
            labelGithub.Name = "labelGithub";
            labelGithub.Size = new Size(43, 15);
            labelGithub.TabIndex = 3;
            labelGithub.Text = "Github";
            labelGithub.Click += labelGithub_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.dc;
            pictureBox1.Location = new Point(12, 73);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(83, 79);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.Image = Properties.Resources.github;
            pictureBox4.Location = new Point(386, 73);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(90, 79);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 7;
            pictureBox4.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = Properties.Resources.dc;
            pictureBox2.Location = new Point(142, 73);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(85, 79);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // labelWebsite
            // 
            labelWebsite.AutoSize = true;
            labelWebsite.Location = new Point(238, 44);
            labelWebsite.Name = "labelWebsite";
            labelWebsite.Size = new Size(49, 15);
            labelWebsite.TabIndex = 2;
            labelWebsite.Text = "Website";
            // 
            // GlassForm
            // 
            ClientSize = new Size(569, 201);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox1);
            Controls.Add(labelTitle);
            Controls.Add(labelSeller);
            Controls.Add(labelWebsite);
            Controls.Add(labelGithub);
            Name = "GlassForm";
            Load += GlassForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void PositionCloseButton()
        {
            btnClose.Location = new Point(ClientSize.Width - btnClose.Width - 10, 10);
            btnClose.BringToFront();
        }

        // Rounded corners + subtle overlay to mimic glass
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int r = 18;
            var rect = new Rectangle(0, 0, Width, Height);
            using var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, r * 2, r * 2, 180, 90);
            path.AddArc(rect.Right - r * 2, rect.Y, r * 2, r * 2, 270, 90);
            path.AddArc(rect.Right - r * 2, rect.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            Region = new Region(path);

            using var overlay = new SolidBrush(Color.FromArgb(20, Color.White));
            e.Graphics.FillPath(overlay, path);
        }

        private void labelGithub_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            const string url = "https://discord.com/users/830939411809697823";
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true   // required on .NET Core/5+/6+ to open in default browser
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open link.\n\n" + ex.Message,
                    "Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GlassForm_Load(object sender, EventArgs e)
        {

        }
    }
}
