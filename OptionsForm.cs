using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using System.IO;

namespace LoginForm
{
    public partial class OptionsForm : Form
    {
        // Drag for borderless form
        private const int WM_NCLBUTTONDOWN = 0xA1;    // drag message [title bar]
        private PictureBox pictureBox1;
        private Button selectAndBackupQC;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Button button1;
        private const int HTCAPTION = 0x2;            // caption hit-test
        [DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private Button update;
        private Button setup;
        private Button scripts;
        private Button btnClose;

        public OptionsForm()
        {
            InitializeComponent();

            // Top header: "Options Menu"
            // Top header: "Options Menu" — also acts as draggable title area
            var header = new Label
            {
                Text = "Options Menu",
                Dock = DockStyle.Top,                            // top bar [web:860]
                Height = 38,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 27, 107),       // hot pink [web:908][web:916]
                Padding = new Padding(12, 0, 0, 0)
            };
            header.MouseDown += OnDragMouseDown;                // drag by header [web:899]
            Controls.Add(header);
            header.BringToFront();                              // top of z-order [web:879]


            // Visual style
            FormBorderStyle = FormBorderStyle.None;                                       // borderless
            StartPosition = FormStartPosition.CenterParent;                               // center over owner
            BackColor = Color.Black;
            Opacity = 0.88;                                                               // translucent
            DoubleBuffered = true;

            // Drag anywhere
            MouseDown += OnDragMouseDown;                                                 // form surface

            // Style: Update button (blue)
            if (Controls["update"] is Button updateBtn)
            {
                updateBtn.FlatStyle = FlatStyle.Flat;                                     // enable FlatAppearance colors
                updateBtn.FlatAppearance.BorderSize = 0;
                updateBtn.BackColor = Color.FromArgb(30, 136, 229);
                updateBtn.ForeColor = Color.White;
                updateBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(66, 165, 245); // hover
                updateBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(25, 118, 210); // pressed
                updateBtn.Cursor = Cursors.Hand;
                updateBtn.AutoSize = false;
                updateBtn.Size = new Size(140, 34);
            }

            // Style: Setup button (blue)
            if (Controls["setup"] is Button setupBtn)
            {
                setupBtn.FlatStyle = FlatStyle.Flat;                                      // enables FlatAppearance colors
                setupBtn.FlatAppearance.BorderSize = 0;
                setupBtn.BackColor = Color.FromArgb(30, 136, 229);
                setupBtn.ForeColor = Color.White;
                setupBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(66, 165, 245); // hover
                setupBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(25, 118, 210); // pressed
                setupBtn.Cursor = Cursors.Hand;
                setupBtn.AutoSize = false;
                setupBtn.Size = new Size(140, 34);
            }

            // Style: Scripts button (pink)
            if (Controls["scripts"] is Button scriptsBtn)
            {
                scriptsBtn.FlatStyle = FlatStyle.Flat;                                    // enable hover/press colors
                scriptsBtn.FlatAppearance.BorderSize = 0;
                scriptsBtn.BackColor = Color.FromArgb(255, 27, 107);                      // base pink
                scriptsBtn.ForeColor = Color.White;
                scriptsBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(244, 143, 177); // hover
                scriptsBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(216, 27, 96);   // pressed
                scriptsBtn.Cursor = Cursors.Hand;
                scriptsBtn.AutoSize = false;
                scriptsBtn.Size = new Size(140, 34);
            }

            if (Controls["selectAndBackupQC"] is Button qcBtn)
            {
                qcBtn.FlatStyle = FlatStyle.Flat;                                   // enable custom colors [11]
                qcBtn.FlatAppearance.BorderSize = 0;

                qcBtn.BackColor = Color.FromArgb(255, 27, 107);                     // pink
                qcBtn.ForeColor = Color.White;

                qcBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 82, 149); // hover [12]
                qcBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(216, 27, 96);  // pressed [12]

                qcBtn.Cursor = Cursors.Hand;

                // Same fixed size as the example
                qcBtn.AutoSize = false;                                             // keep manual size [9]
                qcBtn.Size = new Size(140, 34);                                 // exact size
            }

