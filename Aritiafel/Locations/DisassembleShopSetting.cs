﻿using Aritiafel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Locations
{
    public class DisassembleShopSetting
    {
        public bool RecordValueWithoutEscapeChar { get; set; }
        //public int DiscernNumberMaxLength { get; set; }
        //public ArNumberStringType DiscernNumber { get; set; }
        //public bool DiscernNumberFirst { get; set; }

        public DisassembleShopSetting()
        {
            RecordValueWithoutEscapeChar = true;
            //DiscernNumberMaxLength = 0;
            //DiscernNumber = ArNumberStringType.Undefined;
            //DiscernNumberFirst = false;
        }
    }
}
