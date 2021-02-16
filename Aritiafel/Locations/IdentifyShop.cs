using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Locations
{
    public static class IdentifyShop
    {
        public static long Counter { get; set; } = 0;
        public static string GetNewID(string preposition = "", bool resetCounter = false)
        {
            if (resetCounter)
                Counter = 0;
            string s = DateTime.Now.Ticks.ToString();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
                if(i != s.Length - 1)
                    result.Append((char)(s[i + 1] | s[i]));
            result.Append(Counter++);
            return result.ToString();
        }
    }
}
