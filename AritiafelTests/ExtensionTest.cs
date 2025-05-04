using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Aritiafel;

namespace AritiafelTest
{
    [TestClass]
    public class ExtensionTest
    {
        public TestContext TestContext { get; set; }


        enum HeroStyle
        {
            Fast,
            Cool,
            Power
        }

        [TestMethod]
        public void EnumStringTest()
        {
            Dictionary<string, string> map = new Dictionary<string, string>
            {
                { "Fast", "快速" },
                { "Cool", "酷"}
            };
            map.RecordEnumStringValue();

            map = new Dictionary<string, string>
            {
                { "Cool", "超酷" }
            };
            map.RecordEnumStringValue();
            TestContext.WriteLine(HeroStyle.Fast.ToArString());
            TestContext.WriteLine(HeroStyle.Cool.ToArString());
            TestContext.WriteLine(HeroStyle.Power.ToArString());
        }

        [TestMethod]
        public void ChineseCharacterTest()
        {
            Assert.IsFalse('a'.IsChinese());
            Assert.IsTrue('中'.IsChinese());

            Assert.IsFalse("aaa".ContainChinese());
            Assert.IsTrue("a人a".ContainChinese());
        }

    }
}
