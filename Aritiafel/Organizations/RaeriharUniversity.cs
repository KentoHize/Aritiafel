using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public static class Mathematics
    {
        public static bool GetStandardNumberString(string numberToString)
        {   
            bool hasPoint = false;
            byte hasE = 0;
            for(int i = 0; i < numberToString.Length; i++)
            {   
                if (numberToString[i] == '.')
                    if (hasPoint)
                        return false;
                    else
                        hasPoint = true;
                else if (hasE == 1)
                { 
                    if (numberToString[i] != '+' && numberToString[i] != '-')
                        return false;
                    hasE++;
                }
                else if (numberToString[i] == 'E' || numberToString[i] == 'e')
                {
                    if (hasE == 0)
                        hasE++;
                    else
                        break;
                }
                else if (!char.IsDigit(numberToString[i]) && (i != 0 || numberToString[i] != '-'))
                    break;
                else if (i > 329) // too long for a double
                    break;                
            }
            return true;
        }

        public static int GetIntegerDigitsCount(string numberToString, bool checkString = true)
        {
            
            if(checkString)
                if(GetStandardNumberString(numberToString))
                    throw new ArgumentException(nameof(numberToString));
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
