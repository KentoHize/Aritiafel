using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Aritiafel.Items
{    
    public abstract class ArOutPartInfo
    {
        public string Name { get; set; }
        public ArOutPartInfo()
            : this("")
        { }
        public ArOutPartInfo(string name)
        { Name = name ?? ""; }
    }
}
