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
            //for (int i = 0; i < 1000; i++)
            //{
            //    double d = cb.DrawOutDiversityDouble();
            //    if (string.IsNullOrEmpty(Mathematics.GetStandardNumberString(d.ToString())))
            //        TestContext.WriteLine(d.ToString());
            //}
            string testString = "+300";
            //string testString = "-0.00589662145E-103";
            string testString2 = "-0.03611895678E+51";
            string testString3 = "-0000.00068718";
            string testString4 = "35678943580000000000000000000000000000000";
            string testString5 = "+568.681E-8";
            string testString6 = "-.00035E-20";
            string testString7 = "0.00";
            string testString8 = "-0.000";
            string testString9 = "+300";
            string testString10 = "-0.008";
            string testString11 = "0.00681E+98";
            result = Mathematics.GetStandardNumberString(testString);
            TestContext.WriteLine($"{testString}:{result}");
            result = Mathematics.GetStandardNumberString(testString2);
            TestContext.WriteLine($"{testString2}:{result}");
            result = Mathematics.GetStandardNumberString(testString3);
            TestContext.WriteLine($"{testString3}:{result}");
            result = Mathematics.GetStandardNumberString(testString4);
            TestContext.WriteLine($"{testString4}:{result}");
            result = Mathematics.GetStandardNumberString(testString5);
            TestContext.WriteLine($"{testString5}:{result}");
            result = Mathematics.GetStandardNumberString(testString6);
            TestContext.WriteLine($"{testString6}:{result}");
            result = Mathematics.GetStandardNumberString(testString7);
            TestContext.WriteLine($"{testString7}:{result}");
            result = Mathematics.GetStandardNumberString(testString8);
            TestContext.WriteLine($"{testString8}:{result}");
            result = Mathematics.GetStandardNumberString(testString9);
            TestContext.WriteLine($"{testString9}:{result}");
            result = Mathematics.GetStandardNumberString(testString10);
            TestContext.WriteLine($"{testString10}:{result}");
            result = Mathematics.GetStandardNumberString(testString11);
            TestContext.WriteLine($"{testString11}:{result}");
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

        [TestMethod]
        public void ScientificNotationNumberTest()
        {
            ScientificNotationNumber sn = new ScientificNotationNumber();
            TestContext.WriteLine(sn.ToString());
            sn = new ScientificNotationNumber("123");
            TestContext.WriteLine(sn.ToString());
            sn = new ScientificNotationNumber("456", -2, false);
            TestContext.WriteLine(sn.ToString());
            sn = new ScientificNotationNumber("795315", -30, true);
            TestContext.WriteLine(sn.ToString());
            sn = new ScientificNotationNumber("795495315", -15, false);
            TestContext.WriteLine(sn.ToString(4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sn.ToString(-3));
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                long d = cb.DrawOutLong();
                sn = new ScientificNotationNumber(d);
                TestContext.WriteLine(sn.ToString());
            }
        }

    }
}
