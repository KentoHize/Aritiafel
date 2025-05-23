﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Aritiafel.Organizations.RaeriharUniversity;
using Aritiafel.Artifacts;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Aritiafel.Organizations.ArinaOrganization;
using Aritiafel;
using Aritiafel.Items;
using System.Collections;
using Aritiafel.Characters.Heroes;
using Aritiafel.Definitions;
using System.Diagnostics;

namespace AritiafelTest
{
    
    [TestClass]
    public class RaeriharUniversityTest
    {
        internal static readonly char[] AllStandardFormatChar =
        {
            'd', 'D', 'f', 'F', 'g', 'G', 'm', 'M', 'o', 'O',
            'r', 'R', 's', 't', 'T', 'u', 'U', 'y', 'Y'
        };

        internal static readonly char[] AllStandardFormatCharWithABCZX =
        {
            'd', 'D', 'f', 'F', 'g', 'G', 'm', 'M', 'o', 'O',
            'r', 'R', 's', 't', 'T', 'u', 'U', 'y', 'Y', 'A',
            'a', 'B', 'b', 'C', 'c', 'Z', 'z', 'X', 'x'
        };

        List<TestDateTime> dateTimes = new List<TestDateTime> {
                new TestDateTime(1, 1, 1, 0, 0, 0),  new TestDateTime(400, 1, 1, 0 ,0, 0, 1),
                new TestDateTime(11, 2, 29, 23, 59, 59,999),
                new TestDateTime(401, 1, 1, 12, 20, 10, 20), new TestDateTime(402, 2, 5, 13, 2, 2),
                new TestDateTime(2020, 1, 1, 23, 59, 59, 875), new TestDateTime(2120, 3, 3, 0, 0, 1),
                new TestDateTime(2120, 4, 10, 3, 39, 59), new TestDateTime(2320, 5, 7, 20, 2, 0),
                new TestDateTime(2120, 11, 10, 10, 10, 0), new TestDateTime(3320, 12, 15, 3, 29, 49),
                new TestDateTime(4400, 2, 28), new TestDateTime(4400, 3, 1),
                new TestDateTime(399, 12, 31), new TestDateTime(3, 12, 31),
                TestDateTime.Now, new TestDateTime(new DateTime(2000, 1, 1).AddTicks(-1).Ticks, false),
            };

        internal ArDateTime GetArDateTimeFromTestDateTime(TestDateTime testDateTime)
            => testDateTime.Year == 0 ? new ArDateTime(testDateTime.Ticks) : new ArDateTime(testDateTime.Year,
                testDateTime.Month, testDateTime.Day,
                testDateTime.Hour, testDateTime.Minute,
                testDateTime.Second, testDateTime.Millisecond);

        string[] cultureInfoA = { "zh-TW", "zh-CN", "ja-JP", "en-US", "zh-AA" };

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
                => new TestDateTime(ArDateTime.Now.Ticks, false);

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
            ArDateTime adt = ArDateTime.MinValue;
            ChaosBox cb = new ChaosBox();
            for (int i = -100000; i < 100000; i++)
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
            Sophia.SeeThrough(ArDateTime.IsLeapYear(-2019));
            Assert.IsFalse(ArDateTime.IsLeapYear(1));
            Assert.IsFalse(ArDateTime.IsLeapYear(-1));
            Assert.IsTrue(ArDateTime.IsLeapYear(-2));

            Assert.IsTrue(1 ==  ArDateTime.GetDayOfWeek(8, 4, 21));
            Assert.IsTrue(ArDateTime.IsLeapYear(400, true));
            Assert.IsTrue(ArDateTime.IsLeapYear(4, true));
            Assert.IsFalse(ArDateTime.IsLeapYear(-4, true));
            Assert.IsTrue(ArDateTime.IsLeapYear(-5, true));
            //Assert.IsTrue(ArDateTime.IsLeapYear(2020));

