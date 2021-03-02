using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aritiafel.Artifacts
{
    public class ChaosBox
    {
        protected Random _Random;
        protected Random _Random2;

        //MaxValueDigitsCount
        //(u)byte    3
        //(u)short   5
        //char       5
        //(u)int    10
        //long      19
        //ulong     20
        //decimal   29
        //float     38
        //double    308

        private const int MaxValueDigitsCountByte = 3;
        private const int MaxValueDigitsCountUByte = 3;
        private const int MaxValueDigitsCountShort = 5;
        private const int MaxValueDigitsCountUShort = 5;
        private const int MaxValueDigitsCountChar = 5;
        private const int MaxValueDigitsCountInt = 10;
        private const int MaxValueDigitsCountUInt = 10;
        private const int MaxValueDigitsCountLong = 19;
        private const int MaxValueDigitsCountULong = 20;
        private const int MaxValueDigitsCountDecimal = 29;
        private const int MaxValueDigitsCountFloar = 38;
        private const int MaxValueDigitsCountDouble = 308;

        private const string MinGreaterThanMaxMessage = "Min value greater than max Value.";

        public ChaosBox()
        {
            Thread.Sleep(1);
            _Random = new Random((int)(DateTime.Now.Ticks ^ 5621387647545697893));
            Thread.Sleep(1);
            _Random2 = new Random((int)(DateTime.Now.Ticks ^ _Random.Next(int.MinValue, int.MaxValue)));
        }

        public ChaosBox(int seed)
        {
            _Random = new Random(seed);
            _Random2 = new Random(seed ^ _Random.Next(int.MinValue, int.MaxValue));
        }

        public int GetNumberStringPowOf10(string numberString)
        {
            if (numberString.IndexOf('E') != -1)
            {
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
            }
            else
            {
                if (numberString.IndexOf('.') != -1)
                {
                    if (numberString.StartsWith("0."))
                    {
                        int i;
                        for (i = 2; i < numberString.Length; i++)
                            if (numberString[i] != '0')
                                break;
                        return (i - 1) * -1;
                    }
                    else
                        return numberString.IndexOf('.') - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1);
                }
                else
                    return numberString.Length - (numberString[0] == '-' || numberString[0] == '+' ? 2 : 1);
            }
        }

        public string RandomMinMaxValue(string minValue, string maxValue, out int digitsCountBound)
        {
            bool minValueNegative = minValue[0] == '-',
                maxValueNegative = maxValue[0] == '-';

            //負號時大小值交換 
            string minCompareString, maxCompareString;
            if (minValueNegative && maxValueNegative)
            {
                minCompareString = minValue;
                minValue = maxValue;
                maxValue = minCompareString;
            }

            //200000 -> E = 5
            //20     -> E = 1
            //5 Start
            //0.02 -> E = -2
            //0.00002 E = -5
            //-2 Start

            StringBuilder numberString = new StringBuilder();
            int minCompareDigitsCount = GetNumberStringPowOf10(minValue);
            int maxCompareDigitsCount = GetNumberStringPowOf10(maxValue);
            minCompareString = minValue.Replace(".", "").Replace("-", "");
            if (minCompareString.IndexOf('E') != -1)
                minCompareString = minCompareString.Remove(minCompareString.IndexOf('E'));
            maxCompareString = maxValue.Replace(".", "").Replace("-", "");
            if (maxCompareString.IndexOf('E') != -1)
                maxCompareString = maxCompareString.Remove(maxCompareString.IndexOf('E'));

            digitsCountBound = minCompareDigitsCount;

            bool isDigitsCountUpperBound = true;
            bool isDigitsCountLowerBound = true;
            numberString.Clear();
            int odds, lower, upper;
            int i;
            for (i = maxCompareDigitsCount; maxCompareDigitsCount - i < 30; i--)
            {
                if (minValueNegative == maxValueNegative)
                {
                    lower = 0;
                    upper = 999999999;
                    if (maxCompareDigitsCount - i >= maxCompareString.Length &&
                        minCompareDigitsCount - i >= minCompareString.Length)
                        break;
                    else if (!isDigitsCountLowerBound && !isDigitsCountUpperBound)
                    {
                        if(i % 2 == 0)
                            numberString.Append(_Random.Next(0, 10));
                        else
                            numberString.Append(_Random2.Next(0, 10));
                        continue;
                    }
                    else
                    {
                        if (isDigitsCountUpperBound)
                        {
                            if (maxCompareDigitsCount - i + 9 < maxCompareString.Length)
                                upper = int.Parse(maxCompareString.Substring(maxCompareDigitsCount - i, 9));
                            else if (maxCompareDigitsCount - i < maxCompareString.Length)
                                upper = int.Parse(maxCompareString.Substring(maxCompareDigitsCount - i).PadRight(9, '0'));
                            else
                                upper = 0;                                
                        }

                        if (isDigitsCountLowerBound && minCompareDigitsCount >= i)
                        {
                            if (minCompareDigitsCount - i + 9 < minCompareString.Length)
                                lower = int.Parse(minCompareString.Substring(minCompareDigitsCount - i, 9));
                            else if (minCompareDigitsCount - i < minCompareString.Length)
                                lower = int.Parse(minCompareString.Substring(minCompareDigitsCount - i).PadRight(9, '0'));
                        }
                        
                        if (i % 2 == 0)
                            odds = _Random.Next(lower, upper + 1);
                        else
                            odds = _Random2.Next(lower, upper + 1);
                        string tempString = odds.ToString().PadLeft(9, '0');
                        if (tempString[0] != lower.ToString()[0])
                            isDigitsCountLowerBound = false;
                        if (tempString[0] != upper.ToString()[0])
                            isDigitsCountUpperBound = false;
                        numberString.Append(tempString[0]);
                    }
                }
                else
                {
                    lower = -9;
                    upper = 9;
                    if (!isDigitsCountLowerBound && !isDigitsCountUpperBound)
                    {
                        if (i % 2 == 0)
                            numberString.Append(_Random.Next(0, 10));
                        else
                            numberString.Append(_Random2.Next(0, 10));
                        continue;
                    }
                    else
                    {
                        if (isDigitsCountUpperBound && maxCompareDigitsCount >= i)
                        {

                        }
                    }
                }
            }
            if (maxCompareDigitsCount < -30 || maxCompareDigitsCount > 30)
            {
                numberString.Insert(1, '.');
                numberString.AppendFormat("E{0}{1}", (maxCompareDigitsCount >= 0 ? "+" : ""), maxCompareDigitsCount);
            }
            else if (maxCompareDigitsCount < 0)
            {
                numberString.Insert(0, $"0.{new string('0', -1 * (maxCompareDigitsCount - 1))}");
            }
            else if (i != -1)
            {
                //Console.WriteLine(i);
                numberString.Insert(Math.Abs(i + numberString.Length + 1), '.');
                //if (i < numberString.Length)
                //    numberString.Insert(i, '.');
            }

            return numberString.ToString();
        }

        //if (_Random.Next(0, int.Parse(compareString )  rnd.Next(0, int.Parse(compareString.Substring(0, 9)))
        //    < int.Parse("100000000"))
        //    highestDigit = false;
        //do
        //{
        //    numberString.Clear();
        //    if (canBeNegative && rnd.Next(2) == 0)
        //        numberString.Append('-');
        //    numberString.Append(highestDigit ? 1 : 0);
        //    numberString.Append('.');
        //    for (int i = 1; i < 20; i++)
        //        if (highestDigit)
        //        {
        //            int temp;
        //            if (i < 17)
        //            {
        //                temp = rnd.Next(int.Parse(compareString[i].ToString()) + 1);
        //                highestDigit = temp.ToString() != compareString[i].ToString();
        //            }
        //            else
        //                temp = 0;
        //            numberString.Append(temp);
        //        }
        //        else
        //            numberString.Append(rnd.Next(10));
        //    numberString.Append("E+308");
        //    if (!double.TryParse(numberString.ToString(), out result))
        //        continue;
        //}
        //while (double.IsNaN(result) || double.IsInfinity(result) || double.IsNegativeInfinity(result));
        //return result;

        private int DrawOutNormalizedLong(long minValue, long maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            double d = DrawOutDouble(0, Math.PI);
            if (d < Math.PI / 2)
                return (int)Math.Round(Math.Sin(d) * (maxValue - minValue) / 2 + minValue);
            else
                return (int)Math.Round((1 - Math.Sin(d)) * ((maxValue - minValue) / 2) + (maxValue + minValue) / 2);
        }

        public int DrawOutNormalizedInteger(int minValue, int maxValue)
            => DrawOutNormalizedLong(minValue, maxValue);

        public byte DrawOutNormalizedByte(byte minValue, byte maxValue)
            => (byte)DrawOutNormalizedLong(minValue, maxValue);

        public byte DrawOutNormalizedByte()
        {
            double d = DrawOutDouble(0, Math.PI);
            if (d < Math.PI / 2)
                return (byte)Math.Floor(Math.Sin(d) * 128);
            else
                return (byte)Math.Floor((1 - Math.Sin(d)) * 128 + 128);
        }
        public int DrawOutNormalizedInteger(bool canBeNegative = true)
        {
            double d = DrawOutDouble(0, Math.PI);
            if (canBeNegative)
            {
                if (d < Math.PI / 2)
                    return (int)Math.Round(Math.Sin(d) * int.MaxValue + int.MinValue);
                else
                    return (int)Math.Round((1 - Math.Sin(d)) * int.MaxValue);
            }
            else
                if (d < Math.PI / 2)
                return (int)Math.Round(Math.Sin(d) * (int.MaxValue / 2));
            else
                return (int)Math.Round((1 - Math.Sin(d)) * (int.MaxValue / 2) + int.MaxValue / 2);
        }
        public string DrawOutRandomLengthString(int maxLength = 10000, Encoding encoding = null)
            => DrawOutString(DrawOutInteger(1, maxLength), encoding);
        public string DrawOutString(long length, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;
            if (length < 0)
                new ArgumentOutOfRangeException(nameof(length));
            return encoding.GetString(DrawOutBytes(length));
        }

        public byte[] DrawOutBytes(long count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            byte[] result = new byte[count];
            for (long i = 0; i < count; i++)
                result[i] = DrawOutByte();
            return result;
        }

        public byte DrawOutByte()
        {
            if (DateTime.Now.Millisecond % 2 == 0)
                return (byte)_Random.Next(byte.MaxValue + 1);
            else
                return (byte)_Random2.Next(byte.MaxValue + 1);
        }

        public int DrawOutInteger(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (DateTime.Now.Millisecond % 2 == 0)
                if (maxValue != int.MaxValue)
                    return _Random.Next(minValue, maxValue + 1);
                else
                    return _Random.Next(minValue, maxValue);
            else
                if (maxValue != int.MaxValue)
                return _Random2.Next(minValue, maxValue + 1);
            else
                return _Random2.Next(minValue, maxValue);
        }

        public int DrawOutInteger(bool canBeNegative = true)
        {
            if (DateTime.Now.Millisecond % 2 == 0)
                if (canBeNegative)
                    return _Random.Next(int.MinValue, int.MaxValue);
                else
                    return _Random.Next(int.MaxValue);
            else
                if (canBeNegative)
                return _Random2.Next(int.MinValue, int.MaxValue);
            else
                return _Random2.Next(int.MaxValue);
        }

        public decimal DrawOutDecimal(bool canBeNegative = true)
        {
            if (DateTime.Now.Millisecond % 2 == 0)
                return new decimal(_Random.Next(int.MinValue, int.MaxValue),
                        _Random2.Next(int.MinValue, int.MaxValue),
                        _Random.Next(int.MinValue, int.MaxValue),
                        canBeNegative ? _Random2.Next(2) == 0 : false,
                        (byte)_Random2.Next(29));
            else
                return new decimal(_Random2.Next(int.MinValue, int.MaxValue),
                        _Random.Next(int.MinValue, int.MaxValue),
                        _Random2.Next(int.MinValue, int.MaxValue),
                        canBeNegative ? _Random.Next(2) == 0 : false,
                        (byte)_Random2.Next(29));
        }

        public decimal DrawOutDecimalInteger(bool canBeNegative = true)
        {
            if (DateTime.Now.Millisecond % 2 == 0)
                return new decimal(_Random.Next(int.MinValue, int.MaxValue),
                        _Random2.Next(int.MinValue, int.MaxValue),
                        _Random.Next(int.MinValue, int.MaxValue),
                        canBeNegative ? _Random2.Next(2) == 0 : false, 0);
            else
                return new decimal(_Random2.Next(int.MinValue, int.MaxValue),
                        _Random.Next(int.MinValue, int.MaxValue),
                        _Random2.Next(int.MinValue, int.MaxValue),
                        canBeNegative ? _Random.Next(2) == 0 : false, 0);
        }

        public long DrawOutLong(long minValue, long maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);


            decimal m;
            if (DateTime.Now.Millisecond % 2 == 0)
                m = Convert.ToDecimal(_Random.NextDouble());
            else
                m = Convert.ToDecimal(_Random2.NextDouble());
            return Convert.ToInt64(m * (maxValue - minValue) + minValue);
        }

        public long DrawOutLong(bool canBeNegative = true)
            => canBeNegative ? DrawOutLong(long.MinValue, long.MaxValue) :
                DrawOutLong(0, long.MaxValue);

        public decimal DrawOutDecimalInteger(decimal minValue, decimal maxValue)
        {
            if (decimal.Ceiling(minValue) != minValue)
                throw new ArgumentOutOfRangeException(nameof(minValue));
            else if (decimal.Floor(maxValue) != maxValue)
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            else if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            //int minDigit
            //Max 29

            //ecimal.M

            throw new NotImplementedException();
            //return 1;
        }

        public double DrawOutDouble(bool canBeNegative = true)
        {
            if (DateTime.Now.Millisecond % 2 == 0)
                return _Random.NextRandomDouble(canBeNegative);
            else
                return _Random2.NextRandomDouble(canBeNegative);
        }

        public double DrawOutDouble(double minValue, double maxValue = double.MaxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (DateTime.Now.Millisecond % 2 == 0)
                return _Random.NextRandomDouble(minValue, maxValue);
            else
                return _Random2.NextRandomDouble(minValue, maxValue);
        }

        public double DrawOutDiversityDouble(bool canBeNegative = true)
        {
            StringBuilder numberString = new StringBuilder();
            double result;
            do
            {
                numberString.Clear();
                if (canBeNegative && _Random.Next(2) == 0)
                    numberString.Append('-');
                numberString.Append(_Random2.Next(10));
                numberString.Append('.');
                for (int i = 0; i < 20; i++)
                    numberString.Append(_Random.Next(10));
                numberString.Append($"E{_Random2.Next(-310, 310)}");
                double.TryParse(numberString.ToString(), out result);
            }
            while (double.IsNaN(result) || double.IsInfinity(result) || double.IsNegativeInfinity(result));
            return result;
        }
    }
}
