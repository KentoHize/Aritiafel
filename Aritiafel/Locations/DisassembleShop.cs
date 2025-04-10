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
        //public ArNumberStringType DiscernNumber { get; set; }
        //public int DiscernNumberMaxLength { get; set; }
        //public bool DiscernNumberFirst { get; set; }

        public DisassembleShop()
            : this(new DisassembleShopSetting())
        { }

        public DisassembleShop(DisassembleShopSetting setting)
        {
            RecordValueWithoutEscapeChar = setting.RecordValueWithoutEscapeChar;            
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
        public static string CaptureNumberString(string s, ArNumberStringType numberStringType, int maxLength, out int length)
        {
            length = 0;
            if (string.IsNullOrEmpty(s) || maxLength < 0)
                return "";
            if (maxLength == 0 || maxLength > s.Length)
                maxLength = s.Length;
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

            if (length >= maxLength || !char.IsDigit(s[length]))
                goto RF;

            while (length < maxLength && char.IsDigit(s[length]))
            {
                sb.Append(s[length]);
                length += 1;
            }

            if ((length + 1 >= maxLength) || numberStringType == ArNumberStringType.Integer ||
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

                    while (length < maxLength && char.IsDigit(s[length]))
                    {
                        sb.Append(s[length]);
                        length += 1;
                    }
                }

                if (length + 1 >= maxLength ||
                    numberStringType == ArNumberStringType.Decimal ||
                    (s[length] != 'e' && s[length] != 'E') ||
                    ((s[length] == 'e' || s.Length == 'E') && (s[length + 1] == '+' || s[length + 1] == '-') && (length + 2 >= maxLength || !char.IsDigit(s[length + 2]))) ||
                    ((s[length] == 'e' || s.Length == 'E') && (s[length + 1] != '+' && s[length + 1] != '-' && !char.IsDigit(s[length + 1]))))
                    return sb.ToString();

                sb.Append(s[length]);
                length += 1;

                if (s[length] == '+' || s[length] == '-')
                {
                    sb.Append(s[length]);
                    length += 1;
                }

                while (length < maxLength && char.IsDigit(s[length]))
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
            int i, j;
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException(nameof(s));

            List<ArOutStringPartInfo> result = new List<ArOutStringPartInfo>();
            while (s.Length != 0)
            {
                bool found = false;
                for (i = 0; i < reserved.Length; i++)
                {
                    if (reserved[i].Times == 0)
                        continue;

                    if (!string.IsNullOrEmpty(reserved[i].Value) && s.StartsWith(reserved[i].Value))
                    {
                        if (reserved[i].Type == ArStringPartType.Escape1)
                        {
                            if (s.Length < 2)
                                throw new FormatException(); //逃逸字元後面無字
                            if (RecordValueWithoutEscapeChar)
                                result.Add(new ArOutStringPartInfo(i, reserved[i].Name, s.Substring(1, 1), ArStringPartType.Escape1));
                            else
                                result.Add(new ArOutStringPartInfo(i, reserved[i].Name, s.Substring(0, 2), ArStringPartType.Escape1));
                            s = s.Substring(reserved[i].Value.Length + 1);
                        }
                        else if (reserved[i].Type == ArStringPartType.Normal)
                        {
                            result.Add(new ArOutStringPartInfo(i, reserved[i].Name, reserved[i].Value));
                            s = s.Substring(reserved[i].Value.Length);
                        }
                        else
                            throw new NotImplementedException();
                        found = true;
                    }
                    else if (reserved[i].Type == ArStringPartType.UnsignedInteger ||
                        reserved[i].Type == ArStringPartType.Integer ||
                        reserved[i].Type == ArStringPartType.Decimal ||
                        reserved[i].Type == ArStringPartType.ScientificNotation)
                    {
                        s2 = CaptureNumberString(s, (ArNumberStringType)reserved[i].Type, reserved[i].MaxLength, out j);
                        if (j != 0)
                        {
                            result.Add(new ArOutStringPartInfo(i, reserved[i].Name, s2, reserved[i].Type));
                            s = s.Substring(j);
                            found = true;                            
                        }
                    }

                    if(found)
                    {
                        if (reserved[i].Times > 0)
                            reserved[i].Times -= 1;
                        break;
                    }
                }

                if(!found)
                {
                    result.Add(new ArOutStringPartInfo(-1, "", s[0].ToString(), ArStringPartType.Char));
                    s = s.Substring(1);
                }
            }
            return result.ToArray();
        }
    }
}
