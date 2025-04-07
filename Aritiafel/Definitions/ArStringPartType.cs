using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Definitions
{
    public enum ArStringPartType
    {
        Normal = 0,
        Escape1,
        Escape2,
        ContainerStart,
        ContainerEnd
    }
}
