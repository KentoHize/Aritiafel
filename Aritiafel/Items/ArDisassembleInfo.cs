using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArDisassembleInfo
    {
        public ArStringPartInfo[][] ReservedStringInfo { get; set; }
        public ArContainerPartInfo[] ContainerPartInfo { get; set; }
        public ArDisassembleInfo(List<ArStringPartInfo> reservedStringInfo, List<ArContainerPartInfo> containerPartInfo)
            : this([reservedStringInfo.ToArray()], containerPartInfo.ToArray())
        { }
        public ArDisassembleInfo(ArStringPartInfo[] reservedStringInfo, ArContainerPartInfo[] containerPartInfo = null)
            : this([reservedStringInfo], containerPartInfo)
        { }
        public ArDisassembleInfo(ArStringPartInfo[][] reservedStringInfo = null, ArContainerPartInfo[] containerPartInfo = null)
        {
            ReservedStringInfo = reservedStringInfo ?? [];
            ContainerPartInfo = containerPartInfo ?? [];
        }
        
    }
}
