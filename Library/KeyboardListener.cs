using System;

namespace Library
{
    public class KeyboardListener: IDisposable
    {
        // KeyboardListener start stop onKey
        private const int WM_KEYDOWN = 0x0100, WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104, WM_SYSKEYUP = 0x0105;

        public event Action<KeyboardEvent> OnKey;
        
        private readonly IntPtr HookID;
        private readonly KeyboardState KeyboardState;
        private readonly Keyboard.LowLevelKeyboardProc LowLevelKeyboardProc;

        public bool Pause { get; set; } = false;

        public static void TogglePause(KeyboardListener keyboardListener, bool pause)
        {
            if (keyboardListener != null) keyboardListener.Pause = pause;
        }

        public KeyboardListener(KeyboardState keyboardState)
        {
            KeyboardState = keyboardState;
            LowLevelKeyboardProc = new Keyboard.LowLevelKeyboardProc(HookCallback);
            HookID = Keyboard.SetHook(LowLevelKeyboardProc);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, ref Keyboard.KBDLLHOOKSTRUCT lParam)
        {
            Logger.Log("HookCallback");

            if (nCode < 0 || OnKey == null || Pause)
            {
                return Keyboard.CallNextHookEx(HookID, nCode, wParam, ref lParam);
            }

            var eventType = (int)wParam;
            switch (eventType)
            {
                case WM_SYSKEYDOWN:
                case WM_KEYDOWN:
                    Logger.Log("Keydown " + lParam);
                    KeyboardState.UpdateState(lParam.Key, true);

                    var keyboardEvent = new KeyboardEvent(lParam.Key, KeyboardState);
                    OnKey(keyboardEvent);

                    if (keyboardEvent.Cancel)
                    {
                        return (IntPtr)1;
                    }
                    break;
                case WM_SYSKEYUP:
                case WM_KEYUP:
                    Logger.Log("Keyup " + lParam);
                    KeyboardState.UpdateState(lParam.Key, false);
                    break;
            }

            return Keyboard.CallNextHookEx(HookID, nCode, wParam, ref lParam);

        }

        #region IDisposable Support
        private bool disposedValue = false;

        public void Dispose()
        {
            if (!disposedValue)
            {
                Keyboard.UnhookWindowsHookEx(HookID);
                disposedValue = true;
            }
        }

        #endregion
    }
}
