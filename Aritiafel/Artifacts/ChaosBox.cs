using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Organizations.RaeriharUniversity;

namespace Aritiafel.Artifacts
{
    /// <summary>
    /// 混沌盒子，相傳是大法師Tina所製造，可以產生各種無法預測的事物
    /// </summary>
    public class ChaosBox
    {
        private Random _Random;
        private Random _Random2;

        private const string MinGreaterThanMaxMessage = "Min value greater than max Value.";
        public ChaosBox()
        {
            _Random = new Random((int)(DateTime.Now.Ticks ^ 5621387647545697893));
            _Random2 = new Random((int)(DateTime.Now.Ticks ^ _Random.Next(int.MinValue, int.MaxValue)));
        }
        public ChaosBox(int seed)
        {
            _Random = new Random(seed);
            _Random2 = new Random(seed ^ _Random.Next(int.MinValue, int.MaxValue));
        }

        /// <summary>
        /// 取得一個byte型的數字，在最小值(minValue)以上，最大值(maxValue)以下，不指定最小值時，最小值(minValue)為0
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>結果</returns>
        public byte DrawOutByte(byte minValue, byte maxValue)
            => DateTime.Now.Ticks % 2 == 0 ?
               (byte)_Random.Next(minValue, maxValue + 1) :
               (byte)_Random2.Next(minValue, maxValue + 1);
        public byte DrawOutByte(byte maxValue)
            => DrawOutByte(0, maxValue);
        public byte DrawOutByte()
            => DrawOutByte(0, byte.MaxValue);
        public byte[] DrawOutBytes(long count, byte minValue = 0, byte maxValue = byte.MaxValue)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            byte[] result = new byte[count];
            for (long i = 0; i < count; i++)
                result[i] = DrawOutByte(minValue, maxValue);
            return result;
        }
        public short DrawOutShort(short minValue, short maxValue)
            => DateTime.Now.Ticks % 2 == 0 ?
               (short)_Random.Next(minValue, maxValue + 1) :
               (short)_Random2.Next(minValue, maxValue + 1);
        public short DrawOutShort(short maxValue, bool includeNegative = false)
            => includeNegative ? DrawOutShort(short.MinValue, maxValue) :
                DrawOutShort(0, maxValue);
        public short DrawOutShort(bool includeNegative = false)
            => includeNegative ? DrawOutShort(short.MinValue, short.MaxValue) :
                DrawOutShort(0, short.MaxValue);
        public int DrawOutInteger(int minValue, int maxValue)
        {
            if (minValue == int.MinValue && maxValue == int.MaxValue)
                return int.Parse(RandomMinMaxValue(minValue.ToString(), maxValue.ToString(), true));
            else
                return DateTime.Now.Ticks % 2 == 0 ?
                    _Random.Next(minValue - 1, maxValue) + 1 :
                    _Random2.Next(minValue - 1, maxValue) + 1;
        }       
        public int DrawOutInteger(int maxValue, bool includeNegative = false)
            => includeNegative ? DrawOutInteger(int.MinValue, maxValue) :
                DrawOutInteger(0, maxValue);
        public int DrawOutInteger(bool includeNegative = false)
            => includeNegative ? DrawOutInteger(int.MinValue, int.MaxValue) :
                DrawOutInteger(0, int.MaxValue);
        public long DrawOutLong(long minValue, long maxValue)
            => long.Parse(RandomMinMaxValue(minValue.ToString(), maxValue.ToString(), true));
        public long DrawOutLong(long maxValue, bool includeNegative = false)
            => includeNegative ? DrawOutLong(long.MinValue, maxValue) :
                DrawOutLong(0, maxValue);
        public long DrawOutLong(bool includeNegative = false)
            => includeNegative ? DrawOutLong(long.MinValue, long.MaxValue) :
                DrawOutLong(0, long.MaxValue);
        public float DrawOutFloat(float minValue, float maxValue)
            => float.Parse(RandomMinMaxValue(minValue.ToString(), maxValue.ToString(), false));
        public float DrawOutFloat(float maxValue, bool includeNegative = false)
            => includeNegative ? DrawOutFloat(float.MinValue, maxValue) :
                DrawOutFloat(0, maxValue);
        public float DrawOutFloat(bool includeNegative = false)
            => includeNegative ? DrawOutFloat(float.MinValue, float.MaxValue) :
                DrawOutFloat(0, float.MaxValue);
        public double DrawOutDouble(double minValue, double maxValue)
            => double.Parse(RandomMinMaxValue(minValue.ToString(), maxValue.ToString(), false));
        public double DrawOutDouble(double maxValue, bool includeNegative = false)
            => includeNegative ? DrawOutDouble(double.MinValue, maxValue) :
                DrawOutDouble(0, maxValue);
        public double DrawOutDouble(bool includeNegative = false)
            => includeNegative ? DrawOutDouble(double.MinValue, double.MaxValue) :
                DrawOutDouble(0, double.MaxValue);
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
        public decimal DrawOutDecimal(decimal minValue, decimal maxValue)
            => decimal.Parse(RandomMinMaxValue(minValue.ToString(), maxValue.ToString(), false));
        public decimal DrawOutDecimal(decimal maxValue, bool includeNegative = false)
            => includeNegative ? DrawOutDecimal(decimal.MinValue, maxValue) :
                DrawOutDecimal(0, maxValue);
        public decimal DrawOutDecimal(bool includeNegative = false)
            => includeNegative ? DrawOutDecimal(decimal.MinValue, decimal.MaxValue) :
                DrawOutDecimal(0, decimal.MaxValue);
        protected string RandomMinMaxValue(string minValue, string maxValue, bool isInteger, int maxCompareRound = 30)
        {
            if (minValue == maxValue)
                return minValue;

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
            int minCompareDigitsCount = Mathematics.GetIntegerDigitsCount(minValue);
            int maxCompareDigitsCount = Mathematics.GetIntegerDigitsCount(maxValue);
            int startCompareDigit = minCompareDigitsCount < maxCompareDigitsCount ?
                maxCompareDigitsCount : minCompareDigitsCount;
            int maxLoop;
            minCompareString = minValue.Replace(".", "").Replace("-", "");
            if (minCompareString.IndexOf('E') != -1)
                minCompareString = minCompareString.Remove(minCompareString.IndexOf('E'));
            while (minCompareString.Length != 1 && minCompareString[0] == '0')
                minCompareString = minCompareString.Remove(0, 1);
            if (maxCompareDigitsCount > minCompareDigitsCount)
                if (maxCompareDigitsCount - minCompareDigitsCount > maxCompareRound)
                    minCompareString = minCompareString.Insert(0, new string('0', maxCompareRound));
                else
                    minCompareString = minCompareString.Insert(0, new string('0', maxCompareDigitsCount - minCompareDigitsCount));
            maxLoop = minCompareString.Length;
            minCompareString = minCompareString.PadRight(maxCompareRound, '0');

            maxCompareString = maxValue.Replace(".", "").Replace("-", "");
            if (maxCompareString.IndexOf('E') != -1)
                maxCompareString = maxCompareString.Remove(maxCompareString.IndexOf('E'));
            while (maxCompareString.Length != 1 && maxCompareString[0] == '0')
                maxCompareString = maxCompareString.Remove(0, 1);
            if (minCompareDigitsCount > maxCompareDigitsCount)
                if (minCompareDigitsCount - maxCompareDigitsCount > maxCompareRound)
                    maxCompareString = maxCompareString.Insert(0, new string('0', maxCompareRound));
                else
                    maxCompareString = maxCompareString.Insert(0, new string('0', minCompareDigitsCount - maxCompareDigitsCount));
            if (maxCompareString.Length > maxLoop)
                maxLoop = maxCompareString.Length;
            maxCompareString = maxCompareString.PadRight(maxCompareRound, '0');
            if (!isInteger)
                maxLoop = maxCompareRound;

            bool isDigitsCountUpperBound = true;
            bool isDigitsCountLowerBound = true;
            int isNegative = 0;
            numberString.Clear();
            int odds, lower, upper;
            int i;

            for (i = startCompareDigit; startCompareDigit - i < maxLoop &&
                startCompareDigit - i <= maxCompareRound ; i--)
            {
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
                    lower = 0;
                    upper = isNegative == 1 ? 1 : 1000000000;
                    if (isDigitsCountUpperBound)
                        upper = int.Parse(maxCompareString.Substring(startCompareDigit - i, 9));

                    if (isDigitsCountLowerBound)
                        if (isNegative != 2 || minValueNegative == maxValueNegative)
                            lower = int.Parse(minCompareString.Substring(startCompareDigit - i, 9))
                                    * (minValueNegative ^ maxValueNegative ? -1 : 1);
                    if (i % 2 == 0)
                        odds = _Random.Next(lower, upper);
                    else
                        odds = _Random2.Next(lower, upper);
                    string tempString = odds >= 0 ? odds.ToString().PadLeft(9, '0') :
                        $"-{Math.Abs(odds).ToString().PadLeft(9, '0')}";
                    if ((lower >= 0 && tempString[0] != lower.ToString().PadLeft(9, '0')[0]) ||
                         lower < 0 && tempString[1] != $"-{Math.Abs(lower).ToString().PadLeft(9, '0')}"[1])
                        isDigitsCountLowerBound = false;
                    if (tempString[0] != upper.ToString().PadLeft(9, '0')[0])
                        isDigitsCountUpperBound = false;
                    if (tempString[0] != '-')
                    {
                        if (minCompareString[startCompareDigit - i] != '0' || tempString[0] != '0')
                            isNegative = 2;
                        numberString.Append(tempString[0]);
                    }
                    else
                    {
                        if (minCompareString[startCompareDigit - i] != '0')
                            isNegative = 1;
                        numberString.Append(tempString[1]);
                    }
                }
            }

