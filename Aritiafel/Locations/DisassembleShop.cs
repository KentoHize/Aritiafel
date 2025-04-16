using Aritiafel.Definitions;
using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//拆解字串，提供初步分析
//AA, BB, CC, D1
//BBBCCD22D1AA
//=>  1 -1 2 -1 -1 -1 3 0
//=> BB B CC D 2 2 D1 AA

//合拆
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
        public bool ErrorOccurIfNoMatch { get; set; }

        public DisassembleShop()
            : this(new DisassembleShopSetting())
        { }

        public DisassembleShop(DisassembleShopSetting setting)
        {
            RecordValueWithoutEscapeChar = setting.RecordValueWithoutEscapeChar;
            ErrorOccurIfNoMatch = setting.ErrorOccurIfNoMatch;
        }

        public ArOutPartInfoList Disassemble(string s, string[] reserved)
            => DisassembleStringFull(s, new ArDisassembleInfo(StringToPartInfoArray(reserved), null));
        //public ArOutPartInfoList Disassemble(string s, ArStringPartInfo[] reserved)
        //    => DisassembleStringFull(s, new ArDisassembleInfo(reserved, null));
        public ArOutPartInfoList Disassemble(string s, ArPartInfo[] reserved)
            => DisassembleStringFull(s, new ArDisassembleInfo(reserved, null));
        public ArOutPartInfoList Disassemble(string s, ArPartInfo[] reserved, ArContainerPartInfo[] containers)
            => DisassembleStringFull(s, new ArDisassembleInfo(reserved, containers));
        public ArOutPartInfoList Disassemble(string s, ArPartInfo[][] reserved, ArContainerPartInfo[] containers)
            => DisassembleStringFull(s, new ArDisassembleInfo(reserved, containers));
        public ArOutPartInfoList Disassemble(string s, ArDisassembleInfo dissambleInfo)
            => DisassembleStringFull(s, dissambleInfo);

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

        internal ArOutPartInfoList DisassembleStringFull(string s, ArDisassembleInfo di)
        {
            string s2, containerEndString = "";
            int i, reservedStringsIndex = 0; //-1 = No Reserved
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException(nameof(s));

            ArOutPartInfoList target = new ArOutPartInfoList();
            Stack<ArOutPartInfoList> containers = new Stack<ArOutPartInfoList>();
            Stack<int> reservedIndexes = new Stack<int>();
            while (s.Length != 0)
            {
                bool found = false;
                for (i = 0; i < di.ContainerPartInfo.Length; i++)
                {   
                    if (containerEndString != "" && s.StartsWith(containerEndString))
                    {
                        target = containers.Pop();
                        reservedStringsIndex = reservedIndexes.Pop();
                        s = s.Substring(containerEndString.Length);
                        containerEndString = target.EndString;
                        found = true;
                        break;
                    }
                    else if (s.StartsWith(di.ContainerPartInfo[i].StartString))
                    {
                        ArOutPartInfoList newList = new ArOutPartInfoList(null,
                            di.ContainerPartInfo[i].Name, di.ContainerPartInfo[i].StartString,
                            di.ContainerPartInfo[i].EndString);
                        reservedIndexes.Push(reservedStringsIndex);
                        reservedStringsIndex = di.ContainerPartInfo[i].ReservedStringInfoIndex;
                        containerEndString = di.ContainerPartInfo[i].EndString;
                        target.Value.Add(newList);
                        containers.Push(target);
                        target = newList;
                        s = s.Substring(di.ContainerPartInfo[i].StartString.Length);
                        found = true;
                        break;
                    }
                }
                
                if (reservedStringsIndex != -1 && !found)
                {
                    for (i = 0; i < di.ReservedStringInfo[reservedStringsIndex].Length; i++)
                    {   
                        if(di.ReservedStringInfo[reservedStringsIndex][i] is ArStringPartInfo spi)
                        {
                            if (spi.Times == 0)
                                continue;

                            if (!string.IsNullOrEmpty(spi.Value) && s.StartsWith(spi.Value))
                            {
                                if (spi.Type == ArStringPartType.Escape1)
                                {
                                    if (s.Length < 2)
                                        throw new FormatException(); //逃逸字元後面無字
                                    if (RecordValueWithoutEscapeChar)
                                        target.Value.Add(new ArOutStringPartInfo(i, spi.Name, s.Substring(1, 1), ArStringPartType.Escape1));
                                    else
                                        target.Value.Add(new ArOutStringPartInfo(i, spi.Name, s.Substring(0, 2), ArStringPartType.Escape1));
                                    s = s.Substring(spi.Value.Length + 1);
                                }
                                else if (spi.Type == ArStringPartType.Normal)
                                {
                                    target.Value.Add(new ArOutStringPartInfo(i, spi.Name, spi.Value));
                                    s = s.Substring(spi.Value.Length);
                                }
                                else
                                    throw new NotImplementedException();
                                found = true;
                                if (spi.Times > 0)
                                    spi.Times--;
                                break;
                            }
                            else if (spi.Type == ArStringPartType.UnsignedInteger ||
                                spi.Type == ArStringPartType.Integer ||
                                spi.Type == ArStringPartType.Decimal ||
                                spi.Type == ArStringPartType.ScientificNotation)
                            {
                                s2 = CaptureNumberString(s, (ArNumberStringType)spi.Type, spi.MaxLength, out int j);
                                if (j != 0)
                                {
                                    target.Value.Add(new ArOutStringPartInfo(i, spi.Name, s2, spi.Type));
                                    s = s.Substring(j);
                                    found = true;
                                    if (spi.Times > 0)
                                        spi.Times--;
                                    break;
                                }
                            }
                        }
                        else if (di.ReservedStringInfo[reservedStringsIndex][i] is ArGroupStringPartInfo gspi)
                        {
                            if (gspi.Times == 0)
                                continue;

                            for (int j = 0; j < gspi.Value.Length; j++)
                            {
                                if (s.StartsWith(gspi.Value[j]))
                                {   
                                    target.Value.Add(new ArOutStringPartInfo(i, gspi.Name, gspi.Value[j]));
                                    found = true;
                                    if (gspi.Times > 0)
                                        gspi.Times--;
                                    s = s.Substring(gspi.Value[j].Length);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (!found)
                {
                    if (ErrorOccurIfNoMatch)
                        throw new KeyNotFoundException($"{s}");
                    target.Value.Add(new ArOutStringPartInfo(-1, "", s[0].ToString(), ArStringPartType.Char));
                    s = s.Substring(1);
                }
            }
            return target;
        }
    }
}
