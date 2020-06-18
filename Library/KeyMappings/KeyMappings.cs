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

                    { Keys.Oemplus, AnyRu("æ", "э") },
                    { Keys.Oem6, AnyRu("ø", "щ") },
                    { Keys.Oem5, AnyRu("å", "ш") },

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
                    { Keys.D7, AnyRu(Keys.OemQuestion, "/") },
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

                    { Keys.OemOpenBrackets, "´" },
                    { Keys.OemMinus, "`" },

                    { Keys.Oemplus, AnyRu("Æ", "Э")},
                    { Keys.Oem6, AnyRu("Ø", "Щ") },
                    { Keys.Oem5, AnyRu("Å", "Ш") }
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

                    { Keys.Oemplus, AnyRu("Æ", "Ъ")},
                    { Keys.Oem6, AnyRu("Ø", "Ы") },
                    { Keys.Oem5, AnyRu("Å", "Ь") }

                }
            );


            void addRu(Keys key, string letter)
            {
                AddRu(pure, key, letter.ToLower());
                AddRu(pureShift, key, letter.ToUpper());
            }

            addRu(Keys.Q, "Я");
            addRu(Keys.W, "Ж");
            
            addRu(Keys.E, "Е");
            AddRu(beta, Keys.E, "ё");

            addRu(Keys.R, "Р");
            addRu(Keys.T, "Т");
            addRu(Keys.Y, "У");
            
            addRu(Keys.U, "Ю");
            addRu(Keys.I, "И");
            addRu(Keys.O, "О");
            addRu(Keys.P, "П");
            
            addRu(Keys.A, "А");
            addRu(Keys.S, "С");
            addRu(Keys.D, "Д");
            addRu(Keys.F, "Ф");
            addRu(Keys.G, "Г");
            addRu(Keys.H, "Х");
            addRu(Keys.J, "Й");
            addRu(Keys.K, "К");
            addRu(Keys.L, "Л");

            addRu(Keys.Z, "З");
            addRu(Keys.X, "Ч");
            addRu(Keys.C, "Ц");
            addRu(Keys.V, "В");
            addRu(Keys.B, "Б");
            addRu(Keys.N, "Н");
            addRu(Keys.M, "М");

            AddRu(pure, Keys.Oem7, "ь");
            AddRu(pureShift, Keys.Oem7, "ы");
            AddRu(beta, Keys.Oem7, "ъ");

        }

        private static void AddRu(KeyDictionary dictionary, Keys key, string letter)
        {
            if (dictionary.ContainsKey(key))
            {
                throw new Exception("Duplicate key: " + key);
            } else
            {
                dictionary.Add(key, Ru(letter));
            }
        }

        private static LocaleDictionary AnyRu(object any, object ru)
        {
            return new LocaleDictionary
            {
                { KeyboardLocale.ANY_OTHER, any },
                { KeyboardLocale.ru_RU, ru }
            };
        }

        private static LocaleDictionary Ru(object ru)
        {
            return new LocaleDictionary
            {
                { KeyboardLocale.ru_RU, ru }
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
