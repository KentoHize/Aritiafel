using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel
{
    //待整理
    public static class Extensions
    {
        public static string GetNestedTypeName(this Type type)
        {
            if (type.DeclaringType == null)
                return type.Name;
            return string.Concat(GetNestedTypeName(type.DeclaringType), "+", type.Name);
        }
        
        public static double NextRandomDouble(this Random rnd, bool hasNaN = false)
        {   
            byte[] bytesArray = new byte[8];
            double result;
            do {
                for (int i = 0; i < 8; i++)
                    bytesArray[i] = (byte)rnd.Next(byte.MaxValue + 1);
                result = BitConverter.ToDouble(bytesArray, 0); 
            }
            while (!hasNaN && double.IsNaN(result));
            return result;
        }

        public static double NextRandomDouble(this Random rnd, double minValue, double maxValue)
        {
            if (minValue > double.MinValue / 2 && maxValue < double.MaxValue / 2)
                return rnd.NextDouble() * (maxValue - minValue) + minValue;
            double d;
            do
            { d = NextRandomDouble(rnd); }
            while (d < minValue || d > maxValue);
            return d;
        }
    }
}
