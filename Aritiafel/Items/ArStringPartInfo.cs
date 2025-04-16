using Aritiafel.Definitions;
using System;
using System.Collections.Generic;

namespace Aritiafel.Items
{
    public class ArStringPartInfo : ArPartInfo
    {   
        public string Value { get; set; } //值
        public ArStringPartType Type { get; set; } //類型
        public int MaxLength { get; set; } //最長字元
        public int Times { get; set; } //出現次數
        public ArStringPartInfo(string value)
            : this(value, value)
        { }
        public ArStringPartInfo(string name, string value, ArStringPartType type = ArStringPartType.Normal, int maxLength = 0, int times = -1)
            : base(name)
        {   
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type;
            MaxLength = maxLength;
            Times = times;
        }        
    }
}
