using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Artifacts
{
    public static partial class Allseer
    {
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
