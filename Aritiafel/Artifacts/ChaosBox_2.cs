using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Artifacts
{
    public partial class ChaosBox
    {  

        //因為無法定義泛型加法，先用同樣的方法複製貼上
        public decimal[] DrawOutDecimals(long count, decimal minValue = 0, decimal maxValue = decimal.MaxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue)
                throw new ArgumentException(CountGreaterThanValueRange);

            decimal[] result = new decimal[count];
            if (repeatable)
                for (long i = 0; i < count; i++)
                    result[i] = DrawOutDecimal(minValue, maxValue);
            else
            {
                decimal maxValueMinusCount = maxValue + 1 - count;
                SortedList<decimal, long> choiceList = new SortedList<decimal, long>();
                SortedList<long, decimal> extraSlot = new SortedList<long, decimal>();
                for (long i = 0; i < count; i++)
                {
                    decimal r = DrawOutDecimal(minValue, maxValueMinusCount + i);
                    if (r > maxValueMinusCount)
                        r = extraSlot[(long)Math.Ceiling(r - maxValueMinusCount)];
                    extraSlot.Add(i + 1, r);
                    if (choiceList.ContainsKey(r))
                        choiceList[r]++;
                    else
                        choiceList.Add(r, 1);
                }

                long choiceCount = 0;
                foreach (KeyValuePair<decimal, long> choice in choiceList)
                {
                    for (long i = 0; i < choice.Value; i++)
                    {
                        result[choiceCount] = choice.Key + choiceCount;
                        choiceCount++;
                    }
                }
            }
            return result;
        }

        public long[] DrawOutLongs(long count, long minValue = 0, long maxValue = long.MaxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue)
                throw new ArgumentException(CountGreaterThanValueRange);

            long[] result = new long[count];
            if (repeatable)
                for (long i = 0; i < count; i++)
                    result[i] = DrawOutLong(minValue, maxValue);
            else
            {
                long maxValueMinusCount = maxValue + 1 - count;
                SortedList<long, long> choiceList = new SortedList<long, long>();
                SortedList<long, long> extraSlot = new SortedList<long, long>();
                for (long i = 0; i < count; i++)
                {
                    long r = DrawOutLong(minValue, maxValueMinusCount + i);
                    if (r > maxValueMinusCount)
                        r = extraSlot[r - maxValueMinusCount];
                    extraSlot.Add(i + 1, r);
                    if (choiceList.ContainsKey(r))
                        choiceList[r]++;
                    else
                        choiceList.Add(r, 1);
                }

                long choiceCount = 0;
                foreach (KeyValuePair<long, long> choice in choiceList)
                {
                    for (long i = 0; i < choice.Value; i++)
                    {
                        result[choiceCount] = choice.Key + choiceCount;
                        choiceCount++;
                    }
                }
            }
            return result;
        }

        public double[] DrawOutDoubles(long count, double minValue = 0, double maxValue = double.MaxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue)
                throw new ArgumentException(CountGreaterThanValueRange);

            double[] result = new double[count];
            if (repeatable)
                for (long i = 0; i < count; i++)
                    result[i] = DrawOutDouble(minValue, maxValue);
            else
            {
                double maxValueMinusCount = maxValue + 1 - count;
                SortedList<double, long> choiceList = new SortedList<double, long>();
                SortedList<long, double> extraSlot = new SortedList<long, double>();
                for (long i = 0; i < count; i++)
                {
                    double r = DrawOutDouble(minValue, maxValueMinusCount + i);
                    if (r > maxValueMinusCount)
                        r = extraSlot[(long)Math.Ceiling(r - maxValueMinusCount)];
                    extraSlot.Add(i + 1, r);
                    if (choiceList.ContainsKey(r))
                        choiceList[r]++;
                    else
                        choiceList.Add(r, 1);
                }

                long choiceCount = 0;
                foreach (KeyValuePair<double, long> choice in choiceList)
                {
                    for (long i = 0; i < choice.Value; i++)
                    {
                        result[choiceCount] = choice.Key + choiceCount;
                        choiceCount++;
                    }
                }
            }
            return result;
        }

        public int[] DrawOutIntegers(long count, int minValue = 0, int maxValue = int.MaxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue)
                throw new ArgumentException(CountGreaterThanValueRange);

            int[] result = new int[count];
            if (repeatable)
                for (long i = 0; i < count; i++)
                    result[i] = DrawOutInteger(minValue, maxValue);
            else
            {
                int maxValueMinusCount = (int)(maxValue + 1 - count);
                SortedList<int, long> choiceList = new SortedList<int, long>();
                SortedList<long, int> extraSlot = new SortedList<long, int>();
                for (long i = 0; i < count; i++)
                {
                    int r = DrawOutInteger(minValue, (int)(maxValueMinusCount + i));
                    if (r > maxValueMinusCount)
                        r = extraSlot[r - maxValueMinusCount];
                    extraSlot.Add(i + 1, r);
                    if (choiceList.ContainsKey(r))
                        choiceList[r]++;
                    else
                        choiceList.Add(r, 1);
                }

                long choiceCount = 0;
                foreach (KeyValuePair<int, long> choice in choiceList)
                {
                    for (long i = 0; i < choice.Value; i++)
                    {
                        result[choiceCount] = (int)(choice.Key + choiceCount);
                        choiceCount++;
                    }
                }
            }
            return result;
        }

        public float[] DrawOutFloats(long count, float minValue = 0, float maxValue = float.MaxValue, bool repeatable = true)
            => Array.ConvertAll(DrawOutDoubles(count, minValue, maxValue, repeatable), m => (float)m);

        public short[] DrawOutShorts(long count, short minValue = 0, short maxValue = short.MaxValue, bool repeatable = true)
            => Array.ConvertAll(DrawOutIntegers(count, minValue, maxValue, repeatable), m => (short)m);

        public byte[] DrawOutBytes(long count, byte minValue = 0, byte maxValue = byte.MaxValue, bool repeatable = true)
        {
            if (!repeatable)
                return Array.ConvertAll(DrawOutIntegers(count, minValue, maxValue, repeatable), m => (byte)m);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            byte[] result = new byte[count];
            for (long i = 0; i < count; i++)
                result[i] = DrawOutByte(minValue, maxValue);
            return result;
        }
    }
}
