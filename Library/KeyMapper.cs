using System;

namespace KeyMapperLibrary
{
    public class KeyMapper
    {
        private static readonly KeyboardState KeyboardState;
        private static readonly KeySender KeySender;
        private static readonly KeyMappings KeyMappings;

        static KeyMapper()
        {
            KeySender = new KeySender(ResolveLocale);
            KeySender.OnBeforeSend += () => keyboardListener?.Pause(true);
            KeySender.OnAfterSend += () => keyboardListener?.Pause(false);

            KeyMappings = new KeyMappings(KeySender);

            KeyboardState = new KeyboardState();
        }

        private static KeyboardLocale ResolveLocale(int keyboardLayout)
        {
            return keyboardLayout == (int)KeyboardLocale.uk_UA ? KeyboardLocale.uk_UA : KeyboardLocale.en_US;
        }

        private static KeyboardListener keyboardListener = null;

        public static void Start()
        {
            Stop(); // releasing resources

            Logger.Log("Start");
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
