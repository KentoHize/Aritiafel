using Aritiafel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Locations
{
    public class DisassembleShopSetting
    {
        public bool RecordValueWithoutEscapeChar { get; set; }
        public bool ErrorOccurIfNoMatch { get; set; }
        public bool IgnoreLimitedReservedStringIfNoMatch { get; set; } //沒有對到字串時，之後忽略該字串
        public bool WhenReservedStringNoMatchTimesIgnorePreviousReservedString { get; set; } //任何字串沒有比對次數時前面的字串一起忽略(加速比對)
        public DisassembleShopSetting()
        {
            RecordValueWithoutEscapeChar = true;
            ErrorOccurIfNoMatch = false;
            IgnoreLimitedReservedStringIfNoMatch = false;
            WhenReservedStringNoMatchTimesIgnorePreviousReservedString = false;
        }
    }
}
