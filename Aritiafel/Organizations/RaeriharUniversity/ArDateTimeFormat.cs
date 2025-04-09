using Aritiafel.Definitions;
using Aritiafel.Items;
using Aritiafel.Locations;
using Aritiafel.Organizations.ArinaOrganization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace Aritiafel.Organizations.RaeriharUniversity
{
    //internal
    public static class ArDateTimeFormat
    {
        static readonly string SystemDateTimePattern = "g yyyy/M/d h:m:s.fffffff"; //補空
        static readonly string ArinaBaseCultureName = "ja-JP";

        internal static readonly char[] AllStandardFormatChar = 
        {
            'd', 'D', 'f', 'F', 'g', 'G', 'm', 'M', 'o', 'O',
            'r', 'R', 's', 't', 'T', 'u', 'U', 'y', 'Y'
        };
        
        internal static readonly char[] SupportFormatChar =
        { 'D', 'd', 'F', 'f', 'M', 'm', 'Y', 'y', 'G', 'g', 'T', 't' };

        internal static readonly string[] SortedAllCustomFormatString =
        {
            ":", "/", "hh", "HH", "mm", "ss", "h", "H", "m", "s",
            "dddd", "ddd", "dd", "yyyyy", "yyyy", "yyy", "yy",
            "MMMM", "MMM", "MM", "y", "M", "d",
            "fffffff", "ffffff", "fffff", "ffff", "fff", "ff", "f",
            "tt", "t", "gg", "g", "K", "zzz", "zz", "z", "FFFFFFF", "FFFFFF",
            "FFFFF", "FFFF", "FFF", "FF", "F"
        }; 

        static internal ArStringPartInfo[] CreateReversedStringPartInfo(bool analyzeInteger = false)
        {
            List<ArStringPartInfo> result;
            result = DisassembleShop.StringToPartInfoList(SortedAllCustomFormatString);
            result.Insert(0, new ArStringPartInfo("p", "%", ArStringPartType.Escape1));
            result.Insert(0, new ArStringPartInfo("bs", "\\\\", ArStringPartType.Escape1));
            if (analyzeInteger)
            {
                result.Insert(0, new ArStringPartInfo("n4", "", ArStringPartType.Integer, 4));
                result.Insert(0, new ArStringPartInfo("n2", "", ArStringPartType.Integer, 2));
            }
            return result.ToArray();
        }

        public static string FormatDateTimeFull(ArDateTime adt, string format, IFormatProvider provider = null)
        {
            DateTimeFormatInfo dtf = null;
            if (provider is CultureInfo ci)
                dtf = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo di)
                dtf = di;
            else
                dtf = Mylar.ArinaCultureInfo.DateTimeFormat;
            if (format.Length == 1)
                format = dtf.GetAllDateTimePatterns(format[0])[0];
            
            ArStringPartInfo[] reservedString = CreateReversedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();
            ArOutStringPartInfo[] ospi = ds.Disassemble(format, reservedString);
            
            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            ArDateTime.TimeTicksToTime(timeTicks, out int hour, out int minute, out int second, out int millisecond, out int tick);
            string decimalPart = (millisecond * 10000 + tick).ToString().PadLeft(7, '0');
            string s;

            int dow = ArDateTime.GetDayOfWeek(year, month, day);
            if (dow == 7) 
                dow = 0;
            ArCultureInfo ac = provider as ArCultureInfo;
            if (ac != null)
                year = ArDateTime.GetARYear(year);                

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < ospi.Length; i++)
            {
                if (ospi[i].Index == 1)
                    ospi[i].Index = Array.FindIndex(reservedString, m => m.Value == ospi[i].Value);

                switch (ospi[i].Index)
                {
                    case -1:
                    case 0:
                        sb.Append(ospi[i].Value);
                        break;
                    case 1: // %
                        throw new FormatException("%");
                    case 2: // :
                        sb.Append(dtf.TimeSeparator);
                        break;
                    case 3: // /
                        sb.Append(dtf.DateSeparator);
                        break;
                    case 4: // "hh"
                        sb.Append((hour % 12).ToString("00"));
                        break;
                    case 5: // "HH"
                        sb.Append(hour.ToString("00"));
                        break;
                    case 6: // "mm"
                        sb.Append(minute.ToString("00"));
                        break;
                    case 7: // "ss"
                        sb.Append(second.ToString("00"));
                        break;
                    case 8: // "h"
                        sb.Append(hour % 12);
                        break;
                    case 9: // "H"
                        sb.Append(hour);
                        break;
                    case 10: // "m"
                        sb.Append(minute);
                        break;
                    case 11: // "s"
                        sb.Append(second);
                        break;
                    case 12: // "dddd"                        
                        sb.Append(dtf.GetDayName((DayOfWeek)dow));
                        break;
                    case 13: // "ddd"
                        sb.Append(dtf.GetAbbreviatedDayName((DayOfWeek)dow));
                        break;
                    case 14: // "dd"
                        sb.Append(day.ToString("00"));
                        break;
                    case 15: // "yyyyy"
                        sb.Append(year.ToString("00000"));
                        break;
                    case 16: // "yyyy"
                        sb.Append(year.ToString("0000"));
                        break;
                    case 17: // "yyy"
                        sb.Append(year.ToString("000"));
                        break;
                    case 18: // "yy"
                        sb.Append((year % 100).ToString("00"));
                        break;
                    case 19: // "MMMM"
                        sb.Append(dtf.GetMonthName(month));
                        break;
                    case 20: // "MMM"
                        sb.Append(dtf.GetAbbreviatedMonthName(month));
                        break;
                    case 21: // "MM"
                        sb.Append(month.ToString("00"));
                        break;
                    case 22: // "y"
                        sb.Append(year % 100);
                        break;
                    case 23: // "M"
                        sb.Append(month);
                        break;
                    case 24: // "d"
                        sb.Append(day);
                        break;
                    case 25: // "fffffff"
                        sb.Append(decimalPart.Substring(0, 7));
                        break;
                    case 26: // "ffffff"
                        sb.Append(decimalPart.Substring(0, 6));
                        break;
                    case 27: // "fffff"
                        sb.Append(decimalPart.Substring(0, 5));
                        break;
                    case 28: // "ffff"
                        sb.Append(decimalPart.Substring(0, 4));
                        break;
                    case 29: // "fff"
                        sb.Append(decimalPart.Substring(0, 3));
                        break;
                    case 30: // "ff"
                        sb.Append(decimalPart.Substring(0, 2));
                        break;
                    case 31: // "f"
                        sb.Append(decimalPart[0]);
                        break;
                    case 32: // "tt"                        
                        sb.Append(hour < 12 ? dtf.AMDesignator : dtf.PMDesignator);
                        break;
                    case 33: // "t"
                        sb.Append(hour < 12 ? dtf.AMDesignator[0] : dtf.PMDesignator[0]);
                        break;
                    case 34: // "gg"
                    case 35: // "g"
                        if (ac != null)
                            s = ospi[i].Index == 34 ? "有奈" : "Ar";
                        else if (ospi[i].Index == 34)
                            s = dtf.GetEraName(0);
                        else
                            s = dtf.GetAbbreviatedEraName(0);
                        if (s == "AD")
                            s = "CE";
                            sb.Append(s);
                        break;
                    case 36: // "K"
                    case 37: // "zzz"
                        if (ospi[i].Index == 36 && TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
                            sb.Append("Z");                    
                        else if(TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString("\\+hh\\:mm"));
                        else
                            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString($"\\-hh\\:mm"));
                        break;
                    case 38: // "zz"
                        if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString("\\+00"));
                        else
                            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString("\\-00"));
                        break;
                    case 39: // "z"                        
                        if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                            sb.Append($"+{TimeZoneInfo.Local.BaseUtcOffset.Hours}");
                        else
                            sb.Append($"-{TimeZoneInfo.Local.BaseUtcOffset.Hours}");
                        break;                    
                    case 40: // "FFFFFFF"
                        sb.Append(decimalPart.Substring(0, 7).TrimEnd('0'));
                        break;
                    case 41: // "FFFFFF"
                        sb.Append(decimalPart.Substring(0, 6).TrimEnd('0'));
                        break;
                    case 42: // "FFFFF"
                        sb.Append(decimalPart.Substring(0, 5).TrimEnd('0'));
                        break;
                    case 43: // "FFFF"
                        sb.Append(decimalPart.Substring(0, 4).TrimEnd('0'));
                        break;
                    case 44: // "FFF"
                        sb.Append(decimalPart.Substring(0, 3).TrimEnd('0'));
                        break;
                    case 45: // "FF"
                        sb.Append(decimalPart.Substring(0, 2).TrimEnd('0'));
                        break;
                    case 46: // "F"
                        sb.Append(decimalPart.Substring(0, 1).TrimEnd('0'));
                        break;                    
                    default:
                        throw new NotImplementedException();
                }
            }
            return sb.ToString();
        }

        //未指定長度之相黏數字將會無法判斷，使用上須避免
        public static ArDateTime ParseExactArDateTimeFull(string s, string format, IFormatProvider provider, DateTimeStyles dateTimeStyles)
        {
            
            DateTimeFormatInfo dtf = null;
            if (provider is CultureInfo ci)
                dtf = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo di)
                dtf = di;
            else
                dtf = Mylar.ArinaCultureInfo.DateTimeFormat;
            if (format.Length == 1)
                format = dtf.GetAllDateTimePatterns(format[0])[0];

            ArStringPartInfo[] reservedString = CreateReversedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();
            ArOutStringPartInfo[] ospi = ds.Disassemble(format, reservedString);
            
            //ds.Disassemble()
            return ArDateTime.Now;
            //int dow = ArDateTime.GetDayOfWeek(year, month, day);
            //if (dow == 7)
            //    dow = 0;
            //ArCultureInfo ac = provider as ArCultureInfo;
            //if (ac != null)
            //    year = ArDateTime.GetARYear(year);
            for (int i = 0; i < ospi.Length; i++)
            {
                if (ospi[i].Index == 1)
                    ospi[i].Index = Array.FindIndex(reservedString, m => m.Value == ospi[i].Value);

                //switch (ospi[i].Index)
                //{
                //    case -1:
                //    case 0:
                //        sb.Append(ospi[i].Value);
                //        break;
                //    case 1: // %
                //        throw new FormatException("%");
                //    case 2: // :
                //        sb.Append(dtf.TimeSeparator);
                //        break;
                //    case 3: // /
                //        sb.Append(dtf.DateSeparator);
                //        break;
                //    case 4: // "hh"
                //        sb.Append((hour % 12).ToString("00"));
                //        break;
                //    case 5: // "HH"
                //        sb.Append(hour.ToString("00"));
                //        break;
                //    case 6: // "mm"
                //        sb.Append(minute.ToString("00"));
                //        break;
                //    case 7: // "ss"
                //        sb.Append(second.ToString("00"));
                //        break;
                //    case 8: // "h"
                //        sb.Append(hour % 12);
                //        break;
                //    case 9: // "H"
                //        sb.Append(hour);
                //        break;
                //    case 10: // "m"
                //        sb.Append(minute);
                //        break;
                //    case 11: // "s"
                //        sb.Append(second);
                //        break;
                //    case 12: // "dddd"                        
                //        sb.Append(dtf.GetDayName((DayOfWeek)dow));
                //        break;
                //    case 13: // "ddd"
                //        sb.Append(dtf.GetAbbreviatedDayName((DayOfWeek)dow));
                //        break;
                //    case 14: // "dd"
                //        sb.Append(day.ToString("00"));
                //        break;
                //    case 15: // "yyyyy"
                //        sb.Append(year.ToString("00000"));
                //        break;
                //    case 16: // "yyyy"
                //        sb.Append(year.ToString("0000"));
                //        break;
                //    case 17: // "yyy"
                //        sb.Append(year.ToString("000"));
                //        break;
                //    case 18: // "yy"
                //        sb.Append((year % 100).ToString("00"));
                //        break;
                //    case 19: // "MMMM"
                //        sb.Append(dtf.GetMonthName(month));
                //        break;
                //    case 20: // "MMM"
                //        sb.Append(dtf.GetAbbreviatedMonthName(month));
                //        break;
                //    case 21: // "MM"
                //        sb.Append(month.ToString("00"));
                //        break;
                //    case 22: // "y"
                //        sb.Append(year % 100);
                //        break;
                //    case 23: // "M"
                //        sb.Append(month);
                //        break;
                //    case 24: // "d"
                //        sb.Append(day);
                //        break;
                //    case 25: // "fffffff"
                //        sb.Append(decimalPart.Substring(0, 7));
                //        break;
                //    case 26: // "ffffff"
                //        sb.Append(decimalPart.Substring(0, 6));
                //        break;
                //    case 27: // "fffff"
                //        sb.Append(decimalPart.Substring(0, 5));
                //        break;
                //    case 28: // "ffff"
                //        sb.Append(decimalPart.Substring(0, 4));
                //        break;
                //    case 29: // "fff"
                //        sb.Append(decimalPart.Substring(0, 3));
                //        break;
                //    case 30: // "ff"
                //        sb.Append(decimalPart.Substring(0, 2));
                //        break;
                //    case 31: // "f"
                //        sb.Append(decimalPart[0]);
                //        break;
                //    case 32: // "tt"                        
                //        sb.Append(hour < 12 ? dtf.AMDesignator : dtf.PMDesignator);
                //        break;
                //    case 33: // "t"
                //        sb.Append(hour < 12 ? dtf.AMDesignator[0] : dtf.PMDesignator[0]);
                //        break;
                //    case 34: // "gg"
                //    case 35: // "g"
                //        if (ac != null)
                //            s = ospi[i].Index == 34 ? "有奈" : "Ar";
                //        else if (ospi[i].Index == 34)
                //            s = dtf.GetEraName(0);
                //        else
                //            s = dtf.GetAbbreviatedEraName(0);
                //        if (s == "AD")
                //            s = "CE";
                //        sb.Append(s);
                //        break;
                //    case 36: // "K"
                //    case 37: // "zzz"
                //        if (ospi[i].Index == 36 && TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
                //            sb.Append("Z");
                //        else if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                //            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString("\\+hh\\:mm"));
                //        else
                //            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString($"\\-hh\\:mm"));
                //        break;
                //    case 38: // "zz"
                //        if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                //            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString("\\+00"));
                //        else
                //            sb.Append(TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString("\\-00"));
                //        break;
                //    case 39: // "z"                        
                //        if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                //            sb.Append($"+{TimeZoneInfo.Local.BaseUtcOffset.Hours}");
                //        else
                //            sb.Append($"-{TimeZoneInfo.Local.BaseUtcOffset.Hours}");
                //        break;
                //    case 40: // "FFFFFFF"
                //        sb.Append(decimalPart.Substring(0, 7).TrimEnd('0'));
                //        break;
                //    case 41: // "FFFFFF"
                //        sb.Append(decimalPart.Substring(0, 6).TrimEnd('0'));
                //        break;
                //    case 42: // "FFFFF"
                //        sb.Append(decimalPart.Substring(0, 5).TrimEnd('0'));
                //        break;
                //    case 43: // "FFFF"
                //        sb.Append(decimalPart.Substring(0, 4).TrimEnd('0'));
                //        break;
                //    case 44: // "FFF"
                //        sb.Append(decimalPart.Substring(0, 3).TrimEnd('0'));
                //        break;
                //    case 45: // "FF"
                //        sb.Append(decimalPart.Substring(0, 2).TrimEnd('0'));
                //        break;
                //    case 46: // "F"
                //        sb.Append(decimalPart.Substring(0, 1).TrimEnd('0'));
                //        break;
                //    default:
                //        throw new NotImplementedException();
                //}
            }


            //DateTime.Parse()
            //DateTime.ParseExact()
            //string longDate, shortDate, longTime, shortTime, m, y, o, s, u
            //return null;
        }

        internal static string FormatStandardDateTime(ArDateTime adt, ArDateTimeType type = ArDateTimeType.DateTime, int decimalDigit = 7, IFormatProvider formatProvider = null)
        {
            string era = "CE";
            if (decimalDigit < 0 || decimalDigit > 7)
                throw new ArgumentOutOfRangeException(nameof(decimalDigit));
            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (formatProvider == null || formatProvider is ArCultureInfo)
            {
                year = ArDateTime.GetARYear(year);
                era = "AR";
            }
            else if (formatProvider is CultureInfo ci)
            {
                era = ci.DateTimeFormat.GetEraName(0);
            }

            if (type == ArDateTimeType.Date)
                return $"{era} {year:0000}/{month:00}/{day:00}";
            else if (type == ArDateTimeType.LongDate)
                return $"{era} {year:0000}/{month:00}/{day:00} [{new ArDateTime(adt._data).DayOfWeek}]";
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            ArDateTime.TimeTicksToTime(timeTicks, out int hour, out int minute, out int second, out int millisecond, out int tick);
            string decimalPart = decimalDigit == 0 || type == ArDateTimeType.ShortTime ? "" : (millisecond * 10000 + tick).ToString().PadLeft(7, '0').Substring(0, decimalDigit).TrimEnd('0');

            if (type == ArDateTimeType.DateTime)
                if (decimalPart == "")
                    return $"{era} {year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}";
                else
                    return $"{era} {year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}.{decimalPart}";
            else if (type == ArDateTimeType.System)
                if (decimalPart == "")
                    return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}";
                else
                    return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}.{decimalPart}";
            else if (type == ArDateTimeType.Time || type == ArDateTimeType.ShortTime)
                if (decimalPart == "")
                    return $"{hour:00}:{minute:00}:{second:00}";
                else
                    return $"{hour:00}:{minute:00}:{second:00}.{decimalPart}";
            else
                throw new NotImplementedException();
        }

        internal static ArDateTime ParseExactStandardDateTime(string s, ArDateTimeType type = ArDateTimeType.DateTime)
        {
            int year = 1, month = 1, day = 1, hour = 0, minute = 0, second = 0, decimalSecond = 0;
            s = s.Trim();
            string era = "";
            string[] datePart = null, timePart = null;
            string[] sArray = s.Split(' ');

            if (type == ArDateTimeType.DateTime)
            {
                era = sArray[0];
                datePart = sArray[1].Split('/');
                timePart = sArray[2].Split(':');
            }
            else if (type == ArDateTimeType.System)
            {
                datePart = sArray[0].Split('/');
                timePart = sArray[1].Split(':');
            }
            else if (type == ArDateTimeType.Date || type == ArDateTimeType.LongDate)
            {
                era = sArray[0];
                datePart = sArray[1].Split('/');
            }
            else if (type == ArDateTimeType.Time || type == ArDateTimeType.ShortTime)
            {
                timePart = s.Split(':');
            }
            else
            { throw new FormatException(); }

            if (datePart != null)
            {
                year = int.Parse(datePart[0]);
                month = int.Parse(datePart[1]);
                if (type == ArDateTimeType.LongDate)
                    day = int.Parse(datePart[2].Split(' ')[0]);
                else
                    day = int.Parse(datePart[2]);
            }

            if (timePart != null)
            {
                string[] secondPart = timePart[2].Split('.');
                hour = int.Parse(timePart[0]);
                minute = int.Parse(timePart[1]);
                second = int.Parse(secondPart[0]);
                if (secondPart.Length != 1)
                    decimalSecond = int.Parse(secondPart[1].PadRight(7, '0'));
            }

            if (era == "" || era == "AR")
                return new ArDateTime(year, month, day, hour, minute, second, 0, true).AddTicks(decimalSecond);
            return new ArDateTime(year, month, day, hour, minute, second).AddTicks(decimalSecond);
        }

        //Format
        internal static string FormatArDateTime(string format, ArDateTime adt)
        {
            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            ArDateTime.TimeTicksToTime(timeTicks, out int hour, out int minute, out int second, out int millisecond, out _);
            switch (format)
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
                    return Format(adt, format , CultureInfo.CurrentCulture);
            }
        }

        internal static ArDateTime ParseArDateTime(string s, DateTimeStyles dateTimeStyles)
        {
            ArDateTime result;
            bool arYear = s.IndexOf("Ar") != -1;
            bool arDayOfWeek = s.IndexOf('[') != -1;
            int common = s.Count(v => v == ',');
            int colon = s.Count(v => v == ':');
            int dot = s.Count(v => v == '.');

            //全滿：M, d, Ar. y [ddd] H:m:s.fff            
            if (arDayOfWeek && colon == 2)
            {
                if (TryParseExactArDateTime(s, "F", dateTimeStyles, out result))
                    return result;
                else
                    throw new FormatException();
            }

            if (arDayOfWeek)
            {
                if (TryParseExactArDateTime(s, "D", dateTimeStyles, out result))
                    return result;
                else
                    throw new FormatException();
            }

            if (colon == 0)
            {
                if (common == 2)
                {
                    if (TryParseExactArDateTime(s, "d", dateTimeStyles, out result))
                        return result;
                    else
                        throw new FormatException();
                }
                else if (arYear)
                {
                    if (TryParseExactArDateTime(s, "Y", dateTimeStyles, out result))
                        return result;
                    else
                        throw new FormatException();
                }
                else
                {
                    if (TryParseExactArDateTime(s, "M", dateTimeStyles, out result))
                        return result;
                    else
                        throw new FormatException();
                }
            }

            if (common == 2 && colon == 2) //等同G
            {
                if (TryParseExactArDateTime(s, "f", dateTimeStyles, out result))
                    return result;
                else
                    throw new FormatException();
            }
            else if (common == 2 && colon == 1)
            {
                if (TryParseExactArDateTime(s, "g", dateTimeStyles, out result))
                    return result;
                else
                    throw new FormatException();
            }

            if (common == 0)
            {
                if (dot == 1)
                {
                    if (TryParseExactArDateTime(s, "T", dateTimeStyles, out result))
                        return result;
                    else
                        throw new FormatException();
                }
                else
                {
                    if (TryParseExactArDateTime(s, "t", dateTimeStyles, out result))
                        return result;
                    else
                        throw new FormatException();
                }
            }
            throw new FormatException();
        }

        //Format
        internal static ArDateTime ParseExactArDateTime(string s, string format, DateTimeStyles dateTimeStyles)
        {
            int i = 0, j;
            int year = 1, month = 1, day = 1, hour = 0, minute = 0, second = 0, millisecond = 0, dayOfWeek;
            

            //A表示普通系統時間
            if (format == "A")
                return ParseExact(s, format, null, dateTimeStyles);
            //沒有支援，改用普通ParseExact
            if (format.Length != 1 || !SupportFormatChar.Any(m => m == format[0]))
                return ParseExact(s, format, CultureInfo.GetCultureInfo(ArinaBaseCultureName), dateTimeStyles);
            if ((dateTimeStyles & DateTimeStyles.AllowLeadingWhite) != 0)
                s = s.TrimStart();
            if ((dateTimeStyles & DateTimeStyles.AllowTrailingWhite) != 0)
                s = s.TrimEnd();

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
                if (j == -1)
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
                if (j == -1)
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
                i = s.IndexOf('[', i) + 1;
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
                if (j == -1)
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
            if (format == "T") //f
                millisecond = int.Parse(s.Substring(i).PadLeft(3, '0'));

            return new ArDateTime(year, month, day, hour, minute, second, millisecond, true);

        }

        //public static string Format(ArDateTime adt, string format, IFormatProvider formatProvider)
        //    => FormatDateTimeFull(adt, format, formatProvider);

        public static string Format(ArDateTime adt, string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";
            if (formatProvider == null)
            {
                if (format == "G")
                    return FormatStandardDateTime(adt);
                else if (format == "d")
                    return FormatStandardDateTime(adt, ArDateTimeType.Date);
                else if (format == "T")
                    return FormatStandardDateTime(adt, ArDateTimeType.Time);
                else if (format == "D")
                    return FormatStandardDateTime(adt, ArDateTimeType.LongDate);
                else if (format == "t")
                    return FormatStandardDateTime(adt, ArDateTimeType.ShortTime);
            }

            if (formatProvider is ArCultureInfo)
                return FormatArDateTime(format, adt);
            if (adt._data >= 0)
                return new DateTime(adt._data).ToString(format, formatProvider);

            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks != 0)
                timeTicks += 864000000000L;
            DateTime dt = new DateTime(year * -1, month, day).AddTicks(timeTicks);
            return $"(-){dt.ToString(format, formatProvider)}";

            //if(formatProvider is CultureInfo ci)
            //    return $"(-){dt.ToString(format, RyabrarFactory.CreateOrGet<CultureSupportNegativeYear>(
            //    new ArProductInfo(typeof(CultureSupportNegativeYear), ci.Name)))}";
            //else
            //{
            //    CultureInfo ct = RyabrarFactory.CreateOrGet<CultureSupportNegativeYear>(
            //    new ArProductInfo(typeof(CultureSupportNegativeYear), CultureInfo.CurrentCulture.Name));
            //    return $"(-){dt.ToString(format, ct)}";
            //}

        }

        public static ArDateTime Parse(string s, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException(nameof(s));
            else if (s.Length < 4)
                throw new ArgumentException(nameof(s));

            if (formatProvider == null)
            {
                if (s.IndexOf('[') != -1)
                {
                    if (TryParseExact(s, "D", formatProvider, dateTimeStyles, out ArDateTime r))
                        return r;
                }
                else if (char.IsLetter(s.TrimStart()[0]) && (dateTimeStyles & DateTimeStyles.AllowLeadingWhite) != 0)
                {
                    if (TryParseExact(s, "G", formatProvider, dateTimeStyles, out ArDateTime r))
                        return r;
                    if (s.IndexOf('/') != -1)
                    {
                        if (TryParseExact(s, "d", formatProvider, dateTimeStyles, out ArDateTime r2))
                            return r2;
                    }
                }
                else if (s.TrimStart().Substring(3).IndexOf(' ') != -1 && (dateTimeStyles & DateTimeStyles.AllowLeadingWhite) != 0)
                {
                    if (TryParseExact(s, "A", formatProvider, dateTimeStyles, out ArDateTime r))
                        return r;
                }
                else if (s.IndexOf(':') != -1)
                {
                    if (TryParseExact(s, "T", formatProvider, dateTimeStyles, out ArDateTime r))
                        return r;
                }
            }

            if (formatProvider == null || formatProvider is ArCultureInfo)
            {
                try { return ParseArDateTime(s, dateTimeStyles); }
                catch { }
                formatProvider = CultureInfo.CurrentCulture;
            }

            if (s.StartsWith("(-)"))
            {
                s = s.Substring(3);
                DateTime dt = DateTime.Parse(s, formatProvider, dateTimeStyles);
                return new ArDateTime(dt.Year * -1, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            }
            return new ArDateTime(DateTime.Parse(s, formatProvider, dateTimeStyles).Ticks);
        }

        public static ArDateTime ParseExact(string s, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

            if (formatProvider == null)
            {
                if (format == "G")
                    return ParseExactStandardDateTime(s);
                else if (format == "d")
                    return ParseExactStandardDateTime(s, ArDateTimeType.Date);
                else if (format == "T")
                    return ParseExactStandardDateTime(s, ArDateTimeType.Time);
                else if (format == "D")
                    return ParseExactStandardDateTime(s, ArDateTimeType.LongDate);
                else if (format == "t")
                    return ParseExactStandardDateTime(s, ArDateTimeType.ShortTime);
                else if (format == "A")
                    return ParseExactStandardDateTime(s, ArDateTimeType.System);
                else
                    throw new FormatException();
            }

            if (formatProvider is ArCultureInfo)
                return ParseExactArDateTime(s, format, dateTimeStyles);

            if (s.StartsWith("(-)"))
            {
                s = s.Substring(3);
                DateTime dt = DateTime.ParseExact(s, format, formatProvider, dateTimeStyles);
                return new ArDateTime(dt.Year * -1, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
            }
            return new ArDateTime(DateTime.ParseExact(s, format, formatProvider, dateTimeStyles).Ticks);
        }

        public static bool TryParseExact(string s, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out ArDateTime result)
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

        internal static bool TryParseExactArDateTime(string s, string format, DateTimeStyles dateTimeStyles, out ArDateTime result)
        {
            try
            {
                result = ParseExactArDateTime(s, format, dateTimeStyles);
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

