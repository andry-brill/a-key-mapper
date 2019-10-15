using System.Windows.Forms;

namespace Library
{
    public static class KeysExtensions
    {
        public static string ToKey(this KeyModifier modifier)
        {
            switch (modifier)
            {
                case KeyModifier.Alt:
                    return "%";
                case KeyModifier.Ctrl:
                    return "^";
                case KeyModifier.Shift:
                    return "+";
                default:
                    return "";

            }
        }

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

        public static bool IsShift(this Keys key)
        {
            switch (key)
            {
                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    return true;
                default:
                    return false;
            }
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
        public static bool IsBetta(this Keys key)
        {
            return key == Keys.F14;
        }

        public static bool IsModifier(this Keys key)
        {
            return key.IsAlt() || key.IsCtrl() || key.IsShift() || key.IsAlpha() || key.IsBetta();
        }

        public static bool IsNumber (this Keys key)
        {
            return Keys.D0 <= key && key <= Keys.D9;
        }
    }
}
