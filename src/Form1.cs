using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Caps2CtrlSpace
{
    public partial class Form1 : Form
    {
        private const string AppName = "Caps2CtrlSpace";

        public Form1()
        {
            InitializeComponent();
        }


        private void DoHide()
        {
            NotifyIcon1.Visible = true;
            Hide();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Minimized:
                    DoHide();
                    break;
                case FormWindowState.Normal:
                    NotifyIcon1.Visible = false;
                    break;
                case FormWindowState.Maximized:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e));
            }
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            NotifyIcon1.Visible = false;
        }


        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (registryKey is null) throw new NullReferenceException(nameof(registryKey));
                if (ChkAutoRun.Checked)
                    registryKey.SetValue(AppName, Application.ExecutablePath);
                else
                    registryKey.DeleteValue(AppName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"Failed," + exception.Message);
            }
        }

        /// <summary>
        ///     是否自动启动
        /// </summary>
        /// <returns></returns>
        private static bool IsAutoStart()
        {
            var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            var curValue = rk.GetValue(AppName);
            Console.WriteLine(curValue);

            return curValue != null && curValue.ToString() == Application.ExecutablePath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChkAutoRun.Checked = IsAutoStart();
            DoHide();
        }
    }
}