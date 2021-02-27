using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Locations;
using Aritiafel.Characters.Heroes;

namespace AritiafelTestFormTests
{   
    [TestClass]
    public class TinaTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SaveProject()
        {
            Tina.SaveProject();
        }
    }
}
