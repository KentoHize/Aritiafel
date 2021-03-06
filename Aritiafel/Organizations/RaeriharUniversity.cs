using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public static class Mathematics
    {
        public static string GetStandardNumberString(string numberToString, int maxDigitsCount = 29)
        {
            string result = numberToString;
            bool hasPoint = false;
            byte hasE = 0;            
            for(int i = 0; i < result.Length; i++)
            {
                if (i == 0 && (result[i] == '+' || result[i] == '-'))
                    continue;
                if (result[i] == '.')
                    if (hasPoint)
                        return null;
                    else
                        hasPoint = true;
                else if (hasE == 1)
                {
                    if (result[i] != '+' && result[i] != '-')
                        return null;
                    hasE++;
                }
                else if (result[i] == 'E' || result[i] == 'e')
                    if (hasE == 0)
                        hasE++;
                    else
                        return null;
                else if (!char.IsDigit(result[i]))
                    return null;
            }
            if (result[0] == '+')
                result = result.Remove(0, 1);

            while (result.Length > 1 && result[0] == '0')
                result = result.Remove(0, 1);
            if (result[0] == '.')
                result = $"0{result}";
            //if (hasE == 2 && hasPoint && numberToString.StartsWith("0.");
            return result;
        }

        public static int GetIntegerDigitsCount(string numberToString, bool checkString = true)
        {            
            if(checkString)
                if(string.IsNullOrEmpty(GetStandardNumberString(numberToString)))
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
