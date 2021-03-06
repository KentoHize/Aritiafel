using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public static class Mathematics
    {

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
