using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Text;

//拆解字串，提供初步分析
//AA, BB, CC, D1
//BBBCCD22D1AA
//=>  1 -1 2 -1 -1 -1 3 0
//=> BB B CC D 2 2 D1 AA

//合拆(尚待開發)
//{} [] () 
//{CC3BB(AA)}
//=>
//=> {
//=>    box:0,
//=>    content:[2, -1, 1, {
//=>        box:1,
//=>        content:[0]
//=>    }]
//=> }

//分辨數字功能(待開發)

namespace Aritiafel.Locations
{    
    public static class DisassembleShop
    {
        public static ArOutStringPartInfo[] Disassemble(string s, string[] reserved)
        {
            List<ArStringPartInfo> r2 = new List<ArStringPartInfo>();
            foreach (string item in reserved)
                r2.Add(new ArStringPartInfo(item));
            return DisassembleStringFull(s, r2.ToArray());
        }
        public static ArOutStringPartInfo[] Disassemble(string s, ArStringPartInfo[] reserved)
            => DisassembleStringFull(s, reserved);        
        internal static ArOutStringPartInfo[] DisassembleStringFull(string s, ArStringPartInfo[] reserved)
        {   
            if(string.IsNullOrEmpty(s))
                throw new ArgumentException(nameof(s));

            List<ArOutStringPartInfo> result = new List<ArOutStringPartInfo>();
            while(true)
            {
                bool found = false;
                for (int i = 0; i < reserved.Length; i++)
                {
                    if (s.StartsWith(reserved[i].Value))
                    {
                        result.Add(new ArOutStringPartInfo(i, reserved[i].Name, reserved[i].Value));
                        s = s.Substring(reserved[i].Value.Length);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    result.Add(new ArOutStringPartInfo(0, "", s[0].ToString()));
                    s = s.Substring(1);
                }

                if (s.Length == 0)
                    return result.ToArray();
            }
        }

    }
}
