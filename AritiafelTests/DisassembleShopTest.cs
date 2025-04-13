using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Locations;
using Aritiafel.Characters.Heroes;
using Aritiafel.Items;
using System.Globalization;
using static AritiafelTest.RaeriharUniversityTest;
using Aritiafel.Definitions;
using Aritiafel.Artifacts;
using System.Net;

namespace AritiafelTest
{
    [TestClass]
    public class DisassembleShopTest
    {
        internal static readonly char[] AllStandardFormatChar =
        {
            'd', 'D', 'f', 'F', 'g', 'G', 'm', 'M', 'o', 'O',
            'r', 'R', 's', 't', 'T', 'u', 'U', 'y', 'Y'
        };

        internal static readonly string[] SortedAllCustomFormatString =
        {
            ":", "/", "hh", "HH", "mm", "ss", "h", "H", "m", "s",
            "dddd", "ddd", "dd", "yyyyy", "yyyy", "yyy", "yy",
            "MMMM", "MMM", "MM", "y", "M", "d",
            "fffffff", "ffffff", "fffff", "ffff", "fff", "ff", "f",
            "tt", "t", "gg", "g", "zzz", "zz", "z", "K", "FFFFFFF", "FFFFFF",
            "FFFFF", "FFFF", "FFF", "FF", "F"
        };

        List<TestDateTime> dateTimes = new List<TestDateTime> {
                new TestDateTime(1, 1, 1, 0, 0, 0),  new TestDateTime(400, 1, 1, 0 ,0, 0, 1),
                new TestDateTime(401, 1, 1, 12, 20, 10, 20), new TestDateTime(402, 2, 5, 13, 2, 2),
                new TestDateTime(2020, 1, 1, 23, 59, 59, 875), new TestDateTime(2120, 3, 3, 0, 0, 1),
                new TestDateTime(2120, 4, 10, 3, 39, 59), new TestDateTime(2320, 5, 7, 20, 2, 0),
                new TestDateTime(2120, 11, 10, 10, 10, 0), new TestDateTime(3320, 12, 15, 3, 29, 49),
                new TestDateTime(4400, 2, 29), new TestDateTime(4400, 3, 1),
                new TestDateTime(399, 12, 31), new TestDateTime(3, 12, 31),
                TestDateTime.Now, new TestDateTime(new DateTime(2000, 1, 1).AddTicks(-1).Ticks, false),
            };

        public TestContext TestContext { get; set; }

        internal ArStringPartInfo[] CreateReversedString(bool analyzeInteger = false)
        {
            List<ArStringPartInfo> result;
            result = DisassembleShop.StringToPartInfoList(SortedAllCustomFormatString);
            result.Insert(0, new ArStringPartInfo("p", "%", ArStringPartType.Escape1));
            result.Insert(0, new ArStringPartInfo("bs", "\\\\", ArStringPartType.Escape1));
            if (analyzeInteger)
            {
                result.Insert(0, new ArStringPartInfo("n2", "", ArStringPartType.Integer, 2));
                result.Insert(0, new ArStringPartInfo("n4", "", ArStringPartType.Integer, 4));
            }
            return result.ToArray();
        }

        [TestMethod]
        public void DissaembleTimesTest()
        {   
            //ChaosBox cb = new ChaosBox();
            

            //for (int i = 0; i < 200; i++)
            //{
            //    List<ArStringPartInfo> re2 = new List<ArStringPartInfo>();
            //    re2.Add(new ArStringPartInfo("n4", "", ArStringPartType.Integer, 4, 1));
            //    re2.Add(new ArStringPartInfo("n2", "", ArStringPartType.Integer, 2, 2));
            //    re2.Add(new ArStringPartInfo("n3", "", ArStringPartType.Integer, 2, 1));
            //    re2.Add(new ArStringPartInfo("dc", "", ArStringPartType.Decimal, 0, 1));
            //    int a = 0;
            //    StringBuilder sb = new StringBuilder();
            //    while (a != 4)
            //    {
            //        if (a == 0)
            //            sb.Append(' ');
            //        else if (a == 1)
            //            sb.Append(cb.DrawOutDiversityDouble().ToString());
            //        else if (a == 2)
            //            sb.Append(cb.DrawOutDiversityDouble().ToString());
            //        else if (a == 3)
            //            sb.Append((char)cb.DrawOutByte());
            //        if (sb[sb.Length - 1] == '\\' || sb[sb.Length - 1] == '%')
            //            sb.Remove(sb.Length - 1, 1);
            //        a = cb.DrawOutByte(4);
            //    }


            //    DisassembleShop ds = new DisassembleShop();
            //    string testString = sb.ToString();
            //    TestContext.WriteLine(testString);
            //    ArOutStringPartInfo[] result = ds.Disassemble(testString, re2.ToArray());
            //    foreach (var item in result)
            //    {
            //        TestContext.Write($"{item.Value}");
            //        if (item.Type == ArStringPartType.Escape1)
            //            TestContext.Write("(e)");
            //        else if (item.Type != ArStringPartType.Char)
            //            TestContext.Write($"({item.Name})");
            //        TestContext.Write($"-");
            //    }
            //    TestContext.WriteLine("");
            //}
        }

