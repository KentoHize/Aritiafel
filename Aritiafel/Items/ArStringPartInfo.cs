using Aritiafel.Definitions;
using System;
using System.Linq;

namespace Aritiafel.Items
{
    public class ArStringPartInfo
    {
        protected string[] _Values; //不存null
        public string Name { get; set; } //名稱
        public string[] Values //值(可選擇)
        {
            get => _Values;
            set
            {
                if (value == null || value.Contains(null))
                    throw new ArgumentNullException(nameof(Values));
                else if (value.Length == 0)
                    throw new ArgumentOutOfRangeException(nameof(Values));
                _Values = value;
            }
        }
        public string Value  //值
        {
            get => Values[0];
            set => Values[0] = value ?? throw new ArgumentNullException(nameof(Value));
        }

        public string this[int index] //索引子
        {
            get => Values[index];
            set => Values[index] = value ?? throw new ArgumentNullException(nameof(Values));
        }

        public ArStringPartType Type { get; set; } //類型
        public int MaxLength { get; set; } //最長字元
        public int Times { get; set; } //出現次數
        public ArStringPartInfo(string value)
            : this(value, [value])
        { }

        public ArStringPartInfo(string name, string value, ArStringPartType type = ArStringPartType.Normal, int maxLength = 0, int times = -1)
            : this(name, [value], type, maxLength, times)
        { }

        public ArStringPartInfo(string name, string[] values, ArStringPartType type = ArStringPartType.Normal, int maxLength = 0, int times = -1)
        {
            Name = name;
            Values = values;
            Type = type;
            MaxLength = maxLength;
            Times = times;
        }
    }
}
