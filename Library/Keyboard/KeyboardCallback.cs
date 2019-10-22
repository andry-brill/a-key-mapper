using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyMapperLibrary
{
    public static partial class Keyboard
    {
        public delegate bool KeyboardCallback(Keys keys, bool isKeyDown);

        public class Callback : IDisposable
        {
            private static readonly IntPtr ONE = (IntPtr)1;

            private readonly KeyboardCallback Function;
            private readonly LowLevelKeyboardProc LowLevelKeyboardProc;
            private readonly IntPtr HookID;

            public Callback(KeyboardCallback function) {
                Function = function;
                LowLevelKeyboardProc = new LowLevelKeyboardProc(HookCallback);
                HookID = SetHook(LowLevelKeyboardProc);
            }

            private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                bool isLockScreen = Window.IsLockScreen;
                Logger.Log("HookCallback: " + nCode + " IsLockScreen: " + isLockScreen);

                if (nCode < 0 || isLockScreen) // ignoring key events during PC-lock because key-up event not triggered
                {
                    return CallNextHookEx(HookID, nCode, wParam, lParam);
                }

                bool cancel = false;
                int eventType = (int)wParam;
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (eventType == WM_KEYDOWN || eventType == WM_SYSKEYDOWN)
                {
                    Logger.Log("Down Key: " + key + " Code: " + vkCode);
                    cancel = Function(key, true);
                }

                if (eventType == WM_KEYUP || eventType == WM_SYSKEYUP)
                {
                    Logger.Log("Up Key: " + key + " Code: " + vkCode);
                    cancel = Function(key, false);
                }

                return cancel ? ONE : CallNextHookEx(HookID, nCode, wParam, lParam);
            }

            #region IDisposable Support
            private bool disposedValue = false;

            public void Dispose()
            {
                if (!disposedValue)
                {
                    UnhookWindowsHookEx(HookID);
                    disposedValue = true;
                }
            }
            #endregion
        }
    }
}
