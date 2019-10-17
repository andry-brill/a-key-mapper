using System.Windows.Forms;

namespace KeyMapperLibrary
{
 
    public class KeyboardEvent
    {
        public KeyboardState KeyboardState { get; private set; }
        public Keys Key { get; private set; }

        public bool Cancel { get; set; } = false;

        public KeyboardEvent(Keys key, KeyboardState keyboardState)
        {
            Key = key;
            KeyboardState = keyboardState;
        }

    }


}
