using System.Windows.Forms;
using System.Diagnostics;

namespace KeyMapperLibrary
{
    public class KeyboardState
    {

        public bool Alt { get; private set; } = false;
        public bool Ctrl { get; private set; } = false;
        public bool LShift { get; private set; } = false;
        public bool RShift { get; private set; } = false;
        public bool Alpha { get; private set; } = false;
        public bool Betta { get; private set; } = false;

        public bool Any { 
            get {
                return Alt || Ctrl || RShift || LShift || Alpha || Betta;
            } 
        }

        public void UpdateState(Keys key, bool isKeyDown)
        {
            Alt = key.IsAlt() ? isKeyDown : Alt;
            LShift = key.IsLShift() ? isKeyDown : LShift;
            RShift = key.IsRShift() ? isKeyDown : RShift;
            Ctrl = key.IsCtrl() ? isKeyDown : Ctrl;
            Alpha = key.IsAlpha() ? isKeyDown : Alpha;
            Betta = key.IsBetta() ? isKeyDown : Betta;    
        }

    }

}
