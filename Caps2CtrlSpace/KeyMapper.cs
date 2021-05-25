using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caps2CtrlSpace
{
    public class KeyMapper
    {
        private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(100);

        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;

        private static LowLevelKeyboardProc _proc = HookCallback;

        private static IntPtr _hookID = IntPtr.Zero;

        public void SetupHook()
        {
            _hookID = SetHook(_proc);
        }


        ~KeyMapper()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
            }
        }


        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {

                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);

            }

        }


        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);


        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {

                int vkCode = Marshal.ReadInt32(lParam);
                if ((Keys)vkCode == Keys.Capital)
                {
                    SendKeys.Send("^ "); //将CapsLock转换为Ctrl+Space
                    System.Threading.Thread.Sleep(Interval);

                    return (IntPtr)1;
                }

            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);

        }



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        //[DllImport("user32.dll")]
        //static extern IntPtr GetForegroundWindow();
        //[DllImport("user32.dll")]
        //static extern uint GetWindowThreadProcessId(IntPtr hwnd, IntPtr proccess);
        //[DllImport("user32.dll")]
        //static extern IntPtr GetKeyboardLayout(uint thread);
        //public static CultureInfo GetCurrentKeyboardLayout()
        //{
        //    try
        //    {
        //        IntPtr foregroundWindow = GetForegroundWindow();
        //        uint foregroundProcess = GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
        //        var layout = GetKeyboardLayout(foregroundProcess);
        //        Console.WriteLine(layout);
        //        int keyboardLayout = layout.ToInt32() & 0xFFFF;
        //        return new CultureInfo(keyboardLayout);
        //    }
        //    catch (Exception _)
        //    {
        //        return new CultureInfo(1033); // Assume English if something went wrong.
        //    }
        //}



        //[DllImport("imm32.dll")]
        //public static extern IntPtr ImmGetContext(IntPtr hWnd);

        //[DllImport("Imm32.dll")]
        //public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        //[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        //private static extern int ImmGetCompositionStringW(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);

        //[DllImport("Imm32.dll")]
        //public static extern bool ImmGetOpenStatus(IntPtr hIMC);
    }



}
