using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArDisassembleInfo
    {
        public ArPartInfo[][] ReservedStringInfo { get; set; }
        public ArContainerPartInfo[] ContainerPartInfo { get; set; }
        public ArDisassembleInfo(List<ArPartInfo> reservedStringInfo, List<ArContainerPartInfo> containerPartInfo)
            : this([reservedStringInfo.ToArray()], containerPartInfo.ToArray())
        { }
        public ArDisassembleInfo(ArPartInfo[] reservedStringInfo, ArContainerPartInfo[] containerPartInfo = null)
            : this([reservedStringInfo], containerPartInfo)
        { }
        public ArDisassembleInfo(ArPartInfo[][] reservedStringInfo = null, ArContainerPartInfo[] containerPartInfo = null)
        {
            ReservedStringInfo = reservedStringInfo ?? [];
            ContainerPartInfo = containerPartInfo ?? [];
        }
        
    }
}
