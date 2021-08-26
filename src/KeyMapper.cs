using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Caps2CtrlSpace
{
    public class KeyMapper
    {
        private const int WhKeyboardLl = 13;

        private const int WmKeydown = 0x0100;

        private const uint KeyEventFlagExtendedKey = 0x0001;

        private const uint KeyEventFlagKeyUp = 0x0002;

        private const byte KeyControl = 17;

        private const byte KeySpace = 32;

        private static readonly LowLevelKeyboardProc Proc = HookCallback;

        private static nint _hookId;

        public static void SetupHook()
        {
            _hookId = SetHook(Proc);
        }


        ~KeyMapper()
        {
            if (_hookId != 0) UnhookWindowsHookEx(_hookId);
        }


        private static nint SetHook(LowLevelKeyboardProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            if (curModule is null) throw new NullReferenceException(nameof(curProcess.MainModule));
            return SetWindowsHookEx(WhKeyboardLl, proc, GetModuleHandle(curModule.ModuleName), 0);
        }


        private static nint HookCallback(int nCode, nint wParam, nint lParam)
        {
            if (nCode < 0 || wParam != WmKeydown)
            {
                return CallNextHookEx(_hookId, nCode, wParam, lParam);
            }

            var vkCode = Marshal.ReadInt32(lParam);
            if ((Keys)vkCode != Keys.Capital)
            {
                return CallNextHookEx(_hookId, nCode, wParam, lParam);
            }

            // 如果按下了控制键，则不执行输入法切换。
            if (Control.ModifierKeys == Keys.None)
            {
                keybd_event(KeyControl, 0, 0, 0);
                keybd_event(KeySpace, 0, 0, 0);
                keybd_event(KeyControl, 0, KeyEventFlagKeyUp, 0);
                keybd_event(KeySpace, 0, KeyEventFlagKeyUp, 0);

                return 1; //阻止功能按键消息传递
            }
            else
            {
                return 0;
            }

           

           

            //如果返回1，则结束消息，这个消息到此为止，不再传递。
            //如果返回0或调用CallNextHookEx函数则消息出了这个钩子继续往下传递，也就是传给消息真正的接受者
        }

        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, nint hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(nint hhk);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern nint GetModuleHandle(string lpModuleName);


        private delegate nint LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);
    }
}