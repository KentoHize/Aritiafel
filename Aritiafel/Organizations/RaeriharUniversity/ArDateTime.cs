using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    public struct ArDateTime
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
        //    //Test Area
        //    //DateTime
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

        internal static void TicksToDate(long ticks, out int year, out int month, out int day, bool onlyGetYear = false)
        {
            int[] dayInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            long n1, n2, n3, n4, n5, n6, n7, n8, n9, nr = 0;
            
            month = 1;
            day = 1;
            if (ticks >= 0)
            {
                n1 = ticks / 864000000000;
            }
            else
            {
                n1 = Math.DivRem(ticks, 864000000000, out nr);
                //整除不可 - ToDo
                //n1 = ticks / 864000000000;
                //_data % t400 + t400
                //_data / t400 + 399 - dt.Year
            }
            //n1 = days
            if (ticks >= 0)
            {
                n2 = Math.DivRem(n1, 146097, out n3);
            }
            else
            {
                //146097 - n1 n1 -= 1; //扣除1天
                n2 = Math.DivRem(n1, 146097, out n3) - 1;
                n3 += 146097; //從此為正數
                //扣掉1天因為沒整除
                if (nr != 0)
                    n3 -= 1;
                //扣一天因為潤年
                n3 -= 1;
            }
            n4 = Math.DivRem(n3, 36524, out n5); //n4 = 多少個100年
            n6 = Math.DivRem(n5, 1461, out n7); //n6 = 多少個4年
            n8 = Math.DivRem(n7, 365, out n9); //n8 = 多少個1年
            //n9剩餘天數
            //if(onlyGetYear)
            //    Console.WriteLine($"n9:{n9} n8:{n8} n7:{n7} n6:{n6} n5:{n5} n4:{n4} n3:{n3} n2:{n2} n1:{n1}");
            if(ticks >= 0)
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
                        Console.WriteLine($"year:{year} n8:{n8} n6:{n6} n4:{n4}");                        
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
            if (ticks >= 0)
                day = (int)n9 + 1;
            else
                day = (int)n9 + 2;
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
            => _data >= 0 ? (int)(_data % 864000000000 / 36000000000) : 0;
        public int Minute
            => _data >= 0 ? (int)(_data % 36000000000 / 600000000) : 0;
        public int Second
            => _data >= 0 ? (int)(_data % 600000000 / 10000000) : 0;
        public int Millisecond
            => _data >= 0 ? (int)(_data % 10000000 / 10000) : 0;
        public int DayOfWeek
            => _data >= 0 ? (int)(_data % 6048000000000 / 864000000000 + 1) : 0;

    }
}
