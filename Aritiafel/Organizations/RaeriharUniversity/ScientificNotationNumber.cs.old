using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ScientificNotationNumber : ICloneable, IFormattable
    {
        public bool IsNegative { get; set; }
        public string Digits
        {
            get => _Digits;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(Digits));
                else if (value == "0")
                {
                    _Digits = value;
                    Exponent = 0;
                    IsNegative = false;
                    return;
                }
                else if (value[0] == '0')
                    throw new ArgumentException($"{nameof(Digits)}:{value}");
                else if (value[value.Length - 1] == '0')
                    throw new ArgumentException($"{nameof(Digits)}:{value}");
                for (int i = 0; i < value.Length; i++)
                    if (!char.IsDigit(value[i]))
                        throw new ArgumentException($"{nameof(Digits)}:{value}");
                _Digits = value;
            }
        }
        private string _Digits;
        public int Exponent { get; set; }
        public ScientificNotationNumber()
            : this("0")
        { }

        public ScientificNotationNumber(ScientificNotationNumber snn)
        {
            _Digits = snn._Digits;
            Exponent = snn.Exponent;
            IsNegative = snn.IsNegative;
        }
        public ScientificNotationNumber(string digits, int e = 0, bool isNegative = false)
        {
            Digits = digits;
            Exponent = e;
            IsNegative = isNegative;
        }

        public ScientificNotationNumber(byte number)
            : this((ulong)number)
        { }
        public ScientificNotationNumber(short number)
            : this((long)number)
        { }
        public ScientificNotationNumber(ushort number)
            : this((ulong)number)
        { }
        public ScientificNotationNumber(int number)
            : this((long)number)
        { }
        public ScientificNotationNumber(uint number)
            : this((ulong)number)
        { }
        public ScientificNotationNumber(long number)
        {
            string numberString = Math.Abs(number).ToString();
            Exponent = numberString.Length - 1;
            while (numberString.Length > 1 && numberString[numberString.Length - 1] == '0')
                numberString = numberString.Remove(numberString.Length - 1, 1);
            Digits = numberString;
            IsNegative = number < 0;
        }
        public ScientificNotationNumber(ulong number)
        {
            string numberString = number.ToString();
            Exponent = numberString.Length - 1;
            while (numberString.Length > 1 && numberString[numberString.Length - 1] == '0')
                numberString = numberString.Remove(numberString.Length - 1, 1);
            Digits = numberString;
        }
        public ScientificNotationNumber(float number)
            : this((double)number)
        { }
        public ScientificNotationNumber(double number)
        {
            ParseSelf(number.ToString());
        }
        public ScientificNotationNumber(decimal number)
        {
            ParseSelf(number.ToString());
        }

        public static ScientificNotationNumber Parse(string s)
        {
            ScientificNotationNumber result = new ScientificNotationNumber();
            result.ParseSelf(s);
            return result;
        }
        private void ParseSelf(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException(nameof(s));
            IsNegative = false;
            string numberString = s;
            int eIndex = -2, pointIndex = -1;
            if (numberString[0] == '+')
                numberString = numberString.Remove(0, 1);
            else if (numberString[0] == '-')
            {
                IsNegative = true;
                numberString = numberString.Remove(0, 1);
            }
            while (numberString.Length > 1 && numberString[0] == '0')
                numberString = numberString.Remove(0, 1);

            for (int i = 0; i < numberString.Length; i++)
            {
                if (i == eIndex + 1)
                {
                    if (numberString[i] != '+' && numberString[i] != '-')
                        throw new ArgumentException($"{nameof(s)}:{s}");
                }
                else if (numberString[i] == '.')
                    if (pointIndex == -1)
                        pointIndex = i;
                    else
                        throw new ArgumentException($"{nameof(s)}:{s}");
                else if (numberString[i] == 'E' || numberString[i] == 'e')
                    if (i == 0)
                        throw new ArgumentException($"{nameof(s)}:{s}");
                    else if (eIndex == -2)
                        eIndex = i;
                    else
                        throw new ArgumentException($"{nameof(s)}:{s}");
                else if (!char.IsDigit(numberString[i]))
                    throw new ArgumentException($"{nameof(s)}:{s}");
            }

            if (eIndex != -2)
            {
                Exponent = int.Parse(numberString.Substring(eIndex + 1));
                numberString = numberString.Remove(eIndex);
            }
            else
                Exponent = 0;

            if (pointIndex != -1)
            {
                while (numberString[numberString.Length - 1] == '0')
                    numberString = numberString.Remove(numberString.Length - 1, 1);
                numberString = numberString.Remove(pointIndex, 1);
                if (numberString.Length == 0)
                {
                    Digits = "0";
                    return;
                }
                Exponent += pointIndex - 1;
                while (numberString[0] == '0')
                {
                    numberString = numberString.Remove(0, 1);
                    Exponent--;
                }
            }
            else
                Exponent += numberString.Length - 1;

            while (numberString.Length > 1 && numberString[numberString.Length - 1] == '0')
                numberString = numberString.Remove(numberString.Length - 1, 1);
            Digits = numberString;
        }

        public static bool TryParse(string s, out ScientificNotationNumber result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public ScientificNotationNumber Round(int digits)
        {
            if (digits <= 0)
                throw new ArgumentOutOfRangeException(nameof(digits));
            else if (digits > Digits.Length)
                digits = Digits.Length;

            ScientificNotationNumber result = new ScientificNotationNumber(this);
            if (Digits.Length == digits || Digits[digits] <= '4')
            {
                string s = Digits.Substring(0, digits);
                while (s.Length > 1 && s[s.Length - 1] == '0')
                    s = s.Remove(s.Length - 1, 1);
                result.Digits = s;
                return result;
            }
            else if (digits == 1 && Digits[0] == '9')
            {
                result.Digits = "1";
                result.Exponent++;
                return result;
            }
            else if (Digits[digits - 1] != '9')
            {
                result.Digits = string.Concat(Digits.Substring(0, digits - 1), (char)(Digits[digits - 1] + 1));
                return result;
            }
            return Round(digits - 1);
        }

        private string ToString(int digits, IFormatProvider provider, bool toExponential = true)
        {
            if (digits < 0)
                throw new ArgumentOutOfRangeException(nameof(digits));
            else if (digits == 0 || digits > Digits.Length)
                digits = Digits.Length;

            if (Digits == "0")
                return 0.ToString(provider);

            ScientificNotationNumber snn = this;
            if (digits != Digits.Length)
            {
                snn = Round(digits);
                if (digits > snn.Digits.Length)
                    digits = snn.Digits.Length;
            }

            if (snn.Digits.Length == 1)
                if (snn.Exponent == 0)
                    return $"{(snn.IsNegative ? "-" : "")}{snn.Digits[0]}";
                else
                    if (toExponential)
                        return $"{(snn.IsNegative ? "-" : "")}{snn.Digits[0]}E{(snn.Exponent >= 0 ? "+" : "")}{snn.Exponent}";
                    else if (snn.Exponent >= 0)
                        return $"{(snn.IsNegative ? "-" : "")}{snn.Digits[0]}{new string('0', snn.Exponent)}";
                    else
                        return $"{(snn.IsNegative ? "-" : "")}0.{new string('0', Math.Abs(snn.Exponent + 1))}{snn.Digits[0]}";
            else
                if (snn.Exponent == 0)
                    return $"{(snn.IsNegative ? "-" : "")}{snn.Digits[0]}.{snn.Digits.Substring(1, digits - 1)}";
                else
                    if (toExponential)
                        return $"{(snn.IsNegative ? "-" : "")}{snn.Digits[0]}.{snn.Digits.Substring(1, digits - 1)}E{(snn.Exponent > 0 ? "+" : "")}{snn.Exponent}";
                    else if (snn.Exponent >= digits - 1)
                        return $"{(snn.IsNegative ? "-" : "")}{snn.Digits.Substring(0, digits)}{new string('0', snn.Exponent - digits + 1)}";
                    else if (snn.Exponent >= 0)
                        return $"{(snn.IsNegative ? "-" : "")}{snn.Digits.Substring(0, snn.Exponent + 1)}.{snn.Digits.Substring(Exponent + 1)}";
                    else
                        return $"{(snn.IsNegative ? "-" : "")}0.{new string('0', Math.Abs(snn.Exponent + 1))}{snn.Digits.Substring(0)}";
        }
        public object Clone()
            => new ScientificNotationNumber(_Digits, Exponent, IsNegative);
        public override string ToString()
            => ToString("G");
        public string ToString(string format)
            => ToString(format, null);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);
        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";
            format = format.Trim().ToUpperInvariant();
            if (provider == null)
                provider = NumberFormatInfo.CurrentInfo;
            int length;
            switch (format[0])
            {
                // TO DO
                case 'C':
                    if (format.Length == 1)
                        return ToString(0, provider, false);
                    if (!int.TryParse(format.Substring(1), out length))
                        goto default;
                    return ToString(length, provider, false);
                //case 'D':
                //    break;
                //case 'F':
                //case 'N':
                //case 'P':
                //case 'R':
                //case 'X':
                case 'E':
                case 'G':
                    if (format.Length == 1)
                        return ToString(0, provider);
                    if (!int.TryParse(format.Substring(1), out length))
                        goto default;
                    return ToString(length, provider);
                default:
                    throw new FormatException(string.Format("The '{0}' format string is not supported.", format));
            }
        }
    }
}
