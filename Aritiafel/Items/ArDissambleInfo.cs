using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArDissambleInfo
    {
        public ArStringPartInfo[] StringPartInfo { get; set; }
        public ArContainerPartInfo[] ContainerPartInfo { get; set; }        
        public ArDissambleInfo(ArStringPartInfo[] stringPartInfo = null, ArContainerPartInfo[] containerPartInfo = null)
        {   
            StringPartInfo = stringPartInfo ?? [];
            ContainerPartInfo = containerPartInfo ?? [];
        }
        public ArDissambleInfo(List<ArStringPartInfo> stringPartInfo, List<ArContainerPartInfo> containerPartInfo)
            : this(stringPartInfo.ToArray(), containerPartInfo.ToArray())
        { }
    }
}
