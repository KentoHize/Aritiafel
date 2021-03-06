using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ScientificNotationNumber : ICloneable
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
                    throw new ArgumentException(nameof(Digits));
                else if (value[value.Length - 1] == '0')
                    throw new ArgumentException(nameof(Digits));
                for (int i = 0; i < value.Length; i++)
                    if (!char.IsDigit(value[i]))
                        throw new ArgumentException(nameof(Digits));
                _Digits = value;
            }
        }
        private string _Digits;
        public int Exponent { get; set; }        
        public ScientificNotationNumber()
            : this("0")
        { }
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
            : this((double) number)
        { }
        public ScientificNotationNumber(double number)
        {
            ScientificNotationNumber snn = Parse(number.ToString());
            Digits = snn.Digits;
            Exponent = snn.Exponent;
            IsNegative = snn.IsNegative;
        }
        public ScientificNotationNumber(decimal number)
        {
            ScientificNotationNumber snn = Parse(number.ToString());
            Digits = snn.Digits;
            Exponent = snn.Exponent;
            IsNegative = snn.IsNegative;
        }
        public static ScientificNotationNumber Parse(string s)
        {

        }
        public string ToString(int DigitsCount)
        {
            if (DigitsCount < 0)
                throw new ArgumentOutOfRangeException(nameof(DigitsCount));
            if (Digits.Length == 1)
                if (Digits == "0")
                    return "0";
                else if (Exponent == 0)
                    return $"{(IsNegative ? "-" : "")}{Digits[0]}";
                else
                    return $"{(IsNegative ? "-" : "")}{Digits[0]}E{(Exponent >= 0 ? "+" : "")}{Exponent}";
            else if (Exponent == 0)
                return $"{(IsNegative ? "-" : "")}{Digits[0]}.{Digits.Substring(1, DigitsCount == 0 ? Digits.Length - 1 : DigitsCount - 1)}";
            else
                return $"{(IsNegative ? "-" : "")}{Digits[0]}.{Digits.Substring(1, DigitsCount == 0 ? Digits.Length - 1 : DigitsCount - 1)}E{(Exponent > 0 ? "+" : "")}{Exponent}";
        }
        public override string ToString()
            => ToString(0);

        public object Clone()
            => new ScientificNotationNumber(_Digits, Exponent, IsNegative);
    }
}
