using System;
using System.Windows.Forms;

namespace KeyMapperLibrary
{
    public delegate void OnKeyListener(KeyboardEvent keyboardEvent);

    public class KeyboardListener: IDisposable
    {
        public event OnKeyListener OnKey;

        private readonly KeyboardState KeyboardState;
        private readonly Keyboard.Callback KeyboardCallback;

        private bool paused = false;
        public void Pause(bool p)
        {
            paused = p;
        }

        public KeyboardListener(KeyboardState keyboardState)
        {
            KeyboardState = keyboardState;
            KeyboardCallback = new Keyboard.Callback(Callback);
        }

        private bool Callback(Keys key, bool isKeyDown, int keyboardLayout)
        {
            KeyboardState.UpdateState(key, isKeyDown, keyboardLayout);

            if (OnKey == null || paused)
            {
                Logger.Log("Ignored");
                return false;
            }
            
            if (isKeyDown)
            {
                Logger.Log("OnKey: " + key);

                var keyboardEvent = new KeyboardEvent(key, KeyboardState);
                OnKey(keyboardEvent);
                return keyboardEvent.Cancel;
            }

            return false;
        }

        public void Dispose()
        {
            KeyboardCallback.Dispose();
        }

    }
}
