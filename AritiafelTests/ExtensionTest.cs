using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;
using Aritiafel.Locations.StorageHouse;
using System.Collections.Generic;
using Aritiafel;

namespace AritiafelTestFormTests
{
    [TestClass]
    public class ExtensionTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void RandomDoubleTest()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            rnd.NextRandomDouble();
            double d;
            for(int i = 0; i < 1000; i++)
                d = rnd.NextRandomDouble(double.MinValue / 10, double.MaxValue / 10);
            
            for (int i = 0; i < 1000; i++)
            {                
                d = rnd.NextRandomDouble(double.MinValue / Math.Pow(10, 100), double.MaxValue / Math.Pow(10, 100));
                if (i < 20)
                    TestContext.WriteLine(d.ToString());
            }
                

        }
    }
}
