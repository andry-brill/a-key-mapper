using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Library;

namespace KeyMapper
{
    static class Program
    {
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);

        [STAThread]
        static void Main()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);

            Logger.OnLog += Console.WriteLine;

            var process = Process.GetProcessesByName("KeyMapper");
            if (process.Length > 1 /* counting current process */)
            {
                MessageBox.Show("KeyMapper already running");
                return;
            }

            Library.KeyMapper.Start();
            Application.Run();
            Library.KeyMapper.Stop();
        }

        //    IntPtr hwnd = GetForegroundWindow();
        //    uint pid;
        //    GetWindowThreadProcessId(hwnd, out pid);
        //    Process p = Process.GetProcessById((int)pid);
        //    PostMessage(process.MainWindowHandle, WM_SYSKEYDOWN, 0x11, 0);
        //    PostMessage(process.MainWindowHandle, WM_KEYDOWN, 0x70, 0);

    }
}
