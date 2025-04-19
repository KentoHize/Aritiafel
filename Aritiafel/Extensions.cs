using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel
{
    //待整理
    public static class Extensions
    {
        

        //public static double NextRandomDouble(this Random rnd, bool canBeNegative = true)
        //{
        //    bool highestDigit = true;
        //    StringBuilder numberString = new StringBuilder();
        //    double result;
        //    string compareString = "179769313486261570000"; //1.7976931348623157E+308;
        //    if (rnd.Next(0, int.Parse(compareString.Substring(0, 9)))
        //        < int.Parse("100000000"))
        //        highestDigit = false;
        //    do
        //    { 
        //        numberString.Clear();
        //        if (canBeNegative && rnd.Next(2) == 0)
        //            numberString.Append('-');
        //        numberString.Append(highestDigit ? 1 : 0);
        //        numberString.Append('.');
        //        for (int i = 1; i < 20; i++)
        //            if (highestDigit)
        //            {
        //                int temp;
        //                if (i < 17)
        //                {
        //                    temp = rnd.Next(int.Parse(compareString[i].ToString()) + 1);
        //                    highestDigit = temp.ToString() != compareString[i].ToString();
        //                }
        //                else
        //                    temp = 0;
        //                numberString.Append(temp);
        //            }
        //            else
        //                numberString.Append(rnd.Next(10));
        //        numberString.Append("E+308");
        //        if(!double.TryParse(numberString.ToString(), out result))
        //            continue;
        //    }
        //    while (double.IsNaN(result) || double.IsInfinity(result) || double.IsNegativeInfinity(result));
        //    return result;
        //}

        //public static double NextRandomDouble(this Random rnd, double minValue, double maxValue)
        //{
        //    if (minValue > double.MinValue / 2 && maxValue < double.MaxValue / 2)
        //        return rnd.NextDouble() * (maxValue - minValue) + minValue;
        //    double d;
        //    do
        //    { d = NextRandomDouble(rnd); }
        //    while (d < minValue || d > maxValue);
        //    return d;
        //}
    }
}
