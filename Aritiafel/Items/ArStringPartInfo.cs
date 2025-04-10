using Aritiafel.Definitions;
using System;

namespace Aritiafel.Items
{
    public class ArStringPartInfo
    {
        public string Name { get; set; } //名稱
        public string Value { get; set; } //值
        public ArStringPartType Type { get; set; } //類型
        public int MaxLength { get; set; } //最長字元
        public int Times { get; set; } //出現次數
        public ArStringPartInfo(string value)
            : this(value, value)
        { }
        public ArStringPartInfo(string name, string value, ArStringPartType type = ArStringPartType.Normal, int maxLength = 0, int times = -1)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Type = type;
            MaxLength = maxLength;
            Times = times;
        }
    }
}
