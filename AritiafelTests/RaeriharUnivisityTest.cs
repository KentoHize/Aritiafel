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
           
            //result = Mathematics.GetStandardNumberString(testString);
            //TestContext.WriteLine($"{testString}:{result}");
            //result = Mathematics.GetStandardNumberString(testString2);
            //TestContext.WriteLine($"{testString2}:{result}");
            //result = Mathematics.GetStandardNumberString(testString3);
            //TestContext.WriteLine($"{testString3}:{result}");
            //result = Mathematics.GetStandardNumberString(testString4);
            //TestContext.WriteLine($"{testString4}:{result}");
            //result = Mathematics.GetStandardNumberString(testString5);
            //TestContext.WriteLine($"{testString5}:{result}");
            //result = Mathematics.GetStandardNumberString(testString6);
            //TestContext.WriteLine($"{testString6}:{result}");
            //result = Mathematics.GetStandardNumberString(testString7);
            //TestContext.WriteLine($"{testString7}:{result}");
            //result = Mathematics.GetStandardNumberString(testString8);
            //TestContext.WriteLine($"{testString8}:{result}");
            //result = Mathematics.GetStandardNumberString(testString9);
            //TestContext.WriteLine($"{testString9}:{result}");
            //result = Mathematics.GetStandardNumberString(testString10);
            //TestContext.WriteLine($"{testString10}:{result}");
            //result = Mathematics.GetStandardNumberString(testString11);
            //TestContext.WriteLine($"{testString11}:{result}");
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
        public void RoundTest()
        {
            ScientificNotationNumber sn = new ScientificNotationNumber(123.53);
            TestContext.WriteLine(sn.Round(6).ToString());
            TestContext.WriteLine(sn.Round(5).ToString());
            TestContext.WriteLine(sn.Round(4).ToString());
            TestContext.WriteLine(sn.Round(3).ToString());

            sn = new ScientificNotationNumber(199.99);
            TestContext.WriteLine(sn.Round(6).ToString());
            TestContext.WriteLine(sn.Round(5).ToString());
            TestContext.WriteLine(sn.Round(4).ToString());
            TestContext.WriteLine(sn.Round(3).ToString());

            sn = new ScientificNotationNumber(9.99999);
            TestContext.WriteLine(sn.Round(6).ToString());
            TestContext.WriteLine(sn.Round(5).ToString());
            TestContext.WriteLine(sn.Round(4).ToString());
            TestContext.WriteLine(sn.Round(3).ToString());

            sn = new ScientificNotationNumber(0.0999999);
            TestContext.WriteLine(sn.Round(6).ToString());
            TestContext.WriteLine(sn.Round(5).ToString());
            TestContext.WriteLine(sn.Round(4).ToString());
            TestContext.WriteLine(sn.Round(3).ToString());
        }

        [TestMethod]
        public void ScientificNotationNumberTest()
        {
            ScientificNotationNumber sn = new ScientificNotationNumber();
            Assert.IsTrue(sn.ToString() == "0");
            sn = new ScientificNotationNumber("123");
            Assert.IsTrue(sn.ToString() == "1.23");
            sn = new ScientificNotationNumber("456", -2, false);
            Assert.IsTrue(sn.ToString() == "4.56E-2");
            sn = new ScientificNotationNumber("795315", -30, true);
            Assert.IsTrue(sn.ToString() == "-7.95315E-30");
            sn = new ScientificNotationNumber("795495315", -15, false);
            Assert.IsTrue(sn.ToString("E4") == "7.955E-15");
            Assert.IsTrue(sn.ToString("E9") == "7.95495315E-15");
            sn = new ScientificNotationNumber(999.999);            
            Assert.IsTrue(sn.ToString("E1") == "1E+3");
            Assert.IsTrue(sn.ToString("E2") == "1E+3");
            Assert.IsTrue(sn.ToString("E3") == "1E+3");
            Assert.IsTrue(sn.ToString("E4") == "1E+3");
            Assert.IsTrue(sn.ToString("E5") == "1E+3");
            Assert.IsTrue(sn.ToString("E6") == "9.99999E+2");
            sn = new ScientificNotationNumber(0.1999);            
            Assert.IsTrue(sn.ToString("E1") == "2E-1");
            Assert.IsTrue(sn.ToString("E2") == "2E-1");
            Assert.IsTrue(sn.ToString("E3") == "2E-1");
            Assert.IsTrue(sn.ToString("E4") == "1.999E-1");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => sn.ToString("E-3"));

            string[] testStrings = {
                "-0.00589662145E-103",
                "-0.03611895678E+51",
                "-0000.0006871800",
                "35678943580000000000000000000000000000000",
                "+568.68100E-8",
                "-.00035E-20",
                "0.00",
                "-0.000",
                "+300",
                "-0.008",
                "0.00681E+98",
                "0.01",
                "0"
            };

            sn = ScientificNotationNumber.Parse(testStrings[0]);
            Assert.IsTrue(sn.ToString() == "-5.89662145E-106");            
            sn = ScientificNotationNumber.Parse(testStrings[1]);
            Assert.IsTrue(sn.ToString() == "-3.611895678E+49");
            sn = ScientificNotationNumber.Parse(testStrings[2]);
            Assert.IsTrue(sn.ToString() == "-6.8718E-4");
            Assert.IsTrue(sn.ToString("C") == "-0.00068718");
            sn = ScientificNotationNumber.Parse(testStrings[3]);
            Assert.IsTrue(sn.ToString() == "3.567894358E+40");
            sn = ScientificNotationNumber.Parse(testStrings[4]);
            Assert.IsTrue(sn.ToString() == "5.68681E-6");
            Assert.IsTrue(sn.ToString("C") == "0.00000568681");
            TestContext.WriteLine(sn.ToString("C3"));
            Assert.IsTrue(sn.ToString("C3") == "0.00000569");
            //Assert.IsTrue(sn.ToString("C3") == "0.00");
            sn = ScientificNotationNumber.Parse(testStrings[5]);
            Assert.IsTrue(sn.ToString() == "-3.5E-24");
            sn = ScientificNotationNumber.Parse(testStrings[6]);
            Assert.IsTrue(sn.ToString() == "0");
            Assert.IsTrue(sn.ToString("C") == "0");
            sn = ScientificNotationNumber.Parse(testStrings[7]);
            Assert.IsTrue(sn.ToString() == "0");
            Assert.IsTrue(sn.ToString("C") == "0");
            sn = ScientificNotationNumber.Parse(testStrings[8]);
            Assert.IsTrue(sn.ToString() == "3E+2");
            sn = ScientificNotationNumber.Parse(testStrings[9]);
            Assert.IsTrue(sn.ToString() == "-8E-3");
            sn = ScientificNotationNumber.Parse(testStrings[10]);
            Assert.IsTrue(sn.ToString() == "6.81E+95");
            sn = ScientificNotationNumber.Parse(testStrings[11]);
            Assert.IsTrue(sn.ToString() == "1E-2");
            sn = ScientificNotationNumber.Parse(testStrings[12]);
            Assert.IsTrue(sn.ToString() == "0");

            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                double d = cb.DrawOutDiversityDouble();
                    sn = new ScientificNotationNumber(d);
                    TestContext.WriteLine($"{d}: {sn.ToString("E-4")}");                
            }
        }

    }
}
