using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Definitions
{
    /// <summary>
    /// Parse ArDateTime時可使用的選項
    /// 標準系統格式不能使用中間含空白
    /// </summary>
    [Flags]
    public enum ArDateTimeStyles
    {
        None = 0,        
        AllowLeadingWhite = 1,        
        AllowTrailingWhite = 2,        
        AllowInnerWhite = 4,        
        AllowWhiteSpaces = 7,        
        CurrentDateDefault = 8
    }
}
