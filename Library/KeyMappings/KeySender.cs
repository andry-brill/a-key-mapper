using System;
using System.Windows.Forms;
using WindowsInput;

namespace KeyMapperLibrary
{

    public class KeySender
    {
        private readonly IKeyboardSimulator Keyboard = new InputSimulator().Keyboard;

        public event Action OnBeforeSend;
        public event Action OnAfterSend;

        private void BeginSending()
        {
            OnBeforeSend?.Invoke();
        }
        private void EndSending()
        {
            OnAfterSend?.Invoke();
        }

        private void KeyDown(Keys key)
        {
            Logger.Log("Sending key down: " + key);
            Keyboard.KeyDown(key.ToVitual());
        }

        private void KeyUp(Keys key)
        {
            Logger.Log("Sending key up: " + key);
            Keyboard.KeyUp(key.ToVitual());
        }

        private void SendKeys(int[] aKey)
        {
            foreach (int k in aKey)
            {
                if (k < 0) KeyUp((Keys)(-k));
                if (k >= 0) KeyDown((Keys)k);
            }
        }

        private void SendKey(Keys key)
        {
            Logger.Log("Sending key: " + key);
            Keyboard.KeyPress(key.ToVitual());
        }

        private void SendText(string text)
        {
            Logger.Log("Sending text: " + text);
            Keyboard.TextEntry(text);
        }

        public bool Send(Keys key, KeyDictionary mappings)
        {
            return mappings.TryGetValue(key, out object mapped) && Send(mapped);
        }

        public bool Send(object key)
        {
            BeginSending();

            if (key is Keys mKey)
            {
                if (mKey != Keys.None) SendKey(mKey);
            }
            else if (key is string sKey)
            {
                SendText(sKey);
            }
            else if (key is int[] aKey)
            {
                SendKeys(aKey);
            }
            else
            {
                throw new Exception("Unsupported type: " + key.GetType());
            }

            EndSending();

            return true;
        }


    }
}
