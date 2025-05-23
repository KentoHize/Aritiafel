﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Aritiafel.Definitions;
using Aritiafel.Items;
using Aritiafel.Locations;
using Aritiafel.Organizations.ArinaOrganization;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    internal static class ArDateTimeFormat
    {
        internal const int MinimumCharForLongFormat = 11; //1/1/1 1:1:1
        internal const int MinimumCharForLongDate = 7; //1/1/1-1

        //可調整
        internal static readonly string[] SortedAllCustomFormatString =
        {
            ":", "/", "hh", "HH", "mm", "ss", "h", "H", "m", "s",
            "dddd", "ddd", "dd", "yyyyyy", "yyyyy", "yyyy", "yyy", "yy",
            "MMMM", "MMM", "MM", "y", "M", "d",
            "fffffff", "ffffff", "fffff", "ffff", "fff", "ff", "f",
            "tt", "t", "ggg", "gg", "g", "K", "zzz", "zz", "z", "FFFFFFF", "FFFFFF",
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

        static internal ArDisassembleInfo CreateScanDisassembleInfo(ArOutPartInfo patternInfo, DateTimeFormatInfo dtfi, ArDateTimeStyles dateTimeStyles)
        {
            ArDisassembleInfo di = new ArDisassembleInfo();
            di.ReservedStringInfo = [CreateScanStringPartInfo(patternInfo, dtfi, dateTimeStyles)];
            di.ContainerPartInfo = [];
            return di;
        }

        static internal ArStringPartInfo[] CreateScanStringPartInfo(ArOutPartInfo patternInfo, DateTimeFormatInfo dtfi, ArDateTimeStyles dateTimeStyles)
        {
            List<ArStringPartInfo> result = CreateScanStringPartInfoLoop(patternInfo, dtfi);
            string[] sa;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Type == ArStringPartType.Special)
                {
                    result[i].Type = ArStringPartType.Normal;
                    switch (result[i].Name)
                    {
                        case "dddd":
                            result[i].Values = (string[])dtfi.DayNames.Clone();
                            break;
                        case "ddd":
                            result[i].Values = (string[])dtfi.AbbreviatedDayNames.Clone();
                            break;
                        case "MMMM":
                            sa = new string[12];
                            Array.Copy(dtfi.MonthNames, 0, sa, 0, 12);
                            result[i].Values = sa;
                            break;
                        case "MMM":
                            sa = new string[12];
                            Array.Copy(dtfi.AbbreviatedMonthNames, 0, sa, 0, 12);
                            result[i].Values = sa;
                            break;
                        case "tt":
                            result[i].Values = [dtfi.AMDesignator, dtfi.PMDesignator];
                            break;
                        case "t":
                            result[i].Values = [dtfi.AMDesignator[0].ToString(), dtfi.PMDesignator[0].ToString()];
                            break;
                        case "ggg":
                            result[i].Values = Mylar.GetAllStandardCalendarEraName();
                            if(dateTimeStyles.HasFlag(ArDateTimeStyles.AllowLeadingWhite))
                                for(int j = 0; j < result[i].Values.Length; j++)
                                    result[i].Values[j] = result[i].Values[j].TrimStart();
                            break;
                        case "gg":
                            if (dtfi == Mylar.ArinaCulture.DateTimeFormat) // ArCulture
                                result[i].Value = ArCultureInfo.EraName;
                            else
                            {
                                List<string> ls = [];
                                for (int j = 0; j < dtfi.Calendar.Eras.Length; j++)
                                    ls.Add(dtfi.GetEraName(dtfi.Calendar.Eras[j]));
                                result[i].Values = ls.ToArray();
                            }
                            break;
                        case "g":
                            if (dtfi == Mylar.ArinaCulture.DateTimeFormat) // ArCulture
                                result[i].Value = ArCultureInfo.AbbreviatedEraName;
                            else
                            {
                                List<string> ls = [];
                                for (int j = 0; j < dtfi.Calendar.Eras.Length; j++)
                                    ls.Add(dtfi.GetAbbreviatedEraName(dtfi.Calendar.Eras[j]));
                                result[i].Values = ls.ToArray();
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(result[i].Name);
                    }
                }
            }

            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowInnerWhite))
                result.Insert(0, new ArStringPartInfo(" ", " "));
            return result.ToArray();
        }

        static internal List<ArStringPartInfo> CreateScanStringPartInfoLoop(ArOutPartInfo patternInfo, DateTimeFormatInfo dtfi)
        {
            bool K = false;
            bool zzz = false;
            List<ArStringPartInfo> result = new List<ArStringPartInfo>();
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
                    case 20:
                    case 21:
                    case 33: //tt
                    case 34: //t
                    case 35: //ggg
                    case 36: //gg
                    case 37: //g
                        type = ArStringPartType.Special; //多選
                        break;
                    case 38: //K
                        type = ArStringPartType.Escape1;
                        K = true;
                        break;
                    case 39: //zzz
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
                    case 19: //yy
                    case 22:
                    case 23: //y                    
                    case 24: //m
                    case 25: //d
                    case 31: //ff
                    case 47:
                        type = ArStringPartType.Integer;
                        maxLength = 2;
                        break;
                    case 32: //f                    
                    case 48:
                        type = ArStringPartType.Integer;
                        maxLength = 1;
                        break;
                    case 15: //yyyyyy
                    case 16: //yyyyy
                    case 17: //yyyy
                    case 18: //yyy
                        type = ArStringPartType.Integer;
                        break;
                    case 30:
                    case 40: //zz
                    case 41: //z
                    case 46:
                        type = ArStringPartType.Integer;
                        maxLength = 3;
                        break;
                    case 29:
                    case 45:
                        type = ArStringPartType.Integer;
                        maxLength = 4;
                        break;
                    case 28:
                    case 44:
                        type = ArStringPartType.Integer;
                        maxLength = 5;
                        break;
                    case 27:
                    case 43:
                        type = ArStringPartType.Integer;
                        maxLength = 6;
                        break;
                    case 26: //fffffff
                    case 42: //FFFFFFF
                        type = ArStringPartType.Integer;
                        maxLength = 7;
                        break;
                    default:
                        value = ospi.Value;
                        break;
                }
                if (type == ArStringPartType.Normal)
                    result.Add(new ArStringPartInfo(ospi.Name, [value], type, maxLength, 1));
                else if (type == ArStringPartType.Special)
                    result.Add(new ArStringPartInfo(ospi.Name, [""], type, maxLength, 1));
                else if (type == ArStringPartType.Integer)
                    result.Add(new ArStringPartInfo(ospi.Name, "", type, maxLength, 1));
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
                provider = Mylar.ArinaCulture;
            if (provider is CultureInfo ci)
                dtfi = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo di)
                dtfi = di;
            else if (provider is ICustomFormatter icf)
                return ArDateTime.Parse(icf.Format(format, s, provider));
            else
                dtfi = Mylar.ArinaCulture.DateTimeFormat;
            if (string.IsNullOrEmpty(format))
                format = "G";
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowLeadingWhite))
                s = s.TrimStart();
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowTrailingWhite))
                s = s.TrimEnd();

            ArStringPartInfo[] reservedString = CreateDateTimeReservedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();
            ArDateTime result;
            if (format.Length == 1)
            {
                string[] allPatterns = ArCultureInfo.GetAllDateTimePatterns(format[0], dtfi);
                for (int i = 0; i < allPatterns.Length; i++)
                {
                    if (allPatterns[i].Length == 1)
                        throw new FormatException(allPatterns[i]);
                    if (TryParseExact(s, allPatterns[i], provider, dateTimeStyles, out result))
                        return result;
                }
                throw new FormatException();
            }

            ArOutPartInfoList ospi = ds.Disassemble(format, CreateFormatDisassembleInfo(reservedString));
            ds.ErrorOccurIfNoMatch = ds.WhenReservedStringNoMatchTimesIgnorePreviousReservedString = true;
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowInnerWhite) || ospi.Value.Find(m => m.Name == "K") != null)
                ds.ReservedStringMatchPolicy = StringMatchPolicy.IgnoreLimitedReservedStringIfNoMatch; //忽略模式，對K較為棘手
            else
                ds.ReservedStringMatchPolicy = StringMatchPolicy.SkipAllReservedStringIfFirstNoMatch; //嚴格模式
            ospi = ds.Disassemble(s, CreateScanDisassembleInfo(ospi, dtfi, dateTimeStyles));

            int year = 1, month = 1, day = 1, hour = 0, minute = 0, second = 0, decimalPart = 0, tt = -1,
                zHour = 0, zMinute = 0;
            string era = "";
            bool negative = false, getYear = false, useCEDate = false;
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.CurrentDateDefault))
            {
                ArDateTime.TicksToDateTime(ArDateTime.Today.Ticks, out year, out month, out day, out _);
                if (provider == Mylar.ArinaCulture)
                    year = ArDateTime.GetARYear(year);
            }

            ospi.Value.ParallelForEach((m, pls, i) =>
            {
                switch (ospi.Value[i].Name)
                {
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
                    case "yyyyyy":
                    case "yyyyy":
                    case "yyyy":
                    case "yyy":
                        year = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
                        getYear = true;
                        break;
                    case "yy":
                    case "y":
                        if (!getYear)
                            year = int.Parse(((ArOutStringPartInfo)ospi.Value[i]).Value);
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
                    case "ggg":
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
            });

            
            //if (era != "" && era.Length < 3)
            //    era = era.PadLeft(3, ' ');
            //AR -> era != "" 也!= "AR"
            //Other => != "Ar"
            //先寫死 => 待修改
            if (era.Trim() != Mylar.GetStandardCalendarEraName().Trim() && era != ArCultureInfo.EraName && era != ArCultureInfo.AbbreviatedEraName &&
                (dtfi != Mylar.ArinaCulture.DateTimeFormat || era != ""))
                useCEDate = true;

            result = new ArDateTime(year, month, day, hour + (tt == 1 ? 12 : 0), minute, second, 0, useCEDate).AddTicks(decimalPart);
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
            int indexOfDS = 0, indexOfTS = 1;
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowLeadingWhite))
                s = s.TrimStart();
            if (dateTimeStyles.HasFlag(ArDateTimeStyles.AllowTrailingWhite))
                s = s.TrimEnd();
            if (s.Length < 3)
                throw new FormatException(s);
            if (provider == null)
                provider = Mylar.ArinaCulture;
            if (provider is CultureInfo ci)
                dtf = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo dtfi)
                dtf = dtfi;
            else if (provider is ICustomFormatter cf)
                return ArDateTime.ParseExact(s, null, provider, dateTimeStyles);
            else
                dtf = Mylar.ArinaCulture.DateTimeFormat;

            DisassembleShop ds = new DisassembleShop();
            List<string> discernStringList = new List<string> { "/", ":", ".", " ", "GMT", "Z", "T", "-" };

            if (dtf.DateSeparator != "/")
            {
                indexOfDS = discernStringList.IndexOf(dtf.DateSeparator);
                if (indexOfDS == -1)
                {
                    discernStringList.Add(dtf.DateSeparator);
                    indexOfDS = discernStringList.Count - 1;
                }
            }

            if (dtf.TimeSeparator != ":")
            {
                indexOfTS = discernStringList.IndexOf(dtf.TimeSeparator);
                if (indexOfTS == -1)
                {
                    discernStringList.Add(dtf.TimeSeparator);
                    indexOfTS = discernStringList.Count - 1;
                }
            }

            ArDisassembleInfo di = new ArDisassembleInfo(discernStringList.ToArray());
            ArOutPartInfoList pi = ds.Disassemble(s, di);
            int dsr = 0, tsr = 0, dot = 0, sp = 0, gmt = 0, z = 0, t = 0, dash = 0, colon = 0, slash = 0;
            pi.Value.ParallelForEach(m =>
            {
                int i = ((ArOutStringPartInfo)m).Index;
                if (i == indexOfDS)
                    dsr++;
                if (i == indexOfTS)
                    tsr++;
                switch (i)
                {
                    case 0:
                        slash++;
                        break;
                    case 1:
                        colon++;
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
            });

            ArDateTime result;
            //偵測標準格式的ggg開頭
            if (s.Length >= Mylar.StandardDateLength - 1 && (s[9] == '/' || s[8] == '/'))
            {
                if (char.IsLetter(s[0]) && char.IsLetter(s[1]))
                {
                    if (s.Length == Mylar.StandardDateTimeLength - 1)
                    {
                        if (s[2] == ' ')
                            if (TryParseExact(s, "A", provider, dateTimeStyles, out result))
                                return result;
                        if (s[2] == '-' || char.IsDigit(s[2]))
                            if (TryParseExact(s, "Z", provider, dateTimeStyles, out result))
                                return result;
                    }
                    else if (s.Length == Mylar.StandardShortDateTimeLength - 1)
                    {
                        if (s[2] == ' ')
                            if (TryParseExact(s, "a", provider, dateTimeStyles, out result))
                                return result;
                        if (s[2] == '-' || char.IsDigit(s[2]))
                            if (TryParseExact(s, "z", provider, dateTimeStyles, out result))
                                return result;
                    }
                    else if (s.Length == Mylar.StandardDateLength - 1)
                    {
                        if (s[2] == ' ')
                            if (TryParseExact(s, "B", provider, dateTimeStyles, out result))
                                return result;
                        if (s[2] == '-' || char.IsDigit(s[2]))
                            if (TryParseExact(s, "X", provider, dateTimeStyles, out result))
                                return result;
                    }
                }
                else if ((char.IsLetter(s[0]) || char.IsWhiteSpace(s[0])) && char.IsLetter(s[1]) && char.IsLetter(s[2]))
                {
                    if (s.Length == Mylar.StandardDateTimeLength)
                    {
                        if (s[3] == ' ')
                            if (TryParseExact(s, "A", provider, dateTimeStyles, out result))
                                return result;
                        if (s[3] == '-' || char.IsDigit(s[3]))
                            if (TryParseExact(s, "Z", provider, dateTimeStyles, out result))
                                return result;
                    }
                    else if (s.Length == Mylar.StandardShortDateTimeLength)
                    {
                        if (s[3] == ' ')
                            if (TryParseExact(s, "a", provider, dateTimeStyles, out result))
                                return result;
                        if (s[3] == '-' || char.IsDigit(s[3]))
                            if (TryParseExact(s, "z", provider, dateTimeStyles, out result))
                                return result;
                    }
                    else if (s.Length == Mylar.StandardDateLength)
                    {
                        if (s[3] == ' ')
                            if (TryParseExact(s, "B", provider, dateTimeStyles, out result))
                                return result;
                        if (s[3] == '-' || char.IsDigit(s[3]))
                            if (TryParseExact(s, "X", provider, dateTimeStyles, out result))
                                return result;
                    }
                }
            }

            //偵測標準格式去除歷紀年開頭
            if (s.Length == Mylar.StandardShortDateLength && s[5] == '/' && slash == 2)
                if (TryParseExact(s, "b", provider, dateTimeStyles, out result))
                    return result;

            if (s.Length == Mylar.StandardShortDateExtensionLength && s[6] == '/' && slash == 2)
                if (TryParseExact(s, "x", provider, dateTimeStyles, out result))
                    return result;

            if (t > 0 && dash >= 2 && colon >= 2)
            {
                if (dot > 0)
                {
                    if (TryParseExact(s, "O", provider, dateTimeStyles, out result))
                        return result;
                }
                else
                {
                    if (TryParseExact(s, "s", provider, dateTimeStyles, out result))
                        return result;
                }
            }

            if (sp != 0)
            {
                //偵測標準格式去除歷紀年開頭
                if (s.Length == Mylar.StandardShortDateLength && s[5] == '/' && slash == 2)
                {
                    if (dot == 1)
                    {
                        if (TryParseExact(s, "A", provider, dateTimeStyles, out result))
                            return result;
                    }
                    else if (colon == 2)
                    {
                        if (TryParseExact(s, "a", provider, dateTimeStyles, out result))
                            return result;
                    }
                }

                if (s.Length == Mylar.StandardShortDateExtensionLength && s[6] == '/' && slash == 2)
                {
                    if (dot == 1)
                    {
                        if (TryParseExact(s, "Z", provider, dateTimeStyles, out result))
                            return result;
                    }
                    else if (colon == 2)
                    {
                        if (TryParseExact(s, "z", provider, dateTimeStyles, out result))
                            return result;
                    }
                }

                if (gmt > 0)
                {
                    if (TryParseExact(s, "R", provider, dateTimeStyles, out result))
                        return result;
                }

                if (z > 0 && dash >= 2 && colon >= 2)
                    if (TryParseExact(s, "u", provider, dateTimeStyles, out result))
                        return result;

                //常態情況
                if (s.Length >= MinimumCharForLongFormat && (dsr >= 2 || dsr == 0) && tsr > 0)
                {
                    if (tsr == 2)
                    {
                        if (TryParseExact(s, "G", provider, dateTimeStyles, out result))
                            return result;
                        if (TryParseExact(s, "F", provider, dateTimeStyles, out result))
                            return result;
                    }
                    else if (tsr == 1)
                    {
                        if (TryParseExact(s, "f", provider, dateTimeStyles, out result))
                            return result;
                        if (TryParseExact(s, "g", provider, dateTimeStyles, out result))
                            return result;
                    }
                }
            }

            if (s.Length == Mylar.StandardTimeLength && dot == 1 && colon == 2)
                if (TryParseExact(s, "C", provider, dateTimeStyles, out result))
                    return result;
            if (s.Length == Mylar.StandardShorTimeLength && colon == 2)
                if (TryParseExact(s, "c", provider, dateTimeStyles, out result))
                    return result;
            if (tsr == 2)
                if (TryParseExact(s, "T", provider, dateTimeStyles, out result))
                    return result;
            if (tsr == 1)
                if (TryParseExact(s, "t", provider, dateTimeStyles, out result))
                    return result;
            if (dsr == 2)
            {
                if (s.Length >= MinimumCharForLongDate)
                    if (TryParseExact(s, "D", provider, dateTimeStyles, out result))
                        return result;
                if (TryParseExact(s, "d", provider, dateTimeStyles, out result))
                    return result;
            }
            if (s.Length >= MinimumCharForLongDate)
                if (TryParseExact(s, "D", provider, dateTimeStyles, out result))
                    return result;
            if (TryParseExact(s, "d", provider, dateTimeStyles, out result))
                return result;
            if (TryParseExact(s, "M", provider, dateTimeStyles, out result))
                return result;
            if (TryParseExact(s, "Y", provider, dateTimeStyles, out result))
                return result;

            //非常態情況            
            if (sp != 0 && s.Length >= MinimumCharForLongFormat)
            {
                if (TryParseExact(s, "G", provider, dateTimeStyles, out result))
                    return result;
                if (TryParseExact(s, "F", provider, dateTimeStyles, out result))
                    return result;
                if (TryParseExact(s, "f", provider, dateTimeStyles, out result))
                    return result;
                if (TryParseExact(s, "g", provider, dateTimeStyles, out result))
                    return result;
            }
            if (TryParseExact(s, "T", provider, dateTimeStyles, out result))
                return result;
            if (TryParseExact(s, "t", provider, dateTimeStyles, out result))
                return result;
            if (sp == 0 && s.Length >= MinimumCharForLongFormat)
                if (TryParseExact(s, "F", provider, dateTimeStyles, out result))
                    return result;

            throw new FormatException(s);
            //M = m, Y = y, F = U, R = r, O = o, F包含G常見
        }

        public static string FormatDateTimeLoop(ArOutPartInfoList opil, ArStringPartInfo[] reservedString, DateTimeFormatInfo dtf, int year, int month, int day, int hour, int minute, int second, string decimalPart, int dow, bool isCEDate, bool isText = false)
        {
            StringBuilder sb = new StringBuilder();
            string s;
            foreach (ArOutPartInfo opi in opil.Value)
            {
                if (isText)
                    sb.Append(((ArOutStringPartInfo)opi).Value);
                else if (opi is ArOutPartInfoList pl)
                    sb.Append(FormatDateTimeLoop(pl, reservedString, dtf, year, month, day, hour, minute, second, decimalPart, dow, isCEDate, true));
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
                        case 15: // "yyyyyy"
                            sb.Append(year.ToString("000000;-00000"));
                            break;
                        case 16: // "yyyyy"
                            sb.Append(year.ToString("00000;-0000"));
                            break;
                        case 17: // "yyyy"
                            if (dtf == Mylar.ArinaCulture.DateTimeFormat)
                                sb.Append(year);
                            else
                                sb.Append(year.ToString("0000;-000"));
                            break;
                        case 18: // "yyy"
                            sb.Append(year.ToString("000;-00"));
                            break;
                        case 19: // "yy"
                            if (year > -100 && year < 100)
                                sb.Append(year.ToString("00;-0"));
                            else
                                sb.Append(Math.Abs(year % 100).ToString("00;-0"));
                            break;
                        case 20: // "MMMM"
                            sb.Append(dtf.GetMonthName(month));
                            break;
                        case 21: // "MMM"
                            sb.Append(dtf.GetAbbreviatedMonthName(month));
                            break;
                        case 22: // "MM"
                            sb.Append(month.ToString("00"));
                            break;
                        case 23: // "y"
                            if (year > -100 && year < 100)
                                sb.Append(year);
                            else
                                sb.Append(Math.Abs(year % 100));
                            break;
                        case 24: // "M"
                            sb.Append(month);
                            break;
                        case 25: // "d"
                            sb.Append(day);
                            break;
                        case 26: // "fffffff"
                            sb.Append(decimalPart.Substring(0, 7));
                            break;
                        case 27: // "ffffff"
                            sb.Append(decimalPart.Substring(0, 6));
                            break;
                        case 28: // "fffff"
                            sb.Append(decimalPart.Substring(0, 5));
                            break;
                        case 29: // "ffff"
                            sb.Append(decimalPart.Substring(0, 4));
                            break;
                        case 30: // "fff"
                            sb.Append(decimalPart.Substring(0, 3));
                            break;
                        case 31: // "ff"
                            sb.Append(decimalPart.Substring(0, 2));
                            break;
                        case 32: // "f"
                            sb.Append(decimalPart[0]);
                            break;
                        case 33: // "tt"                        
                            sb.Append(hour < 12 ? dtf.AMDesignator : dtf.PMDesignator);
                            break;
                        case 34: // "t"
                            sb.Append(hour < 12 ? dtf.AMDesignator[0] : dtf.PMDesignator[0]);
                            break;
                        case 35: // "ggg"                            
                            sb.Append(Mylar.GetStandardCalendarEraName(dtf));
                            break;
                        case 36: // "gg"
                        case 37: // "g"
                            if (!isCEDate)
                                s = pi.Index == 36 ? ArCultureInfo.EraName : ArCultureInfo.AbbreviatedEraName;
                            else if (pi.Index == 36)
                                s = dtf.GetEraName(0);
                            else
                                s = dtf.GetAbbreviatedEraName(0);
                            sb.Append(s);
                            break;
                        case 38: // "K"
                        case 39: // "zzz"
                            if (pi.Index == 38 && TimeZoneInfo.Local.BaseUtcOffset.Ticks == 0)
                                sb.Append("Z");
                            else if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                                sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString("\\+hh\\:mm"));
                            else
                                sb.Append(TimeZoneInfo.Local.BaseUtcOffset.ToString("\\-hh\\:mm"));
                            break;
                        case 40: // "zz"
                            if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                                sb.Append(TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString("\\+00"));
                            else
                                sb.Append(TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString("\\-00"));
                            break;
                        case 41: // "z"                        
                            if (TimeZoneInfo.Local.BaseUtcOffset.Ticks >= 0)
                                sb.Append($"+{TimeZoneInfo.Local.BaseUtcOffset.Hours}");
                            else
                                sb.Append($"-{TimeZoneInfo.Local.BaseUtcOffset.Hours}");
                            break;
                        case 42: // "FFFFFFF"
                            sb.Append(decimalPart.Substring(0, 7).TrimEnd('0'));
                            break;
                        case 43: // "FFFFFF"
                            sb.Append(decimalPart.Substring(0, 6).TrimEnd('0'));
                            break;
                        case 44: // "FFFFF"
                            sb.Append(decimalPart.Substring(0, 5).TrimEnd('0'));
                            break;
                        case 45: // "FFFF"
                            sb.Append(decimalPart.Substring(0, 4).TrimEnd('0'));
                            break;
                        case 46: // "FFF"
                            sb.Append(decimalPart.Substring(0, 3).TrimEnd('0'));
                            break;
                        case 47: // "FF"
                            sb.Append(decimalPart.Substring(0, 2).TrimEnd('0'));
                            break;
                        case 48: // "F"
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
                provider = Mylar.ArinaCulture;
            if (provider is CultureInfo ci)
                dtf = ci.DateTimeFormat;
            else if (provider is DateTimeFormatInfo di)
                dtf = di;
            else if (provider is ICustomFormatter cf)
                return cf.Format(format, adt, provider);
            else
                dtf = Mylar.ArinaCulture.DateTimeFormat;
            if (string.IsNullOrEmpty(format))
                format = "G";
            if (format.Length == 1)
                format = ArCultureInfo.GetFirstDateTimePattern(format[0], dtf);

            ArStringPartInfo[] reservedString = CreateDateTimeReservedStringPartInfo();
            DisassembleShop ds = new DisassembleShop();
            ArOutPartInfoList opil = ds.Disassemble(format, CreateFormatDisassembleInfo(reservedString));

            ArDateTime.TicksToDateTime(adt._data, out int year, out int month, out int day, out long timeTicks);
            if (timeTicks < 0)
                timeTicks += 864000000000L;
            ArDateTime.TimeTicksToTime(timeTicks, out int hour, out int minute, out int second, out int millisecond, out int tick);
            string decimalPart = (millisecond * 10000 + tick).ToString().PadLeft(7, '0');
            int dow = ArDateTime.GetDayOfWeek(year, month, day, true);
            dow = dow == 7 ? 0 : dow;
            if (provider == Mylar.ArinaCulture)
                year = ArDateTime.GetARYear(year);
            return FormatDateTimeLoop(opil, reservedString, dtf, year, month, day, hour, minute, second, decimalPart, dow, provider != Mylar.ArinaCulture);
        }
    }
}
