using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using KeyMapperLibrary;

namespace KeyMapperApplication
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

            KeyMapper.Start();
            Application.Run();
            KeyMapper.Stop();
        }

    }
}
