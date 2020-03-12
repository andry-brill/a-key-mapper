using System;
using System.Windows.Forms;
using WindowsInput.Native;

namespace KeyMapperLibrary
{
    public static class KeysExtensions
    {

        public static bool IsAlt(this Keys key)
        {
            switch (key)
            {
                case Keys.Alt:
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLShift(this Keys key)
        {
            return key == Keys.LShiftKey;
        }

        public static bool IsRShift(this Keys key)
        {
            return key == Keys.RShiftKey;
        }

        public static bool IsCtrl(this Keys key)
        {
            switch (key)
            {
                case Keys.Control:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.ControlKey:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAlpha(this Keys key)
        {
            return key == Keys.F13;
        }
        public static bool IsBeta(this Keys key)
        {
            return key == Keys.F14;
        }

        public static bool IsNumber (this Keys key)
        {
            return Keys.D0 <= key && key <= Keys.D9;
        }

        public static VirtualKeyCode ToVitual(this Keys key)
        {
            switch (key)
            {
                case Keys.None:
                case Keys.LineFeed:
                case Keys.KeyCode:
                case Keys.Modifiers:
                case Keys.Alt:
                case Keys.Control:
                case Keys.Shift:
                    throw new Exception("Unsupported key: " + key);
                default:
                    return (VirtualKeyCode)key;
            }
        }
    }
}
