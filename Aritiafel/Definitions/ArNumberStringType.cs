using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Definitions
{
    public enum ArNumberStringType
    {
        Undefined = 0,
        UnsignedInteger, // 22, 333
        Integer, // 14, -1, +4
        Decimal, // 4.3, -4.3
        ScientificNotation //3.5e+5, 3.6e-5
    }
}
