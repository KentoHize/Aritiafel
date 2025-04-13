using Aritiafel.Definitions;
using Aritiafel.Items;
using Aritiafel.Locations;
using Aritiafel.Organizations.ArinaOrganization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public static class ArDateTimeFormat
    {
        internal static readonly string[] SortedAllCustomFormatString =
{
            ":", "/", "hh", "HH", "mm", "ss", "h", "H", "m", "s",
            "dddd", "ddd", "dd", "yyyyy", "yyyy", "yyy", "yy",
            "MMMM", "MMM", "MM", "y", "M", "d",
            "fffffff", "ffffff", "fffff", "ffff", "fff", "ff", "f",
            "tt", "t", "gg", "g", "K", "zzz", "zz", "z", "FFFFFFF", "FFFFFF",
            "FFFFF", "FFFF", "FFF", "FF", "F"
        };

        static internal ArStringPartInfo[] CreateTextReservedStringPartInfo()
        {
            List<ArStringPartInfo> result = new List<ArStringPartInfo>();
            result.Add(new ArStringPartInfo("bs", "\\\\", ArStringPartType.Escape1));
            return result.ToArray();
        }

        static internal ArStringPartInfo[] CreateDateTimeReservedStringPartInfo()
        {
            List<ArStringPartInfo> result = DisassembleShop.StringToPartInfoList(SortedAllCustomFormatString);
            result.Insert(0, new ArStringPartInfo("p", "%", ArStringPartType.Escape1));
            result.Insert(0, new ArStringPartInfo("bs", "\\\\", ArStringPartType.Escape1));
            return result.ToArray();
        }

        public static string Format(ArDateTime adt, string format, IFormatProvider formatProvider)
        {
            return "";
        }

        public static ArDateTime ParseExact(string s, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return ArDateTime.Now;
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

        public static ArDateTime Parse(string s, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return ArDateTime.Now;
        }

        public static string FormatDateTimeLoop(ArOutPartInfoList opil, ArStringPartInfo[] reservedString, DateTimeFormatInfo dtf, int year, int month, int day, int hour, int minute, int second, string decimalPart, int dow, bool isArDate, bool isText = false)
        {            
            StringBuilder sb = new StringBuilder();
            string s;
            foreach (ArOutPartInfo opi in opil.Value)
            {
                if(isText)
                {
                    sb.Append(((ArOutStringPartInfo)opi).Value);
                }
                else if (opi is ArOutPartInfoList pl)
                {
                    sb.Append(FormatDateTimeLoop(pl, reservedString, dtf, year, month, day, hour, minute, second, decimalPart, dow, isArDate, true));
                }
                else if (opi is ArOutStringPartInfo pi)
                {
                    if (pi.Index == 1)
                        pi.Index = Array.FindIndex(reservedString, m => m.Value == pi.Value);

                    switch (pi.Index)
                    {
                        case -1:
                        case 0:
                            sb.Append(pi.Value);
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
                            if (isArDate)
                                s = pi.Index == 34 ? "有奈" : "Ar";
                            else if (pi.Index == 34)
                                s = dtf.GetEraName(0);
                            else
                                s = dtf.GetAbbreviatedEraName(0);
                            if (s == "AD")
                                s = "CE";
                            sb.Append(s);
                            break;
                        case 36: // "K"
                        case 37: // "zzz"
                            if (pi.Index == 36 && TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
                                sb.Append("Z");
                            else if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                                sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString("\\+hh\\:mm"));
                            else
                                sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString("\\-hh\\:mm"));
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
            }
            return sb.ToString();
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

            ArStringPartInfo[] reservedString = CreateDateTimeReservedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();
            ArOutPartInfoList opil = ds.Disassemble(format, [reservedString, CreateTextReservedStringPartInfo()],
                [new ArContainerPartInfo("", "'", "'", 1), new ArContainerPartInfo("", "\"", "\"", 1)]);

            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            ArDateTime.TimeTicksToTime(timeTicks, out int hour, out int minute, out int second, out int millisecond, out int tick);
            string decimalPart = (millisecond * 10000 + tick).ToString().PadLeft(7, '0');
            int dow = ArDateTime.GetDayOfWeek(year, month, day);
            if (dow == 7)
                dow = 0;
            ArCultureInfo ac = provider as ArCultureInfo;
            if (ac != null)
                year = ArDateTime.GetARYear(year);
            return FormatDateTimeLoop(opil, reservedString, dtf, year, month, day, hour, minute, second, decimalPart, dow, ac != null);
        }
    }
}
