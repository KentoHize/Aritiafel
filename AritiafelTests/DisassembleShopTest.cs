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

        internal ArStringPartInfo[] CreateReversedString()
        {
            List<ArStringPartInfo> result;
            DisassembleShop ds = new DisassembleShop();
            result = DisassembleShop.StringToPartInfoList(SortedAllCustomFormatString);
            result.Insert(0, new ArStringPartInfo("EscapeChar", "%", ArStringPartType.Escape1));
            result.Insert(0, new ArStringPartInfo("EscapeChar", "\\", ArStringPartType.Escape1));
            return result.ToArray();
        }

        [TestMethod]
        public void DissaembleTest()
        {
            string[] testString = { 
                "\\n\\d\\e\\r",
                "%d%r%f%g"
            };

            ArStringPartInfo[] re2 = CreateReversedString();
            DisassembleShop ds = new DisassembleShop();
            for (int i = 0; i < testString.Length; i++)
            {   
                ArOutStringPartInfo[] result = ds.Disassemble(testString[i], re2);
                foreach (var item in result)
                {
                    TestContext.Write($"{item.Value}-");
                }
                TestContext.WriteLine("");
            }
        }

        [TestMethod]
        public void DissaembleCultureInfo()
        {   
            ArStringPartInfo[] re2 = CreateReversedString();

            CultureInfo ci = CultureInfo.GetCultureInfo("zh-TW");
            DisassembleShop ds = new DisassembleShop();
            for (int i = 0; i < AllStandardFormatChar.Length; i++)
            {
                TestContext.WriteLine($"{AllStandardFormatChar[i]}:");
                string[] sa = ci.DateTimeFormat.GetAllDateTimePatterns(AllStandardFormatChar[i]);
                foreach (var item in sa)
                {
                    ArOutStringPartInfo[] result = ds.Disassemble(item.ToString(), re2);
                    foreach (var item2 in result)
                    {   
                        TestContext.Write($"{item2.Value}-");
                    }
                    TestContext.WriteLine("");
                }
            }
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
