using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Artifacts
{
    //主要用來Debug時送出物件資訊
    public static class Allseer
    {
        public static string SeeThrough(ArOutPartInfo opi)
        {
            if(opi is ArOutPartInfoList opil)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ArOutPartInfo opi2 in opil.Value)
                    sb.Append(SeeThrough(opi2));
                return sb.ToString();
            }
            else if(opi is ArOutStringPartInfo ospi)
            {
                if(!string.IsNullOrEmpty(ospi.Name))
                    return $"{ospi.Value}({ospi.Name})";
                else
                    return $"{ospi.Value}";
            }
            throw new ArgumentException(nameof(opi));
        }       

        public static string SeeThrough(object o)
        {
            if (o is ArOutPartInfo opi)
                return SeeThrough(opi);
            return o.ToArString();
        }            
    }
}
