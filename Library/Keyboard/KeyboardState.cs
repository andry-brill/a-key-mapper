using System.Windows.Forms;
using System.Diagnostics;

namespace KeyMapperLibrary
{

    public class KeyboardState
    {
        public int Layout { get; private set; }

        public bool Alt { get; private set; } = false;
        public bool Ctrl { get; private set; } = false;
        public bool Shift { get; private set; } = false;
        public bool Alpha { get; private set; } = false;
        public bool Beta { get; private set; } = false;

        public bool Any { 
            get {
                return Alt || Ctrl || Shift || Alpha || Beta;
            } 
        }

        public void UpdateState(Keys key, bool isKeyDown, int layout)
        {
            Layout = layout;

            Alt = key.IsAlt() ? isKeyDown : Alt;
            Shift = key.IsLShift() || key.IsRShift() ? isKeyDown : Shift;
            Ctrl = key.IsCtrl() ? isKeyDown : Ctrl;
            Alpha = key.IsAlpha() ? isKeyDown : Alpha;
            Beta = key.IsBeta() ? isKeyDown : Beta;
        }

    }

}
