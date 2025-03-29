using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Organizations.RaeriharUniversity;
using Aritiafel.Artifacts;
using System.Collections.Generic;

namespace AritiafelTest
{
    [TestClass]
    public class RaeriharUniversityTest
    {
        public TestContext TestContext { get; set; }

        internal void GetTickString(long ticks, string format = "")
        {
            if (string.IsNullOrEmpty(format))
                format = "";

            //TestContext.WriteLine($"{ticks} Tick(s)[{format}]:{new JDateTime(ticks).ToString(format)}");
            //TestContext.WriteLine($"{ticks} Tick(s)[LongDate]:{new JDateTime(ticks).ToLongDateString()}");
            //TestContext.WriteLine($"{ticks} Tick(s)[ShortDate]:{new JDateTime(ticks).ToShortDateString()}");
            //TestContext.WriteLine($"{ticks} Tick(s)[LongTime]:{new JDateTime(ticks).ToLongTimeString()}");
            //TestContext.WriteLine($"{ticks} Tick(s)[ShortTime]:{new JDateTime(ticks).ToShortTimeString()}");
            //TestContext.WriteLine($"{ticks} Tick(s)[Year]:{new ArDateTime(ticks).Year}");
            TestContext.WriteLine($"{ticks} Tick(s)[Year]:{new ArDateTime(ticks)}");
            //return $"{ticks} Tick(s)[{format}]:{new JDateTime(ticks).ToString(format)}";
        }

        internal void PrintDateTimeString(DateTime dt)
        {
            TestContext.WriteLine($"DT: {dt.Year}/{dt.Month}/{dt.Day} [{dt.DayOfWeek}] {dt.Hour}:{dt.Minute}:{dt.Second}.{dt.Millisecond}");
            ArDateTime adt = new ArDateTime(dt);
            TestContext.WriteLine($"AR: {adt.Year}/{adt.Month}/{adt.Day} [{adt.DayOfWeek}] {adt.Hour}:{adt.Minute}:{adt.Second}.{adt.Millisecond}");
        }

        [TestMethod]
        public void ArDateTimeTranTest()
        {
            //int d = Math.DivRem(-4, 5, out int r);
            //TestContext.WriteLine((d - 1).ToString() + " " + (r + 5).ToString());
            //d = Math.DivRem(-1, 5, out r);
            //TestContext.WriteLine((d - 1).ToString() + " " + (r + 5).ToString());
            //d = Math.DivRem(-5, 5, out r);
            //TestContext.WriteLine((d - 1).ToString() + " " + (r + 5).ToString());
            //d = Math.DivRem(-6, 5, out r);
            //TestContext.WriteLine((d - 1).ToString() + " " + (r + 5).ToString());
            //d = Math.DivRem(0, 5, out r);
            //TestContext.WriteLine((d - 1).ToString() + " " + (r + 5).ToString());

            List<DateTime> dateTimes = new List<DateTime> {
                new DateTime(1, 1, 1, 0, 0, 0),  new DateTime(400, 1, 1, 0 ,0 , 0), 
                new DateTime(401, 1, 1, 12, 20, 10), new DateTime(402, 2, 5, 13, 2, 2), 
                new DateTime(2020, 1, 1, 23, 59, 59), new DateTime(2120, 3, 3, 0, 0, 1),
                new DateTime(2120, 4, 10, 3, 39, 59), new DateTime(2320, 5, 7, 20, 2, 0),
                new DateTime(2120, 11, 10, 10, 10, 0), new DateTime(3320, 12, 15, 3, 29, 49),
                new DateTime(4400, 2, 29), new DateTime(4400, 3, 1),
                DateTime.Now};
            for(int i = 0; i < dateTimes.Count; i++)
            {
                //if (dateTimes[i].Year == 2020)
                //{
                //    ;
                //}
                PrintDateTimeString(dateTimes[i]);
            }
        }

        [TestMethod]
        public void ArDateTimeTest()
        {
            List<long> testTicks = new List<long> { -36000000000L, -600000000L, -10000000L, -1000, -1 };
            for (int i = testTicks.Count - 1; i >= 0; i--)
                testTicks.Add(-testTicks[i]);

            for (int i = 0; i < testTicks.Count; i++)
            {
                GetTickString(testTicks[i]);
            }
        }

        [TestMethod]
        public void GetStandardNumberStringTest()
        {
            //string result;
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
       
    }
}
