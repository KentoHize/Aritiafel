using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Artifacts
{
    //主要用來Debug時送出物件資訊
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
            else if (o is Array a)
            {
                StringBuilder sb = new StringBuilder();
                for (long i = 0; i < a.LongLength; i++)
                    sb.Append($"[{i}]:{SeeThrough(a.GetValue(i))} ");
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
