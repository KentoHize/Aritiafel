using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Aritiafel.Organizations.ArinaOrganization
{
    //Extension and others    
    public static class ArinaOrganization
    {
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
                return Color.FromArgb(int.Parse(s.Replace("#", ""), NumberStyles.HexNumber));
            else if (t == typeof(DateTime))
                return DateTime.Parse(s);
            else if (t == typeof(TimeSpan))
                return TimeSpan.Parse(s);
            else if (t == typeof(CultureInfo))
                return CultureInfo.GetCultureInfo(s);
            else if (t.IsEnum)
                return Enum.Parse(t, s);
            else
                return s;
        }
        public static string ToArString(this object o)
        {
            if (o.GetType() == typeof(Color))
                return "#" + ((Color)o).R.ToString("X2") + ((Color)o).G.ToString("X2") + ((Color)o).B.ToString("X2");
            return o.ToString();
        }
    }
}
