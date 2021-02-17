using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel
{
    //待整理
    public static class Extensions
    {
        public static double NextRandomDouble(this Random rnd)
        {   
            byte[] bytesArray = new byte[8];
            for (int i = 0; i < 8; i++)
                bytesArray[i] = (byte)rnd.Next(byte.MaxValue + 1);
            return BitConverter.ToDouble(bytesArray, 0);
        }

        public static double NextRandomDouble(this Random rnd, double minValue, double maxValue)
        {
            if (minValue > float.MinValue && maxValue < float.MaxValue)
                return rnd.NextDouble() * (maxValue - minValue) + minValue;
            double d;
            do
            {
                d = NextRandomDouble(rnd);
            }
            while (d > minValue && d < maxValue);
            return d;
        }
    }
}
