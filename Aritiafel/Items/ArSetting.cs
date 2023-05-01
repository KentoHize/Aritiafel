using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace Aritiafel.Items
{
    public class ArSetting
    {
        public string Section { get; set; }
        public string Key { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }

        public ArSetting(string key, object value = null, string section = null, string description = null)
        {
            Key = key;
            if (value == null)
                Type = typeof(object);
            else
                Type = value.GetType();
            Value = value;
            Section = section;
            Description = description;
        }
    }
}
