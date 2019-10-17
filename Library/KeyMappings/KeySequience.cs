using System.Windows.Forms;
using System.Collections.Generic;

namespace KeyMapperLibrary
{
    public static class KS
    {
        public static KeySequience Down(params Keys[] keys)
        {
            return new KeySequience().Down(keys);
        }

        public static KeySequience Up(params Keys[] keys)
        {
            return new KeySequience().Up(keys);
        }
    }

    public class KeySequience
    {

        private readonly List<int> keys = new List<int>();

        public KeySequience Down(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                this.keys.Add((int)key);
            }
            return this;
        }

        public KeySequience Up(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                this.keys.Add(-(int)key);
            }
            return this;
        }

        public int[] Build(bool appendReverse = true)
        {
            if (!appendReverse)
            {
                return keys.ToArray();
            }

            int[] result = new int[keys.Count + keys.Count];
            for (int i = 0; i < keys.Count; i++)
            {
                result[i] = keys[i];
                result[result.Length - 1 - i] = -keys[i];
            }
            return result;
        }
    }
}
