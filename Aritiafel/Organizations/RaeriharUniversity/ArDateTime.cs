using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Globalization;

//To Do 3, 30, Ar.8


//private const long TicksPerMillisecond = 10000L;
//private const long TicksPerSecond = 10000000L;
//private const long TicksPerMinute = 600000000L;
//private const long TicksPerHour = 36000000000L;
//private const long TicksPerDay = 864000000000;
//private const int DaysPer400Years = 146097;
//private const int DaysPer100Years = 36524;
//private const int DaysPer4Years = 1461;
//private const int DaysPerYear = 365;

//265248000000000
//864000000000

//public void AAA()
//{
//n1 = ticks / 864000000000;
//_data % t400 + t400
//_data / t400 + 399 - dt.Year
//}

//private static long getModTick(long ticks)
//{

//    if (ticks < 0)
//        return ticks % TicksPer400Years + TicksPer400Years;
//    else
//        return ticks % TicksPer400Years;
//}


//用毫秒為單位
//0:1年1月1日0時0分0秒
//-1000:-1年12月31日23時59分59秒
//可填負值與零的DateTime
//ToString =>複雜

namespace Aritiafel.Organizations.RaeriharUniversity
{
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct ArDateTime : IComparable, IComparable<ArDateTime>, IConvertible, IEquatable<ArDateTime>, IFormattable, ISerializable
    {
        long _data;
        static readonly int[] ConstDayInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        static readonly ArDateTime MaxValue = new ArDateTime(9999, 12, 31, 23, 59, 59, 999);
        static readonly ArDateTime MinValue = new ArDateTime(-9999, 1, 1, 1, 0, 0, 0);
        internal enum DatePart
        {
            Year,
            Month,
            Day
        }
        public long Ticks { get => _data; set => _data = value; }
        public ArDateTime(long ticks)
            => _data = ticks;
        public ArDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {   
            //Check
            //            if(year < -9999 || year > 9999)
            //                throw new Exception()
            _data = DateToTicks(year, month, day, hour, minute, second, millisecond);
        }

        public ArDateTime(DateTime dt)
            => _data = dt.Ticks;
        public static ArDateTime Now
            => new ArDateTime(DateTime.Now);
        public static ArDateTime UtcNow
            => new ArDateTime(DateTime.UtcNow);
        public static ArDateTime Today
            => Now.Date;

        public static bool IsLeapYear(int year)
        {
            if (year == 0)
                throw new ArgumentException(nameof(year));
            if (year < 0)
                year = year % 400 + 401;
            if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))
                return true;
            return false;
        }

        public override string ToString()
            => ToString(null, null);
        
        public static int DaysInMonth(int year, int month)
        {   
            //Check Month
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month));
            if (IsLeapYear(year) && month == 2)
                return 29;
            return ConstDayInMonth[month];
        }

        internal int GetDatePart(DatePart part = DatePart.Year)
        {
            int result;
            if (part == DatePart.Year)
                TicksToDate(_data, out result, out _, out _, true);
            else if (part == DatePart.Month)
                TicksToDate(_data, out _, out result, out _);
            else
                TicksToDate(_data, out _, out _, out result);
            return result;
        }

        internal static long DateToTicks(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            long result = 0, oy = year, d = 0;
            //Day Part            
            d += day - 1;
            for (int i = 0; i < month - 1; i++)
            {
                d += ConstDayInMonth[i];
                if (i == 1 && IsLeapYear(year))
                    d += 1;
            }
            if (year < 0)
                oy = oy % 400 + 401;//變為正數
            oy -= 1;
            d += Math.DivRem(oy, 400, out oy) * 146097;
            d += Math.DivRem(oy, 100, out oy) * 36524;
            d += Math.DivRem(oy, 4, out oy) * 1461;
            d += oy * 365;                
            result += 864000000000L * d;
            //Time Part
            result += 36000000000L * hour;
            result += 600000000L * minute;
            result += 10000000L * second;
            result += 10000L * millisecond;

            if(year < 0)
                result += year / 400 * 126227808000000000L - 126227808000000000L;
            return result;
        }

        internal static void TicksToDate(long ticks, out int year, out int month, out int day, bool onlyGetYear = false)
        {
            long n1, n2, n3, n4, n5, n6, n7, n8, n9, nr = 0;

            month = 1;
            day = 1;
            n1 = Math.DivRem(ticks, 864000000000, out nr);
            n2 = Math.DivRem(n1, 146097, out n3);

            if(ticks < 0)
            {
                n2 -= 1;
                n3 += 146097; //從此為正數
                    //扣掉1天因為沒整除
                if (nr != 0)
                    n3 -= 1;
            }
            
            n4 = Math.DivRem(n3, 36524, out n5); //n4 = 多少個100年
            if (n4 == 4) //整除為4 其實為3
            {
                n4 = 3;
                n5 += 36524;
            }
            n6 = Math.DivRem(n5, 1461, out n7); //n6 = 多少個4年
            n8 = Math.DivRem(n7, 365, out n9); //n8 = 多少個1年
            if (n8 == 4) //整除為4 其實為3
            {
                n8 = 3;
                n9 += 365;
            }
            //n9剩餘天數
            //if(onlyGetYear)
            //    Console.WriteLine($"n9:{n9} n8:{n8} n7:{n7} n6:{n6} n5:{n5} n4:{n4} n3:{n3} n2:{n2} n1:{n1}");
            year = (int)(n2 * 400 + n4 * 100 + n6 * 4 + n8);
            if (ticks >= 0)
                year +=  + 1;

            if (onlyGetYear)
                return;

            n9 += 1;
            for (int i = 0; i < 12; i++)
            {
                if (n9 > ConstDayInMonth[i])
                {
                    if (i == 1 && ((n8 == 3 && n6 != 24) || (n8 == 3 && n4 == 3 && n6 == 24))) //Leap Year(4x || 400x) // To do
                    {   
                        if(n9 == 29)
                        {
                            month = i + 1;
                            break;
                        }
                        n9 -= 1;                     
                    }
                    n9 -= ConstDayInMonth[i];
                }
                else
                {
                    month = i + 1;
                    break;
                }
            }
            day = (int)n9;
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(this, obj))
                return 0;
            else if (obj is null)
                return 1;
            else if (obj is ArDateTime b)
                return CompareTo(b);
            else
                throw new ArgumentException(nameof(obj));
        }

        public int CompareTo(ArDateTime other)
            => _data.CompareTo(other._data);

        public bool Equals(ArDateTime other)
            => _data == other._data;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj is null)
                return false;
            else if (obj is ArDateTime b)
                return Equals(b);
            else
                return false;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{Year}/{Month}/{Day} {Hour}:{Minute}:{Second}.{Millisecond}";
            return _data.ToString(format, formatProvider);
            // To Do
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
            => info.AddValue("data", _data);

        public TypeCode GetTypeCode()
            => TypeCode.Int64;
        public bool ToBoolean(IFormatProvider provider)
            => throw new InvalidCastException();
        public byte ToByte(IFormatProvider provider)
            => throw new InvalidCastException();
        public char ToChar(IFormatProvider provider)
            => throw new InvalidCastException();
        public DateTime ToDateTime(IFormatProvider provider)
            => _data >= 0 ? new DateTime(_data) : throw new InvalidCastException();
        public decimal ToDecimal(IFormatProvider provider)
            => ((IConvertible)_data).ToDecimal(provider);
        public double ToDouble(IFormatProvider provider)
            => ((IConvertible)_data).ToDouble(provider);
        public short ToInt16(IFormatProvider provider)
            => ((IConvertible)_data).ToInt16(provider);
        public int ToInt32(IFormatProvider provider)
            => ((IConvertible)_data).ToInt32(provider);
        public long ToInt64(IFormatProvider provider)
            => ((IConvertible)_data).ToInt64(provider);
        public sbyte ToSByte(IFormatProvider provider)
            => ((IConvertible)_data).ToSByte(provider);
        public float ToSingle(IFormatProvider provider)
            => ((IConvertible)_data).ToSingle(provider);
        public object ToType(Type conversionType, IFormatProvider provider)
            => ((IConvertible)_data).ToType(conversionType, provider);
        public ushort ToUInt16(IFormatProvider provider)
            => ((IConvertible)_data).ToUInt16(provider);
        public uint ToUInt32(IFormatProvider provider)
            => ((IConvertible)_data).ToUInt32(provider);
        public ulong ToUInt64(IFormatProvider provider)
            => ((IConvertible)_data).ToUInt64(provider);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);

        public ArDateTime Date
        {
            get
            {
                long r = Math.DivRem(_data, 864000000000, out long nr);
                if (_data < 0 && nr == 0)
                    r += 1;
                return new ArDateTime(r * 864000000000);
            }
        }
            
        public int Year
        {
            get
            {
                return GetDatePart();
            }
        }

        public int Month
        {
            get
            {
                return GetDatePart(DatePart.Month);
            }
        }

        public int Day
        {
            get
            {
                return GetDatePart(DatePart.Day);
            }
        }

        public int Hour
        {
            get
            {
                long l = _data % 864000000000;
                if (_data < 0 && l != 0)
                    l += 864000000000;
                return (int)(l / 36000000000);
            }
        }

        public int Minute
        {
            get
            {
                long l = _data % 36000000000;
                if (_data < 0 && l != 0)
                    l += 36000000000;
                return (int)(l / 600000000);
            }
        }
        public int Second
        {
            get
            {
                long l = _data % 600000000;
                if (_data < 0 && l != 0)
                    l += 600000000;
                return (int)(l / 10000000);
            }
        }
        public int Millisecond
        {
            get
            {
                long l = _data % 10000000;
                if (_data < 0 && l != 0)
                    l += 10000000;
                return (int)(l / 10000);
            }
        }
        public int DayOfWeek
        {
            get
            {
                long l = _data % 6048000000000;
                if (_data < 0 && l != 0)
                    l += 6048000000000;
                return (int)(l / 864000000000 + 1);
            }
        }

        public TimeSpan TimeOfDay
        {
            get
            {
                long l = _data % 864000000000;
                if (_data < 0 && l != 0)
                    l += 864000000000;
                return new TimeSpan(l);
            }
        }  

        public static ArDateTime Parse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles style)
        {
            //先視為Null
            //先認定基本yyyy/mm/dd hh:MM:ss
            string[] s1 = s.Trim().Split(' ');
            string[] s2 = s1[0].Split('/');
            string[] s3 = s1[1].Split(':');
            return new ArDateTime(int.Parse(s2[0]), int.Parse(s2[1]), int.Parse(s2[2]),
                int.Parse(s3[0]), int.Parse(s3[1]), int.Parse(s3[2]), 0);

            //DateTime.Parse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles style)
        }

    }
}
