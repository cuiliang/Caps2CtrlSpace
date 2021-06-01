using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Caps2CtrlSpace
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            KeyMapper.SetupHook();

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}