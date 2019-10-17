using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace KeyMapperLibrary
{
    public static partial class Keyboard
    {
        private static readonly IntPtr MainWindowHandle;

        static Keyboard()
        {
            MainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
        }


        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100, WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104, WM_SYSKEYUP = 0x0105;

        public static IntPtr SetHook(LowLevelKeyboardProc callback)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, callback, MainWindowHandle, 0);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
