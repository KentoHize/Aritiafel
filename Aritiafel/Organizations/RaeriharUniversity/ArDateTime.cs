using Aritiafel.Definitions;
using Aritiafel.Organizations.ArinaOrganization;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

//private const long TicksPerMillisecond = 10000L;
//private const long TicksPerSecond = 10000000L;
//private const long TicksPerMinute = 600000000L;
//private const long TicksPerHour = 36000000000L;
//private const long TicksPerDay = 864000000000;
//private const int DaysPer400Years = 146097;
//private const int DaysPer100Years = 36524;
//private const int DaysPer4Years = 1461;
//private const int DaysPerYear = 365;

//用毫秒為單位
//0:1年1月1日0時0分0秒
//-1000:-1年12月31日23時59分59秒
//可填負值與零的DateTime

namespace Aritiafel.Organizations.RaeriharUniversity
{
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct ArDateTime : IComparable, IComparable<ArDateTime>, IConvertible, IEquatable<ArDateTime>, IFormattable, ISerializable
    {
        internal long _data;
        static readonly int[] ConstDayInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// 最大值
        /// </summary>
        public static readonly ArDateTime MaxValue = new ArDateTime(9999, 12, 31, 23, 59, 59, 999);
        /// <summary>
        /// 最小值
        /// </summary>
        public static readonly ArDateTime MinValue = new ArDateTime(-9999, 1, 1, 0, 0, 0, 0);
        internal enum DatePart
        {
            Year,
            Month,
            Day
        }
        public long Ticks { get => _data; set => _data = value; }
        public ArDateTime(long ticks)
        {
            if (ticks > MaxValue.Ticks || ticks < MinValue.Ticks)
                throw new ArgumentOutOfRangeException(nameof(ticks));
            _data = ticks;
        }

        public ArDateTime(int year, int month, int day, bool isCEDate = false)
            : this(year, month, day, 0, 0, 0, 0, 0, isCEDate)
        { }

        public ArDateTime(int year, int month, int day, int hour, int minute, int second = 0, int millisecond = 0, bool isCEDate = false)
            : this(year, month, day, hour, minute, second, millisecond, 0, isCEDate)
        { }

        public ArDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int tick, bool isCEDate = false)
        {   
            if (!isCEDate)
                year = GetCEYear(year);
            ValidateDateTime(year, month, day, hour, minute, second, millisecond, tick, true);
            _data = DateTimeToTicks(year, month, day, hour, minute, second, millisecond, tick);
        }

        public ArDateTime(DateTime dt)
            : this(dt.Ticks)
        { }
        public static ArDateTime Now
            => new ArDateTime(DateTime.UtcNow);
        public static ArDateTime Today
            => Now.Date;

