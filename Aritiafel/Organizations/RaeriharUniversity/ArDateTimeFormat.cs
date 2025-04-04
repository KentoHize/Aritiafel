using Aritiafel.Organizations.RaeriharUniversity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    internal static class ArDateTimeFormat
    {

        public static string FormatArDateTime(string format, ArDateTime adt)
        {
            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            int hour = (int)Math.DivRem(timeTicks, 36000000000L, out timeTicks);
            int minute = (int)Math.DivRem(timeTicks, 600000000L, out timeTicks);
            int second = (int)Math.DivRem(timeTicks, 10000000L, out timeTicks);
            int millisecond = (int)Math.DivRem(timeTicks, 10000, out timeTicks);
            return $"{month}, {day}, Ar. {(year >= 2017 ? year - 2017 : year - 2018)} {hour}:{minute}:{second}";
        }

        public static ArDateTime ParseExactArDateTime(string s, string format, DateTimeStyles dateTimeStyles)
        {
            string[] s1;
            int year, month, day, hour, minute, second, millisecond;
            s1 = s.Split(',');
            month = int.Parse(s1[0]);
            day = int.Parse(s1[1]);
            s1 = s1[2].Split(':');
            second = int.Parse(s1[2]);
            minute = int.Parse(s1[1]);
            s1 = s1[0].Trim().Split(' ');
            year = int.Parse(s1[1]);
            hour = int.Parse(s1[2]);            
            return new ArDateTime(year, month, day, hour, minute, second, 0, true);
        }

        //Parse
        //Format
        //+(-)
        public static string Format(string format, ArDateTime adt, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
                return FormatArDateTime(format, adt);

            if (adt._data >= 0)
                return new DateTime(adt._data).ToString(format, formatProvider);

            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            DateTime dt = new DateTime(year * -1, month, day).AddTicks(timeTicks);
            return $"(-){dt.ToString(format, formatProvider)}";
        }

        public static ArDateTime ParseExact(string s, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            if (formatProvider == null)
                return ParseExactArDateTime(s, format, dateTimeStyles);

            if (s.StartsWith("(-)"))
            {
                s = s.Substring(3);
                DateTime dt = DateTime.ParseExact(s, format, formatProvider, dateTimeStyles);
                return new ArDateTime(dt.Year * -1, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            }

            return new ArDateTime(DateTime.ParseExact(s, format, formatProvider, dateTimeStyles).Ticks);
        }
    }
}
//public static ArDateTime Parse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles style)
//{
//    //先視為Null
//    //先認定基本yyyy/mm/dd hh:MM:ss
//    string[] s1 = s.Trim().Split(' ');
//    string[] s2 = s1[0].Split('/');
//    string[] s3 = s1[1].Split(':');
//    return new ArDateTime(int.Parse(s2[0]), int.Parse(s2[1]), int.Parse(s2[2]),
//        int.Parse(s3[0]), int.Parse(s3[1]), int.Parse(s3[2]), 0);

//    //DateTime.Parse(string s, IFormatProvider provider, System.Globalization.DateTimeStyles style)
//}
