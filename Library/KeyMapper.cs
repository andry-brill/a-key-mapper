using System;
using System.Diagnostics;

namespace KeyMapperLibrary
{
    public class KeyMapper
    {
        private static readonly KeyboardState KeyboardState;
        private static readonly KeySender KeySender;
        private static readonly KeyMappings KeyMappings;

        static KeyMapper()
        {
            KeySender = new KeySender();
            KeySender.OnBeforeSend += () => keyboardListener?.Pause(true);
            KeySender.OnAfterSend += () => keyboardListener?.Pause(false);

            KeyMappings = new KeyMappings(KeySender);

            KeyboardState = new KeyboardState();
        }

        private static KeyboardListener keyboardListener = null;

        public static void Start()
        {
            Logger.Log("Start");
            Stop();

            keyboardListener = new KeyboardListener(KeyboardState);
            foreach (var mapper in KeyMappings.Mappings)
                keyboardListener.OnKey += mapper;
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
