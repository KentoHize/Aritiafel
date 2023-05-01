using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Locations;
using Aritiafel.Items;

namespace AritiafelTest
{
    [TestClass]
    public class SettingShopTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AddAndSaveIni()
        {
            ArSettingGroup arSettingGroup = new ArSettingGroup();
            arSettingGroup.Add("aa", "bb", "Test");
            arSettingGroup.Add("Color", System.Drawing.Color.Firebrick, "Color");
            arSettingGroup.Add("dd", 3, "Test");
            arSettingGroup.Add("ff", 7, null, "this is ff\n and ff");
            arSettingGroup.Add("gg", 0.44);
            arSettingGroup.Add("hh", "ar");
            arSettingGroup.Add("ll", @"C:\BBB\DDD\rrr.txt");

            //arSettingGroup.Remove()
            SettingShop.SaveIniFile(arSettingGroup, @"C:\Programs\TestArea\asg.ini");
            //TestVar tv = new TestVar();
            SettingShop.LoadIniFile(typeof(TestVar), @"C:\Programs\TestArea\asg.ini");
            TestContext.WriteLine($"aa:{TestVar.aa}");
            TestContext.WriteLine($"dd:{TestVar.dd}");
            TestContext.WriteLine($"ff:{TestVar.ff}");
            TestContext.WriteLine($"gg:{TestVar.gg}");
            TestContext.WriteLine($"hh:{TestVar.hh}");
            TestContext.WriteLine($"ll:{TestVar.ll}");
            TestContext.WriteLine($"Color:{TestVar.Color}");
            //Field
        }

        [TestMethod]
        public void SaveIniFromStaticClass()
        {
            TestVar.aa = "sss";
            TestVar.dd = "dd";
            TestVar.ff = 222;
            TestVar.gg = 0.0444;
            TestVar.ll = @"C:\BBB\RRR\AAArr.txt";
            TestVar.Color = System.Drawing.Color.Azure;
            SettingShop.SaveIniFile(typeof(TestVar), @"C:\Programs\TestArea\tv.ini");
        }

        [TestMethod]
        public void SaveIniFromGeneralClass()
        {
            TestVar2 tv2 = new TestVar2();
            tv2.cc = "aadsd";
            tv2.dd = 2.4f;
            tv2.Color2 = System.Drawing.Color.DarkKhaki;
            SettingShop.SaveIniFile(tv2, @"C:\Programs\TestArea\tv2.ini");
        }
    }

    public static class TestVar
    {
        public static string aa { get; set; }
        public static string dd { get; set; }
        public static int ff { get; set; }
        public static double gg { get; set; }
        public static string hh { get; set; }
        public static string ll { get; set; }
        public static System.Drawing.Color Color { get; set; }
    }

    public class TestVar2
    {
        public string cc { get; set; }
        public float dd { get; set; }
        public System.Drawing.Color Color2 { get; set; }
    }
}