using Aritiafel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Locations
{
    public enum StringMatchPolicy
    {
        Normal, //沒有對到字串時，下一個字串
        IgnoreLimitedReservedStringIfNoMatch, //沒有對到字串時，之後忽略該字串
        SkipAllReservedStringIfFirstNoMatch, //沒有對到字串時，跳過全部字串
    }

    public class DisassembleShopSetting
    {
        public bool RecordValueWithoutEscapeChar { get; set; }
        public bool ErrorOccurIfNoMatch { get; set; }
        public StringMatchPolicy ReservedStringMatchPolicy { get; set; } //拆解字串的策略
        public bool WhenReservedStringNoMatchTimesIgnorePreviousReservedString { get; set; } //任何字串沒有比對次數時前面的字串一起忽略(加速比對)
        public DisassembleShopSetting()
        {
            RecordValueWithoutEscapeChar = true;
            ErrorOccurIfNoMatch = false;
            ReservedStringMatchPolicy = StringMatchPolicy.Normal;            
            WhenReservedStringNoMatchTimesIgnorePreviousReservedString = false;            
        }
    }
}
