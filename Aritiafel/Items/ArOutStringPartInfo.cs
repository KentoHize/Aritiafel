using Aritiafel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArOutStringPartInfo : ArOutPartInfo
    {
        public int Index { get; set; }        
        public string Value { get; set; } //如果為Char或是String => 值為Name
        public ArStringPartType Type { get; set; }

        public ArOutStringPartInfo(int index, string name = "", string value = null, ArStringPartType type = ArStringPartType.Normal)
            : base(name)
        {
            Index = index;
            Value = value;
            Type = type;
        }

    }
}
