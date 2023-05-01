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
        public void AddSetting()
        {
            ArSettingGroup arSettingGroup = new ArSettingGroup();
            arSettingGroup.Add("aa", "bb", "Test");
            arSettingGroup.Add("dd", 3, "Test");
            arSettingGroup.Add("Color", System.Drawing.Color.Aqua, "Color");
            
            //arSettingGroup.Remove()
            SettingShop.SaveIniFile(arSettingGroup, @"C:\Programs\TestArea\asg.ini");
        }
    }
}