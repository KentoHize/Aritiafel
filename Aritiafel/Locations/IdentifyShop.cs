using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aritiafel.Locations
{
    public static class IdentifyShop
    {
        public static bool NoPause { get; set; }
        public static long Counter { get; set; } = 0;
        private static char GetLetterOrDigitFromRandom62(int i)            
        {   
            if (i < 10)
                return (char)(i + 48);
            else if (i < 36)
                return (char)(i + 55);
            else
                return (char)(i + 61);
        }

        public static string GetNewID(string preposition = "", bool resetCounter = false)
        {
            if (resetCounter)
                Counter = 0;
            StringBuilder result = new StringBuilder();
            result.Append(preposition);
            Random rnd = new Random((int)DateTime.Now.Ticks);
            if(!NoPause)
                Thread.Sleep(1);
            result.Append(GetLetterOrDigitFromRandom62(rnd.Next(10, 62)));
            for (int i = 0; i < 10; i++)
                result.Append(GetLetterOrDigitFromRandom62(rnd.Next(62)));
            result.Append(Counter++);
            return result.ToString();
        }
    }
}
