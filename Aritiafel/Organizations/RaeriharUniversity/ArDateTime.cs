using System;
using System.Collections.Generic;
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
        public long Ticks {  get => _data; set => _data = value; }
        public ArDateTime(long ticks)
            => _data = ticks;
        public ArDateTime(DateTime dt)
            => _data = dt.Ticks;

        //private const long TicksPerDay = 864000000000;
        //private const int DaysPer400Years = 146097;
        //private const int DaysPer100Years = 36524;
        //private const int DaysPer4Years = 1461;
        //private const int DaysPerYear = 365;


        //private const long TicksPer400Years = 126227808000000000;
        //private const long TicksPer100Years = 31556736000000000;
        //private const long TicksPer4Years = 

        public void AAA ()
        {
            //Test Area
            //DateTime
        }

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
            return base.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="part">0: 1: 2:</param>
        /// <returns></returns>
        internal static void TicksToDate(long ticks, out int year, out int month, out int day)
        {
            long n1, n2, n3, n4, n5, n6, n7, n8, n9, n10;
            if(ticks >= 0)
            {
                n1 = ticks / 864000000000;
            }
            else
            {
                n1 = ticks / 864000000000;
            }
            //n1 = days
            if(n1 >= 0)             
            {
                n2 = Math.DivRem(n1, 146097, out n3);                
            }
            else
            {
                n2 = Math.DivRem(n1, 146097, out n3) - 1;
                n3 += 146097; //從此為正數
            }

            n4 = Math.DivRem(n3, 36524, out n5);
            n6 = Math.DivRem(n5, 1461, out n7);

            year = (int)n2;
            month = (int)n3;
            day = 1;
        }

        public int Year
        {
            get
            {
                //_data < 0
                //事前處理
                TicksToDate(_data, out int y, out int m, out int d);
                return y;
                //long x, result, n1, n2;
                //x = _data;
                //result = 0;
                //if(x >= 0)
                //{   
                //    n1 = x / TicksPer400Years;
                //    n2 = x % TicksPer400Years;
                //    x = n2;
                //}
                //else
                //{
                //    n1 = x / TicksPer400Years;
                //    n2 = x % TicksPer400Years + TicksPer400Years;
                //    x = n2;
                //}

                //100
                //x / 

                //Console.Write(x.ToString());
                return 3;// (int)x;
            }
        }
    }
}
