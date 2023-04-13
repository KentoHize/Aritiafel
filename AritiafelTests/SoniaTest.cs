using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Locations;
using Aritiafel.Characters.Heroes;

namespace AritiafelTest
{
    [TestClass]
    public class SoniaTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void BackupGameSaveTest()
        {
            Sonia.BackupGameSave(@"C:\Users\kentn\OneDrive\ドキュメント", @"AliceSoft\ランス１０");
        }

        //Need
    }
}
