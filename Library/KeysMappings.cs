using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library
{
    public class KeysMappings
    {

        private readonly static Dictionary<Keys, Keys> alphaMappings;
        private readonly static Dictionary<Keys, Keys> bettaMappings;
        private readonly static Dictionary<Keys, string> alphaNumbersToCurvesMapper;

        static KeysMappings()
        {

            alphaNumbersToCurvesMapper = new Dictionary<Keys, string>
            {
                { Keys.D7, "{" },
                { Keys.D8, "[" },
                { Keys.D9, "]" },
                { Keys.D0, "}" }
            };

            alphaMappings = new Dictionary<Keys, Keys>
            {
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
                { Keys.OemPeriod, Keys.PageDown }
            };

            bettaMappings = new Dictionary<Keys, Keys>
            {
                { Keys.U, Keys.D7 },
                { Keys.I, Keys.D8 },
                { Keys.O, Keys.D9 },

                //{ Keys.H, Keys.Home },
                { Keys.J, Keys.D4 },
                { Keys.K, Keys.D5 },
                { Keys.L, Keys.D6 },
                { Keys.Oemtilde, Keys.D0 },

                { Keys.M, Keys.D1 },
                { Keys.Oemcomma, Keys.D2 },
                { Keys.OemPeriod, Keys.D3 }
            };
        }

        private readonly KeysSender KeysSender;
        private readonly List<Func<Keys, KeyboardState, bool>> mappers;

        public KeysMappings(KeysSender keysSender)
        {
            KeysSender = keysSender;

            mappers = new List<Func<Keys, KeyboardState, bool>>
            {
                // Priority order
                DisablePureKeys,
                ForNumbers,
                ForAlpha,
                ForBetta
            };
        }

        public void OnKey(KeyboardEvent e)
        {
            if (e.Key.IsAlpha() || e.Key.IsBetta())
            {
                Logger.Log("Alpha or Betta");
                e.Cancel = true;
                return;
            }

            if (e.Key.IsModifier())
            {
                Logger.Log("Alt or Ctrl or Shift");
                return;
            }

            Logger.Log("OnKey: " + e.Key);

            foreach (var mapper in mappers)
            {
                e.Cancel = mapper(e.Key, e.KeyboardState);
                if (e.Cancel) break;
            }

        }
        private bool DisablePureKeys(Keys key, KeyboardState state)
        {
            if (!state.Any)
            {
                switch (key)
                {
                    case Keys.Delete:
                    case Keys.Back:
                    case Keys.Enter:

                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        private bool ForNumbers(Keys key, KeyboardState state)
        {
            if (key.IsNumber())
            {
                if (!state.Any)
                {
                    if (key == Keys.D4)
                    {
                        KeysSender.SendText("$");
                    }
                    else
                    {
                        KeysSender.SendKey(key, KeyModifier.Shift);
                    }
                    return true;
                }
                else if (state.Betta)
                {
                    KeysSender.SendKey(key);
                    return true;
                }
            }
            return false;
        }

        private bool ForAlpha(Keys key, KeyboardState state)
        {
            if (!state.Betta && (state.Alt || state.Ctrl || state.Alpha))
            {
                if (key.IsNumber())
                {
                    Logger.Log("For Alpha Numbers");
                    return KeysSender.SendMappedText(key, alphaNumbersToCurvesMapper);
                }

                Logger.Log("For Alpha");
                return KeysSender.SendMappedKey(key, alphaMappings);
            }

            return false;
        }

        private bool ForBetta(Keys key, KeyboardState state)
        {
            if (state.Betta)
            {
                Logger.Log("For Betta");
                return KeysSender.SendMappedKey(key, bettaMappings);
            }

            return false;
        }
    }
}
