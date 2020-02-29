using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyMapperLibrary
{
    public class KeyMappings
    {
        public delegate bool KeyLinstener(Keys keys, KeyboardState keyboardState);

        private readonly static KeyLinstener pureTest, unshiftTest, shiftTest, alphaTest, bettaTest;
        private readonly static KeyDictionary pureKeys, unshiftKeys, shiftKeys, alphaKeys, bettaKeys;

        static KeyMappings()
        {

            pureTest = (key, state) => !state.Any;
            pureKeys = new KeyDictionary
            {
                { Keys.OemMinus, "+" },
                { Keys.OemQuestion, "-" },
                { Keys.OemOpenBrackets, "=" },

                { Keys.Oemplus, "æ" },
                { Keys.Oem6, "ø" },
                { Keys.Oem5, "å" },

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
                { Keys.D0, Keys.OemBackslash }
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

                { Keys.OemMinus, "±" },
                { Keys.OemQuestion, "_" },
                { Keys.OemOpenBrackets, "≈" }
            };

            alphaTest = (key, state) => !state.Betta && (state.Alt || state.Ctrl || state.Alpha);
            alphaKeys = new KeyDictionary
            {
                { Keys.U, Keys.Enter },
                { Keys.I, Keys.Up },
                { Keys.O, Keys.Escape },

                { Keys.H, Keys.Home },
                { Keys.J, Keys.Left },
                { Keys.K, Keys.Down },
                { Keys.L, Keys.Right },
                { Keys.Oem1, Keys.End },

                { Keys.M, Keys.Delete },
                { Keys.Oemcomma, Keys.Down },
                { Keys.OemPeriod, Keys.Back },

                { Keys.Oemplus, "`" },
                { Keys.Oem6, "´" },
                { Keys.Oem5, "¨" },

                { Keys.OemMinus, "×" },
                { Keys.OemQuestion, "—" },
                { Keys.OemOpenBrackets, "≠" }
            };

            bettaTest = (key, state) => state.Betta;
            bettaKeys = new KeyDictionary
            {
                { Keys.U, Keys.D7 },
                { Keys.I, Keys.D8 },
                { Keys.O, Keys.D9 },
                { Keys.P, Keys.PageUp },

                { Keys.J, Keys.D4 },
                { Keys.K, Keys.D5 },
                { Keys.L, Keys.D6 },
                { Keys.Oem1, Keys.D0 },

                { Keys.M, Keys.D1 },
                { Keys.Oemcomma, Keys.D2 },
                { Keys.OemPeriod, Keys.D3 },
                { Keys.OemMinus, Keys.PageDown },

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
