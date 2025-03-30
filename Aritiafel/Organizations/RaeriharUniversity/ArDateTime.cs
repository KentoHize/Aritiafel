﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

//To Do 3, 30, Ar.8


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

        internal enum DatePart
        {
            Year,
            Month,
            Day
        }
        
        public long Ticks { get => _data; set => _data = value; }
        
        
        public ArDateTime(long ticks)
            => _data = ticks;
        public ArDateTime(DateTime dt, bool Negative = false)
        {
            if(!Negative)
            {
                _data = dt.Ticks;
            }
            else
            {                
                //    TicksPer400Years = 126227808000000000
                //To Do
                long n1, n2;
                n1 = Math.DivRem(dt.Ticks, 126227808000000000, out n2);
                Console.WriteLine($"n1:{n1} n2:{n2} ");
                _data = -(n1) * 126227808000000000 - (126227808000000000 - n2);
            }
        }


        //private const long TicksPerMillisecond = 10000L;
        //private const long TicksPerSecond = 10000000L;
        //private const long TicksPerMinute = 600000000L;
        //private const long TicksPerHour = 36000000000L;
        //private const long TicksPerDay = 864000000000;
        //private const int DaysPer400Years = 146097;
        //private const int DaysPer100Years = 36524;
        //private const int DaysPer4Years = 1461;
        //private const int DaysPerYear = 365;

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

        public override string ToString()
        {
            return Ticks.ToString();
        }

        internal int GetDatePart(DatePart part = DatePart.Year)
        {
            int result;
            if (part == DatePart.Year)
                TicksToDate(_data, out result, out _, out _, true);
            else if(part == DatePart.Month)
                TicksToDate(_data, out _, out result, out _);
            else
                TicksToDate(_data, out _, out _, out result);
            return result;
        }

        //internal static long DateToTicks(int year, int month, int day, int hour, int minute, int second, int milliseconds)
        //{
            
        //    return 0;
        //}

        internal static void TicksToDate(long ticks, out int year, out int month, out int day, bool onlyGetYear = false)
        {
            int[] dayInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            long n1, n2, n3, n4, n5, n6, n7, n8, n9, nr = 0;
            
            month = 1;
            day = 1;
            n1 = Math.DivRem(ticks, 864000000000, out nr);
            

            if (ticks >= 0)
            {
                n2 = Math.DivRem(n1, 146097, out n3);
            }
            else
            {   
                n2 = Math.DivRem(n1, 146097, out n3) - 1;
                n3 += 146097; //從此為正數
                //扣掉1天因為沒整除
                if (nr != 0)
                    n3 -= 1;                
            }
            n4 = Math.DivRem(n3, 36524, out n5); //n4 = 多少個100年
            if(n4 == 4) //整除為4 其實為3
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
            if (ticks >= 0)
                year = (int)(n2 * 400 + n4 * 100 + n6 * 4 + n8) + 1;
            else
                year = (int)(n2 * 400 + n4 * 100 + n6 * 4 + n8);
            if (onlyGetYear)
                return;
            
            for (int i = 0; i < 12; i++)
            {   
                if (n9 > dayInMonth[i])
                {
                    if(i == 1)
                    {
                        //Console.WriteLine($"n8:{n8} n7:{n7} n6:{n6} n5:{n5} n4:{n4} n3:{n3} n2:{n2}");
                    }
                    
                    if (i == 1 && ((n8 == 3 && n6 != 24) || (n8 == 3 && n4 == 3 && n6 == 24))) //Leap Year(4x || 400x) // To dO
                    {
                        //Console.WriteLine($"year:{year} n8:{n8} n6:{n6} n4:{n4}");                        
                        n9 -= 1;                        
                        if (n9 == dayInMonth[i])
                        {
                            n9 -= dayInMonth[i];
                            month = i + 2;
                            break;
                        }
                    }   
                    n9 -= dayInMonth[i]; 
                }
                else
                {
                    month = i + 1;
                    break;
                }                
            }
            day = (int)n9 + 1;            
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
            throw new NotImplementedException();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
            => info.AddValue("data", _data);

        public TypeCode GetTypeCode()
            => TypeCode.Int64;
        public bool ToBoolean(IFormatProvider provider)
            => ((IConvertible)_data).ToBoolean(provider);
        public byte ToByte(IFormatProvider provider)
            => ((IConvertible)_data).ToByte(provider);
        public char ToChar(IFormatProvider provider)
            => ((IConvertible)_data).ToChar(provider);
        public DateTime ToDateTime(IFormatProvider provider)
            => ((IConvertible)_data).ToDateTime(provider);
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
        public string ToString(IFormatProvider provider)
            => _data.ToString(provider); // To Do
        public object ToType(Type conversionType, IFormatProvider provider)        
            => ((IConvertible)_data).ToType(conversionType, provider);
        public ushort ToUInt16(IFormatProvider provider)        
            => ((IConvertible)_data).ToUInt16(provider);
        public uint ToUInt32(IFormatProvider provider)
            => ((IConvertible)_data).ToUInt32(provider);
        public ulong ToUInt64(IFormatProvider provider)
            => ((IConvertible)_data).ToUInt64(provider);

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

    }
}
