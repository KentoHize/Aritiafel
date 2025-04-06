using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Organizations.RaeriharUniversity;
using Aritiafel.Artifacts;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data.SqlTypes;
using Aritiafel.Organizations.ArinaOrganization;
using Aritiafel;

namespace AritiafelTest
{
    [TestClass]
    public class RaeriharUniversityTest
    {
        List<TestDateTime> dateTimes = new List<TestDateTime> {
                new TestDateTime(1, 1, 1, 0, 0, 0),  new TestDateTime(400, 1, 1, 0 ,0, 0, 1),
                new TestDateTime(401, 1, 1, 12, 20, 10, 20), new TestDateTime(402, 2, 5, 13, 2, 2),
                new TestDateTime(2020, 1, 1, 23, 59, 59, 875), new TestDateTime(2120, 3, 3, 0, 0, 1),
                new TestDateTime(2120, 4, 10, 3, 39, 59), new TestDateTime(2320, 5, 7, 20, 2, 0),
                new TestDateTime(2120, 11, 10, 10, 10, 0), new TestDateTime(3320, 12, 15, 3, 29, 49),
                new TestDateTime(4400, 2, 29), new TestDateTime(4400, 3, 1),
                new TestDateTime(399, 12, 31), new TestDateTime(3, 12, 31),
                TestDateTime.Now, new TestDateTime(new DateTime(2000, 1, 1).AddTicks(-1).Ticks, false),
            };

        internal ArDateTime GetArDateTimeFromTestDateTime(TestDateTime testDateTime)
            => testDateTime.Year == 0 ? new ArDateTime(testDateTime.Ticks) : new ArDateTime(testDateTime.Year,
                testDateTime.Month, testDateTime.Day,
                testDateTime.Hour, testDateTime.Minute,
                testDateTime.Second, testDateTime.Millisecond);

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
            //TestContext.WriteLine($"{ticks} Tick(s)[Year]:{new ArDateTime(ticks)}");
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
                if (tdt.Ticks == 0)
                    adt = new ArDateTime(tdt.Year, tdt.Month, tdt.Day, tdt.Hour, tdt.Minute, tdt.Second, tdt.Millisecond);
                else
                    adt = new ArDateTime(tdt.Ticks);
            }

            //TestContext.WriteLine($"ARD: {adt.Year}/{adt.Month}/{adt.Day} [{adt.DayOfWeek}] {adt.Hour}:{adt.Minute}:{adt.Second}.{adt.Millisecond} {adt.Ticks}");
            TestContext.WriteLine($"ARD: {adt.ToLongDateString()} {adt.ToLongTimeString()}");
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
                => new TestDateTime(DateTime.Now.Ticks, false);

