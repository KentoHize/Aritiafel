using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArDissambleInfo
    {
        public ArContainerPartInfo[] ContainerPartInfo { get; set; }
        public ArStringPartInfo[] StringPartInfo { get; set; }        
        public ArDissambleInfo(ArContainerPartInfo[] containerPartInfo = null, ArStringPartInfo[] stringPartInfo = null)
        {
            ContainerPartInfo = containerPartInfo ?? [];
            StringPartInfo = stringPartInfo ?? [];
        }

        public ArDissambleInfo(List<ArContainerPartInfo> containerPartInfo, List<ArStringPartInfo> stringPartInfo)
            : this(containerPartInfo.ToArray(), stringPartInfo.ToArray())
        { }
    }
}