        [TestMethod]
        public void DissaembleTest()
        {
            //    string[] testString = { 
            //        "\\n\\d23e\\e\\r",
            //        "%d%r%f%g"
            //    };

            //產生隨機測試文字
            //ChaosBox cb = new ChaosBox();
            //ArStringPartInfo[] re2 = CreateReversedString(true);

            //for (int i = 0; i < 200; i++)
            //{
            //    int a = 0;
            //    StringBuilder sb = new StringBuilder();
            //    while (a != 4)
            //    {   
            //        if (a == 0)
            //            sb.Append(' ');
            //        else if (a == 1)
            //            sb.Append(cb.DrawOutFromArray(re2).Value);
            //        else if (a == 2)
            //            sb.Append(cb.DrawOutDiversityDouble().ToString());
            //        else if (a == 3)
            //            sb.Append((char)cb.DrawOutByte());
            //        if (sb[sb.Length - 1] == '\\' || sb[sb.Length - 1] == '%')
            //            sb.Remove(sb.Length - 1, 1);
            //        a = cb.DrawOutByte(4);
            //    }
                

            //    DisassembleShop ds = new DisassembleShop();
            //    string testString = sb.ToString();
            //    TestContext.WriteLine(testString);
            //    ArOutStringPartInfo[] result = ds.Disassemble(testString, re2);
            //    foreach (var item in result)
            //    {
            //        TestContext.Write($"{item.Value}");
            //        if (item.Type == ArStringPartType.Escape1)
            //            TestContext.Write("(e)");
            //        else if (item.Type != ArStringPartType.Char)
            //            TestContext.Write($"({item.Name})");
            //        TestContext.Write($"-");
            //    }
            //    TestContext.WriteLine("");
            //}
        }

        [TestMethod]
        public void CaptureNumberStringTest()
        {
            string[] testString = { "a","e45e", "+", "-e", "+3", "-4", "5d", "-20f",
                "6.d", "-7.e", "2.333", "-4.44ef", "6.2e", "-7.2e+", "0.3e+1", "0.2e-4",
                "0.4e1", "4e1", "5dfffe.1", "2.55e+300", "2e+401a", "2e-356", "3e-.33",
                "-0.63e+500", "99..3e+5", "98.84e+30.52e" };
            int length;
            string s;
            for (int i = 0; i < testString.Length; i++)
            {
                TestContext.WriteLine($"\"{testString[i]}\"：");
                s = DisassembleShop.CaptureNumberString(testString[i], ArNumberStringType.UnsignedInteger, 4, out length);
                TestContext.WriteLine($"UI:\"{s}\"");
                Assert.AreEqual(s.Length, length);

                s = DisassembleShop.CaptureNumberString(testString[i], ArNumberStringType.Integer, 4,out length);
                TestContext.WriteLine($" I:\"{s}\"");
                Assert.AreEqual(s.Length, length);

                s = DisassembleShop.CaptureNumberString(testString[i], ArNumberStringType.Decimal, 4,out length);
                TestContext.WriteLine($" D:\"{s}\"");
                Assert.AreEqual(s.Length, length);

                s = DisassembleShop.CaptureNumberString(testString[i], ArNumberStringType.ScientificNotation, 9, out length);
                TestContext.WriteLine($"SN:\"{s}\"");
                Assert.AreEqual(s.Length, length);

                TestContext.WriteLine("");
            }
        }

        [TestMethod]
        public void DissaembleCultureInfo()
        {
            //ArStringPartInfo[] re2 = CreateReversedString();

            //CultureInfo ci = CultureInfo.GetCultureInfo("zh-TW");
            //DisassembleShop ds = new DisassembleShop();
            //for (int i = 0; i < AllStandardFormatChar.Length; i++)
            //{
            //    TestContext.WriteLine($"{AllStandardFormatChar[i]}:");
            //    string[] sa = ci.DateTimeFormat.GetAllDateTimePatterns(AllStandardFormatChar[i]);
            //    foreach (var item in sa)
            //    {
            //        ArOutPartInfoList result = ds.Disassemble(item.ToString(), re2);
            //        foreach (var item2 in result.Value)
            //        {
            //            TestContext.Write($"{item2.Value}-");
            //        }
            //        TestContext.WriteLine("");
            //    }
            //}
        }


        [TestMethod]
        public void TestArea()
        {
            TestContext.WriteLine(SortedAllCustomFormatString.Length.ToString());
            //TestContext.WriteLine(DateTime.Now.ToString("\\\\dddd"));
            //TestContext.WriteLine(DateTime.Now.ToString("ddd"));
            //TestContext.WriteLine(DateTime.Now.ToString("dd"));
            //TestContext.WriteLine(DateTime.Now.ToString("d"));
            //TestContext.WriteLine(DateTime.Now.ToString("%dddd"));
            //TestContext.WriteLine(DateTime.Now.ToString("%yyyyy"));
            //TestContext.WriteLine(DateTime.Now.ToString("\\nHH"));
            //TestContext.WriteLine(DateTime.Now.ToString("\\HHH"));
        }

    }
}
