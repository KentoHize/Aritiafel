using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ScientificNotationNumber
    {
        public bool IsNegative { get; set; }
        public string Digits { 
            get => _Digits;
            set
            {
                if(string.IsNullOrEmpty(value))
                    throw new ArgumentNullException(nameof(Digits));
                if(value[0] == '0')
                    throw new ArgumentException(nameof(Digits));
                else if(value[value.Length - 1] == '0')
                    throw new ArgumentException(nameof(Digits));
                for (int i = 0; i < value.Length; i++)
                    if (!char.IsDigit(value[i]))
                        throw new ArgumentException(nameof(Digits));
                _Digits = value;
            }
        }
        private string _Digits;
        public int Exponent { get; set; }

        public ScientificNotationNumber(string digits, int e = 0, bool isNegative = false)
        {
            Digits = digits;
            Exponent = e;
            IsNegative = isNegative;
        }
        public string ToString(int DigitsCount = 0)        
            => Digits.Length == 1 ? $"{(IsNegative ? "-" : "")}{Digits[0]}E{(Exponent >= 0 ? "+" : "")}{Exponent}" :
                $"{(IsNegative ? "-" : "")}{Digits[0]}.{Digits.Substring(1, DigitsCount == 0 ? Digits.Length - 1 : DigitsCount - 1)}E{(Exponent >= 0 ? "+" : "")}{Exponent}";

        public override string ToString()
            => ToString(0);

    }
}
