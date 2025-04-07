using Aritiafel.Definitions;
using System;

namespace Aritiafel.Items
{
    public class ArStringPartInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ArStringPartType Type { get; set; }
        public ArStringPartInfo(string value)
            : this(value, value)
        { }
        public ArStringPartInfo(string name, string value, ArStringPartType type = ArStringPartType.Normal)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type;
        }
    }
}