        internal static void ValidateDateTime(int year, int month, int day = 1, int hour = 0, int minute = 0, int second = 0, int millisecond = 0, int tick = 0, bool isCEDate = false)
        {
            if(isCEDate)
                year = GetARYear(year);
            if (year == 0 || year < -9999 || year > 9999)
                throw new ArgumentOutOfRangeException(nameof(year));
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month));
            if (hour < 0 || hour > 23)
                throw new ArgumentOutOfRangeException(nameof(hour));
            if (minute < 0 || minute > 59)
                throw new ArgumentOutOfRangeException(nameof(minute));
            if (second < 0 || second > 59)
                throw new ArgumentOutOfRangeException(nameof(second));
            if (millisecond < 0 || millisecond > 999)
                throw new ArgumentOutOfRangeException(nameof(millisecond));
            if (tick < 0 || tick > 9999)
                throw new ArgumentOutOfRangeException(nameof(tick));
            if (day < 1 || day > 31 ||
                day > ConstDayInMonth[month - 1] + 1 ||
                (day > ConstDayInMonth[month - 1] && (month != 2 || !IsLeapYear(year))))
                throw new ArgumentOutOfRangeException(nameof(day));
        }

        public static bool IsLeapYear(int year, bool isCEDate = false)
        {
            if (year == 0)
                throw new ArgumentException(nameof(year));
            if (!isCEDate)
                year = GetCEYear(year);
            if (year < 0)
                year = year % 400 + 401;
            if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))
                return true;
            return false;
        }

        public static int DaysInMonth(int year, int month, bool isCEDate = false)
        {
            ValidateDateTime(year, month);
            if (IsLeapYear(year, isCEDate) && month == 2)
                return ConstDayInMonth[month - 1] + 1;
            return ConstDayInMonth[month - 1];
        }
        public static int GetDayOfWeek(int year, int month, int day, bool isCEDate = false)
            => new ArDateTime(year, month, day, isCEDate).DayOfWeek;
        internal int GetDatePart(DatePart part = DatePart.Year)
        {
            int result;
            if (part == DatePart.Year)
                TicksToDateTime(_data, out result, out _, out _, out _, true);
            else if (part == DatePart.Month)
                TicksToDateTime(_data, out _, out result, out _, out _);
            else
                TicksToDateTime(_data, out _, out _, out result, out _);
            return result;
        }

        internal static void TimeTicksToTime(long timeTicks, out int hour, out int minute, out int second, out int millisecond, out int remainTicks)
        {
            hour = (int)Math.DivRem(timeTicks, 36000000000L, out timeTicks);
            minute = (int)Math.DivRem(timeTicks, 600000000L, out timeTicks);
            second = (int)Math.DivRem(timeTicks, 10000000L, out timeTicks);
            millisecond = (int)Math.DivRem(timeTicks, 10000, out timeTicks);
            remainTicks = (int)timeTicks;
        }

        internal static long DateTimeToTicks(int year, int month, int day, int hour, int minute, int second, int millisecond, int tick)
        {
            long result = 0, oy = year, d = 0;
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
            result += 36000000000L * hour;
            result += 600000000L * minute;
            result += 10000000L * second;
            result += 10000L * millisecond;
            result += tick;
            if (year < 0)
                result += year / 400 * 126227808000000000L - 126227808000000000L;
            return result;
        }

        internal static void TicksToDateTime(long ticks, out int year, out int month, out int day, out long timeTicks, bool onlyGetYear = false)
        {
            long n1, n2, n3, n4, n5, n6, n7, n8, n9;

            month = 1;
            day = 1;
            n1 = Math.DivRem(ticks, 864000000000, out timeTicks);
            n2 = Math.DivRem(n1, 146097, out n3);

            if (ticks < 0)
            {
                n2 -= 1;
                n3 += 146097; //從此為正數
                              //扣掉1天因為沒整除
                if (timeTicks != 0)
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

            year = (int)(n2 * 400 + n4 * 100 + n6 * 4 + n8);
            if (ticks >= 0)
                year += +1;

            if (onlyGetYear)
                return;

            n9 += 1;
            for (int i = 0; i < 12; i++)
            {
                if (n9 > ConstDayInMonth[i])
                {
                    if (i == 1 && ((n8 == 3 && n6 != 24) || (n8 == 3 && n4 == 3 && n6 == 24))) //Leap Year(4x || 400x) // To do
                    {
                        if (n9 == 29)
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
        public override string ToString()
            => ToString(null, null);
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);
        public string ToString(string format)
            => ToString(format, null);
        public string ToString(string format, IFormatProvider formatProvider)
            => ArDateTimeFormat.Format(this, format, formatProvider);
        public string ToLongDateString(IFormatProvider formatProvider = null)
            => ArDateTimeFormat.Format(this, "D", formatProvider);
        public string ToShortDateString(IFormatProvider formatProvider = null)
            => ArDateTimeFormat.Format(this, "d", formatProvider);
        public string ToLongTimeString(IFormatProvider formatProvider = null)
            => ArDateTimeFormat.Format(this, "T", formatProvider);
        public string ToShortTimeString(IFormatProvider formatProvider = null)
            => ArDateTimeFormat.Format(this, "t", formatProvider);
        public string ToStandardString(ArStandardDateTimeType type = ArStandardDateTimeType.DateTime, IFormatProvider formatProvider = null)
            => type switch
            {
                ArStandardDateTimeType.DateTime => ArDateTimeFormat.Format(this, "A", formatProvider),
                ArStandardDateTimeType.ShortDateTime => ArDateTimeFormat.Format(this, "a", formatProvider),
                ArStandardDateTimeType.Date => ArDateTimeFormat.Format(this, "B", formatProvider),
                ArStandardDateTimeType.ShortDate => ArDateTimeFormat.Format(this, "b", formatProvider),
                ArStandardDateTimeType.Time => ArDateTimeFormat.Format(this, "C", formatProvider),
                ArStandardDateTimeType.ShortTime => ArDateTimeFormat.Format(this, "c", formatProvider),
                _ => throw new NotSupportedException()
            };

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

        internal static int GetCEYear(int arYear)
            => arYear > 0 || arYear < -2017 ? arYear + 2017 : arYear + 2018;
        internal static int GetARYear(int year)
            => year > 2017 || year < 0 ? year - 2017 : year - 2018;

        public int CEYear
            => GetDatePart();
        public int Year
            => GetARYear(GetDatePart());
        public int Month
            => GetDatePart(DatePart.Month);
        public int Day
            => GetDatePart(DatePart.Day);
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

        public int RemainTick
        {
            get
            {
                long l = _data % 10000;
                if (_data < 0 && l != 0)
                    l += 10000;
                return (int)l;
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
        internal ArDateTime Add(long ticks)
            => new ArDateTime(_data + ticks);
        public ArDateTime Add(TimeSpan ts)
            => new ArDateTime(_data + ts.Ticks);

        public ArDateTime AddYears(int years)
        {
            TicksToDateTime(_data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            if (year + years == 0)
                years -= 1;
            return new ArDateTime(year + years, month, day).Add(timeTicks);
        }
        public ArDateTime AddMonths(int months)
        {
            TicksToDateTime(_data, out int year, out int month, out int day, out long timeTicks);
            int nm, y;
            y = Math.DivRem(month + months, 12, out nm);
            if (nm <= 0)
            {
                y -= 1;
                nm += 12;
            }
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            if (year + y == 0)
                y -= 1;
            return new ArDateTime(year + y, nm, day).Add(timeTicks);
        }

        public ArDateTime AddDays(long days)
            => new ArDateTime(_data + 864000000000L * days);
        public ArDateTime AddHours(long hours)
            => new ArDateTime(_data + 36000000000L * hours);
        public ArDateTime AddMinutes(long minutes)
            => new ArDateTime(_data + 600000000L * minutes);
        public ArDateTime AddSeconds(long seconds)
            => new ArDateTime(_data + 10000000L * seconds);
        public ArDateTime AddMilliSeconds(long milliseconds)
            => new ArDateTime(_data + 1000L * milliseconds);
        public ArDateTime AddTicks(long ticks)
            => new ArDateTime(_data + ticks);
        public ArDateTime ToTimeZoneTime(TimeZoneInfo tzi)
            => new ArDateTime(_data += tzi.BaseUtcOffset.Ticks);
        public ArDateTime ToLocalTimeZoneTime()
            => ToTimeZoneTime(TimeZoneInfo.Local);
        public static ArDateTime ParseExact(string s, string format)
            => ArDateTimeFormat.ParseExact(s, format, null, ArDateTimeStyles.None);
        public static ArDateTime ParseExact(string s, string format, IFormatProvider formatProvider)
            => ArDateTimeFormat.ParseExact(s, format, formatProvider, ArDateTimeStyles.None);
        public static ArDateTime ParseExact(string s, string format, ArDateTimeStyles dateTimeStyles)
            => ArDateTimeFormat.ParseExact(s, format, null, dateTimeStyles);
        public static ArDateTime ParseExact(string s, string format, IFormatProvider formatProvider, ArDateTimeStyles dateTimeStyles)
            => ArDateTimeFormat.ParseExact(s, format, formatProvider, dateTimeStyles);
        public static bool TryParseExact(string s, string format, out ArDateTime result)
            => ArDateTimeFormat.TryParseExact(s, format, null, ArDateTimeStyles.None, out result);
        public static bool TryParseExact(string s, string format, IFormatProvider formatProvider, out ArDateTime result)
            => ArDateTimeFormat.TryParseExact(s, format, formatProvider, ArDateTimeStyles.None, out result);
        public static bool TryParseExact(string s, string format, ArDateTimeStyles dateTimeStyles, out ArDateTime result)
            => ArDateTimeFormat.TryParseExact(s, format, null, dateTimeStyles, out result);
        public static bool TryParseExact(string s, string format, IFormatProvider formatProvider, ArDateTimeStyles dateTimeStyles, out ArDateTime result)
            => ArDateTimeFormat.TryParseExact(s, format, formatProvider, dateTimeStyles, out result);           
        public static ArDateTime Parse(string s)
            => Parse(s, null, ArDateTimeStyles.AllowWhiteSpaces);
        public static ArDateTime Parse(string s, IFormatProvider formatProvider)
            => Parse(s, formatProvider, ArDateTimeStyles.AllowWhiteSpaces);
        public static ArDateTime Parse(string s, ArDateTimeStyles dateTimeStyles)
            => Parse(s, null, dateTimeStyles);
        public static ArDateTime Parse(string s, IFormatProvider formatProvider, ArDateTimeStyles dateTimeStyles)
            => ArDateTimeFormat.Parse(s, formatProvider, dateTimeStyles);
        public static bool TryParse(string s, out ArDateTime result)
            => TryParse(s, null, ArDateTimeStyles.None, out result);
        public static bool TryParse(string s, IFormatProvider formatProvider, out ArDateTime result)
            => TryParse(s, formatProvider, ArDateTimeStyles.None, out result);
        public static bool TryParse(string s, ArDateTimeStyles dateTimeStyles, out ArDateTime result)
            => TryParse(s, null, dateTimeStyles, out result);
        public static bool TryParse(string s, IFormatProvider formatProvider, ArDateTimeStyles dateTimeStyles, out ArDateTime result)
        {
            try
            {
                result = Parse(s, formatProvider, dateTimeStyles);
                return true;
            }
            catch
            {
                result = default;
            }
            return false;
        }
        public override int GetHashCode()
            => (int)(_data ^ nameof(ArDateTime).GetHashCode());
        public static ArDateTime operator +(ArDateTime t1, ArDateTime t2)
            => new ArDateTime(t1._data + t2._data);
        public static ArDateTime operator +(ArDateTime t1, TimeSpan ts)
            => t1.AddTicks(ts.Ticks);
        public static TimeSpan operator -(ArDateTime t1, ArDateTime t2)
            => new TimeSpan(t1._data - t2._data);
        public static ArDateTime operator -(ArDateTime t1, TimeSpan ts)
            => t1.AddTicks(-ts.Ticks);
        public static bool operator !=(ArDateTime t1, ArDateTime t2)
            => t1._data != t2._data;
        public static bool operator ==(ArDateTime t1, ArDateTime t2)
            => t1._data == t2._data;
        public static bool operator <(ArDateTime t1, ArDateTime t2)
            => t1._data < t2._data;
        public static bool operator <=(ArDateTime t1, ArDateTime t2)
            => t1._data <= t2._data;
        public static bool operator >(ArDateTime t1, ArDateTime t2)
            => t1._data > t2._data;
        public static bool operator >=(ArDateTime t1, ArDateTime t2)
            => t1._data >= t2._data;

    }
}
