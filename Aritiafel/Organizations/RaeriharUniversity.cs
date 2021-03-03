using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public static class Mathematics
    {
        public static int GetNumberStringPowOf10(string numberString)
        {   
            if (numberString.IndexOf('E') != -1)
                if (numberString.IndexOf('.') != -1)
                    if (numberString[numberString.IndexOf('E')] == '-')
                        return numberString.IndexOf('.') - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1) -
                            int.Parse(numberString.Substring(numberString.IndexOf('E') + 1));
                    else
                        return numberString.IndexOf('.') - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1) +
                            int.Parse(numberString.Substring(numberString.IndexOf('E') + 1));
                else
                    if (numberString[numberString.IndexOf('E')] == '-')
                    return numberString.IndexOf('E') - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1) -
                        int.Parse(numberString.Substring(numberString.IndexOf('E') + 1));
                else
                    return numberString.IndexOf('E') - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1) +
                        int.Parse(numberString.Substring(numberString.IndexOf('E') + 1));
            else
                if (numberString.IndexOf('.') != -1)
                if (numberString.StartsWith("0.") ||
                    numberString.StartsWith("-0.") ||
                    numberString.StartsWith("+0."))
                {
                    for (int i = 2 + (numberString[0] == '-' || numberString[0] == '+' ? 1 : 0); i < numberString.Length; i++)
                        if (numberString[i] != '0')
                            return (i - 1) * -1;
                    throw new ArgumentException();
                }
                else
                    return numberString.IndexOf('.') - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1);
            else
                return numberString.Length - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1);
        }
    }
}