            //Assert.IsFalse(ArDateTime.IsLeapYear(1, true));
            //Assert.IsTrue(ArDateTime.IsLeapYear(-6, true));

        }

        [TestMethod]
        public void ArDateTimeAddTest()
        {
            //int tl = dateTimes.Count;
            //for (int i = 0; i < tl; i++)
            //{
            //    if (dateTimes[i].Year == 4400 && dateTimes[i].Day == 29)
            //        dateTimes.Add(new TestDateTime(-4400, 2, 28));
            //    else
            //        dateTimes.Add(dateTimes[i].Reverse());
            //}

            //dateTimes.Add(new TestDateTime(-1, 12, 31, 23, 59, 59, 999));

            //ChaosBox cb = new ChaosBox();

            //for (int i = 0; i < dateTimes.Count; i++)
            //{
            //    int y = cb.DrawOutInteger(-100, 100);
            //    int d = cb.DrawOutInteger(-20, 20);
            //    int m = cb.DrawOutInteger(-10, 10);
            //    ArDateTime adt = GetArDateTimeFromTestDateTime(dateTimes[i]);
            //    //Console.WriteLine($"{adt} + {y}年{m}月{d}天:{adt.AddYears(y).AddMonths(m).AddDays(d)}");
            //    Console.WriteLine($"{adt} + {d}天:{adt.AddDays(d)}");

            //    int h = cb.DrawOutInteger(-10, 10);
            //    Console.WriteLine($"{adt} + {h}時:{adt.AddHours(h)}");

            //    int mm = cb.DrawOutInteger(-100, 100);
            //    Console.WriteLine($"{adt} + {mm}分:{adt.AddMinutes(mm)}");

            //    int s = cb.DrawOutInteger(-100, 100);
            //    Console.WriteLine($"{adt} + {s}秒:{adt.AddSeconds(s)}");

            //    int f = cb.DrawOutInteger(-100, 100);
            //    Console.WriteLine($"{adt} + {f}豪秒:{adt.AddMilliSeconds(f).ToStandardString(ArDateTimeType.DateTime, 7)}");
            //}
        }

        [TestMethod]
        public void TestArea()
        {
            //ArDateTime ad = ArDateTime.Parse(" AR 00021/03/03 00:00:00.0000000");
            ArDateTime ad = ArDateTime.Parse("2, 29, Ar. 11");
            Sophia.SeeThrough(ad);

            Sophia.SeeThrough(Mylar.GetStandardDateTimePattern(Mylar.ArinaCulture.DateTimeFormat, ArStandardDateTimeType.DateExtension));
            return;
            //ArDateTime ad = ArDateTime.Now;
            //Sophia.SeeThrough(ad);
            ad = new ArDateTime(7, 10, 15);
            
            Sophia.SeeThrough(ad);
            //Stopwatch sw = Stopwatch.StartNew();
            //for (int i = 0; i < 100; i++)
            //{
            //    ArDateTime.TryParse("Tuesday, February 29, 2028", CultureInfo.GetCultureInfo("en"), ArDateTimeStyles.AllowWhiteSpaces, out ad);
            //    //ArDateTime.TryParse("abcdd", out ad);
            //}
            //Sophia.SeeThrough(ad);
            //Sophia.SeeThrough(sw);
            //ArDateTime ad2;

            //string format = "yyyy yy yyy yy y / HH hh HH hh HH hh HH :mm:ss";
            //string s = ad.ToString(format);
            //ad2 = ArDateTime.ParseExact(s, format);
            //Sophia.SeeThrough(ad2);

        }

