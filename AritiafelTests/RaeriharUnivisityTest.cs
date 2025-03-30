using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Organizations.RaeriharUniversity;
using Aritiafel.Artifacts;
using System.Collections.Generic;
using System.Text;

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

        internal void PrintDateTimeString(TestDateTime tdt)
        {
            ArDateTime adt;
            //DateTime dt;
            TestContext.WriteLine($"TDT: {tdt.Year}/{tdt.Month}/{tdt.Day} {tdt.Hour}:{tdt.Minute}:{tdt.Second}.{tdt.Millisecond} {tdt.Ticks}");
            if (tdt.Year > 0)
            {
                adt = new ArDateTime(tdt.Year, tdt.Month, tdt.Day, tdt.Hour, tdt.Minute, tdt.Second, tdt.Millisecond);
            }
            else
            {
                if(tdt.Ticks == 0)
                    adt = new ArDateTime(tdt.Year, tdt.Month, tdt.Day, tdt.Hour, tdt.Minute, tdt.Second, tdt.Millisecond);
                else
                            adt = new ArDateTime(tdt.Ticks);
                //tdt.Year *= -1;
                //adt = new ArDateTime(new DateTime(tdt.Year, tdt.Month, tdt.Day, tdt.Hour, tdt.Minute, tdt.Second, tdt.Millisecond), true);
            }
                
            TestContext.WriteLine($"ARD: {adt.Year}/{adt.Month}/{adt.Day} [{adt.DayOfWeek}] {adt.Hour}:{adt.Minute}:{adt.Second}.{adt.Millisecond} {adt.Ticks}");
        }

        public class TestDateTime
        {
            public int Year { get; set; } // can negative
            public int Month { get; set; }
            public int Day { get; set; }
            public int Hour { get; set; }
            public int Minute { get; set; }
            public int Second { get; set; }
            public int Millisecond { get; set; }            
            public long Ticks { get; set; }
                

            public TestDateTime(long ticks, bool reverse)
                => Ticks = ticks;

            public TestDateTime(int year = 0, int month = 0, int day = 0, int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
            {
                Year = year;
                Month = month;
                Day = day;
                Hour = hour;
                Minute = minute;
                Second = second;
                Millisecond = millisecond;
            }

            public static TestDateTime Now
                => new TestDateTime(DateTime.Now);

            public TestDateTime Reverse()
            {
                return new TestDateTime(Year * -1, Month, Day, Hour, Minute, Second,Millisecond);
            }
                
            
            public TestDateTime(DateTime dt)
                : this(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Minute, dt.Millisecond)
            { }

        }

        [TestMethod]
        public void ArDateTimeTranTest2()
        {
            StringBuilder sb = new StringBuilder();
            //for(int d = -1000000; d < 1000000; d++)
            //{
            //    ArDateTime a1 = new ArDateTime(864000000000 * d);
            //    ArDateTime a2 = new ArDateTime(864000000000 * d + 1);                
            //    //sb.AppendLine(a1.ToString());
            //    //sb.AppendLine(a2.ToString());
            //}

            for (int d = 0; d < 1000000; d++)
            {
                ArDateTime b1 = new ArDateTime(864000000000L * d + 1);
                DateTime d1 = new DateTime(864000000000L * d + 1);
                
                Assert.IsTrue(
                    b1.ToString() == d1.ToString("yyy/M/d H:m:s.f") ||
                    b1.ToString() == d1.ToString("y/M/d H:m:s.f")
                    );
            }
            sb.Append("OK");
            Console.Write(sb.ToString());
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

            List<TestDateTime> dateTimes = new List<TestDateTime> {
                new TestDateTime(1, 1, 1, 0, 0, 0),  new TestDateTime(400, 1, 1, 0 ,0 , 0),
                new TestDateTime(401, 1, 1, 12, 20, 10), new TestDateTime(402, 2, 5, 13, 2, 2),
                new TestDateTime(2020, 1, 1, 23, 59, 59), new TestDateTime(2120, 3, 3, 0, 0, 1),
                new TestDateTime(2120, 4, 10, 3, 39, 59), new TestDateTime(2320, 5, 7, 20, 2, 0),
                new TestDateTime(2120, 11, 10, 10, 10, 0), new TestDateTime(3320, 12, 15, 3, 29, 49),
                new TestDateTime(4400, 2, 29), new TestDateTime(4400, 3, 1),
                new TestDateTime(399, 12, 31), new TestDateTime(3, 12, 31),
                TestDateTime.Now, new TestDateTime(new DateTime(2000, 1, 1).AddTicks(-1)),
            };

            dateTimes.Add(new TestDateTime(-1, 12, 31, 23, 59, 59, 999));

            int tl = dateTimes.Count;
            for (int i = 0; i < tl; i++)
            {
                if (dateTimes[i].Year == 4400 && dateTimes[i].Day == 29)
                    dateTimes.Add(new TestDateTime(-4400, 2, 28));
                else
                    dateTimes.Add(dateTimes[i].Reverse());
                
            }
                

            //for (int i = 0; i < 10; i++)
            //    dateTimes.Add(new TestDateTime(i * -1000, false));

            dateTimes.Add(new TestDateTime(-100000, false));
            dateTimes.Add(new TestDateTime(-10000000, false));
            dateTimes.Add(new TestDateTime(-1000000000, false));
            dateTimes.Add(new TestDateTime(-100000000000, false));
            dateTimes.Add(new TestDateTime(-10000000000000, false));
            dateTimes.Add(new TestDateTime(-1000000000000000, false));
            dateTimes.Add(new TestDateTime(-100000000000000000, false));
            dateTimes.Add(new TestDateTime(-864000000000, false));
            dateTimes.Add(new TestDateTime(-864000000001, false));
            dateTimes.Add(new TestDateTime(-8640000000000, false));
            for (int i = 0; i < dateTimes.Count; i++)
            {
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
        public void DateTest()
        {
            StringBuilder sb = new StringBuilder();
            //864000000000
            ChaosBox cb = new ChaosBox();
            for (int i = -100000; i < 1000000; i++)
            {
                sb.AppendLine(new ArDateTime(i * 864000000000 + cb.DrawOutLong(864000000000 - 1)).Date.ToString());
            }
            TestContext.WriteLine(sb.ToString());
        }

        [TestMethod]
        public void IsLeapYear()
        {
            Assert.IsTrue(ArDateTime.IsLeapYear(-1));
            Assert.IsTrue(ArDateTime.IsLeapYear(400));
            Assert.IsTrue(ArDateTime.IsLeapYear(4));
            Assert.IsFalse(ArDateTime.IsLeapYear(-4));
            Assert.IsTrue(ArDateTime.IsLeapYear(-5));
            Assert.IsTrue(ArDateTime.IsLeapYear(2020));
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
