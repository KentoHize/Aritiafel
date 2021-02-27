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
        
        public static double NextRandomDouble(this Random rnd, bool canBeNegative = true)
        {
            StringBuilder numberString = new StringBuilder();
            double result;
            do
            {
                numberString.Clear();
                if (canBeNegative && rnd.Next(2) == 0)
                    numberString.Append('-');
                numberString.Append(rnd.Next(10));
                numberString.Append('.');
                for (int i = 0; i < 20; i++)
                    numberString.Append(rnd.Next(10));                
                numberString.Append($"E{rnd.Next(-310, 310)}");
                double.TryParse(numberString.ToString(), out result);
            }
            while (double.IsNaN(result) || double.IsInfinity(result) || double.IsNegativeInfinity(result));
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
