using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public abstract class ArPartInfo
    {
        public string Name { get; set; }
        public ArPartInfo()
            : this("")
        { }
        public ArPartInfo(string name)
        { Name = name ?? ""; }
    }
}
