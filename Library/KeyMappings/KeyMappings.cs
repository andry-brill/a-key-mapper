using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyMapperLibrary
{
    public class KeyMappings
    {
        public delegate bool KeyLinstener(Keys keys, KeyboardState keyboardState);

        private readonly static KeyLinstener  lShiftCheck, shiftCheck, disableCheck, pureCheck, alphaCheck, bettaCheck;
        private readonly static KeyDictionary lShiftKeys,  shiftKeys,  disableKeys,  pureKeys,  alphaKeys,  bettaKeys;

        static KeyMappings()
        {
            lShiftCheck = (key, state) => state.LShift && !state.Alpha && !state.Betta && !state.Alt && !state.Ctrl;
            lShiftKeys = new KeyDictionary();
            foreach (var key in new [] { 
                Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, 
                Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, 
                Keys.Z, Keys.X, Keys.C, Keys.V, 
                Keys.Space 
            })
            {
                lShiftKeys.Add(key, KS.Up(Keys.LShiftKey).Down(Keys.LControlKey, key).Build());
            }

            shiftCheck = (key, state) => state.LShift || state.RShift;
            shiftKeys = new KeyDictionary
            {
                { Keys.OemQuestion, "\"" }
            };

            disableCheck = (key, state) => true;
            disableKeys = new KeyDictionary
            {
                { Keys.Delete, Keys.None },
                { Keys.Back, Keys.None },
                { Keys.Enter, Keys.None },

                { Keys.Up, Keys.None },
                { Keys.Left, Keys.None },
                { Keys.Down, Keys.None },
                { Keys.Right, Keys.None }
            };

            pureCheck = (key, state) => !state.Any;
            pureKeys = new KeyDictionary
            {
                { Keys.D1, "!" },
                { Keys.D2, "@" },
                { Keys.D3, "#" },
                { Keys.D4, "$" },
                { Keys.D5, "*" },
                { Keys.D6, "&" },
                { Keys.D7, "/" },
                { Keys.D8, "(" },
                { Keys.D9, ")" },
                { Keys.D0, "=" }
            };

            alphaCheck = (key, state) => !state.Betta && (state.Alt || state.Ctrl || state.Alpha);
            alphaKeys = new KeyDictionary
            {
                { Keys.Y, Keys.Escape },
                { Keys.U, Keys.Enter },
                { Keys.I, Keys.Up },
                { Keys.O, Keys.PageUp },

                { Keys.H, Keys.Home },
                { Keys.J, Keys.Left },
                { Keys.K, Keys.Down },
                { Keys.L, Keys.Right },
                { Keys.Oemtilde, Keys.End },

                { Keys.M, Keys.Delete },
                { Keys.Oemcomma, Keys.Back },
                { Keys.OemPeriod, Keys.PageDown },

                { Keys.D7, "{" },
                { Keys.D8, "[" },
                { Keys.D9, "]" },
                { Keys.D0, "}" }
            };

            bettaCheck = (key, state) => state.Betta;
            bettaKeys = new KeyDictionary
            {
                { Keys.U, Keys.D7 },
                { Keys.I, Keys.D8 },
                { Keys.O, Keys.D9 },

                { Keys.H, "%" },
                { Keys.J, Keys.D4 },
                { Keys.K, Keys.D5 },
                { Keys.L, Keys.D6 },
                { Keys.Oemtilde, Keys.D0 },

                { Keys.M, Keys.D1 },
                { Keys.Oemcomma, Keys.D2 },
                { Keys.OemPeriod, Keys.D3 },

                // for Git Bash
                { Keys.C, KS.Down(Keys.LControlKey, Keys.Insert).Build() },
                { Keys.V, KS.Down(Keys.LShiftKey, Keys.Insert).Build() },
            };
        }

        private readonly KeySender KeysSender;
        public readonly List<OnKeyListener> Mappings = new List<OnKeyListener>();

        public KeyMappings(KeySender keysSender)
        {
            KeysSender = keysSender;

            Add((key, state) => key.IsAlpha() || key.IsBetta());

            Add("Shift", shiftCheck, shiftKeys);
            Add("LShift", lShiftCheck, lShiftKeys);
            Add("Disables", disableCheck, disableKeys);
            Add("Pure", pureCheck, pureKeys);
            Add("Alpha", alphaCheck, alphaKeys);
            Add("Betta", bettaCheck, bettaKeys);
        }

        private void Add(string title, KeyLinstener check, KeyDictionary mappings)
        {
            Add((key, state) => {
                if (!check(key, state)) return false;
                Logger.Log("Check passed: " + title);
                return KeysSender.Send(key, mappings);
            });
        }
        
        private void Add(KeyLinstener keyLinstener)
        {
            Mappings.Add(e =>
            {
                if (!e.Cancel) e.Cancel = keyLinstener(e.Key, e.KeyboardState);
            });
        }

    }
}
