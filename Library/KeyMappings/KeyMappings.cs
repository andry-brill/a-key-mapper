using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyMapperLibrary
{
    public class KeyMappings
    {
        public delegate bool KeyLinstener(Keys keys, KeyboardState keyboardState);

        private readonly KeySender KeysSender;
        public readonly List<OnKeyListener> Mappings = new List<OnKeyListener>();

        public KeyMappings(KeySender keysSender)
        {
            KeysSender = keysSender;

            /* Disabling any combinations with alpha and beta keys */
            Add((key, state) => key.IsAlpha() || key.IsBeta());

            KeyDictionary pure = Add("Pure",
                (key, state) => !state.Any,
                new KeyDictionary
                {
                    { Keys.OemOpenBrackets, "+" },
                    { Keys.OemQuestion, "-" },
                    { Keys.OemMinus, "=" },

                    { Keys.Oem1, "'" },

                    { Keys.Oemplus, AnyUa("æ", "є") },
                    { Keys.Oem6, AnyUa("ø", "щ") },
                    { Keys.Oem5, AnyUa("å", "ш") },

                    { Keys.OemPeriod, "." },
                    { Keys.Oemcomma, "," }
                }
            );

            Add("Unshift",
                (key, state) => !state.Shift,
                new KeyDictionary{
                    { Keys.Oemtilde, "$" },
                    { Keys.D1, "!" },
                    { Keys.D2, "@" },
                    { Keys.D3, "&" },
                    { Keys.D4, "|" },
                    { Keys.D5, "#" },
                    { Keys.D6, "*" },
                    { Keys.D7, AnyUa(Keys.OemQuestion, "/") },
                    { Keys.D8, "(" },
                    { Keys.D9, ")" },
                    { Keys.D0, Keys.OemBackslash }
                }
            );

            Add("Shift", 
                (key, state) => state.Shift,
                new KeyDictionary
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
                    { Keys.D0, "]" }

                }
            );

            KeyDictionary pureShift = Add("PureShift", 
                (key, state) => state.Shift && !state.Beta && !state.Alpha && !state.Ctrl,
                new KeyDictionary
                {
                    { Keys.Oem1, "\"" },

                    { Keys.OemQuestion, "_" },
                    { Keys.OemPeriod, ":" },
                    { Keys.Oemcomma, ";" },

                    { Keys.OemOpenBrackets, "±" },
                    { Keys.OemMinus, "≈" },

                    { Keys.Oemplus, AnyUa("Æ", "Є")},
                    { Keys.Oem6, AnyUa("Ø", "Щ") },
                    { Keys.Oem5, AnyUa("Å", "Ш") }
                }
            );

            Add("Unctrl", 
                (key, state) => !state.Beta && !state.Ctrl && (state.Alt || state.Alpha),
                new KeyDictionary
                {
                    { Keys.H, Keys.Home },
                    { Keys.OemQuestion, Keys.End }
                }
            );
            
            Add("Ctrl", 
                (key, state) => !state.Beta && state.Ctrl,
                new KeyDictionary
                {
                    { Keys.H, KS.Up(Keys.LControlKey).Down(Keys.Home).Build() },
                    { Keys.OemQuestion, KS.Up(Keys.LControlKey).Down(Keys.End).Build() }
                }
            );

            Add("Alpha",
                (key, state) => !state.Beta && (state.Alt || state.Ctrl || state.Alpha),
                new KeyDictionary
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
                }
            );

            KeyDictionary beta = Add("Beta", 
                (key, state) => state.Beta,
                new KeyDictionary
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

                    { Keys.Oemplus, AnyUa("æ", "И")},
                    { Keys.Oem6, AnyUa("ø", "Ь") },
                    { Keys.Oem5, AnyUa("å", "Ї") }

                }
            );


            void addUa(Keys key, string letter, string betaLetter = null)
            {
                AddUa(pure, key, letter.ToLower());
                AddUa(pureShift, key, letter.ToUpper());
                if (betaLetter != null) AddUa(beta, key, betaLetter);
            }

            addUa(Keys.Q, "Я");
            addUa(Keys.W, "Ж");
            addUa(Keys.E, "Е");
            addUa(Keys.R, "Р");
            addUa(Keys.T, "Т");
            addUa(Keys.Y, "У");
            
            addUa(Keys.U, "Ю");
            addUa(Keys.I, "І");
            addUa(Keys.O, "О");
            addUa(Keys.P, "П");
            
            addUa(Keys.A, "А");
            addUa(Keys.S, "С");
            addUa(Keys.D, "Д");
            addUa(Keys.F, "Ф");
            addUa(Keys.G, "Г");
            addUa(Keys.H, "Х");
            addUa(Keys.J, "Й");
            addUa(Keys.K, "К");
            addUa(Keys.L, "Л");

            addUa(Keys.Z, "З");
            addUa(Keys.X, "Ч");
            addUa(Keys.C, "Ц");
            addUa(Keys.V, "В");
            addUa(Keys.B, "Б");
            addUa(Keys.N, "Н");
            addUa(Keys.M, "М");

            AddAnyUa(pure, Keys.Oem7, "`", "и");
            AddAnyUa(pureShift, Keys.Oem7, "´", "ь");
            AddUa(beta, Keys.Oem7, "ї");

        }

        private static void AddUa(KeyDictionary dictionary, Keys key, string letter)
        {
            if (dictionary.ContainsKey(key))
            {
                throw new Exception("Duplicate key: " + key);
            } else
            {
                dictionary.Add(key, Ua(letter));
            }
        }

        private static void AddAnyUa(KeyDictionary dictionary, Keys key, object any, string letter)
        {
            if (dictionary.ContainsKey(key))
            {
                throw new Exception("Duplicate key: " + key);
            }
            else
            {
                dictionary.Add(key, AnyUa(any, letter));
            }
        }

        private static LocaleDictionary AnyUa(object any, object ua)
        {
            return new LocaleDictionary
            {
                { KeyboardLocale.ANY_OTHER, any },
                { KeyboardLocale.uk_UA, ua }
            };
        }

        private static LocaleDictionary Ua(object ua)
        {
            return new LocaleDictionary
            {
                { KeyboardLocale.uk_UA, ua }
            };
        }

        private KeyDictionary Add(string title, KeyLinstener test, KeyDictionary mappings)
        {
            Add((key, state) =>
            {
                if (!test(key, state)) return false;
                Logger.Log("Test passed: " + title);
                return KeysSender.Send(key, mappings, state.Layout);
            });

            return mappings;
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
