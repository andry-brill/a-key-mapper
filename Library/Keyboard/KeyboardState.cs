using System.Windows.Forms;
using System.Diagnostics;

namespace KeyMapperLibrary
{
    public class KeyboardState
    {
        private readonly Stopwatch Stopwatch;

        public KeyboardState()
        {
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        private void PreventSticking()
        {
            // usually period of key down events is 33ms
            // but after press another key they stops, so
            // to prevent sticking of keys will be used 7s as inactivivity 
            if (Stopwatch.ElapsedMilliseconds > 7000)
            {
                Alt = Ctrl = LShift = RShift = Alpha = Betta = false;
            }
            Stopwatch.Restart();
        }

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
            PreventSticking();

            if (!key.IsModifier()) return;

            Alt = key.IsAlt() ? isKeyDown : Alt;
            LShift = key.IsLShift() ? isKeyDown : LShift;
            RShift = key.IsRShift() ? isKeyDown : RShift;
            Ctrl = key.IsCtrl() ? isKeyDown : Ctrl;
            Alpha = key.IsAlpha() ? isKeyDown : Alpha;
            Betta = key.IsBetta() ? isKeyDown : Betta;
                
        }

    }

}
