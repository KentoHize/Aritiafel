using Aritiafel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArContainerPartInfo
    {
        public string Name { get; set; }
        public string StartString { get; set; }
        public string EndString { get; set; }
        public int ReservedStringInfoIndex { get; set; }

        public ArContainerPartInfo(string name = "", string startString = "", string endString = "", int reservedStringInfoIndex = 0)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            StartString = startString ?? throw new ArgumentNullException(nameof(startString));
            EndString = endString ?? throw new ArgumentNullException(nameof(endString));
            ReservedStringInfoIndex = reservedStringInfoIndex >= -1 ? reservedStringInfoIndex : throw new ArgumentOutOfRangeException(nameof(reservedStringInfoIndex));
        }
    }
}
