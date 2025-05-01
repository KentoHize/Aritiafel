using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Definitions
{
    public enum ArStringPartType
    {
        Normal = 0,

        //Number
        UnsignedInteger, // 22, 333
        Integer, // 14, -1, +4
        Decimal, // 4.3, -4.3
        ScientificNotation, //3.5e+5, 3.6e-5

        Escape1,
        Escape2,

        Char,
        String,
        Special //考慮增加同名全砍一次
    }
}