        [TestMethod]
        public void TestNotDateTimeSpeed()
        {
            ChaosBox cb = new ChaosBox();
            Stopwatch sw = Stopwatch.StartNew();
            for(int i = 0; i < 10000; i++)
            {
                byte[] s = cb.DrawOutBytes(200, true);
                char[] c = Encoding.Unicode.GetChars(s);                
                string ss = new string(c);                
                ArDateTime.TryParse(ss, out ArDateTime result);
            }
            Sophia.SeeThrough(sw);
            sw.Stop();
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
        public void FormatDateTimeTest()
        {
            int tl = dateTimes.Count;
            for (int i = 0; i < tl; i++)
            {
                if (dateTimes[i].Year == 11 && dateTimes[i].Day == 29)
                    dateTimes.Add(new TestDateTime(-11, 2, 28));
                else
                    dateTimes.Add(dateTimes[i].Reverse());
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dateTimes.Count; i++)
            {
                ArDateTime adt = GetArDateTimeFromTestDateTime(dateTimes[i]);
                ArDateTime adt2 = ArDateTime.Now;

                for (int j = 0; j < cultureInfoA.Length; j++)
                {   
                    CultureInfo ci;
                    ci = ArCultureInfo.GetCultureInfo(cultureInfoA[j]);
                    sb.AppendLine($"{ci.Name} {ci.DisplayName}");
                    //Sophia.SeeThrough($"{ci.Name} {ci.DisplayName}");
                    for (int k = 0; k < AllStandardFormatCharWithABCZX.Length; k++)
                    {
                        //if (AllStandardFormatChar[k] != 'r')
                        //    continue;
                        //Sophia.SeeThrough(adt.Ticks);
                        string s = adt.ToString(AllStandardFormatCharWithABCZX[k].ToString(), ci);                        
                        //Sophia.SeeThrough(s);
                        if ((AllStandardFormatCharWithABCZX[k] == 'M' || AllStandardFormatCharWithABCZX[k] == 'm') &&
                            adt.Month == 2 && adt.Day == 29)
                            continue;
                        //adt = ArDateTimeFormat.ParseExactFull(s, AllStandardFormatChar[k].ToString(), ci, DateTimeStyles.None);
                        //adt2 = ArDateTime.Parse(s, ci);
                        //if (ci.Name == "en-US" && AllStandardFormatCharWithABCZX[k] == 'D')
                        //    ;
                        adt2 = ArDateTime.Parse(s, ci, ArDateTimeStyles.None);
                        sb.AppendLine(adt2.ToString(AllStandardFormatCharWithABCZX[k].ToString(), ci));
                        //Sophia.SeeThrough(adt2.ToString(AllStandardFormatCharWithABCZX[k].ToString(), ci));                        
                    }
                    //adt2 = ArDateTime.Now;
                    string f = "K g yyyyy/MM/dd tt hh:mm:ss.FFFFF zz";
                    string s2 = adt2.ToString(f, ci);
                    //s2 = s2.Insert(6, " ");
                    //s2 = s2.Insert(9, " ");
                    
                    s2 = " " + s2 + "   ";
                    //Sophia.SeeThrough(s2);
                    adt2 = ArDateTime.ParseExact(s2, f, ci, ArDateTimeStyles.AllowLeadingWhite | ArDateTimeStyles.AllowTrailingWhite | ArDateTimeStyles.CurrentDateDefault);
                    //Sophia.SeeThrough(adt2);                    
                    sb.AppendLine(adt2.ToString());

                    f = "K g yyyyy/MM/dd tt 'aaa\\d' hh:mm:ss.FFFFF zz \"ff\"";
                    s2 = adt2.ToString(f, ci);
                    s2 = " " + s2 + "   ";
                    //Sophia.SeeThrough(s2);
                    adt2 = ArDateTime.ParseExact(s2, f, ci, ArDateTimeStyles.AllowLeadingWhite | ArDateTimeStyles.AllowTrailingWhite | ArDateTimeStyles.CurrentDateDefault);
                    //Sophia.SeeThrough(adt2);
                    sb.AppendLine(adt2.ToString());
                }
            }

            Sophia.SeeThrough(sb);
            
        }


        [TestMethod]
        public void ChaosTest()
        {
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                long l = cb.DrawOutLong(long.MinValue / 10, long.MaxValue / 10);
                ArDateTime adt = new ArDateTime(l);
                Sophia.SeeThrough(adt);
            }
        }

    }
}
