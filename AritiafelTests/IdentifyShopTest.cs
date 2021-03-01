using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Locations;

namespace AritiafelTest
{
    [TestClass]
    public class IdentifyShopTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void RandomTest()
        {   
            
            TestContext.WriteLine(DateTime.Now.Ticks.ToString());
            TestContext.WriteLine(DateTime.Now.Ticks.ToString());
            TestContext.WriteLine(((int)DateTime.Now.Ticks).ToString());
            
            TestContext.WriteLine(DateTime.Now.Ticks.ToString());
            TestContext.WriteLine(((int)DateTime.Now.Ticks).ToString());
            
        }

        [TestMethod]
        public void GetNewID()
        {
            TestContext.WriteLine(IdentifyShop.GetNewID());
            TestContext.WriteLine(IdentifyShop.GetNewID());
            TestContext.WriteLine(IdentifyShop.GetNewID());
            TestContext.WriteLine(IdentifyShop.GetNewID());
            TestContext.WriteLine(IdentifyShop.GetNewID());
        }       
    }
}
