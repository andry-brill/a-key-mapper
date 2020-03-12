using System;
using System.Windows.Forms;
using WindowsInput;

namespace KeyMapperLibrary
{

    public class KeySender
    {
        private readonly IKeyboardSimulator Keyboard;
        private readonly Func<int, KeyboardLocale> KeyboardLocaleResolver;

        public event Action OnBeforeSend;
        public event Action OnAfterSend;

        public KeySender(Func<int, KeyboardLocale> keyboardLocaleResolver)
        {
            KeyboardLocaleResolver = keyboardLocaleResolver;
            Keyboard = new InputSimulator().Keyboard;
        }

        private void Send<T>(Action<T> action, T param)
        {
            OnBeforeSend?.Invoke();
            action(param);
            OnAfterSend?.Invoke();
        }

        private void IntKeys(int[] aKey)
        {
            foreach (int k in aKey)
            {
                Keys key = (Keys)Math.Abs(k);
                if (k < 0)
                {
                    Logger.Log("Sending key up: " + key);
                    Keyboard.KeyUp(key.ToVitual());
                } else
                {
                    Logger.Log("Sending key down: " + key);
                    Keyboard.KeyDown(key.ToVitual());
                }
            }
        }

        private void KeyPress(Keys key)
        {
            Logger.Log("Sending key: " + key);
            Keyboard.KeyPress(key.ToVitual());
        }

        private void Text(string text)
        {
            Logger.Log("Sending text: " + text);
            Keyboard.TextEntry(text);
        }

        public bool Send(Keys key, KeyDictionary mappings, int keyboardLayout)
        {
            KeyboardLocale locale = KeyboardLocaleResolver(keyboardLayout);
            return mappings.TryGetValue(key, out object mapped) && Send(mapped, locale);
        }

        private bool Send(object key, KeyboardLocale locale)
        {

            if (key is Keys mKey)
            {
                if (mKey != Keys.None) Send(KeyPress, mKey);
            }
            else if (key is string sKey)
            {
                Send(Text, sKey);
            }
            else if (key is int[] aKey)
            {
                Send(IntKeys, aKey);
            }
            else if (key is LocaleDictionary ld)
            {
                if (ld.TryGetValue(locale, out object loKey))
                {
                    return Send(loKey, locale);
                } else if (ld.TryGetValue(KeyboardLocale.ANY_OTHER, out object aoKey)) {
                    return Send(aoKey, locale);
                } else
                {
                    return false;
                }
            } 
            else
            {
                throw new Exception("Unsupported type: " + key.GetType());
            }

            return true;
        }


    }
}
