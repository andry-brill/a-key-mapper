using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using WindowsInput;

namespace Library
{

    public class KeysSender
    {
        private readonly InputSimulator InputSimulator = new InputSimulator();

        public delegate void SendEvent();
        public event SendEvent OnBeforeSend;
        public event SendEvent OnAfterSend;

        private void InvokeEvent(SendEvent sendEvent)
        {
            sendEvent?.Invoke();
        }

        public void SendKey(Keys key, params KeyModifier[] modifiers)
        {
            string sKey = key.ToString().ToUpper();
            if (key.IsNumber()) sKey = sKey.Replace("D", "");
            
            if (key == Keys.PageDown) sKey = "PGDN";
            if (key == Keys.PageUp) sKey = "PGUP";
            if (key == Keys.Back) sKey = "BKSP";
            if (key == Keys.Enter) sKey = "ENTER";

            string sModifiers = string.Join("", modifiers.Select(m => m.ToKey()).ToArray());
            
            sKey = sModifiers + "{" + sKey + "}";
            
            Logger.Log("Sending key: " + sKey);
            InvokeEvent(OnBeforeSend);
            SendKeys.Send(sKey);
            InvokeEvent(OnAfterSend);
        }

        public void SendText(string text)
        {
            Logger.Log("Sending text: " + text);
            InvokeEvent(OnBeforeSend);
            InputSimulator.Keyboard.TextEntry(text);
            InvokeEvent(OnAfterSend);
        }

        public bool SendMappedKey(Keys key, Dictionary<Keys, Keys> mappings)
        {
            if (mappings.TryGetValue(key, out Keys mapped))
            {
                SendKey(mapped);
                return true;
            }
            return false;
        }

        public bool SendMappedText(Keys key, Dictionary<Keys, string> mappings)
        {
            if (mappings.TryGetValue(key, out string mapped))
            {
                SendText(mapped);
                return true;
            }
            return false;
        }

    }
}
