using Aritiafel.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Aritiafel.Artifacts
{
    //主要用來Debug時送出物件資訊
    //增加方法需要增加新的If判斷
    public static partial class Allseer
    {
        public static string SeeThrough(object o)
        {
            if (o is ISeeThrough st)
                return st.ReflectString();
            else if (o is ArOutPartInfo opi)
                return SeeThrough(opi);
            else if (o is ArStringPartInfo spi)
                return SeeThrough(spi);
            else if (o is string s)
                return s;
            else if (o is Stopwatch sw)
                return sw.ElapsedTicks.ToString();
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
    }

    public interface ISeeThrough
    {
        string ReflectString();
    }
}