            if (startCompareDigit < -30 || startCompareDigit > 30) // Check
            {
                numberString.Insert(1, '.');
                numberString.AppendFormat("E{0}{1}", (startCompareDigit >= 0 ? "+" : ""), startCompareDigit);
            }
            else if (startCompareDigit < 0)
                numberString.Insert(0, $"0.{new string('0', -1 * (startCompareDigit - 1))}");
            else if (i >= 0)
                numberString.Append(new string('0', i));
            else if (startCompareDigit + 1 != numberString.Length)
                numberString.Insert(startCompareDigit + 1, '.');

            while (numberString.Length > 1 && numberString[0] == '0')
                numberString.Remove(0, 1);
            if (numberString[0] == '.')
                numberString.Insert(0, '0');
            if ((minValueNegative && maxValueNegative) || isNegative == 1)
                numberString.Insert(0, '-');
            return numberString.ToString();
        }

        public DateTime DrawOutDateTime()
            => DrawOutDateTime(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks);
        public DateTime DrawOutDateTime(DateTime maxValue)
            => DrawOutDateTime(DateTime.MinValue, maxValue);
        public DateTime DrawOutDateTime(DateTime minValue, DateTime maxValue)
            => DrawOutDateTime(minValue.Ticks, maxValue.Ticks);
        public DateTime DrawOutDateTime(long minTicks, long maxTicks)
            => new DateTime(DrawOutLong(minTicks, maxTicks));
        public DateTime DrawOutDate()
            => DrawOutDate(DateTime.MinValue.Date, DateTime.MaxValue.Date);
        public DateTime DrawOutDate(DateTime maxValue)
            => DrawOutDate(DateTime.MinValue.Date, maxValue);
        public DateTime DrawOutDate(DateTime minValue, DateTime maxValue)
            => DrawOutDateTime(minValue.Date, maxValue.Date).Date;

        //public int[] DrawOutIntegers(long count, int minValue = 0, int maxValue = byte.MaxValue, bool repetition = true)
        //{
        //    if (count < 0)
        //        throw new ArgumentOutOfRangeException(nameof(count));
        //    int[] result = new int[count];
        //    for (long i = 0; i < count; i++)
        //        result[i] = DrawOutByte(minValue, maxValue);
        //    return result;
        //}

        //public List<T> DrawOutFromList<T>(IList<T> list, int quantity = 1)
        //{
        //    if (list == null)
        //        throw new ArgumentNullException(nameof(list));
        //    if (quantity > list.Count)
        //        throw new ArgumentOutOfRangeException(nameof(quantity));
        //    else if (quantity == list.Count)
        //        return new List<T>(list);

        //    List<T> result = new List<T>();

        //    DrawOutBytes
        //}
    }
}
