using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Artifacts
{
    public partial class ChaosBox
    {

        //因為無法定義泛型加法，先用同樣的方法複製貼上
        public decimal[] DrawOutDecimals(long count, bool repeatable = true)
            => DrawOutDecimals(count, 0, decimal.MaxValue, repeatable);
        public decimal[] DrawOutDecimals(long count, decimal maxValue, bool repeatable = true)
            => DrawOutDecimals(count, 0, maxValue, repeatable);
        public decimal[] DrawOutDecimals(long count, decimal minValue, decimal maxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            else if (count == 0)
                return new decimal[] { };
            else if (count == 1)
                return new decimal[] { DrawOutDecimal(minValue, maxValue) };
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue + 1)
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
        public long[] DrawOutLongs(long count, bool repeatable = true)
            => DrawOutLongs(count, 0, long.MaxValue, repeatable);
        public long[] DrawOutLongs(long count, long maxValue, bool repeatable = true)
            => DrawOutLongs(count, 0, maxValue, repeatable);
        public long[] DrawOutLongs(long count, long minValue, long maxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            else if (count == 0)
                return new long[] { };
            else if (count == 1)
                return new long[] { DrawOutLong(minValue, maxValue) };
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue + 1)
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

        public double[] DrawOutDoubles(long count, bool repeatable = true)
            => DrawOutDoubles(count, 0, double.MaxValue, repeatable);
        public double[] DrawOutDoubles(long count, double maxValue, bool repeatable = true)
            => DrawOutDoubles(count, 0, maxValue, repeatable);
        public double[] DrawOutDoubles(long count, double minValue, double maxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            else if (count == 0)
                return new double[] { };
            else if (count == 1)
                return new double[] { DrawOutDouble(minValue, maxValue) };
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue + 1)
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

        public int[] DrawOutIntegers(long count, bool repeatable = true)
            => DrawOutIntegers(count, 0, int.MaxValue, repeatable);
        public int[] DrawOutIntegers(long count, int maxValue, bool repeatable = true)
            => DrawOutIntegers(count, 0, maxValue, repeatable);
        public int[] DrawOutIntegers(long count, int minValue, int maxValue, bool repeatable = true)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            else if (count == 0)
                return new int[] { };
            else if (count == 1)
                return new int[] { DrawOutInteger(minValue, maxValue) };
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            if (!repeatable && count > maxValue - minValue + 1)
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

        public float[] DrawOutFloats(long count, bool repeatable = true)
            => DrawOutFloats(count, 0, float.MaxValue, repeatable);
        public float[] DrawOutFloats(long count, float maxValue, bool repeatable = true)
            => DrawOutFloats(count, 0, maxValue, repeatable);
        public float[] DrawOutFloats(long count, float minValue, float maxValue, bool repeatable = true)
            => Array.ConvertAll(DrawOutDoubles(count, minValue, maxValue, repeatable), m => (float)m);

        public short[] DrawOutShorts(long count, bool repeatable = true)
            => DrawOutShorts(count, 0, short.MaxValue, repeatable);
        public short[] DrawOutShorts(long count, short maxValue, bool repeatable = true)
            => DrawOutShorts(count, 0, maxValue, repeatable);
        public short[] DrawOutShorts(long count, short minValue, short maxValue, bool repeatable = true)
            => Array.ConvertAll(DrawOutIntegers(count, minValue, maxValue, repeatable), m => (short)m);

        public byte[] DrawOutBytes(long count, bool repeatable = true)
            => DrawOutBytes(count, 0, byte.MaxValue, repeatable);
        public byte[] DrawOutBytes(long count, byte maxValue, bool repeatable = true)
            => DrawOutBytes(count, 0, maxValue, repeatable);
        public byte[] DrawOutBytes(long count, byte minValue, byte maxValue, bool repeatable = true)
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

        public T DrawOutFromArray<T>(T[] array)
            => array[DrawOutLong(array.LongLength - 1)];
        public T DrawOutFromList<T>(IList<T> list)
            => list[DrawOutInteger(list.Count - 1)];
        public T DrawOutFromCollection<T>(ICollection<T> collection)
        {
            if (collection is IList<T>)
                return DrawOutFromList((IList<T>)collection);
            int i = DrawOutInteger(collection.Count - 1);
            foreach(T item in collection)
            {
                if (i == 0)
                    return item;
                i--;
            }
            throw new IndexOutOfRangeException();
        }
        public T[] DrawOutItemsFromArray<T>(T[] array, long count)
        {
            if(count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            long[] indexes = DrawOutLongs(count, array.LongLength - 1, false);
            T[] result = new T[count];
            for (long i = 0; i < indexes.LongLength; i++)
                result[i] = array[indexes[i]];
            return result;
        }
          
        public List<T> DrawOutItemsFromList<T>(IList<T> list, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int[] indexes = DrawOutIntegers(count, list.Count - 1, false);
            List<T> result = new List<T>();
            for (int i = 0; i < indexes.Length; i++)
                result.Add(list[indexes[i]]);
            return result;
        }
        
        public List<T> DrawOutItemsFromCollection<T>(ICollection<T> collection, int count)
        {
            if (collection is IList<T>)
                return DrawOutItemsFromList((IList<T>)collection, count);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            int[] indexes = DrawOutIntegers(count, collection.Count - 1, false);
            List<T> result = new List<T>();
            Array.Sort(indexes);
            int i = 0, j = 0;
            foreach(T item in collection)
            {
                if (i == indexes[j])
                {
                    result.Add(item);
                    j++;
                    if (indexes.Length == j)
                        break;
                }   
                i++;
            }            
            return result;
        }   
    }
}
