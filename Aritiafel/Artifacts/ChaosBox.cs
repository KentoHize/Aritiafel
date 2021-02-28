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
            _Random2 = new Random((int)(seed ^ _Random.Next(int.MinValue, int.MaxValue)));
        }

        public int DrawOutNormalizedInteger(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException(MinGreaterThanMaxMessage);
            double d = DrawOutDouble(0, Math.PI);
            if(d < Math.PI/ 2)
                return (int)Math.Round(Math.Sin(d) * (maxValue - minValue) / 2 + minValue);
            else
                return (int)Math.Round((1 - Math.Sin(d)) * ((maxValue - minValue) / 2) + (maxValue + minValue) / 2);
        }
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
                return (int)Math.Round((double)int.MaxValue * 2 * Math.Sin(d) + int.MinValue);
            }   
            if (d < Math.PI / 2)
                return (int)Math.Round(Math.Sin(d) * (double)int.MaxValue + int.MinValue);
            else
                return (int)Math.Round((1 - Math.Sin(d)) * (double)int.MaxValue);
        }
        public string DrawOutRandomLengthString(int maxLength = 10000, Encoding encoding = null)
            => DrawOutString((int)DrawOutInteger(1, maxLength), encoding);
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
    }
}
