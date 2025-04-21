using Aritiafel.Characters.Heroes;
using Aritiafel.Definitions;
using Aritiafel.Items;
using System;
using System.Collections.Generic;
using System.Text;
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
        public bool RemoveLimitedReservedStringIfNoMatch { get; set; }

        public DisassembleShop()
            : this(new DisassembleShopSetting())
        { }

        public DisassembleShop(DisassembleShopSetting setting)
        {
            RecordValueWithoutEscapeChar = setting.RecordValueWithoutEscapeChar;
            ErrorOccurIfNoMatch = setting.ErrorOccurIfNoMatch;
            RemoveLimitedReservedStringIfNoMatch = setting.RemoveLimitedReservedStringIfNoMatch;
        }

        public ArOutPartInfoList Disassemble(string s, string[] reserved)
            => DisassembleStringFull(s, new ArDisassembleInfo(StringToPartInfoArray(reserved), null));
        //public ArOutPartInfoList Disassemble(string s, ArStringPartInfo[] reserved)
        //    => DisassembleStringFull(s, new ArDisassembleInfo(reserved, null));
        public ArOutPartInfoList Disassemble(string s, ArStringPartInfo[] reserved)
            => DisassembleStringFull(s, new ArDisassembleInfo(reserved, null));
        public ArOutPartInfoList Disassemble(string s, ArStringPartInfo[] reserved, ArContainerPartInfo[] containers)
            => DisassembleStringFull(s, new ArDisassembleInfo(reserved, containers));
        public ArOutPartInfoList Disassemble(string s, ArStringPartInfo[][] reserved, ArContainerPartInfo[] containers)
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
        internal static string CaptureNumberString(string s, ArNumberStringType numberStringType, int maxLength, out int length)
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
            //Sophia.SeeThrough(di.ReservedStringInfo);
            string s2, containerEndString = "";
            int index = 0;
            int i, reservedStringsIndex = 0; //-1 = No Reserved
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException(nameof(s));

            ArOutPartInfoList target = new ArOutPartInfoList();
            Stack<ArOutPartInfoList> containers = new Stack<ArOutPartInfoList>();
            Stack<int> reservedIndexes = new Stack<int>();
            while (index < s.Length)
            {
                bool found = false;
                for (i = 0; i < di.ContainerPartInfo.Length; i++)
                {
                    if (containerEndString != "" && containerEndString.MatchString(s, index))
                    {
                        target = containers.Pop();
                        reservedStringsIndex = reservedIndexes.Pop();
                        index += containerEndString.Length;
                        containerEndString = target.EndString;
                        found = true;
                        break;
                    }
                    else if (di.ContainerPartInfo[i].StartString.MatchString(s, index))
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
                        index += di.ContainerPartInfo[i].StartString.Length;
                        found = true;
                        break;
                    }
                }

                if (reservedStringsIndex != -1 && !found) //-1指完全不核對
                {
                    for (i = 0; i < di.ReservedStringInfo[reservedStringsIndex].Length; i++)
                    {   
                        if (di.ReservedStringInfo[reservedStringsIndex][i].Times == 0)
                            continue;

                        if (di.ReservedStringInfo[reservedStringsIndex][i].Type == ArStringPartType.UnsignedInteger ||
                                di.ReservedStringInfo[reservedStringsIndex][i].Type == ArStringPartType.Integer ||
                                di.ReservedStringInfo[reservedStringsIndex][i].Type == ArStringPartType.Decimal ||
                                di.ReservedStringInfo[reservedStringsIndex][i].Type == ArStringPartType.ScientificNotation)
                        {
                            s2 = CaptureNumberString(s.Substring(index), (ArNumberStringType)di.ReservedStringInfo[reservedStringsIndex][i].Type, di.ReservedStringInfo[reservedStringsIndex][i].MaxLength, out int k);
                            if (k != 0)
                            {
                                target.Value.Add(new ArOutStringPartInfo(i, di.ReservedStringInfo[reservedStringsIndex][i].Name, s2, di.ReservedStringInfo[reservedStringsIndex][i].Type));
                                index += k;
                                found = true;
                                if (di.ReservedStringInfo[reservedStringsIndex][i].Times > 0)
                                    di.ReservedStringInfo[reservedStringsIndex][i].Times--;
                                break;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < di.ReservedStringInfo[reservedStringsIndex][i].Values.Length; j++)
                            {
                                if (di.ReservedStringInfo[reservedStringsIndex][i][j].MatchString(s, index))
                                {
                                    if (di.ReservedStringInfo[reservedStringsIndex][i].Type == ArStringPartType.Normal)
                                    {
                                        target.Value.Add(new ArOutStringPartInfo(i, di.ReservedStringInfo[reservedStringsIndex][i].Name, di.ReservedStringInfo[reservedStringsIndex][i][j], ArStringPartType.Normal, j));
                                        index += di.ReservedStringInfo[reservedStringsIndex][i][j].Length;
                                    }
                                    else if (di.ReservedStringInfo[reservedStringsIndex][i].Type == ArStringPartType.Escape1)
                                    {
                                        if (s.Length - index < 2)
                                            throw new FormatException(); //逃逸字元後面無字
                                        if (RecordValueWithoutEscapeChar)
                                            target.Value.Add(new ArOutStringPartInfo(i, di.ReservedStringInfo[reservedStringsIndex][i].Name, s.Substring(index + 1, 1), ArStringPartType.Escape1, j));
                                        else
                                            target.Value.Add(new ArOutStringPartInfo(i, di.ReservedStringInfo[reservedStringsIndex][i].Name, s.Substring(index, 2), ArStringPartType.Escape1, j));
                                        index += di.ReservedStringInfo[reservedStringsIndex][i][j].Length + 1;
                                    }
                                    else
                                        throw new NotImplementedException();
                                    found = true;
                                    if (di.ReservedStringInfo[reservedStringsIndex][i].Times > 0)
                                        di.ReservedStringInfo[reservedStringsIndex][i].Times--;
                                    break;
                                }
                            }
                        }   
                        if (found)
                            break;
                        if (RemoveLimitedReservedStringIfNoMatch && di.ReservedStringInfo[reservedStringsIndex][i].Times != -1) //Not Fonud
                            di.ReservedStringInfo[reservedStringsIndex][i].Times = 0;
                    }
                }
                if (!found)
                {
                    if (ErrorOccurIfNoMatch)
                        throw new KeyNotFoundException($"{s.Substring(index)}");
                    target.Value.Add(new ArOutStringPartInfo(-1, "", s[index].ToString(), ArStringPartType.Char));
                    index += 1;
                }
            }
            return target;
        }
    }
}
