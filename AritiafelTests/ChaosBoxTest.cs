using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Aritiafel.Artifacts;

namespace AritiafelTestFormTests
{
    [TestClass]
    public class ChaosBoxTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void NewChaosBox()
        {
            var ChaosBox = new ChaosBox();

            TestContext.WriteLine((3 << 2).ToString());
        }

        [TestMethod]
        public void SimpleTest()
        {
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10; i++)
                TestContext.WriteLine(cb.DrawOutDouble().ToString());
            for (int i = 0; i < 10; i++)
                TestContext.WriteLine(cb.DrawOutDecimalInteger().ToString());
        }

        [TestMethod]
        public void DrawOutByteTest()
        {
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10; i++)
                TestContext.WriteLine(cb.DrawOutByte().ToString());
            TestContext.WriteLine("----------");
            byte[] bytes = cb.DrawOutBytes(30);
            for (int i = 0; i < bytes.Length; i++)
                TestContext.WriteLine(bytes[i].ToString());
        }


        [TestMethod]
        public void SimpleTest2()
        {
            TestContext.WriteLine(decimal.MaxValue.ToString().Length.ToString());
            return;
            ChaosBox cb = new ChaosBox();
            decimal a = cb.DrawOutDecimalInteger();
            decimal c = new decimal(30, 60, 0, false, 0);
            TestContext.WriteLine("30, 60, 0");
            TestContext.WriteLine(c.ToString());
            TestContext.WriteLine(c.ToString().Length.ToString());
            c = new decimal(30, 60, 1, false, 0);
            TestContext.WriteLine("30, 60, 1");
            TestContext.WriteLine(c.ToString());
            TestContext.WriteLine(c.ToString().Length.ToString());
            c = new decimal(30, 60, 2, false, 0);
            TestContext.WriteLine("30, 60, 2");
            TestContext.WriteLine(c.ToString());
            TestContext.WriteLine(c.ToString().Length.ToString());
            c = new decimal(30, 60, 5, false, 0);
            TestContext.WriteLine("30, 60, 5");
            TestContext.WriteLine(c.ToString());
            TestContext.WriteLine(c.ToString().Length.ToString());
            c = new decimal(0, 60, 5, false, 0);
            TestContext.WriteLine("0, 60, 5");
            TestContext.WriteLine(c.ToString());
            TestContext.WriteLine(c.ToString().Length.ToString());
            c = new decimal(30, 60, 50, false, 0);
            TestContext.WriteLine("30, 60, 50");
            TestContext.WriteLine(c.ToString());
            TestContext.WriteLine(c.ToString().Length.ToString());
            int[] b = decimal.GetBits(c);
            for (int i = 0; i < b.Length; i++)
                TestContext.WriteLine(b[i].ToString());
            
        }
    }
}
