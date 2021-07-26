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

        //public double[] DrawOutDoubles(long count, double minValue = 0, double maxValue = double.MaxValue, bool repeatable = true)
        //{
        //    if (count < 0)
        //        throw new ArgumentOutOfRangeException(nameof(count));
        //    if (minValue > maxValue)
        //        throw new ArgumentException(MinGreaterThanMaxMessage);
        //    if (!repeatable && count > maxValue - minValue)
        //        throw new ArgumentException(CountGreaterThanValueRange);

        //    double[] result = new double[count];
        //    if (repeatable)
        //        for (long i = 0; i < count; i++)
        //            result[i] = DrawOutDecimal(minValue, maxValue);
        //    else
        //    {
        //        double maxValueMinusCount = maxValue + 1 - count;
        //        SortedList<double, long> choiceList = new SortedList<double, long>();
        //        SortedList<double, double> extraSlot = new SortedList<double, double>();
        //        for (long i = 0; i < count; i++)
        //        {
        //            double r = DrawOutDecimal(minValue, maxValueMinusCount + i);
        //            if (r > maxValueMinusCount)
        //                r = extraSlot[Math.Ceiling(r - maxValueMinusCount)];
        //            extraSlot.Add(i + 1, r);
        //            if (choiceList.ContainsKey(r))
        //                choiceList[r]++;
        //            else
        //                choiceList.Add(r, 1);
        //        }

        //        long choiceCount = 0;
        //        foreach (KeyValuePair<decimal, long> choice in choiceList)
        //        {
        //            for (long i = 0; i < choice.Value; i++)
        //            {
        //                result[choiceCount] = choice.Key + choiceCount;
        //                choiceCount++;
        //            }
        //        }
        //    }
        //    return result;
        //}
        public byte[] DrawOutBytes(long count, byte minValue = 0, byte maxValue = byte.MaxValue)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            byte[] result = new byte[count];
            for (long i = 0; i < count; i++)
                result[i] = DrawOutByte(minValue, maxValue);
            return result;
        }
    }
}
