using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public static class Mathematics
    {
        //右移降e左移升e
        public static string GetStandardNumberString(string numberToString, int maxDigitsCount = 29)
        {
            if (string.IsNullOrEmpty(numberToString))
                return null;
            string result = numberToString;
            int pointIndex = -1;
            int e;
            bool isNegative = false;
            int eIndex = -2;

            if (result[0] == '+')
                result = result.Remove(0, 1);
            else if (result[0] == '-')
            {
                isNegative = true;
                result = result.Remove(0, 1);
            }
            while (result.Length > 1 && result[0] == '0')
                result = result.Remove(0, 1);

            for (int i = 0; i < result.Length; i++)
            {
                if (i == eIndex + 1)
                {
                    if(result[i] != '+' && result[i] != '-')
                        return null;
                }
                else if (result[i] == '.')
                    if (pointIndex == -1)
                        pointIndex = i;
                    else
                        return null;
                else if (result[i] == 'E' || result[i] == 'e')
                    if (i == 0)
                        return null;
                    else if (eIndex == -2)
                        eIndex = i;
                    else
                        return null;
                else if (!char.IsDigit(result[i]))
                    return null;
            }

            if (pointIndex == -1)
                pointIndex = result.Length;

            if (eIndex != -2)
            {
                e = int.Parse(result.Substring(eIndex + 1));
                result = result.Remove(eIndex);
                e += pointIndex - 1;
            }   
            else
                e = 0;
            
            if (result.Length != pointIndex)
                result = result.Remove(pointIndex, 1);
            if (Math.Abs(e + result.Length) < maxDigitsCount)
            {   
                if (e > 0)
                {
                    while (result.Length > 0 && result[0] == '0')
                        result = result.Remove(0, 1);
                    result += new string('0', e);
                }   
                else if (e < 0)
                {
                    while (result.Length > 0 && result[result.Length - 1] == '0')
                        result = result.Remove(result.Length -1, 1);
                    if (result.Length == 0)
                        return "0";
                    result = $"0.{new string('0', Math.Abs(e) - 1)}{result}";
                }   
            }
            else
            {
                if (pointIndex == 0)
                {
                    while(result[0] == '0')
                    {
                        result = result.Remove(0, 1);
                        e--;
                    }
                }
                
                result = result.Insert(1, ".");

                while (result[result.Length - 1] == '0')
                    result = result.Remove(result.Length - 1, 1);

                result = $"{result}E{(e >= 0 ? "+" : "")}{e}";
            }
            return $"{(isNegative ? "-" : "")}{result}";
        }

        public static int GetIntegerDigitsCount(string numberToString, bool checkString = true)
        {
            //if (checkString)
            //    if (string.IsNullOrEmpty(GetStandardNumberString(numberToString)))
            //        throw new ArgumentException(nameof(numberToString));
            if (numberToString.IndexOf('E') != -1)
                if (numberToString.IndexOf('.') != -1)
                    if (numberToString[numberToString.IndexOf('E')] == '-')
                        return numberToString.IndexOf('.') - (numberToString[0] == '-' || numberToString[0] == '+' ? 2 : 1) -
                            int.Parse(numberToString.Substring(numberToString.IndexOf('E') + 1));
                    else
                        return numberToString.IndexOf('.') - (numberToString[0] == '-' || numberToString[0] == '+' ? 2 : 1) +
                            int.Parse(numberToString.Substring(numberToString.IndexOf('E') + 1));
                else
                    if (numberToString[numberToString.IndexOf('E')] == '-')
                    return numberToString.IndexOf('E') - (numberToString[0] == '-' || numberToString[0] == '+' ? 2 : 1) -
                        int.Parse(numberToString.Substring(numberToString.IndexOf('E') + 1));
                else
                    return numberToString.IndexOf('E') - (numberToString[0] == '-' || numberToString[0] == '+' ? 2 : 1) +
                        int.Parse(numberToString.Substring(numberToString.IndexOf('E') + 1));
            else
                if (numberToString.IndexOf('.') != -1)
                if (numberToString.StartsWith("0.") ||
                    numberToString.StartsWith("-0.") ||
                    numberToString.StartsWith("+0."))
                {
                    for (int i = 2 + (numberToString[0] == '-' || numberToString[0] == '+' ? 1 : 0); i < numberToString.Length; i++)
                        if (numberToString[i] != '0')
                            return (i - 1) * -1;
                    throw new ArgumentException();
                }
                else
                    return numberToString.IndexOf('.') - (numberToString[0] == '-' || numberToString[0] == '+' ? 2 : 1);
            else
                return numberToString.Length - (numberToString[0] == '-' || numberToString[0] == '+' ? 2 : 1);
        }
    }
}