            public TestDateTime Reverse()
                => Year != 0 ? new TestDateTime(Year * -1, Month, Day, Hour, Minute, Second, Millisecond) : new TestDateTime(Ticks * -1, false);

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
                    b1.ToString() == d1.ToString("yyy/M/d H:m:s") ||
                    b1.ToString() == d1.ToString("y/M/d H:m:s")
                    );
            }
            sb.Append("OK");
            Console.Write(sb.ToString());
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
            ArDateTime adt = ArDateTime.MinValue;
            ChaosBox cb = new ChaosBox();
            for (int i = -100000; i < 1000000; i++)
            {
                sb.AppendLine($"{adt} {adt.Ticks}");
                adt = adt.Add(new TimeSpan(864000000000 - 1));
                sb.AppendLine($"{adt} {adt.Ticks}");
                adt = adt.Add(new TimeSpan(1));
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

            Assert.IsFalse(ArDateTime.IsLeapYear(1, true));
            Assert.IsTrue(ArDateTime.IsLeapYear(-6, true));
            
        }

        [TestMethod]
        public void DateTimeToStringTest()
        {
            string format;
            string[] formatArray = { "M", "Y", "F", "f", "g" };
            string[] cultureInfoA = { "zh-TW", "ja-JP", "en-US", "zh-CN" };
            int tl = dateTimes.Count;
            for (int i = 0; i < tl; i++)
            {
                if (dateTimes[i].Year == 4400 && dateTimes[i].Day == 29)
                    dateTimes.Add(new TestDateTime(-4400, 2, 28));
                else
                    dateTimes.Add(dateTimes[i].Reverse());
            }

            dateTimes.Add(new TestDateTime(-1, 12, 31, 23, 59, 59, 999));


            for (int i = 0; i < dateTimes.Count; i++)
            {
                ArDateTime adt = GetArDateTimeFromTestDateTime(dateTimes[i]);
                //TestContext.WriteLine($"Ticks: {adt.Ticks}:{adt}");
                //TestContext.WriteLine($"LongDateString D: {ArDateTime.ParseExact(adt.ToLongDateString(), "D").ToLongDateString()}");
                //TestContext.WriteLine($"ShortDateString d: {ArDateTime.ParseExact(adt.ToShortDateString(), "d").ToShortDateString()}");
                //TestContext.WriteLine($"LongTimeString T: {ArDateTime.ParseExact(adt.ToLongTimeString(), "T").ToLongTimeString()}");
                //TestContext.WriteLine($"ShortTimeString t: {ArDateTime.ParseExact(adt.ToShortTimeString(), "t").ToShortTimeString()}");
                TestContext.WriteLine($"StandardString: {ArDateTime.Parse(adt.ToString()).ToString()}");
                TestContext.WriteLine($"ArinaString: {ArDateTime.Parse(adt.ToArString()).ToArString("F")}");
                TestContext.WriteLine($"LongDateString D: {ArDateTime.Parse(adt.ToLongDateString()).ToLongDateString()}");
                TestContext.WriteLine($"ShortDateString d: {ArDateTime.Parse(adt.ToShortDateString()).ToShortDateString()}");
                TestContext.WriteLine($"LongTimeString T: {ArDateTime.Parse(adt.ToLongTimeString()).ToLongTimeString()}");
                TestContext.WriteLine($"ShortTimeString t: {ArDateTime.Parse(adt.ToShortTimeString()).ToShortTimeString()}");
                TestContext.WriteLine($"ToString(\"d, M, yyyy/hh:mm:ss.f\")自訂: {ArDateTime.Parse(adt.ToString()).ToString("d, M, yyyy/hh:mm:ss.f")}");

                for (int j = 0; j < formatArray.Length; j++)
                {
                    if (adt.Day == 29 && adt.Month == 2 &&
                       (formatArray[j] == "M" || formatArray[j] == "m"))
                        continue;
                    //TestContext.WriteLine($"{formatArray[j]}:{ArDateTime.ParseExact(adt.ToString(formatArray[j]), formatArray[j]).ToString(formatArray[j])}");
                    TestContext.WriteLine($"{formatArray[j]}:{ArDateTime.Parse(adt.ToString(formatArray[j])).ToString(formatArray[j])}");
                }

                for (int j = 0; j < cultureInfoA.Length; j++)
                {
                    //TestContext.WriteLine(adt.ToString(CultureInfo.GetCultureInfo(cultureInfoA[j])));
                    //TestContext.WriteLine($"{cultureInfoA[j]}:{ArDateTime.ParseExact(adt.ToString(CultureInfo.GetCultureInfo(cultureInfoA[j])), null,
                    //    CultureInfo.GetCultureInfo(cultureInfoA[j])).ToString("F", CultureInfo.GetCultureInfo(cultureInfoA[j]))}");
                    TestContext.WriteLine($"{cultureInfoA[j]}:{ArDateTime.Parse(adt.ToString(CultureInfo.GetCultureInfo(cultureInfoA[j])), CultureInfo.GetCultureInfo(cultureInfoA[j])).ToString("F", CultureInfo.GetCultureInfo(cultureInfoA[j]))}");
                }


                TestContext.WriteLine("");
                //TestContext.WriteLine($"{adt.ToString()} {adt.Ticks}");
                //TestContext.WriteLine($"{ArDateTime.ParseExact(adt.ToString(), null, null, DateTimeStyles.None)} {adt.Ticks}");
                //TestContext.WriteLine($"{adt.ToString("G", CultureInfo.CurrentCulture) }");
                //TestContext.WriteLine($"{ArDateTime.ParseExact(adt.ToString("G", CultureInfo.CurrentCulture), "G", CultureInfo.CurrentCulture, DateTimeStyles.None).ToString("G", CultureInfo.CreateSpecificCulture("en-US"))}");
                //TestContext.WriteLine($"{ArDateTime.Parse(adt.ToString("G", CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, DateTimeStyles.None).ToString("f", CultureInfo.CreateSpecificCulture("zh-CN"))}");
                //TestContext.WriteLine($"{adt.ToString(CultureInfo.CurrentCulture)}");                
            }
        }

        [TestMethod]
        public void ArDateTimeAddTest()
        {
            int tl = dateTimes.Count;
            for (int i = 0; i < tl; i++)
            {
                if (dateTimes[i].Year == 4400 && dateTimes[i].Day == 29)
                    dateTimes.Add(new TestDateTime(-4400, 2, 28));
                else
                    dateTimes.Add(dateTimes[i].Reverse());
            }

            dateTimes.Add(new TestDateTime(-1, 12, 31, 23, 59, 59, 999));

            ChaosBox cb = new ChaosBox();

            for (int i = 0; i < dateTimes.Count; i++)
            {
                int y = cb.DrawOutInteger(-100, 100);
                int d = cb.DrawOutInteger(-20, 20);
                int m = cb.DrawOutInteger(-10, 10);
                ArDateTime adt = GetArDateTimeFromTestDateTime(dateTimes[i]);
                //Console.WriteLine($"{adt} + {y}年{m}月{d}天:{adt.AddYears(y).AddMonths(m).AddDays(d)}");
                Console.WriteLine($"{adt} + {d}天:{adt.AddDays(d)}");

                int h = cb.DrawOutInteger(-10, 10);
                Console.WriteLine($"{adt} + {h}時:{adt.AddHours(h)}");

                int mm = cb.DrawOutInteger(-100, 100);
                Console.WriteLine($"{adt} + {mm}分:{adt.AddMinutes(mm)}");

                int s = cb.DrawOutInteger(-100, 100);
                Console.WriteLine($"{adt} + {s}秒:{adt.AddSeconds(s)}");

                int f = cb.DrawOutInteger(-100, 100);
                Console.WriteLine($"{adt} + {f}豪秒:{adt.AddMilliSeconds(f).ToStandardString(ArDateTimeType.DateTime, 7)}");
            }
        }

        [TestMethod]
        public void TestArea()
        {
            //StringBuilder sb = null;
            //TestContext.WriteLine(ArDateTime.DaysInMonth(1, 1).ToString());
            TestContext.WriteLine("AR +3".CompareTo("AR -4").ToString());
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

        [TestMethod]
        public void TestRoundToDigit()
        {
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 100000; i++)
            {
                long a = cb.DrawOutLong();
                int b = cb.DrawOutInteger(-7, 7);
                TestContext.WriteLine($"{a}-{b}:{Mathematics.RoundToDigit(a, b)}");
            }
        }

        [TestMethod]
        public void StandFormat()
        {
            int tl = dateTimes.Count;
            for (int i = 0; i < tl; i++)
            {
                if (dateTimes[i].Year == 4400 && dateTimes[i].Day == 29)
                    dateTimes.Add(new TestDateTime(-4400, 2, 28));
                else
                    dateTimes.Add(dateTimes[i].Reverse());
            }

            ChaosBox cb = new ChaosBox();

            for (int i = 0; i < dateTimes.Count; i++)
            {
                ArDateTime adt = GetArDateTimeFromTestDateTime(dateTimes[i]);
                string s = adt.ToStandardString(ArDateTimeType.DateTime, 7);
                //TestContext.WriteLine(s);
                int d = cb.DrawOutByte(7);
                TestContext.WriteLine($"{d}:{ArDateTime.Parse(s).ToStandardString(ArDateTimeType.DateTime, d)}");

                s = adt.ToStandardString(ArDateTimeType.Date);                
                TestContext.WriteLine(ArDateTime.Parse(s).ToStandardString(ArDateTimeType.Date));

                s = adt.ToStandardString(ArDateTimeType.Time);                
                TestContext.WriteLine(ArDateTime.Parse(s).ToStandardString(ArDateTimeType.Time));

                s = adt.ToStandardString(ArDateTimeType.ShortTime);                
                TestContext.WriteLine(ArDateTime.Parse(s).ToStandardString(ArDateTimeType.ShortTime));

                s = adt.ToStandardString(ArDateTimeType.LongDate);                
                TestContext.WriteLine(ArDateTime.Parse(s).ToStandardString(ArDateTimeType.LongDate));

                s = adt.ToStandardString(ArDateTimeType.System);
                TestContext.WriteLine(ArDateTime.Parse(s).ToStandardString(ArDateTimeType.System));
            }
        }

    }
}
