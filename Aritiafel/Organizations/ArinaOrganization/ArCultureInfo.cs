using Aritiafel.Characters.Heroes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aritiafel.Organizations.ArinaOrganization
{
    public partial class ArCultureInfo : CultureInfo
    {
        internal ArCultureInfo()
            : this("zh-TW", true)
        { }

        internal ArCultureInfo(string name, bool useUserOverride)
            : base(name, useUserOverride)
            => CreateDateTimeFormatInfo();

        public override string Name => "zh-AA";
        public override string EnglishName => "Chinese (Arina)";
        public override string DisplayName => "中文（有奈）";
        public override string NativeName => DisplayName;
        //LOCALE_CUSTOM_UNSPECIFIED (0x1000, or 4096))
        public override int LCID => 0x7C04;

        public const string SystemCalendarEraName = " AR"; //暫時
        public override string ToString()
            => Name;

        internal void CreateDateTimeFormatInfo()
        {
            //DateTimeFormat.Calendar = new ArCalendar();            
            DateTimeFormat.DayNames = ["[7]", "[1]", "[2]", "[3]", "[4]", "[5]", "[6]"];
            DateTimeFormat.AbbreviatedDayNames = ["[7]", "[1]", "[2]", "[3]", "[4]", "[5]", "[6]"];
            DateTimeFormat.ShortDatePattern = "M, d, g. yyyy";
            DateTimeFormat.LongDatePattern = "M, d, g. yyyy";
            DateTimeFormat.ShortTimePattern = "H:mm:ss";
            DateTimeFormat.LongTimePattern = "H:mm:ss.fff";
            DateTimeFormat.YearMonthPattern = "M, g. yyyy";
            DateTimeFormat.MonthDayPattern = "M, d";
            DateTimeFormat.FullDateTimePattern = "M, d, g. yyyy H:mm:ss.fff";
            DateTimeFormat.DateSeparator = ",";
        }
        public new static CultureInfo GetCultureInfo(int culture)
        { 
            if (culture == Mylar.ArinaCultureInfo.LCID)
                return Mylar.ArinaCultureInfo;
            else
                return CultureInfo.GetCultureInfo(culture);
        }

        public new static CultureInfo GetCultureInfo(string name)
        {
            if (name == Mylar.ArinaCultureInfo.Name)
                return Mylar.ArinaCultureInfo;
            else
                return CultureInfo.GetCultureInfo(name);
        }
        
        public new static CultureInfo GetCultureInfo(string name, string altname)
        {
            if (name == Mylar.ArinaCultureInfo.Name)
                return Mylar.ArinaCultureInfo;
            else
                return CultureInfo.GetCultureInfo(name, altname);
        }

        public new static CultureInfo[] GetCultures(CultureTypes types)
        {   
            CultureInfo[] cultures = CultureInfo.GetCultures(types);            
            if (types.HasFlag(CultureTypes.UserCustomCulture) ||
                types.HasFlag(CultureTypes.AllCultures))
            {
                Array.Resize(ref cultures, cultures.Length + 1);
                cultures[cultures.Length - 1] = Mylar.ArinaCultureInfo;                
            }   
            return cultures;
        }
        public string[] GetAllDateTimePatterns()
            => GetAllDateTimePatterns(DateTimeFormat);
        public static string[] GetAllDateTimePatterns(DateTimeFormatInfo dtfi = null)
        {
            if (dtfi == null)
                dtfi = Mylar.ArinaCultureInfo.DateTimeFormat;
            string[] result = dtfi.GetAllDateTimePatterns();
            string[] result2 = Mylar.GetAllStandardDateTimePatterns(dtfi);
            return result.Concat(result2).ToArray();
        }

        public string GetFirstDateTimePattern(char format)
            => GetFirstDateTimePattern(format, DateTimeFormat);
        public static string GetFirstDateTimePattern(char format, DateTimeFormatInfo dtfi = null)
            => GetAllDateTimePatterns(format, dtfi)[0];
        public string[] GetAllDateTimePatterns(char format)
            => GetAllDateTimePatterns(format, DateTimeFormat);
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
    }
}
