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
                    return $"{hour}:{minute}:{second}.{millisecond}";
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

        //string[] s1;
        
        //s1 = s.Split(',');
        //month = int.Parse(s1[0]);
        //day = int.Parse(s1[1]);
        //s1 = s1[2].Split(':');
        //second = int.Parse(s1[2]);
        //minute = int.Parse(s1[1]);
        //s1 = s1[0].Trim().Split(' ');
        //year = int.Parse(s1[1]);
        //hour = int.Parse(s1[2]);            
        //return new ArDateTime(year, month, day, hour, minute, second, 0, true);
        internal static ArDateTime ParseExactArDateTime(string s, string format, DateTimeStyles dateTimeStyles)
        {
            string format1, format2 = null, format3 = null, format4 = null;
            ArDateTime result;
            string part;
            int i;
            int year, month, day, dayOfWeek, hour, minute, second, millisecond;
            //自帶邏輯
            //全滿：M, d, Ar. y [ddd] H:m:s.fff
            if (format == "D" || format == "d" ||
                format == "F" || format == "f" ||
                format == "M" || format == "m" ||
                format == "Y" || format == "y" ||
                format == "G" || format == "g") // M
            {
                i = s.IndexOf(',');
                month = int.Parse(s.Substring(0, i));                
                s = s.Substring(i + 1);
            }

            if (format == "D" || format == "d" ||
                format == "F" || format == "f" ||
                format == "M" || format == "m" ||                
                format == "G" || format == "g") // d
            {
                i = s.IndexOf(','); 
                day = int.Parse(s.Substring(0, i));
                s = s.Substring(i + 1);                
            }

            if (format == "D" || format == "d" ||
                format == "F" || format == "f" ||                
                format == "Y" || format == "y" ||
                format == "G" || format == "g") // y
            {
                s = s.Substring(s.IndexOf('.') + 1).TrimStart();                
                i = s.IndexOf(' ');
                year = int.Parse(s.Substring(0, i));
                s = s.Substring(i + 1);
            }

            if (format == "D" || format == "F") // dw
            {
                s = s.Substring(s.IndexOf('[') + 1);
                i = s.IndexOf(']');
                dayOfWeek = int.Parse(s.Substring(0, i));
                s = s.Substring(i + 1);
            }

            if (format == "T" || format == "t" ||
                format == "F" || format == "f" ||                
                format == "G" || format == "g") // h
            {   
                i = s.IndexOf(':');
                hour = int.Parse(s.Substring(0, i));
                s = s.Substring(i + 1);
            }

            if (format == "T" || format == "t" ||
                format == "F" || format == "f" ||                
                format == "G" || format == "g") // m
            {
                i = s.IndexOf(':');                
                if(i == -1)
                {
                    minute = int.Parse(s);
                    //break;

                }
                else
                    minute = int.Parse(s.Substring(0, i));
                s = s.Substring(i + 1);
            }

            if (format == "T" || format == "t" ||
                format == "F" || format == "f" ||
                format == "G") // s
            {
                i = s.IndexOf('.');
                if (i == -1)
                {
                    second = int.Parse(s);                    
                    //break;
                }
                else
                    second = int.Parse(s.Substring(0, i));
                s = s.Substring(i + 1);
            }

            if(format == "T") //f
            {
                millisecond = int.Parse(s.PadLeft(3, '0'));
            }

            //switch (format)
            //{
            //    case "D":
            //        format1 = $"M, d, Ar. y [ddd]";
            //        format2 = $"M, d, Ar. -y [ddd]";
            //        format3 = $"M, d, Ar. yyy [ddd]";                    
            //        format4 = $"M, d, Ar. -yyy [ddd]";
            //        break;
            //    case "d":
            //        format1 = $"M, d, Ar. y";
            //        format2 = $"M, d, Ar. -y";
            //        format3 = $"M, d, Ar. yyy";                    
            //        format4 = $"M, d, Ar. -yyy";
            //        break;
            //    case "T":
            //        format1 = $"H:m:s.fff";
            //        format2 = $"H:m:s.ff";
            //        format3 = $"H:m:s.f";
            //        break;                    
            //    case "t":
            //        format1 = $"H:m:s";
            //        break;
            //    case "F":
            //        format1 = $"M, d, Ar. y [ddd] H:m:s";
            //        format2 = $"M, d, Ar. -y [ddd] H:m:s";
            //        format3 = $"M, d, Ar. yyy [ddd] H:m:s";
            //        format4 = $"M, d, Ar. -yyy [ddd] H:m:s";
            //        break;
            //    case "M":
            //    case "m":
            //        format1 = $"M, d";
            //        break;
            //    case "Y":
            //    case "y":
            //        format1 = $"M, Ar. y";
            //        format2 = $"M, Ar. -y";
            //        format3 = $"M, Ar. yyy";
            //        format4 = $"M, Ar. -yyy";
            //        break;
            //    case "g":
            //        format1 = $"M, d, Ar. y H:m";
            //        format2 = $"M, d, Ar. -y H:m";
            //        format3 = $"M, d, Ar. yyy H:m";
            //        format4 = $"M, d, Ar. -yyy H:m";
            //        break;
            //    case "G":
            //    case "f":
            //        format1 = $"M, d, Ar. y H:m:s";
            //        format2 = $"M, d, Ar. \\-y H:m:s";
            //        format3 = $"M, d, Ar. yyy H:m:s";
            //        format4 = $"M, d, Ar. \\-yyy H:m:s";
            //        break;
            //    case "O":
            //    case "o":
            //    case "R":
            //    case "r":
            //    case "s":
            //    case "U":
            //    case "u":
            //    //不支援
            //    default:
            //        return ParseExact(s, format, CultureInfo.CurrentCulture, dateTimeStyles);
            //}

            //if (TryParseExact(s, format1, CultureInfo.CurrentCulture, dateTimeStyles, out result))
            //    return result;
            //if (format2 != null && TryParseExact(s, format2, CultureInfo.CurrentCulture, dateTimeStyles, out result))
            //    return result;
            //if (format3 != null && TryParseExact(s, format3, CultureInfo.CurrentCulture, dateTimeStyles, out result))
            //    return result;
            //if (format4 != null && TryParseExact(s, format4, CultureInfo.CurrentCulture, dateTimeStyles, out result))
            //    return result;
            throw new FormatException();
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

