using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArDissambleInfo
    {
        public ArStringPartInfo[][] ReservedStringInfo { get; set; }
        public ArContainerPartInfo[] ContainerPartInfo { get; set; }
        public ArDissambleInfo(List<ArStringPartInfo> reservedStringInfo, List<ArContainerPartInfo> containerPartInfo)
            : this([reservedStringInfo.ToArray()], containerPartInfo.ToArray())
        { }
        public ArDissambleInfo(ArStringPartInfo[] reservedStringInfo = null, ArContainerPartInfo[] containerPartInfo = null)
            : this([reservedStringInfo], containerPartInfo)
        { }
        public ArDissambleInfo(ArStringPartInfo[][] reservedStringInfo = null, ArContainerPartInfo[] containerPartInfo = null)
        {
            ReservedStringInfo = reservedStringInfo ?? [];
            ContainerPartInfo = containerPartInfo ?? [];
        }
        
    }
}
