using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Definitions
{
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
