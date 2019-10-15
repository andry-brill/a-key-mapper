using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class Logger
    {

        public static event Action<string> OnLog;

        public static void Log(string message)
        {
            if (OnLog != null) OnLog(message);
        }
    }
}