            if (Controls["button1"] is Button b1)
            {
                b1.FlatStyle = FlatStyle.Flat;                                        // enable custom colors [web:1055]
                b1.FlatAppearance.BorderSize = 0;

                b1.BackColor = Color.FromArgb(124, 77, 255);                          // purple base
                b1.ForeColor = Color.White;
                b1.FlatAppearance.MouseOverBackColor = Color.FromArgb(149, 117, 205); // hover shade [web:1055]
                b1.FlatAppearance.MouseDownBackColor = Color.FromArgb(94, 53, 177);   // pressed shade [web:1055]

                b1.Cursor = Cursors.Hand;
                b1.AutoSize = true;                                                  // same fixed size as before [web:1103]
            }




            // Close button
            btnClose = new Button
            {
                Name = "btnClose",
                Text = "✕",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Size = new Size(32, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false,
                UseMnemonic = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(65, 65, 65);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 90);
            btnClose.Click += (_, __) => Close();
            Controls.Add(btnClose);
            btnClose.Location = new Point(ClientSize.Width - btnClose.Width - 8, 8);
            btnClose.BringToFront();
            Resize += (_, __) => { btnClose.Left = ClientSize.Width - btnClose.Width - 8; btnClose.BringToFront(); };
            Shown += (_, __) => btnClose.BringToFront();
            EnabledChanged += (_, __) => btnClose.Enabled = true;



            // Tutorials drop-down button (NEW)
            var btnTutorials = new Button
            {
                Text = "Tutorials: QC Scripts [ Chinese ] ▾",
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 40, 40),
                Location = new Point(170, 102), // adjust placement as needed
                Cursor = Cursors.Hand
            };
            btnTutorials.FlatAppearance.BorderSize = 0;
            btnTutorials.FlatAppearance.MouseOverBackColor = Color.FromArgb(65, 65, 65);
            btnTutorials.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 90);
            Controls.Add(btnTutorials);

            var tutMenu = new ContextMenuStrip { ShowImageMargin = false };               // dropdown container

            void AddTutorial(string title, string url) =>
                tutMenu.Items.Add(title, null, (_, __) => OpenUrl(url));                 // each item opens a link [1][2]

            // Define tutorial entries here (edit titles/links as desired)
            AddTutorial("1 - Manual Setup", "http://cloud.video.taobao.com/play/u/null/p/1/e/6/t/1/517391758220.mp4");
            AddTutorial("2 - Automatic Bounty Rewards", "http://cloud.video.taobao.com/play/u/null/p/1/e/6/t/1/523644238356.mp4");
            AddTutorial("3 - Automatic Campus Level 4 Control Point", "http://cloud.video.taobao.com/play/u/null/p/1/e/6/t/1/523643926601.mp4");
            AddTutorial("4 - Automatic Decent", "http://cloud.video.taobao.com/play/u/null/p/1/e/6/t/1/523616007150.mp4");
            AddTutorial("5 - Automatic Incursion - Paradise", "http://cloud.video.taobao.com/play/u/null/p/1/e/6/t/1/523615835747.mp4");
            AddTutorial("6 - Dark Hours Raid - Ariport", "http://cloud.video.taobao.com/play/u/null/p/1/e/6/t/1/523615911473.mp4");

