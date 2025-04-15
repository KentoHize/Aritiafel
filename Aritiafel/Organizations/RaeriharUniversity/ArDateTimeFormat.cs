using Aritiafel.Artifacts;
using Aritiafel.Characters.Heroes;
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

        static internal ArDisassembleInfo CreateFormatDisassembleInfo(ArStringPartInfo[] dateTimeReservedString = null)
            => new ArDisassembleInfo([dateTimeReservedString ?? CreateDateTimeReservedStringPartInfo(), CreateTextReservedStringPartInfo()],
                [new ArContainerPartInfo("", "'", "'", 1), new ArContainerPartInfo("", "\"", "\"", 1)]);

        static internal ArStringPartInfo[] CreateTextReservedStringPartInfo()        
            => [new ArStringPartInfo("bs", "\\\\", ArStringPartType.Escape1)];        

        static internal ArStringPartInfo[] CreateDateTimeReservedStringPartInfo()
        {
            List<ArStringPartInfo> result = DisassembleShop.StringToPartInfoList(SortedAllCustomFormatString);
            result.Insert(0, new ArStringPartInfo("p", "%", ArStringPartType.Escape1));
            result.Insert(0, new ArStringPartInfo("bs", "\\\\", ArStringPartType.Escape1));
            return result.ToArray();
        }

        static internal ArDisassembleInfo CreateScanDisassembleInfo()
        {
            ArDisassembleInfo di = new ArDisassembleInfo();
            di.ReservedStringInfo = [CreateScanStringPartInfo(null, null)];
            //di.ContainerPartInfo = 
            return di;
        }

        static internal ArStringPartInfo[] CreateScanStringPartInfo(ArOutPartInfoList patternInfo, DateTimeFormatInfo dtfi)
        {
            bool abbreviatedDayNameAdded = false;
            bool dayNameAdded = false;
            bool monthNameAdded = false;
            bool monthGenitiveNamesAdded = false;
            bool shortestDayNamesAdded = false;
            bool eraNameAdded = false;
            bool abbreviatedEraNamesAdded = false;
            bool designatorAdded = false;
            bool abbreviatedDesignatorAdded = false;
            bool K = false;
            bool zzz = false;
            List<ArStringPartInfo> result = new List<ArStringPartInfo>();
            for (int i = 0; i < patternInfo.Value.Count; i++)
            {
                int maxLength = 0;

                ArStringPartType type = ArStringPartType.Normal;
                patternInfo.Value[i] = ;
                //object value = patternInfo.Value[i];
                switch (patternInfo[i].Index)
                {
                    case 2:
                        value = dtfi.TimeSeparator;
                        break;
                    case 3:
                        value = dtfi.DateSeparator;
                        break;
                    case 12:
                        type = ArStringPartType.Escape1;
                        dayNameAdded = true;
                        break;
                    case 13:
                        type = ArStringPartType.Escape1;
                        abbreviatedDayNameAdded = true;
                        break;
                    case 19:
                        type = ArStringPartType.Escape1;
                        monthNameAdded = true;
                        break;
                    case 20:
                        type = ArStringPartType.Escape1;
                        monthGenitiveNamesAdded = true;
                        break;
                    case 32:
                        type = ArStringPartType.Escape1;
                        designatorAdded = true;
                        break;
                    case 33:
                        type = ArStringPartType.Escape1;
                        abbreviatedDesignatorAdded = true;
                        maxLength = 1;
                        break;
                    case 34:
                        type = ArStringPartType.Escape1;
                        eraNameAdded = true;
                        break;
                    case 35:
                        type = ArStringPartType.Escape1;
                        abbreviatedEraNamesAdded = true;
                        break;
                    case 36: //K
                        type = ArStringPartType.Escape1;
                        K = true;
                        break;
                    case 37: //zzz
                        type = ArStringPartType.Escape1;
                        zzz = true;
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 14:
                    case 18:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 30:
                    case 45:
                        type = ArStringPartType.Integer;
                        maxLength = 2;
                        break;
                    case 31:
                    case 46:
                        type = ArStringPartType.Integer;
                        maxLength = 1;
                        break;
                    case 17:
                    case 29:
                    case 38:
                    case 39:
                    case 44:
                        type = ArStringPartType.Integer;
                        maxLength = 3;
                        break;
                    case 16:
                    case 28:
                    case 43:
                        type = ArStringPartType.Integer;
                        maxLength = 4;
                        break;
                    case 15:
                    case 42:
                        type = ArStringPartType.Integer;
                        maxLength = 5;
                        break;
                    case 26:
                    case 41:
                        type = ArStringPartType.Integer;
                        maxLength = 6;
                        break;
                    case 25:
                    case 40:
                        type = ArStringPartType.Integer;
                        maxLength = 7;
                        break;
                }
                if (type == ArStringPartType.Normal)
                    result.Add(new ArStringPartInfo(patternInfo[i].Name, value, type, maxLength, 1));
                else if (type == ArStringPartType.Integer)
                    result.Add(new ArStringPartInfo(patternInfo[i].Name, "", type, maxLength, 1));
                else

                {
                    if (K || zzz)
                    {
                        if (K)
                            result.Add(new ArStringPartInfo("K", "Z", ArStringPartType.Normal, 0, 1));
                        result.Add(new ArStringPartInfo("zzz1", "", ArStringPartType.Integer, 2, 1));
                        result.Add(new ArStringPartInfo(":", ":", ArStringPartType.Normal, 0, 1));
                        result.Add(new ArStringPartInfo("zzz2", "", ArStringPartType.Integer, 2, 1));
                        K = zzz = false;
                    }
                }
            }

            if (dayNameAdded)
                for (int j = 0; j < 7; j++)
                    result.Add(new ArStringPartInfo($"wl", dtfi.GetDayName((DayOfWeek)j)));
            if (abbreviatedDayNameAdded)
                for (int j = 0; j < 7; j++)
                    result.Add(new ArStringPartInfo($"wa", dtfi.GetAbbreviatedDayName((DayOfWeek)j)));
            if (monthNameAdded)
                for (int j = 0; j < dtfi.MonthNames.Length; j++)
                    result.Add(new ArStringPartInfo($"mn", dtfi.MonthNames[j]));
            if (monthGenitiveNamesAdded)
                for (int j = 0; j < dtfi.MonthGenitiveNames.Length; j++)
                    result.Add(new ArStringPartInfo($"mg", dtfi.MonthGenitiveNames[j]));
            if (designatorAdded)
            {
                result.Add(new ArStringPartInfo($"dn", dtfi.AMDesignator));
                result.Add(new ArStringPartInfo($"dn", dtfi.PMDesignator));
            }
            if (abbreviatedDesignatorAdded)
            {
                result.Add(new ArStringPartInfo($"dn1", dtfi.AMDesignator[0].ToString()));
                result.Add(new ArStringPartInfo($"dn1", dtfi.PMDesignator[0].ToString()));
            }
            if (eraNameAdded)
            {
                for (int i = 0; i < dtfi.Calendar.Eras.Length; i++)
                    result.Add(new ArStringPartInfo($"er", dtfi.GetEraName(dtfi.Calendar.Eras[i])));
            }
            if (abbreviatedEraNamesAdded)
            {
                for (int i = 0; i < dtfi.Calendar.Eras.Length; i++)
                    result.Add(new ArStringPartInfo($"ea", dtfi.GetAbbreviatedEraName(dtfi.Calendar.Eras[i])));
            }

            return result.ToArray();
        }

        public static string Format(ArDateTime adt, string format, IFormatProvider formatProvider)
        {
            return ""; // To DO
        }

        public static ArDateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles dateTimeStyles)
            => ParseExactFull(s, format, provider, dateTimeStyles);

        public static ArDateTime ParseExactFull(string s, string format, IFormatProvider provider, DateTimeStyles dateTimeStyles)
        {   
            DateTimeFormatInfo dtfi = null;
            if (provider is CultureInfo ci)
                dtfi = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo di)
                dtfi = di;
            else if (provider is ICustomFormatter icf)
                return ArDateTime.Parse(icf.Format(format, s, provider));
            else
                dtfi = Mylar.ArinaCultureInfo.DateTimeFormat;
            if (format.Length == 1)
                format = dtfi.GetAllDateTimePatterns(format[0])[0];

            ArStringPartInfo[] reservedString = CreateDateTimeReservedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();
            ArOutPartInfoList ospi = ds.Disassemble(format, CreateFormatDisassembleInfo(reservedString));
            Sophia.SeeThrough(ospi);

            //To Do
            //ArStringPartInfo[] scanString = CreateScanStringPartInfo(ospi, dtfi);
            //ArOutStringPartInfo ospi2 = ds.Disassemble(s, scanString);

            int year = 1, month = 1, day = 1, hour = 0, minute = 0, second = 0, decimalPart = 0, dayOfWeek = 0;
            string era = "";

            //for (int i = 0; i < ospi2.Length; i++)
            //{
            //    switch (ospi2[i].Name)
            //    {
            //        case "yyyyy":
            //        case "yyyy":
            //        case "yyy":
            //            year = int.Parse(ospi2[i].Value);
            //            break;
            //        case "yy":
            //        case "y":
            //            year = int.Parse(ospi2[i].Value);
            //            break;
            //    }
            //}

            Console.WriteLine($"{s}, {format}");
            //Console.WriteLine($"{year}/{month}/{day} {hour}:{minute}:{second}.{decimalPart} [{dayOfWeek}] {era}");

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
            else if (provider is ICustomFormatter cf)
                return cf.Format(format, adt, provider);
            else
                dtf = Mylar.ArinaCultureInfo.DateTimeFormat;
            if (format.Length == 1)
                format = dtf.GetAllDateTimePatterns(format[0])[0];

            ArStringPartInfo[] reservedString = CreateDateTimeReservedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();
            ArOutPartInfoList opil = ds.Disassemble(format, CreateFormatDisassembleInfo(reservedString));

            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            ArDateTime.TimeTicksToTime(timeTicks, out int hour, out int minute, out int second, out int millisecond, out int tick);
            string decimalPart = (millisecond * 10000 + tick).ToString().PadLeft(7, '0');
            int dow = ArDateTime.GetDayOfWeek(year, month, day);
            dow = dow == 7 ? 0 : dow;            
            ArCultureInfo ac = provider as ArCultureInfo;
            if (ac != null)
                year = ArDateTime.GetARYear(year);
            return FormatDateTimeLoop(opil, reservedString, dtf, year, month, day, hour, minute, second, decimalPart, dow, ac != null);
        }
    }
}
