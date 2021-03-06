using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Organizations.RaeriharUniversity;
using Aritiafel.Artifacts;

namespace AritiafelTest
{
    [TestClass]
    public class RaeriharUniversityTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void GetStandardNumberStringTest()
        {
            string result;
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 1000; i++)
            {
                double d = cb.DrawOutDiversityDouble();
                if (string.IsNullOrEmpty(Mathematics.GetStandardNumberString(d.ToString())))
                    TestContext.WriteLine(d.ToString());
            }

            string testString = "-0.00589662145E-103";
            string testString2 = "-0.03611895678E+51";
            string testString3 = "0000.00068718";
            string testString4 = "35678943580000000000000000000000000000000";
            string testString5 = "+568.681E-8";
            result = Mathematics.GetStandardNumberString(testString);
            TestContext.WriteLine(result);
            result = Mathematics.GetStandardNumberString(testString2);
            TestContext.WriteLine(result);
            result = Mathematics.GetStandardNumberString(testString3);
            TestContext.WriteLine(result);
            result = Mathematics.GetStandardNumberString(testString4);
            TestContext.WriteLine(result);
            result = Mathematics.GetStandardNumberString(testString5);
            TestContext.WriteLine(result);
        }

        [TestMethod]
        public void GetIntegerDigitsCountTest()
        {
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                double a = cb.DrawOutDiversityDouble();
                TestContext.WriteLine(Mathematics.GetIntegerDigitsCount(a.ToString()).ToString());
                //a = -0.003213213;
                //if (Math.Abs(a).ToString().Length - 1 != cb.GetNumberStringPowOf10(a.ToString()))
                //    TestContext.WriteLine($"{a}:{cb.GetNumberStringPowOf10(a.ToString())}");
                //TestContext.WriteLine($"{a}:{cb.GetNumberStringPowOf10(a.ToString())}");
            }
        }

    }
}
