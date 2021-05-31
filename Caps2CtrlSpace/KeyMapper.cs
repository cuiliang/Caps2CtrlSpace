using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Caps2CtrlSpace
{
    public class KeyMapper
    {
        //设定一个休眠100ms的进程
        private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(100);

        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;

        private static LowLevelKeyboardProc _proc = HookCallback;

        private static IntPtr _hookID = IntPtr.Zero;

        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;

        private const uint KEYEVENTF_KEYUP = 0x0002;

        private const byte KEY_CONTROL = 17;

        private const byte KEY_SPACEBAR = 32;

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
                    //Console.WriteLine(Thread.CurrentThread.ThreadState );
                    //Thread.Sleep(Interval);

                    keybd_event(KEY_CONTROL, 0, 0, 0);
                    keybd_event(KEY_SPACEBAR, 0, 0, 0);
                    keybd_event(KEY_CONTROL, 0, KEYEVENTF_KEYUP, 0);
                    keybd_event(KEY_SPACEBAR, 0, KEYEVENTF_KEYUP, 0);

                    //SendKeys.Send("^ "); //将CapsLock转换为Ctrl+Space

                    return (IntPtr)1;//阻止功能按键消息传递
                }

            }

            //如果返回1，则结束消息，这个消息到此为止，不再传递。
            //如果返回0或调用CallNextHookEx函数则消息出了这个钩子继续往下传递，也就是传给消息真正的接受者
            return CallNextHookEx(_hookID, nCode, wParam, lParam);

        }

        [DllImport("user32.dll",CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

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
