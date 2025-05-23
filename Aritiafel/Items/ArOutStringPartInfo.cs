﻿using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Definitions;

namespace Aritiafel.Items
{
    public class ArOutStringPartInfo : ArOutPartInfo
    {
        public int Index { get; set; }
        public string Value { get; set; } //如果為Char或是String => 值為Name
        public int GroupIndex { get; set; } //從Group選擇，回傳Group Index
        public ArStringPartType Type { get; set; }

        public ArOutStringPartInfo(int index, string name = "", string value = null, ArStringPartType type = ArStringPartType.Normal, int groupIndex = 0)
            : base(name)
        {
            Index = index;
            Value = value;
            Type = type;
            GroupIndex = groupIndex;
        }

    }
}
