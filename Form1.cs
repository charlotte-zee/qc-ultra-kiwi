using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForm
{
    public partial class Form1 : Form
    {
        private NotifyIcon? _tray;

        // OS-level drag
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        [DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        // For focusing QC before SendKeys
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern bool IsWindow(IntPtr hWnd);

        public Form1()
        {
            InitializeComponent();

            // Spacing tweak (12px gap under the key box)
            txtPassword.Location = new Point(txtPassword.Location.X, txtPassword.Location.Y);
            btnSignIn.Location = new Point(btnSignIn.Location.X, txtPassword.Bottom + 12); // 12px spacing [21]

            // Window style
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.Opacity = 0.9;

            // Hidden focusable control
            var hiddenButton = new Button
            {
                Size = new Size(0, 0),
                Location = new Point(-10, -10),
                TabIndex = 0
            };
            this.Controls.Add(hiddenButton);
            this.ActiveControl = hiddenButton;

            // UI styling
            lblLogo.Text = "QC ULTRA - KIWI ";
            lblLogo.Font = new Font("Arial", 24, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(255, 27, 107);
            lblLogo.BackColor = Color.Transparent;

            txtPassword.BackColor = Color.FromArgb(20, 20, 20);
            txtPassword.ForeColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
#if NET5_0_OR_GREATER
            txtPassword.PlaceholderText = " Key";
#endif
            // Keep key visible
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.PasswordChar = '\0';
            txtPassword.Text = string.Empty;

            // Style helper applied to both buttons
            StylePrimaryButton(btnSignIn);
            StylePrimaryButton(btnMoreOptions);


            // Blue style for the Optimize button (Designer Name must be "optimize")
            // Make Optimize match More Options size and alignment
            if (this.Controls["optimize"] is Button optimize)
            {
                optimize.FlatStyle = FlatStyle.Flat;
                optimize.FlatAppearance.BorderSize = 0;
                optimize.BackColor = Color.FromArgb(30, 136, 229);
                optimize.ForeColor = Color.White;
                optimize.FlatAppearance.MouseOverBackColor = Color.FromArgb(66, 165, 245);
                optimize.FlatAppearance.MouseDownBackColor = Color.FromArgb(25, 118, 210);
                optimize.UseVisualStyleBackColor = false;

                // make pointer show on hover
                optimize.Cursor = Cursors.Hand;                       // hand cursor [1]
            }






            lnkForgotPassword.LinkColor = Color.FromArgb(255, 27, 107);
            lnkForgotPassword.ActiveLinkColor = Color.FromArgb(255, 61, 131);
            lnkForgotPassword.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkForgotPassword.BackColor = Color.Transparent;

            // Keyboard: Enter submits
            this.AcceptButton = btnSignIn; // Enter triggers START [6]

            // Drag anywhere
            this.MouseDown += Form_MouseDownForDrag;
            lblLogo.MouseDown += Form_MouseDownForDrag;

            // Wire button click for More Options
            btnMoreOptions.Click += btnMoreOptions_Click;
        }

        // Apply consistent flat, pink style to primary buttons
        private void StylePrimaryButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;                                      // required for FlatAppearance colors [2]
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Color.FromArgb(255, 27, 107);
            b.ForeColor = Color.White;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 61, 131); // hover [4]
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(220, 20, 90);  // pressed [1]
            b.Cursor = Cursors.Hand;
        }

        // Build path to key.txt in app folder
        private static string GetKeyFilePath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; // app folder [21]
            return Path.Combine(baseDir, "key.txt"); // key.txt at root [21]
        }

        // Drag handler
        private void Form_MouseDownForDrag(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void lblLogo_Click(object sender, EventArgs e)
        {
            // optional
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // optional
        }

        // Forgot: read key.txt and fill textbox ONLY (no QC launch)
        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string keyPath = GetKeyFilePath();
            if (!File.Exists(keyPath))
            {
                MessageBox.Show("key.txt not found in the app folder.", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Information); // guard
                return;
            }

            try
            {
                string keyFromFile = File.ReadAllText(keyPath); // read key [22]
                txtPassword.Text = keyFromFile;                 // just fill
                txtPassword.Focus();
                txtPassword.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to read key.txt: " + ex.Message, "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); // file error [22]
            }
        }

        // Submit: save key.txt, launch QC, paste key, Tab, Enter
        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter QC Key.", "Missing Key",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); // validation
                txtPassword.Focus();
                return;
            }

            string key = txtPassword.Text;
            string keyPath = GetKeyFilePath();

            // Save key to key.txt (overwrite each submit)
            try
            {
                File.WriteAllText(keyPath, key); // simple write [23]
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save key.txt: " + ex.Message, "File Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Optional quick confirmation
            MessageBox.Show("Key saved.", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            await LaunchQcAndSendKeyAsync(key); // elevate + auto input
        }

        // Launch .\QC\QC.exe as admin, then paste key, Tab, Enter, then close form
        private async Task LaunchQcAndSendKeyAsync(string keyText)
        {
            // App folder (where this EXE runs from)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;                 // reliable app path [6][11]
            string qcExe = Path.Combine(baseDir, "QC.exe");

            if (!File.Exists(qcExe))
            {
                MessageBox.Show("QC.exe not found in the app folder.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = qcExe,
                    WorkingDirectory = baseDir,                                     // with UseShellExecute=true, this is the exe location [3][5]
                    UseShellExecute = true,
                    Verb = "runas"                                                  // prompt for admin if required [1]
                };

                var proc = Process.Start(psi);
                if (proc == null)
                {
                    MessageBox.Show("Failed to start QC.exe.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Wait for QC window (up to ~4s)
                for (int i = 0; i < 40; i++)
                {
                    proc.Refresh();
                    if (proc.MainWindowHandle != IntPtr.Zero && IsWindow(proc.MainWindowHandle))
                        break;
                    await Task.Delay(100);
                }
                if (proc.MainWindowHandle == IntPtr.Zero)
                {
                    MessageBox.Show("QC window not found to send keys.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Clipboard paste for robustness
                try { Clipboard.SetText(keyText); } catch { /* ignore */ }

                // Focus QC and send Ctrl+V, Tab, Enter
                if (SetForegroundWindow(proc.MainWindowHandle))
                {
                    await Task.Delay(120);
                    SendKeys.SendWait("^v");          // paste key
                    await Task.Delay(60);
                    SendKeys.SendWait("{TAB}");       // next control
                    await Task.Delay(60);
                    SendKeys.SendWait("{ENTER}");     // submit
                }
                else
                {
                    MessageBox.Show("Could not focus QC to send keys.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("Launch canceled or denied by UAC.", "Canceled",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to launch QC.exe: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Close(); // or Application.Exit() to close all
                    }));
                }
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var path = new GraphicsPath())
            {
                int radius = 20;
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                path.AddArc(rect.Width - radius * 2, rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(rect.X, rect.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);

                using (var brush = new SolidBrush(Color.FromArgb(20, Color.White)))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Optional tray icon
            _tray = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                Visible = true,
                BalloonTipTitle = "QC ULTRA - KIWI"
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                if (_tray != null)
                {
                    _tray.Visible = false;
                    _tray.Dispose();
                    _tray = null;
                }
            }
            catch { /* ignore */ }
        }

        // More Options click
        private void btnMoreOptions_Click(object sender, EventArgs e)
        {
            // Remember current state (Normal/Maximized/Minimized)
            var prevState = this.WindowState;

            // Minimize Form1 while options are open (optional)
            this.WindowState = FormWindowState.Minimized;

            using (var dlg = new OptionsForm())
            {
                dlg.StartPosition = FormStartPosition.CenterScreen; // center on screen
                dlg.ShowInTaskbar = false;
                dlg.ShowIcon = false;

                // Show modally (no need to pass owner if centering on screen)
                dlg.ShowDialog();
            }

            // After dialog closes: restore and foreground Form1
            this.WindowState = prevState;
            this.Show();
            this.Activate();
            this.BringToFront();
        }


        private static void SafeDeleteFile(string path)
        {
            try
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
            catch { /* skip locked/denied */ }
        }

        private static void SafeDeleteDirectoryContents(string dir)
        {
            try
            {
                if (!Directory.Exists(dir)) return;

                foreach (var file in Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly))
                    SafeDeleteFile(file);                                                      // delete files first [2]

                foreach (var sub in Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        // Try full recursive delete
                        Directory.Delete(sub, true);                                           // may throw on locks [2]
                    }
                    catch
                    {
                        // Fall back: clear inside and attempt delete again
                        try
                        {
                            SafeDeleteDirectoryContents(sub);                                  // clear children [1]
                            Directory.Delete(sub, false);                                      // delete now-empty [2]
                        }
                        catch { /* skip locked/denied */ }
                    }
                }
            }
            catch { /* skip parent issues */ }
        }

        private static void TrySetHighPerformancePowerPlan()
        {
            try
            {
                const string highPerf = "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c"; // High performance GUID [3]
                var psi = new ProcessStartInfo
                {
                    FileName = "powercfg",
                    Arguments = "/setactive " + highPerf,                         // set active plan [3]
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    Verb = "runas"
                };
                using var p = Process.Start(psi);
                p?.WaitForExit(5000);
            }
            catch { /* ignore if unavailable */ }
        }

        // Optional: try to run RAMMap to empty standby list
        private static void TryEmptyStandbyWithRamMap()
        {
            try
            {
                // Resolve RAMMap path (placed next to app EXE)
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;                         // current app folder [1]
                string rammap = Path.Combine(baseDir, "RAMMap64.exe");                          // keep the file here for reliability [1]

                if (!File.Exists(rammap)) return;                                              // no-op if missing [1]

                var psi = new ProcessStartInfo
                {
                    FileName = rammap,
                    Arguments = "-Et",                                                          // Empty Standby List [2]
                    UseShellExecute = true,                                                     // needed for Verb=runas [2]
                    Verb = "runas",                                                             // request admin [2]
                    WindowStyle = ProcessWindowStyle.Hidden                                      // silent run [2]
                };
                using var p = Process.Start(psi);
                p?.WaitForExit(10000);                                                          // wait up to 10s [2]
            }
            catch { /* ignore if UAC cancelled or blocked */ }
        }

        private void optimize_Click(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;      // show wait cursor across the form
                Application.DoEvents();         // let UI update to show the cursor
                                                // ... do work ...
            }
            finally
            {
                this.UseWaitCursor = false;     // always restore
            }

            // 1) Power plan
            TrySetHighPerformancePowerPlan();                                                   // attempt high performance [3]

            // 2) Temp cleanup (existing helpers)
            var userTemp = Path.GetTempPath();                                                  // %TEMP% [4]
            SafeDeleteDirectoryContents(userTemp);
            var winTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");
            SafeDeleteDirectoryContents(winTemp);                                               // Windows\Temp [5]
            var prefetch = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Prefetch");
            SafeDeleteDirectoryContents(prefetch);                                              // Prefetch (Windows will rebuild) [6]

            // 3) Optional: Empty Standby List via RAMMap
            TryEmptyStandbyWithRamMap();                                                        // silent purge using -Et [2][1]

            Cursor = Cursors.Default;
            MessageBox.Show(
                "Optimization complete.\n• Power plan attempted: High performance\n• Temp folders cleaned\n• Prefetch cleared (skipped locked items)\n• Standby memory purged (if RAMMap64.exe present)",
                "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);                      // user feedback [1]
        }


    }
}
