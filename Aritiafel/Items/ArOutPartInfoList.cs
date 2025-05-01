using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArOutPartInfoList : ArOutPartInfo
    {
        public string StartString { get; set; }
        public string EndString { get; set; }
        public List<ArOutPartInfo> Value { get; set; }
        public ArOutPartInfoList()
            : this(null, "", "", "")
        { }
        public ArOutPartInfoList(List<ArOutPartInfo> value, string name = "", string startString = null, string endString = null)
            : base(name)
        {
            StartString = startString;
            EndString = endString;
            Value = value ?? new List<ArOutPartInfo>();
        }
    }
}
