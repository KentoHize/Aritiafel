using Aritiafel.Organizations.RaeriharUniversity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.ArinaOrganization
{
    public static class Mylar // Emerald Shrine
    {
        private static ArCultureInfo _ArinaCultureInfo;
        public static ArCultureInfo ArinaCultureInfo
            => _ArinaCultureInfo ??= new ArCultureInfo();

        public const int StandardDateTimeLength = 32;
        public const int StandardShortDateTimeLength = 24;
        public const int StandardDateLength = 15;
        public const int StandardShortDateLength = 11;
        public const int StandardTimeLength = 16;
        public const int StandardShorTimeLength = 8;

        public const string StandardDatePattern = "yyyyy/MM/dd";
        public const string StandardTimePattern = "HH:mm:ss.fffffff";
        public const string StandardShortTimePattern = "HH:mm:ss";
        public static string GetStandardDateTimePattern(DateTimeFormatInfo dtfi, ArStandardDateTimeType type = ArStandardDateTimeType.DateTime)
        {
            StringBuilder sb = new StringBuilder();
            if(type == ArStandardDateTimeType.DateTime || type == ArStandardDateTimeType.ShortDateTime ||
                type == ArStandardDateTimeType.Date)
            {
                if (dtfi.Calendar is ArCalendar)
                    sb.Append(ArCultureInfo.SystemCalendarName);
                else if (dtfi.Calendar is GregorianCalendar)
                    sb.Append(" CE");
                else
                    sb.Append("   ");                
            }

            if (sb.Length > 0)
                sb.Append(" ");

            if (type == ArStandardDateTimeType.DateTime || type == ArStandardDateTimeType.ShortDateTime ||
                type == ArStandardDateTimeType.Date || type == ArStandardDateTimeType.ShortDate)
                sb.Append(StandardDatePattern);

            if (type == ArStandardDateTimeType.DateTime || type == ArStandardDateTimeType.ShortDateTime)
                sb.Append(" ");
            
            if(type == ArStandardDateTimeType.DateTime ||
                type == ArStandardDateTimeType.Time)
                sb.Append(StandardTimePattern);
            else if (type == ArStandardDateTimeType.ShortDateTime ||
                type == ArStandardDateTimeType.ShortTime)
                sb.Append(StandardShortTimePattern);
            return sb.ToString();
        }
    }
}
