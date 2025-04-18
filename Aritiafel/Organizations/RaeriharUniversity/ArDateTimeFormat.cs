using Aritiafel.Artifacts;
using Aritiafel.Characters.Heroes;
using Aritiafel.Definitions;
using Aritiafel.Items;
using Aritiafel.Locations;
using Aritiafel.Organizations.ArinaOrganization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static System.Windows.Forms.DataFormats;

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


        public static string GetFirstDateTimePattern(char format, DateTimeFormatInfo dtfi = null)
            => GetAllDateTimePatterns(format, dtfi)[0];
        public static string[] GetAllDateTimePatterns(char format, DateTimeFormatInfo dtfi = null)
        {
            if (dtfi == null)
                dtfi = Mylar.ArinaCultureInfo.DateTimeFormat;
            return format switch
            {
                'A' => [Mylar.GetStandardDateTimePattern(dtfi)],
                'a' => [Mylar.GetStandardDateTimePattern(dtfi, ArStandardDateTimeType.ShortDateTime)],
                'B' => [Mylar.GetStandardDateTimePattern(dtfi, ArStandardDateTimeType.Date)],
                'b' => [Mylar.GetStandardDateTimePattern(dtfi, ArStandardDateTimeType.ShortDate)],
                'C' => [Mylar.GetStandardDateTimePattern(dtfi, ArStandardDateTimeType.Time)],
                'c' => [Mylar.GetStandardDateTimePattern(dtfi, ArStandardDateTimeType.ShortTime)],
                _ => dtfi.GetAllDateTimePatterns(format)
            };
        }

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

        static internal ArDisassembleInfo CreateScanDisassembleInfo(ArOutPartInfo patternInfo, DateTimeFormatInfo dtfi, bool allowWhiteSpace = false)
        {
            ArDisassembleInfo di = new ArDisassembleInfo();
            di.ReservedStringInfo = [CreateScanStringPartInfo(patternInfo, dtfi, allowWhiteSpace)];
            di.ContainerPartInfo = [];
            return di;
        }

        static internal ArPartInfo[] CreateScanStringPartInfo(ArOutPartInfo patternInfo, DateTimeFormatInfo dtfi, bool allowWhiteSpace = false)
        {
            List<ArPartInfo> result = CreateScanStringPartInfoLoop(patternInfo, dtfi);
            string[] sa;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] is ArGroupStringPartInfo gspi)
                {
                    switch (gspi.Name)
                    {
                        case "dddd":
                            gspi.Value = (string[])dtfi.DayNames.Clone();
                            break;
                        case "ddd":
                            gspi.Value = (string[])dtfi.AbbreviatedDayNames.Clone();
                            break;
                        case "MMMM":
                            sa = new string[12];
                            Array.Copy(dtfi.MonthNames, 0, sa, 0, 12);
                            gspi.Value = sa;
                            break;
                        case "MMM":
                            sa = new string[12];
                            Array.Copy(dtfi.AbbreviatedMonthNames, 0, sa, 0, 12);
                            gspi.Value = sa;
                            break;
                        case "tt":
                            gspi.Value = [dtfi.AMDesignator, dtfi.PMDesignator];
                            break;
                        case "t":
                            gspi.Value = [dtfi.AMDesignator[0].ToString(), dtfi.PMDesignator[0].ToString()];
                            break;
                        case "gg":
                            if (dtfi == Mylar.ArinaCultureInfo.DateTimeFormat) // ArCulture
                                gspi.Value = ["有奈"];
                            else
                            {
                                List<string> ls = [];
                                for (int j = 0; j < dtfi.Calendar.Eras.Length; j++)
                                    ls.Add(dtfi.GetEraName(dtfi.Calendar.Eras[j]));
                                gspi.Value = ls.ToArray();
                            }
                            break;
                        case "g":
                            if (dtfi == Mylar.ArinaCultureInfo.DateTimeFormat) // ArCulture
                                gspi.Value = ["Ar"];
                            else
                            {
                                List<string> ls = [];
                                for (int j = 0; j < dtfi.Calendar.Eras.Length; j++)
                                    ls.Add(dtfi.GetAbbreviatedEraName(dtfi.Calendar.Eras[j]));
                                gspi.Value = ls.ToArray();
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(gspi.Name);
                    }
                }
            }

            if (allowWhiteSpace)
                result.Insert(0, new ArStringPartInfo(" ", " "));
            return result.ToArray();
        }

        static internal List<ArPartInfo> CreateScanStringPartInfoLoop(ArOutPartInfo patternInfo, DateTimeFormatInfo dtfi)
        {
            bool K = false;
            bool zzz = false;
            List<ArPartInfo> result = new List<ArPartInfo>();
            if (patternInfo is ArOutPartInfoList opil)
            {
                for (int i = 0; i < opil.Value.Count; i++)
                    result.AddRange(CreateScanStringPartInfoLoop(opil.Value[i], dtfi));
            }
            else if (patternInfo is ArOutStringPartInfo ospi)
            {
                int maxLength = 0;
                ArStringPartType type = ArStringPartType.Normal;
                string value = "";
                switch (ospi.Index)
                {
                    case 2:
                        value = dtfi.TimeSeparator;
                        break;
                    case 3:
                        value = dtfi.DateSeparator;
                        break;
                    case 12:
                    case 13:
                    case 19:
                    case 20:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                        type = ArStringPartType.Special; //多選
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
                    case 16: //yyyy
                    case 28:
                    case 43:
                        type = ArStringPartType.Integer;
                        maxLength = 4;
                        break;
                    case 15: //yyyyy
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
                    default:
                        value = ospi.Value;
                        break;
                }
                if (type == ArStringPartType.Normal)
                    result.Add(new ArStringPartInfo(ospi.Name, value, type, maxLength, 1));
                else if (type == ArStringPartType.Integer)
                {
                    if (ospi.Index >= 15 && ospi.Index <= 19) //yyyy屬特例
                    {
                        result.Add(new ArStringPartInfo("-", "-", ArStringPartType.Normal, 0, 1));
                        result.Add(new ArStringPartInfo(ospi.Name, "", type, maxLength, 1));
                    }
                    else
                        result.Add(new ArStringPartInfo(ospi.Name, "", type, maxLength, 1));
                }
                else if (type == ArStringPartType.Special)
                    result.Add(new ArGroupStringPartInfo(ospi.Name, null, 1));
                else
                {
                    if (K || zzz)
                    {
                        if (K)
                            result.Add(new ArStringPartInfo("Z", "Z", ArStringPartType.Normal, 0, 1));
                        result.Add(new ArStringPartInfo("zzz1", "", ArStringPartType.Integer, 3, 1));
                        result.Add(new ArStringPartInfo(":", ":", ArStringPartType.Normal, 0, 1));
                        result.Add(new ArStringPartInfo("zzz2", "", ArStringPartType.Integer, 2, 1));
                        K = zzz = false;
                    }
                }
            }
            return result;
        }

        public static string Format(ArDateTime adt, string format, IFormatProvider formatProvider)
            => FormatDateTimeFull(adt, format, formatProvider);

        public static ArDateTime ParseExact(string s, string format, IFormatProvider provider, ArDateTimeStyles dateTimeStyles)
            => ParseExactFull(s, format, provider, dateTimeStyles);

        public static ArDateTime ParseExactFull(string s, string format, IFormatProvider provider, ArDateTimeStyles dateTimeStyles)
        {
            DateTimeFormatInfo dtfi = null;
            if (provider == null)
                provider = Mylar.ArinaCultureInfo;
            if (provider is CultureInfo ci)
                dtfi = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo di)
                dtfi = di;
            else if (provider is ICustomFormatter icf)
                return ArDateTime.Parse(icf.Format(format, s, provider));
            else
                dtfi = Mylar.ArinaCultureInfo.DateTimeFormat;
            if (string.IsNullOrEmpty(format))
                format = "G";
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowLeadingWhite))
                s = s.TrimStart();
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowTrailingWhite))
                s = s.TrimEnd();

            ArStringPartInfo[] reservedString = CreateDateTimeReservedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();

            if (format.Length == 1)
                format = GetFirstDateTimePattern(format[0], dtfi); //需要改成AllDateTimePattern

            ArOutPartInfoList ospi = ds.Disassemble(format, CreateFormatDisassembleInfo(reservedString));
            ds.ErrorOccurIfNoMatch = ds.RemoveLimitedReservedStringIfNoMatch = true;
            ospi = ds.Disassemble(s, CreateScanDisassembleInfo(ospi, dtfi, dateTimeStyles.HasFlag(ArDateTimeStyles.AllowInnerWhite)));

            int year = 1, month = 1, day = 1, hour = 0, minute = 0, second = 0, decimalPart = 0, tt = -1,
                zHour = 0, zMinute = 0;
            string era = "";
            bool negative = false, getYear = false;
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.CurrentDateDefault))
            {
                ArDateTime.TicksToDateTime(ArDateTime.Today.Ticks, out year, out month, out day, out _);
                if (provider == Mylar.ArinaCultureInfo)
                    year = ArDateTime.GetARYear(year);
            }

            for (int i = 0; i < ospi.Value.Count; i++)
            {
                switch (ospi.Value[i].Name)
                {
                    case "-":
                        negative = true;
                        break;
                    case "hh":
                    case "h":
                        hour = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "HH":
                    case "H":
                        hour = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "MM":
                    case "M":
                        month = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "mm":
                    case "m":
                        minute = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "ss":
                    case "s":
                        second = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "yyyyy":
                    case "yyyy":
                    case "yyy":
                        year = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value) * (negative ? -1 : 1);
                        getYear = true;
                        break;
                    case "yy":
                    case "y":
                        if (!getYear)
                            year = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value) * (negative ? -1 : 1);
                        break;
                    case "dd":
                    case "d":
                        day = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "fffffff":
                    case "ffffff":
                    case "fffff":
                    case "ffff":
                    case "fff":
                    case "ff":
                    case "f":
                    case "FFFFFFF":
                    case "FFFFFF":
                    case "FFFFF":
                    case "FFFF":
                    case "FFF":
                    case "FF":
                    case "F":
                        decimalPart = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value.PadRight(7, '0'));
                        break;
                    case "tt":
                    case "t":
                        tt = ((ArOutStringPartInfo)ospi.Value[i]).GroupIndex;
                        break;
                    case "g":
                    case "gg":
                        era = ((ArOutStringPartInfo)ospi.Value[i]).Value;
                        break;
                    case "zzz1":
                    case "zz":
                    case "z":
                        zHour = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "zzz2":
                        zMinute = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        break;
                    case "MMMM":
                    case "MMM":
                        month = ((ArOutStringPartInfo)ospi.Value[i]).GroupIndex + 1;
                        break;
                }
            }
            ArDateTime result = new ArDateTime(year, month, day, hour + (tt == 1 ? 12 : 0), minute, second, 0, provider == Mylar.ArinaCultureInfo).AddTicks(decimalPart);
            result = result.AddHours(zHour).AddMinutes(zMinute);
            return result;
        }

        public static bool TryParseExact(string s, string format, IFormatProvider formatProvider, ArDateTimeStyles dateTimeStyles, out ArDateTime result)
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

        public static ArDateTime Parse(string s, IFormatProvider provider, ArDateTimeStyles dateTimeStyles)
        {
            DateTimeFormatInfo dtf = null;
            if (dateTimeStyles.HasFlag(DateTimeStyles.AllowLeadingWhite))
                s = s.TrimStart();
            if (dateTimeStyles.HasFlag(DateTimeStyles.AllowTrailingWhite))
                s = s.TrimEnd();
            if (s.Trim().Length < 4)
                throw new FormatException(s);
            if (provider == null)
                provider = Mylar.ArinaCultureInfo;
            if (provider is CultureInfo ci)
                dtf = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo dtfi)
                dtf = dtfi;
            else if (provider is ICustomFormatter cf)
                return ArDateTime.ParseExact(s, null, provider, dateTimeStyles);
            else
                dtf = Mylar.ArinaCultureInfo.DateTimeFormat;
            
            DisassembleShop ds = new DisassembleShop();
            ArDisassembleInfo di = new ArDisassembleInfo([dtf.DateSeparator, dtf.TimeSeparator, ".", " ", "GMT", "Z", "T", "-"]);
            ArOutPartInfoList pi = ds.Disassemble(s, di);
            int dsr = 0, tsr = 0, dot = 0, sp = 0, gmt = 0, z = 0, t = 0, dash = 0;
            for (int i = 0; i < pi.Value.Count; i++)
            {
                switch (((ArOutStringPartInfo)pi.Value[i]).Index)
                {
                    case 0:
                        dsr++;
                        break;
                    case 1:
                        tsr++;
                        break;                    
                    case 2:
                        dot++;
                        break;
                    case 3:
                        sp++;
                        break;
                    case 4:
                        gmt++;
                        break;                    
                    case 5:
                        z++;
                        break;
                    case 6:
                        t++;
                        break;
                    case 7:
                        dash++;
                        break;
                }
            }
            ArDateTime result;

            if ((char.IsLetter(s[0]) && char.IsLetter(s[1])) ||
                ((char.IsLetter(s[0]) || char.IsWhiteSpace(s[0])) && char.IsLetter(s[1]) && char.IsLetter(s[2]))) //標準格式開頭
            {
                if (s.Length >= Mylar.StandardDateTimeLength)
                    if (TryParseExact(s, "A", provider, dateTimeStyles, out result))
                        return result;
                if (s.Length >= Mylar.StandardShortDateTimeLength)
                    if (TryParseExact(s, "a", provider, dateTimeStyles, out result))
                        return result;
                if (s.Length >= Mylar.StandardDateLength)
                    if (TryParseExact(s, "B", provider, dateTimeStyles, out result))
                        return result;
                if (s.Length >= Mylar.StandardShortDateLength)
                    if (TryParseExact(s, "b", provider, dateTimeStyles, out result))
                        return result;
            }
            
            if(sp != 0)
            {
                if (gmt > 0)
                {
                    if (TryParseExact(s, "R", provider, dateTimeStyles, out result))
                        return result;
                    if (TryParseExact(s, "r", provider, dateTimeStyles, out result))
                        return result;
                }

                if (z > 0)
                    if (TryParseExact(s, "u", provider, dateTimeStyles, out result))
                        return result;

                if (dsr > 0 && tsr > 0)
                {
                    if (tsr == 2)
                    {
                        if (TryParseExact(s, "G", provider, dateTimeStyles, out result))
                            return result;
                        if (TryParseExact(s, "F", provider, dateTimeStyles, out result))
                            return result;
                    }
                    else if(tsr == 1)
                    {
                        if (TryParseExact(s, "g", provider, dateTimeStyles, out result))
                            return result;
                        if (TryParseExact(s, "f", provider, dateTimeStyles, out result))
                            return result;
                    }   
                }
            }            
            if (sp == 0 || dateTimeStyles.HasFlag(ArDateTimeStyles.AllowInnerWhite))
            {
                if (t > 0)
                {
                    if (dash > 0)
                        if (TryParseExact(s, "s", provider, dateTimeStyles, out result))
                            return result;
                    if (TryParseExact(s, "O", provider, dateTimeStyles, out result))
                        return result;
                    if (TryParseExact(s, "o", provider, dateTimeStyles, out result))
                        return result;
                    throw new FormatException(s);
                }
                if (s.Length >= Mylar.StandardTimeLength && dot > 0)
                    if (TryParseExact(s, "C", provider, dateTimeStyles, out result))
                        return result;
                if (s.Length >= Mylar.StandardShorTimeLength)
                    if (TryParseExact(s, "c", provider, dateTimeStyles, out result))
                        return result;
                if (dsr == 2)
                    if (TryParseExact(s, "d", provider, dateTimeStyles, out result))
                        return result;
                if (tsr == 2)
                    if (TryParseExact(s, "T", provider, dateTimeStyles, out result))
                        return result;
                if (tsr == 1)
                    if (TryParseExact(s, "t", provider, dateTimeStyles, out result))
                        return result;
                if (s.Length > 10)
                    if (TryParseExact(s, "D", provider, dateTimeStyles, out result))
                        return result;
                if (TryParseExact(s, "M", provider, dateTimeStyles, out result))
                    return result;
                if (TryParseExact(s, "Y", provider, dateTimeStyles, out result))
                    return result;
                if (TryParseExact(s, "m", provider, dateTimeStyles, out result))
                    return result;
                if (TryParseExact(s, "y", provider, dateTimeStyles, out result))
                    return result;
            }            
            if (TryParseExact(s, "U", provider, dateTimeStyles, out result))
                return result;
            throw new FormatException(s);
        }

        public static string FormatDateTimeLoop(ArOutPartInfoList opil, ArStringPartInfo[] reservedString, DateTimeFormatInfo dtf, int year, int month, int day, int hour, int minute, int second, string decimalPart, int dow, bool isArDate, bool isText = false)
        {
            StringBuilder sb = new StringBuilder();
            string s;
            foreach (ArOutPartInfo opi in opil.Value)
            {
                if (isText)
                    sb.Append(((ArOutStringPartInfo)opi).Value);
                else if (opi is ArOutPartInfoList pl)
                    sb.Append(FormatDateTimeLoop(pl, reservedString, dtf, year, month, day, hour, minute, second, decimalPart, dow, isArDate, true));
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
                            sb.Append(year.ToString("00000;-0000"));
                            break;
                        case 16: // "yyyy"
                            sb.Append(year.ToString("0000;-000"));
                            break;
                        case 17: // "yyy"
                            sb.Append(year.ToString("000;-00"));
                            break;
                        case 18: // "yy"
                            sb.Append((year % 100).ToString("00;-0"));
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
                            //if (s == "AD")
                            //    s = "CE";
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
            if (provider == null)
                provider = Mylar.ArinaCultureInfo;
            if (provider is CultureInfo ci)
                dtf = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo di)
                dtf = di;
            else if (provider is ICustomFormatter cf)
                return cf.Format(format, adt, provider);
            else
                dtf = Mylar.ArinaCultureInfo.DateTimeFormat;
            if (string.IsNullOrEmpty(format))
                format = "G";
            if (format.Length == 1)
                format = GetFirstDateTimePattern(format[0], dtf);

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
            if (provider == Mylar.ArinaCultureInfo)
                year = ArDateTime.GetARYear(year);
            return FormatDateTimeLoop(opil, reservedString, dtf, year, month, day, hour, minute, second, decimalPart, dow, provider == Mylar.ArinaCultureInfo);
        }
    }
}
