using Aritiafel.Organizations.ArinaOrganization;
using Aritiafel.Organizations.RaeriharUniversity;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aritiafel
{
    //Extension and others    
    public static partial class ArinaOrganizationExtension
    {        
        public static ConcurrentDictionary<string, string> EnumStringDictionary { get; set; }

        public static object ParseArString(this string s, Type t)
        {
            if (t == typeof(bool))
                return bool.Parse(s);
            else if (t == typeof(byte))
                return byte.Parse(s);
            else if (t == typeof(sbyte))
                return sbyte.Parse(s);
            else if (t == typeof(short))
                return short.Parse(s);
            else if (t == typeof(ushort))
                return ushort.Parse(s);
            else if (t == typeof(int))
                return int.Parse(s);
            else if (t == typeof(uint))
                return uint.Parse(s);
            else if (t == typeof(long))
                return long.Parse(s);
            else if (t == typeof(ulong))
                return ulong.Parse(s);
            else if (t == typeof(float))
                return float.Parse(s);
            else if (t == typeof(double))
                return double.Parse(s);
            else if (t == typeof(decimal))
                return decimal.Parse(s);
            else if (t == typeof(char))
                return char.Parse(s);
            else if (t == typeof(Color))
                return Color.FromArgb(int.Parse(s.Substring(1, 2), NumberStyles.HexNumber),
                    int.Parse(s.Substring(3, 2), NumberStyles.HexNumber),
                    int.Parse(s.Substring(5, 2), NumberStyles.HexNumber));
            else if (t == typeof(DateTime))
                return DateTime.Parse(s);
            else if (t == typeof(ArDateTime))
                return ArDateTime.Parse(s);
            else if (t == typeof(TimeSpan))
                return TimeSpan.Parse(s);
            else if (t == typeof(CultureInfo))
                return ArCultureInfo.GetCultureInfo(s);
            else if (t.IsEnum)
                return Enum.Parse(t, s);
            else
                return s;
        }
        public static string ToArString(this object o, string format = null)
        {   
            Type type = o.GetType();
            if (type == typeof(Color))
                return "#" + ((Color)o).R.ToString("X2") + ((Color)o).G.ToString("X2") + ((Color)o).B.ToString("X2");
            else if (type.IsEnum)
            {
                if (EnumStringDictionary != null)
                    return EnumStringDictionary.ContainsKey(o.ToString()) ? EnumStringDictionary[o.ToString()] : o.ToString();
                else
                    return o.ToString();
            }
            else if (o is IFormattable r1)
            {
                return r1.ToString(format, Mylar.ArinaCultureInfo);
            }   
            return o.ToString();
        }

        public static void RecordEnumStringValue(this IDictionary<string, string> mapTable)
        {
            if (EnumStringDictionary == null)
                EnumStringDictionary = new ConcurrentDictionary<string, string>(mapTable);
            else
                foreach (KeyValuePair<string, string> kvp in mapTable)
                    EnumStringDictionary.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => kvp.Value);
        }

        public static string GetNestedTypeName(this Type type)
        {
            if (type.DeclaringType == null)
                return type.Name;
            return string.Concat(GetNestedTypeName(type.DeclaringType), "+", type.Name);
        }

        /// <summary>
        /// 字串A對字串B從Index開始比較是否相等
        /// </summary>
        /// <param name="s">字串A</param>
        /// <param name="s2">字串B</param>
        /// <param name="index">起始位置</param>
        /// <returns>是否相等</returns>
        public static bool MatchString(this string s, string s2, int index = 0)
        {
            if (s.Length > s2.Length - index)
                return false;
            for (int i = 0; i < s.Length; i++)
                if (s[i] != s2[i + index])
                    return false;
            return true;
        }

    }
}
