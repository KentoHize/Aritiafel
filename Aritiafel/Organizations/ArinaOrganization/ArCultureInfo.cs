using Aritiafel.Organizations.RaeriharUniversity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.ArinaOrganization
{
    public partial class ArCultureInfo : CultureInfo
    {
        internal ArCultureInfo()
            : this("zh-TW", true)
        { }

        internal ArCultureInfo(string name, bool useUserOverride) 
            : base(name, useUserOverride)
        {
            CreateDateTimeFormatInfo();
        }

        public override string Name => "zh-AO";
        public override string EnglishName => "Chinese (Arina)";
        public override string DisplayName => "中文 (有奈)";

        internal void CreateDateTimeFormatInfo()
        {
            DateTimeFormat.DayNames = ["[7]", "[1]", "[2]", "[3]", "[4]", "[5]", "[6]"];
            DateTimeFormat.AbbreviatedDayNames = ["[7]", "[1]", "[2]", "[3]", "[4]", "[5]", "[6]"];
            DateTimeFormat.ShortDatePattern = "M, d, Ar. yyyy";
            DateTimeFormat.LongDatePattern = "M, d, Ar. yyyy";
            DateTimeFormat.ShortTimePattern = "H:m:s";
            DateTimeFormat.LongTimePattern = "H:m:s.fff";
            DateTimeFormat.YearMonthPattern = "M, Ar. yyyy";
            DateTimeFormat.MonthDayPattern = "M, d";            
            DateTimeFormat.FullDateTimePattern = "M, d, Ar. yyyy H:m:s.fff";
        }
    }
}
