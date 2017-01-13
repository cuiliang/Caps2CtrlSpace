using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Caps2CtrlSpace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void DoHide()
        {
            notifyIcon1.Visible = true;
            this.Hide();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                DoHide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
           ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (chkAutoRun.Checked)
                {
                    rk.SetValue(AppName, Application.ExecutablePath.ToString());
                }
                else
                {
                    rk.DeleteValue(AppName);
                }

               
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed," + exception.Message);
            }

            
            
        }

        private const string AppName = "Caps2CtrlSpace";

        /// <summary>
        /// 是否自动启动
        /// </summary>
        /// <returns></returns>
        private bool IsAutoStart()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            var currValue = rk.GetValue(AppName);
            Console.WriteLine(currValue);

            return currValue != null && currValue.ToString() == Application.ExecutablePath.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chkAutoRun.Checked = IsAutoStart();
            DoHide();
        }
    }
}
