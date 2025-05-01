using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Aritiafel.Organizations.ArinaOrganization
{
    public static class Mylar // Emerald Shrine
    {
        static ArCultureInfo _ArinaCulture;
        public static ArCultureInfo ArinaCulture
            => _ArinaCulture ??= new ArCultureInfo();

        public const int StandardDateTimeLength = 32;
        public const int StandardDateTimeExtensionLength = 32;
        public const int StandardShortDateTimeLength = 24;
        public const int StandardShortDateTimeExtensionLength = 24;
        public const int StandardDateLength = 15;
        public const int StandardDateExtensionLength = 15;
        public const int StandardShortDateLength = 11;
        public const int StandardShortDateExtensionLength = 12;
        public const int StandardTimeLength = 16;
        public const int StandardShorTimeLength = 8;

        public const string StandardCalendarEraPattern = "ggg";
        public const string StandardDateExtensionPattern = "yyyyyy'/'MM'/'dd";
        public const string StandardDatePattern = "yyyyy'/'MM'/'dd";
        public const string StandardTimePattern = "HH':'mm':'ss'.'fffffff";
        public const string StandardShortTimePattern = "HH':'mm':'ss";

        public static string GetStandardCalendarEraName()
            => GetStandardCalendarEraName(ArinaCulture.DateTimeFormat);
        public static string GetStandardCalendarEraName(DateTimeFormatInfo dtfi)
        {
            //增加 判斷正確的紀年三字
            if (dtfi == null)
                return "   ";
            else if (dtfi == ArinaCulture.DateTimeFormat)
                return ArCultureInfo.SystemCalendarEraName;
            else if (dtfi.Calendar is GregorianCalendar)
                return " CE";
            else
                return "ZZZ";
        }

        public static string[] GetAllStandardCalendarEraName() //先暫時寫死
            => [ArCultureInfo.SystemCalendarEraName, " CE", "ZZZ"];
        public static string[] GetAllStandardDateTimePatterns(DateTimeFormatInfo dtfi)
        {
            Array allStandardDateTimeTypes = Enum.GetValues(typeof(ArStandardDateTimeType));
            string[] result = new string[allStandardDateTimeTypes.Length];
            int i = 0;
            foreach (ArStandardDateTimeType item in allStandardDateTimeTypes)
            {
                result[i] = GetStandardDateTimePattern(dtfi, item);
                i++;
            }
            return result;
        }

        public static string GetStandardDateTimePattern(DateTimeFormatInfo dtfi, ArStandardDateTimeType type = ArStandardDateTimeType.DateTime)
        {
            StringBuilder sb = new StringBuilder();
            if (type == ArStandardDateTimeType.DateTime || type == ArStandardDateTimeType.ShortDateTime ||
                type == ArStandardDateTimeType.Date || type == ArStandardDateTimeType.DateTimeExtension ||
                type == ArStandardDateTimeType.ShortDateTimeExtension || type == ArStandardDateTimeType.DateExtension)
                sb.Append(StandardCalendarEraPattern);

            if (sb.Length > 0 && type != ArStandardDateTimeType.DateTimeExtension && type != ArStandardDateTimeType.ShortDateTimeExtension &&
                type != ArStandardDateTimeType.DateExtension && type != ArStandardDateTimeType.ShortDateExtension)
                sb.Append(" ");

            if (type == ArStandardDateTimeType.DateTime || type == ArStandardDateTimeType.ShortDateTime ||
                type == ArStandardDateTimeType.Date || type == ArStandardDateTimeType.ShortDate)
                sb.Append(StandardDatePattern);
            else if (type == ArStandardDateTimeType.DateTimeExtension || type == ArStandardDateTimeType.ShortDateTimeExtension ||
                type == ArStandardDateTimeType.DateExtension || type == ArStandardDateTimeType.ShortDateExtension)
                sb.Append(StandardDateExtensionPattern);

            if (type == ArStandardDateTimeType.DateTime || type == ArStandardDateTimeType.ShortDateTime ||
                type == ArStandardDateTimeType.DateTimeExtension || type == ArStandardDateTimeType.ShortDateTimeExtension)
                sb.Append(" ");

            if (type == ArStandardDateTimeType.DateTime || type == ArStandardDateTimeType.Time ||
                type == ArStandardDateTimeType.DateTimeExtension)
                sb.Append(StandardTimePattern);
            else if (type == ArStandardDateTimeType.ShortDateTime || type == ArStandardDateTimeType.ShortTime ||
                type == ArStandardDateTimeType.ShortDateTimeExtension)
                sb.Append(StandardShortTimePattern);
            return sb.ToString();
        }
    }
}
