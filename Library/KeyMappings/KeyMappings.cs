using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyMapperLibrary
{
    public class KeyMappings
    {
        public delegate bool KeyLinstener(Keys keys, KeyboardState keyboardState);

        private readonly static KeyLinstener pureTest, unshiftTest, shiftTest, unctrlTest, ctrlTest, alphaTest, bettaTest;
        private readonly static KeyDictionary pureKeys, unshiftKeys, shiftKeys, unctrlKeys, ctrlKeys, alphaKeys, bettaKeys;

        static KeyMappings()
        {

            pureTest = (key, state) => !state.Any;
            pureKeys = new KeyDictionary
            {
                { Keys.OemOpenBrackets, "+" },
                { Keys.OemQuestion, "-" },
                { Keys.OemMinus, "=" },

                { Keys.Oem1, "'" }
            };

            unshiftTest = (key, state) => !state.LShift && !state.RShift;
            unshiftKeys = new KeyDictionary
            {
                { Keys.Oemtilde, "$" },
                { Keys.D1, "!" },
                { Keys.D2, "@" },
                { Keys.D3, "&" },
                { Keys.D4, "|" },
                { Keys.D5, "#" },
                { Keys.D6, "*" },
                { Keys.D7, Keys.OemQuestion },
                { Keys.D8, "(" },
                { Keys.D9, ")" },
                { Keys.D0, Keys.OemBackslash },

                { Keys.Oemplus, "æ" },
                { Keys.Oem6, "ø" },
                { Keys.Oem5, "å" }
            };

            shiftTest = (key, state) => state.LShift || state.RShift;
            shiftKeys = new KeyDictionary
            {
                { Keys.Oemtilde, "€" },
                { Keys.D1, "~" },
                { Keys.D2, "?" },
                { Keys.D3, "<" },
                { Keys.D4, ">" },
                { Keys.D5, "%" },
                { Keys.D6, "^" },
                { Keys.D7, "[" },
                { Keys.D8, "{" },
                { Keys.D9, "}" },
                { Keys.D0, "]" },

                { Keys.Oemplus, "Æ" },
                { Keys.Oem6, "Ø" },
                { Keys.Oem5, "Å" },

                { Keys.Oem1, "\"" },
                { Keys.OemPeriod, ":" },
                { Keys.Oemcomma, ";" },

                { Keys.OemOpenBrackets, "´" },
                { Keys.OemQuestion, "_" },
                { Keys.OemMinus, "`" }
            };

            unctrlTest = (key, state) => !state.Betta && !state.Ctrl && (state.Alt || state.Alpha);
            unctrlKeys = new KeyDictionary
            {
                { Keys.H, Keys.Home },
                { Keys.OemQuestion, Keys.End }
            };

            ctrlTest = (key, state) => !state.Betta && state.Ctrl;
            ctrlKeys = new KeyDictionary
            {
                { Keys.H, KS.Up(Keys.LControlKey).Down(Keys.Home).Build() },
                { Keys.OemQuestion, KS.Up(Keys.LControlKey).Down(Keys.End).Build() }
            };

            alphaTest = (key, state) => !state.Betta && (state.Alt || state.Ctrl || state.Alpha);
            alphaKeys = new KeyDictionary
            {
                { Keys.U, Keys.Enter },
                { Keys.I, Keys.Up },
                { Keys.O, Keys.Escape },
                { Keys.P, Keys.PageUp },

                { Keys.J, Keys.Left },
                { Keys.K, Keys.Down },
                { Keys.L, Keys.Right },
                { Keys.Oem1, Keys.PageDown },

                { Keys.OemOpenBrackets, "+" },
                { Keys.OemMinus, "=" },
                
                { Keys.M, Keys.Delete },
                { Keys.Oemcomma, Keys.Down },
                { Keys.OemPeriod, Keys.Back }
            };

            bettaTest = (key, state) => state.Betta;
            bettaKeys = new KeyDictionary
            {
                { Keys.U, Keys.D7 },
                { Keys.I, Keys.D8 },
                { Keys.O, Keys.D9 },
                { Keys.P, "." },

                { Keys.J, Keys.D4 },
                { Keys.K, Keys.D5 },
                { Keys.L, Keys.D6 },
                { Keys.Oem1, Keys.D0 },

                { Keys.M, Keys.D1 },
                { Keys.Oemcomma, Keys.D2 },
                { Keys.OemPeriod, Keys.D3 },
                
                // must be the same as pure to make possible enter numbers without switching modifiers
                { Keys.OemOpenBrackets, "+" },
                { Keys.OemQuestion, "-" },
                { Keys.OemMinus, "=" },

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

            Add("Pure", pureTest, pureKeys);
            Add("Unshift", unshiftTest, unshiftKeys);
            Add("Shift", shiftTest, shiftKeys);
            Add("Unctrl", unctrlTest, unctrlKeys);
            Add("Ctrl", ctrlTest, ctrlKeys);
            Add("Alpha", alphaTest, alphaKeys);
            Add("Betta", bettaTest, bettaKeys);
        }

        private void Add(string title, KeyLinstener test, KeyDictionary mappings)
        {
            Add((key, state) => {
                if (!test(key, state)) return false;
                Logger.Log("Test passed: " + title);
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
