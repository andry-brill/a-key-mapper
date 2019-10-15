using System;
using System.Diagnostics;

namespace Library
{
    public class KeyMapper
    {
        private static readonly KeyboardState KeyboardState;
        private static readonly KeysSender KeysSender;
        private static readonly KeysMappings KeysMappings;

        static KeyMapper()
        {
            KeysSender = new KeysSender();
            KeysSender.OnBeforeSend += () => KeyboardListener.TogglePause(keyboardListener, true);
            KeysSender.OnAfterSend += () => KeyboardListener.TogglePause(keyboardListener, false);

            KeysMappings = new KeysMappings(KeysSender);

            KeyboardState = new KeyboardState();
        }

        private static KeyboardListener keyboardListener = null;

        public static void Start()
        {
            Logger.Log("Start");
            Stop();

            keyboardListener = new KeyboardListener(KeyboardState);
            keyboardListener.OnKey += KeysMappings.OnKey;

        }

        public static void Stop()
        {
            if (keyboardListener != null)
            {
                Logger.Log("Stop");
                keyboardListener.Dispose();
                keyboardListener = null;
            }
        }

    }
}
