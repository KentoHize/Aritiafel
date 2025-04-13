using Aritiafel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Locations
{
    public class DisassembleShopSetting
    {
        public bool RecordValueWithoutEscapeChar { get; set; }        
        public DisassembleShopSetting()
        {
            RecordValueWithoutEscapeChar = true;            
        }
    }
}
