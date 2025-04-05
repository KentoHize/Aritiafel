﻿using Aritiafel.Organizations.RaeriharUniversity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    internal static class ArDateTimeFormat
    {
        //public static bool ValidateDateTime(string s, string format)
        //{
        //    //先接受普通format

        //}
        
        //Format
        internal static string FormatArDateTime(string format, ArDateTime adt)
        {
            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            ArDateTime.TimeTicksToTime(timeTicks, out int hour, out int minute, out int second, out int millisecond, out _);
            switch(format)
            {   
                case "D":
                    return $"{month}, {day}, Ar. {ArDateTime.GetARYear(year)} [{adt.DayOfWeek}]";
                case "d":
                    return $"{month}, {day}, Ar. {ArDateTime.GetARYear(year)}";
                case "T":
                    return $"{hour}:{minute}:{second}.{millisecond:000}";
                case "t":
                    return $"{hour}:{minute}:{second}";
                case "F":                
                    return $"{month}, {day}, Ar. {ArDateTime.GetARYear(year)} [{adt.DayOfWeek}] {hour}:{minute}:{second}";
                case "M":
                case "m":
                    return $"{month}, {day}";
                case "Y":
                case "y":
                    return $"{month}, Ar. {ArDateTime.GetARYear(year)}";
                case "g":
                    return $"{month}, {day}, Ar. {ArDateTime.GetARYear(year)} {hour}:{minute}";
                case "G":
                case "f":
                    return $"{month}, {day}, Ar. {ArDateTime.GetARYear(year)} {hour}:{minute}:{second}";
                case "O":
                case "o":
                case "R":
                case "r":
                case "s":
                case "U":
                case "u":
                    //不支援
                default:
                    return Format(format, adt, CultureInfo.CurrentCulture);
            }
        }

        //Format
        internal static ArDateTime ParseExactArDateTime(string s, string format, DateTimeStyles dateTimeStyles)
        {
            int i = 0, j;
            int year = 1, month = 1, day = 1, hour = 0, minute = 0, second = 0, millisecond = 0, dayOfWeek;
            char[] SupportedFormatChar = { 'D', 'd', 'F', 'f', 'M', 'm', 'Y', 'y', 'G', 'g', 'T', 't' };

            //沒有支援，普通Parse
            if (format.Length != 1 || !SupportedFormatChar.Any(m => m == format[0]))
                return ParseExact(s, format, CultureInfo.CurrentCulture, dateTimeStyles);
            
            //支援格式
            //全滿：M, d, Ar. y [ddd] H:m:s.fff
            if (format == "D" || format == "d" ||
                format == "F" || format == "f" ||
                format == "M" || format == "m" ||
                format == "Y" || format == "y" ||
                format == "G" || format == "g") // M
            {
                j = s.IndexOf(',');
                month = int.Parse(s.Substring(i, j - i));
                i = j + 1;                
            }

            if (format == "D" || format == "d" ||
                format == "F" || format == "f" ||
                format == "M" || format == "m" ||                
                format == "G" || format == "g") // d
            {
                j = s.IndexOf(',', i);
                if(j == - 1)
                {
                    day = int.Parse(s.Substring(i));
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
                }
                day = int.Parse(s.Substring(i, j - i));
                i = j + 1;
                if (i == s.Length)
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
            }

            if (format == "D" || format == "d" ||
                format == "F" || format == "f" ||                
                format == "Y" || format == "y" ||
                format == "G" || format == "g") // y
            {
                i = s.IndexOf('.', i) + 1;
                i = s.IndexOf(' ', i) + 1;
                j = s.IndexOf(' ', i);
                if(j == - 1)
                {
                    year = int.Parse(s.Substring(i));
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
                }
                year = int.Parse(s.Substring(i, j - i));
                i = j + 1;
                if (i == s.Length)
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
            }

            if (format == "D" || format == "F") // dw
            {
                i = s.IndexOf('[' , i) + 1;
                j = s.IndexOf(']', i);
                dayOfWeek = int.Parse(s.Substring(i, j - i));                
                i = j + 1;
                if (i == s.Length)
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true); //先不驗證
                //{
                //    ArDateTime adt = new ArDateTime(year, month, day, hour, minute, second, millisecond, true);                    
                //    if (adt.DayOfWeek != dayOfWeek)
                //        throw new ArgumentException(nameof(dayOfWeek));
                //    return adt;
                //}
            }

            if (format == "T" || format == "t" ||
                format == "F" || format == "f" ||                
                format == "G" || format == "g") // h
            {
                j = s.IndexOf(':', i);
                hour = int.Parse(s.Substring(i, j - i));
                i = j + 1;
            }

            if (format == "T" || format == "t" ||
                format == "F" || format == "f" ||                
                format == "G" || format == "g") // m
            {
                j = s.IndexOf(':', i);
                if(j == -1)
                {
                    minute = int.Parse(s.Substring(i));
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
                }
                minute = int.Parse(s.Substring(i, j - i));
                i = j + 1;
                if (i == s.Length)
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
            }

            if (format == "T" || format == "t" ||
                format == "F" || format == "f" ||
                format == "G") // s
            {
                j = s.IndexOf('.', i);
                if (j == -1)
                {
                    second = int.Parse(s.Substring(i));
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
                }
                second = int.Parse(s.Substring(i, j - i));
                i = j + 1;
                if (i == s.Length)
                    return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);
            }
            if(format == "T") //f
                millisecond = int.Parse(s.Substring(i).PadLeft(3, '0'));

            return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);       
           
        }
        //+(-)
        public static string Format(string format, ArDateTime adt, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

            if (formatProvider == null)
                return FormatArDateTime(format, adt);

            if (adt._data >= 0)
                return new DateTime(adt._data).ToString(format, formatProvider);

            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks != 0)
                timeTicks += 864000000000L;
            DateTime dt = new DateTime(year * -1, month, day).AddTicks(timeTicks);
            return $"(-){dt.ToString(format, formatProvider)}";
        }

        public static ArDateTime ParseExact(string s, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

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

        public static bool TryParseExact (string s, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out ArDateTime result)
        {
            try
            {
                result = ParseExact(s, format, formatProvider, dateTimeStyles);
                return true;
            }
            catch
            {
                result = default;
            }
            return false;
        }
    }
}

