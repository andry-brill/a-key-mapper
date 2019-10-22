using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KeyMapperLibrary
{
    public static class Window
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return string.Empty;
        }

        // Defautl title of the lock screen is: Windows Default Lock Screen
        public static bool IsLockScreen {
            get {         
                return GetActiveWindowTitle().Contains("Lock Screen");
            } 
        }
    }
}
