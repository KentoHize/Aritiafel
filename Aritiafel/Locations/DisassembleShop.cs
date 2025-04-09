using Aritiafel.Definitions;
using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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

//分辨數字功能

namespace Aritiafel.Locations
{
    public class DisassembleShop
    {
        public bool RecordValueWithoutEscapeChar { get; set; }
        public ArNumberStringType DiscernNumber { get; set; }
        public bool DiscernNumberFirst { get; set; } = false;

        public DisassembleShop()
            : this(new DisassembleShopSetting())
        { }

        public DisassembleShop(DisassembleShopSetting setting)
        { 
            RecordValueWithoutEscapeChar = setting.RecordValueWithoutEscapeChar;
            DiscernNumber = setting.DiscernNumber;
            DiscernNumberFirst = setting.DiscernNumberFirst;
        }

        public ArOutStringPartInfo[] Disassemble(string s, string[] reserved)
            => DisassembleStringFull(s, StringToPartInfoArray(reserved));
        public ArOutStringPartInfo[] Disassemble(string s, ArStringPartInfo[] reserved)
            => DisassembleStringFull(s, reserved);

        public static List<ArStringPartInfo> StringToPartInfoList(string[] reserved)
        {
            List<ArStringPartInfo> result = new List<ArStringPartInfo>();
            foreach (string s in reserved)
                result.Add(new ArStringPartInfo(s));
            return result;
        }

        public static ArStringPartInfo[] StringToPartInfoArray(string[] reserved)
            => StringToPartInfoList(reserved).ToArray();

        //[+-]?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?
        public static string CaptureNumberString(string s, ArNumberStringType numberStringType, out int length)
        {   
            length = 0;
            if (string.IsNullOrEmpty(s))
                return "";
            StringBuilder sb = new StringBuilder();
            if (s[length] == '+' || s[length] == '-')
            {
                if (numberStringType != ArNumberStringType.UnsignedInteger)
                {
                    sb.Append(s[length]);
                    length += 1;
                }
                else
                    return "";
            }

            if (s.Length == length || !char.IsDigit(s[length]))
                goto RF;

            while (s.Length != length && char.IsDigit(s[length]))
            {
                sb.Append(s[length]);
                length += 1;
            }

            if (s.Length == length || s.Length == length + 1 || numberStringType == ArNumberStringType.Integer ||
                numberStringType == ArNumberStringType.UnsignedInteger ||
                (s[length] != '.' && s[length] != 'e' && s[length] != 'E') ||
                (s[length] != '.' && numberStringType == ArNumberStringType.Decimal) ||
                (s[length] == '.' && !char.IsDigit(s[length + 1])))                
                return sb.ToString();
            else
            {
                if (s[length] == '.')
                {
                    sb.Append(s[length]);
                    length += 1;

                    while (s.Length != length && char.IsDigit(s[length]))
                    {
                        sb.Append(s[length]);
                        length += 1;
                    }
                }

                if (s.Length == length || s.Length == length + 1 ||
                    numberStringType == ArNumberStringType.Decimal ||
                    (s[length] != 'e' && s[length] != 'E') ||
                    ((s[length] == 'e' || s.Length == 'E') && (s[length + 1] == '+' || s[length + 1] == '-') && (s.Length == length + 2 || !char.IsDigit(s[length + 2]))) ||
                    ((s[length] == 'e' || s.Length == 'E') && (s[length + 1] != '+' && s[length + 1] != '-'  && !char.IsDigit(s[length + 1]))))
                    return sb.ToString();

                sb.Append(s[length]);
                length += 1;

                if (s[length] == '+' || s[length] == '-')
                {
                    sb.Append(s[length]);
                    length += 1;
                }

                while(s.Length != length && char.IsDigit(s[length]))
                {
                    sb.Append(s[length]);
                    length += 1;
                }

                return sb.ToString();
            }
        RF:
            length = 0;
            return "";
        }

        internal ArOutStringPartInfo[] DisassembleStringFull(string s, ArStringPartInfo[] reserved)
        {
            string s2;
            int i;
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException(nameof(s));

            List<ArOutStringPartInfo> result = new List<ArOutStringPartInfo>();
            while (s.Length != 0)
            {
                bool found = false;                
                if (DiscernNumber != ArNumberStringType.Undefined && DiscernNumberFirst)
                {
                   s2 = CaptureNumberString(s, DiscernNumber, out i);
                   if(i != 0)
                    {
                        result.Add(new ArOutStringPartInfo(i, "d", s2, ArStringPartType.Number));
                        s = s.Substring(i);
                        continue;
                    }
                }

                for (i = 0; i < reserved.Length; i++)
                {   
                    if (s.StartsWith(reserved[i].Value))                 
                    {
                        if (reserved[i].Type == ArStringPartType.Escape1)
                        {
                            if (s.Length < 2)
                                throw new FormatException(); //逃逸字元後面無字                            
                            result.Add(new ArOutStringPartInfo(i, reserved[i].Name, s.Substring(RecordValueWithoutEscapeChar ? 1 : 0,  RecordValueWithoutEscapeChar ? 1 : 2), ArStringPartType.Escape1));
                            s = s.Substring(reserved[i].Value.Length + 1);
                        }
                        else
                        {
                            result.Add(new ArOutStringPartInfo(i, reserved[i].Name, reserved[i].Value));
                            s = s.Substring(reserved[i].Value.Length);
                        }
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (DiscernNumber != ArNumberStringType.Undefined && !DiscernNumberFirst)
                    {
                        s2 = CaptureNumberString(s, DiscernNumber, out i);
                        if (i != 0)
                        {
                            result.Add(new ArOutStringPartInfo(i, "d", s2, ArStringPartType.Number));
                            s = s.Substring(i);
                            continue;
                        }
                    }

                    result.Add(new ArOutStringPartInfo(-1, "", s[0].ToString()));
                    s = s.Substring(1);
                }
            }

            return result.ToArray();
        }
    }
}
