using Aritiafel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArOutStringPartInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ArStringPartType Type { get; set; }

        public ArOutStringPartInfo(int index, string name = "", string value = "", ArStringPartType type = ArStringPartType.Normal)
        {
            Index = index;
            Name = name ?? throw new ArgumentNullException(nameof(value));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type;
        }

    }
}