            btnTutorials.Click += (_, __) => tutMenu.Show(btnTutorials, new Point(0, btnTutorials.Height)); // show under button [1]
        }

        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                }); // open using default browser [2][3]
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open link:\n" + ex.Message, "Open Link",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); // fallback [4]
            }
        }

        private void OnDragMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        // Minimal inline InitializeComponent so ClientSize is applied once
        private void InitializeComponent()
        {
            update = new Button();
            setup = new Button();
            scripts = new Button();
            pictureBox1 = new PictureBox();
            selectAndBackupQC = new Button();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // update
            // 
            update.Location = new Point(12, 51);
            update.Name = "update";
            update.Size = new Size(107, 23);
            update.TabIndex = 0;
            update.Text = "Check Updates";
            update.UseVisualStyleBackColor = true;
            update.Click += update_Click;
            // 
            // setup
            // 
            setup.Location = new Point(12, 101);
            setup.Name = "setup";
            setup.Size = new Size(107, 23);
            setup.TabIndex = 1;
            setup.Text = "One Click Setup";
            setup.UseVisualStyleBackColor = true;
            setup.Click += setup_Click;
            // 
            // scripts
            // 
            scripts.Location = new Point(12, 197);
            scripts.Name = "scripts";
            scripts.Size = new Size(107, 23);
            scripts.TabIndex = 3;
            scripts.Text = "Load Scripts";
            scripts.UseVisualStyleBackColor = true;
            scripts.Click += scripts_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.animecat2;
            pictureBox1.Location = new Point(307, 154);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(193, 197);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // selectAndBackupQC
            // 
            selectAndBackupQC.Location = new Point(12, 154);
            selectAndBackupQC.Name = "selectAndBackupQC";
            selectAndBackupQC.Size = new Size(107, 23);
            selectAndBackupQC.TabIndex = 0;
            selectAndBackupQC.Text = "Load QC.exe";
            selectAndBackupQC.Click += selectAndBackupQC_Click_1;
            // 
            // button1
            // 
            button1.Location = new Point(12, 311);
            button1.Name = "button1";
            button1.Size = new Size(59, 25);
            button1.TabIndex = 8;
            button1.Text = "Contact";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // OptionsForm
            // 
            ClientSize = new Size(497, 348);
            Controls.Add(button1);
            Controls.Add(selectAndBackupQC);
            Controls.Add(pictureBox1);
            Controls.Add(scripts);
            Controls.Add(setup);
            Controls.Add(update);
            Name = "OptionsForm";
            Load += OptionsForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = new GraphicsPath();
            int r = 18;
            var rect = new Rectangle(0, 0, Width, Height);
            path.AddArc(rect.X, rect.Y, r * 2, r * 2, 180, 90);
            path.AddArc(rect.Right - r * 2, rect.Y, r * 2, r * 2, 270, 90);
            path.AddArc(rect.Right - r * 2, rect.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            Region = new Region(path);
            using var brush = new SolidBrush(Color.FromArgb(20, Color.White));
            e.Graphics.FillPath(brush, path);
        }

        private void update_Click(object sender, EventArgs e)
        {
            const string url = "https://wwpo.lanzoue.com/iRRh335x3v3e";
            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true }); // default browser
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the link.\n" + ex.Message, "Open Link",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e) { }

        private void setup_Click(object sender, EventArgs e)
        {
            // Confirmation prompt about Defender
            var confirm = MessageBox.Show(
                "Before continuing, make sure Windows Defender (Real‑time protection) is turned OFF.\n\nClick OK to proceed, or Cancel to stop.",
                "Confirm: Turn off Defender first",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.OK) return;

            if (!IsAdministrator())
            {
                MessageBox.Show("Administrator privileges are required to apply system setup.",
                    "Admin Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var errors = new List<string>();

            // 1) Disable Core Isolation (Memory Integrity / HVCI)
            try
            {
                using (var key = Registry.LocalMachine.CreateSubKey(
                    @"SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity", true))
                {
                    key?.SetValue("Enabled", 0, RegistryValueKind.DWord);
                }
                using (var key2 = Registry.LocalMachine.CreateSubKey(
                    @"SYSTEM\CurrentControlSet\Control\DeviceGuard", true))
                {
                    key2?.SetValue("EnableVirtualizationBasedSecurity", 0, RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                errors.Add("Core Isolation toggle failed: " + ex.Message);
            }

            // 2) Set time zone to China Standard Time (UTC+08:00 Beijing)
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "tzutil",
                    Arguments = "/s \"China Standard Time\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };
                using var p = Process.Start(psi);
                p?.WaitForExit(8000);
                if (p == null || p.ExitCode != 0)
                    errors.Add("Time zone change failed: " + (p?.StandardError.ReadToEnd() ?? "unknown error"));
            }
            catch (Exception ex)
            {
                errors.Add("Time zone change failed: " + ex.Message);
            }

            // 3) Set system locale (language for non‑Unicode programs) to Chinese (Simplified, Mainland China)
            try
            {
                var psiLocale = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"Set-WinSystemLocale -SystemLocale zh-CN\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };
                using var pl = Process.Start(psiLocale);
                pl?.WaitForExit(15000);
                if (pl == null || pl.ExitCode != 0)
                    errors.Add("System locale change failed: " + (pl?.StandardError.ReadToEnd() ?? "unknown error"));
            }
            catch (Exception ex)
            {
                errors.Add("System locale change failed: " + ex.Message);
            }

            if (errors.Count == 0)
            {
                MessageBox.Show(
                    "Setup complete.\n\n• Core Isolation (Memory Integrity): Disabled.\n• Time zone: China Standard Time (UTC+08:00).\n• System locale (non‑Unicode): Chinese (Simplified, Mainland China).\n\nA restart is required for the system locale and may be needed for Core Isolation changes.",
                    "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(string.Join("\n", errors), "Some steps failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static bool IsAdministrator()
        {
            try
            {
                using var identity = WindowsIdentity.GetCurrent();
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch { return false; }
        }

        private static void CopyDirectory(string sourceDir, string destDir, bool skipScriptsLoopGuard = false)
        {
            var source = new DirectoryInfo(sourceDir);
            if (!source.Exists)
                throw new DirectoryNotFoundException("Source not found: " + sourceDir); // guard [2]

            Directory.CreateDirectory(destDir);                                         // ensure target exists [2]

            // Copy files in current directory
            foreach (var file in source.GetFiles())
            {
                try
                {
                    string targetFile = Path.Combine(destDir, file.Name);
                    file.CopyTo(targetFile, overwrite: true);                            // overwrite existing [13]
                }
                catch { /* skip locked/denied files */ }
            }

            // Recurse into subdirectories
            foreach (var sub in source.GetDirectories())
            {
                try
                {
                    string nextDest = Path.Combine(destDir, sub.Name);
                    CopyDirectory(sub.FullName, nextDest);                               // recursive copy [2]
                }
                catch { /* skip problematic subfolders */ }
            }
        }

        private void scripts_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog
            {
                Description = "Select the folder that contains all the files to copy to C:\\yy",
                ShowNewFolderButton = false
            };

            if (fbd.ShowDialog(this) != DialogResult.OK) return;

            string sourceDir = fbd.SelectedPath;                                         // user-selected folder [18]
            string destDir = @"C:\yy";

            try
            {
                Directory.CreateDirectory(destDir);                                      // create C:\yy if missing [2]
                CopyDirectory(sourceDir, destDir);                                       // copy all contents [2]
                MessageBox.Show("Files copied to C:\\yy.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Access denied. Try running as Administrator.\n\n" + ex.Message,
                    "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error copying files:\n\n" + ex.Message,
                    "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        // Add a menu item or button click handler

        private void selectAndBackupQC_Click_1(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Select QC.exe",
                Filter = "QC executable|QC.exe|Executables|*.exe|All files|*.*",   // prioritize exact name [5]
                CheckFileExists = true,                                           // only existing files [4]
                Multiselect = false
            };

            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            var sourcePath = ofd.FileName;
            var appDir = AppDomain.CurrentDomain.BaseDirectory;                   // current app folder [2]
            var targetPath = Path.Combine(appDir, "QC.exe");
            var backupPath = Path.Combine(appDir, "QC.backup.exe");

            try
            {
                Directory.CreateDirectory(appDir);                                // ensure exists (usually does) [10]

                if (!File.Exists(targetPath))
                {
                    // No QC.exe yet — simple copy
                    File.Copy(sourcePath, targetPath, overwrite: false);          // copy; will throw if exists [10]
                    MessageBox.Show("QC.exe copied to the app folder.", "Done",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // QC.exe exists — backup then replace
                    try
                    {
                        // Backup existing QC.exe (overwrite old backup if present)
                        File.Copy(targetPath, backupPath, overwrite: true);       // create/refresh backup [10]
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not create backup:\n" + ex.Message,
                            "Backup Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // Replace QC.exe with the selected one
                    File.Copy(sourcePath, targetPath, overwrite: true);           // replace existing [10]

                    MessageBox.Show("QC.exe replaced. A backup was saved as QC.backup.exe.",
                        "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Access denied.\nTry running as Administrator.\n\n" + ex.Message,
                    "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("I/O error during copy.\n\n" + ex.Message,
                    "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error.\n\n" + ex.Message,
                    "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Show modeless so the main form remains usable/moveable
            var f = new GlassForm();
            f.Show(this);   // owner set for proper z-order; not modal
        }
    }
}
