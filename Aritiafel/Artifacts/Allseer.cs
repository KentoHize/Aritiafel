using Aritiafel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Aritiafel.Artifacts
{
    //主要用來Debug時送出物件資訊
    //
    //Allseer.RegisterCustomSeeThroughFunction<int>(m => (m + 3).ToString());    
    //Sophia.SeeThrough(5);
    //8
    public static partial class Allseer
    {
        internal static Dictionary<Type, Func<object, string>> _CustomSeeThrough;
        static Allseer()
        {
            _CustomSeeThrough = new Dictionary<Type, Func<object, string>>();
        }
        internal static Func<object, string> ConvertToCustomSeeThroughItem<T>(Func<T, string> customSeeThroughFunction)
            => m => customSeeThroughFunction((T)m);
        public static void RegisterCustomSeeThroughFunction<T>(Func<T, string> customSeeThroughFunction)
            => _CustomSeeThrough[typeof(T)] = ConvertToCustomSeeThroughItem(customSeeThroughFunction);
        public static bool RemoveCustomSeeThroughFunction<T>()
        {
            if (_CustomSeeThrough.ContainsKey(typeof(T)))
            {
                _CustomSeeThrough.Remove(typeof(T));
                return true;
            }
            return false;
        }
        public static string SeeThrough(object o)
        {
            Type t = o.GetType();
            if (_CustomSeeThrough.ContainsKey(t))
                return _CustomSeeThrough[t].Invoke(o);
            else if (o is string s)
                return s;
            else if (o is Stopwatch sw)
                return sw.ElapsedTicks.ToString();
            else if (o is ArOutPartInfo opi)
                return SeeThrough(opi);
            else if (o is ArStringPartInfo spi)
                return SeeThrough(spi);
            else if (o is IEnumerable ie)
            {
                StringBuilder sb = new StringBuilder();
                var en = ie.GetEnumerator();
                for (int i = 0; en.MoveNext(); i++)
                    sb.Append($"[{i}]:{SeeThrough(en.Current)} ");
                if (sb.Length != 0)
                    sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return o.ToArString();
        }

        public static string SeeThrough(ArStringPartInfo spi)
        {
            if (string.IsNullOrEmpty(spi.Name))
                return $"{spi.Value}";
            return $"{spi.Value}({spi.Name})";
        }

        public static string SeeThrough(ArOutPartInfo opi)
        {
            if (opi is ArOutPartInfoList opil)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ArOutPartInfo opi2 in opil.Value)
                    sb.Append(SeeThrough(opi2));
                return sb.ToString();
            }
            else if (opi is ArOutStringPartInfo ospi)
            {
                if (!string.IsNullOrEmpty(ospi.Name))
                    return $"{ospi.Value}({ospi.Name})";
                else
                    return $"{ospi.Value}";
            }
            throw new ArgumentException(nameof(opi));
        }
    }
}
