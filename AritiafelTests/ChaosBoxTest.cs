using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Aritiafel.Artifacts;
using System.Text;
using System;
using System.Threading;

namespace AritiafelTest
{
    [TestClass]
    public class ChaosBoxTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SmoothTestRandomTest()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            int a = 0, b = 0, t = 0;
            for(int j = 0; j < 100; j++)
            {
                a = 0; b = 0;
                for(int i = 0; i < 100000; i++)
                {
                    if (rnd.Next(0, 10) >= 1)
                        a++;

                    if (rnd.Next(0, 1000000000) >= 100000000)
                        b++;
                }
                TestContext.WriteLine($"a:{a}, b:{b}, b - a: {b - a}");
                t = b - a;
            }
            Console.WriteLine($"Average b - a:{t / 100}"); // Correct
        }

        [TestMethod]
        public void GetNumberStringPowOf10Test()
        {
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                double a = cb.DrawOutDiversityDouble();
                //a = -0.003213213;
                if (Math.Abs(a).ToString().Length - 1 != cb.GetNumberStringPowOf10(a.ToString()))
                    TestContext.WriteLine($"{a}:{cb.GetNumberStringPowOf10(a.ToString())}");
                //TestContext.WriteLine($"{a}:{cb.GetNumberStringPowOf10(a.ToString())}");
            }
        }

        [TestMethod]
        public void RandomMinMaxIntegerTest()
        {   
            Random rnd = new Random();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 0; i < 10000; i++)
            {
                int a = rnd.Next();
                int b = rnd.Next();
                int c;
                if (a > b)
                { c = a; a = b; b = c; }
                int x = rnd.Next(a, b);
                int key = x.ToString().Length;
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void RandomStringDoubleMinMaxTest()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            int wrongNumber = 0;
            for (int i = 1; i < 10000; i++)
            {
                double a = cb.DrawOutDiversityDouble();
                double b = cb.DrawOutDiversityDouble();
                double c;
                if (a > b)
                { c = a; a = b; b = c; }

                //a = 5.665465E-150;
                //b = 0.076789387490;
                //a = -0.08782316379783744;
                //a = -0.01;
                //b = 8.353550460771968E-50;


                string s = cb.RandomMinMaxValue(a.ToString(), b.ToString());
                try
                {
                    double d = double.Parse(s);
                    if (d < a || d > b)
                        throw new Exception();
                    int key;
                    if (s.Contains('E'))
                        key = int.Parse(s.Substring(s.IndexOf('E') + 1));
                    else
                        key = cb.GetNumberStringPowOf10(s);
                    if (!test.ContainsKey(key))
                        test.Add(key, 1);
                    else
                        test[key]++;
                }
                catch
                {
                    TestContext.WriteLine($"{a} :A");
                    TestContext.WriteLine($"{b} :B");
                    TestContext.WriteLine($"Wrong:");
                    TestContext.WriteLine(s);
                    TestContext.WriteLine(s.Length.ToString());
                    wrongNumber++;
                }
            }
            Console.WriteLine(wrongNumber);
            //Assert.IsTrue(wrongNumber == 0);
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void RandomStringIntegerMinMaxTest()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            int wrongNumber = 0;
            for (int i = 0; i < 10000; i++)
            {
                int a = cb.DrawOutInteger();
                int b = cb.DrawOutInteger();
                int c;
                
                if (a > b)
                { c = a; a = b; b = c; }
                //a = 100;
                //c = 10000;
                //a = 0.0267987465;
                //b = 300.3;
                //b = 1000.654989;
                a = 0;
                b = 1500;
                string s = cb.RandomMinMaxValue(a.ToString(), b.ToString());
                try
                {
                    int d = int.Parse(s);
                    if (d < a || d > b)
                        throw new Exception();
                    int key = (d >= 0 ? 1 : -1) * Math.Abs(d).ToString().Length;

                    if (!test.ContainsKey(key))
                        test.Add(key, 1);
                    else
                        test[key]++;                    
                }
                catch
                {
                    TestContext.WriteLine($"{a} :A");
                    TestContext.WriteLine($"{b} :B");
                    TestContext.WriteLine($"Wrong:");
                    TestContext.WriteLine(s);
                    TestContext.WriteLine(s.Length.ToString());
                    wrongNumber++;
                    //TestContext.WriteLine(cb.RandomMinMaxValue(a.ToString(), b.ToString(), out int _));
                }

            }
            Assert.IsTrue(wrongNumber == 0);
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void SmoothTestDouble()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                double b = cb.DrawOutDouble(0, double.MaxValue / 10);
                string s = b.ToString();
                int key;
                if (s.Contains('E'))
                    key = int.Parse(s.Substring(s.IndexOf('E') + 1));
                else
                    key = 0;
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void SmoothTestInteger()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                int b = cb.DrawOutInteger(0, 1000000);
                string s = b.ToString();
                int key;
                key = s.Length;
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
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
        public void DrawOutStringTest()
        {
            ChaosBox cb = new ChaosBox();
            for (int i = 1; i < 10; i++)
                TestContext.WriteLine(cb.DrawOutString(i));
            TestContext.WriteLine("----------");
            for (int i = 1; i < 20; i++)
                TestContext.WriteLine(cb.DrawOutRandomLengthString(100, Encoding.Unicode));
        }

        [TestMethod]
        public void DrawOutDoubleTest()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                double b = cb.DrawOutDouble();
                string s = b.ToString();
                int key;
                if (s.Contains('E'))
                    key = int.Parse(s.Substring(s.IndexOf('E') + 1));
                else
                    key = 0;
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void DrawOutNormalizedByteTest()
        {
            //Is Normalize?
            ChaosBox cb = new ChaosBox();
            SortedList<byte, int> test = new SortedList<byte, int>();
            for (int i = 1; i < 10000; i++)
            {
                byte b = cb.DrawOutNormalizedByte();
                if (!test.ContainsKey(b))
                    test.Add(b, 0);
                else
                    test[b]++;
            }
            foreach (KeyValuePair<byte, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }


        [TestMethod]
        public void DrawOutNormalizedIntegerTest()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                int b = cb.DrawOutNormalizedInteger(false);
                //int b = cb.DrawOutNormalizedInteger(-20000, 20000);                
                string s = Math.Abs(b).ToString();
                int key;
                key = Math.Abs((b - int.MaxValue / 2)).ToString().Length;
                //key = b;
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void DrawOutNormalizedByteTest2()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<byte, int> test = new SortedList<byte, int>();
            for (int i = 1; i < 10000; i++)
            {
                byte b = cb.DrawOutNormalizedByte(3, 5);
                if (!test.ContainsKey(b))
                    test.Add(b, 0);
                else
                    test[b]++;
            }
            foreach (KeyValuePair<byte, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void DrawOutLongTest()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                long b = cb.DrawOutLong(false);
                //int b = cb.DrawOutNormalizedInteger(-20000, 20000);                
                string s = Math.Abs(b).ToString();
                int key;
                key = s.Length;
                //key = b;
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }

        [TestMethod]
        public void DrawOutIntegerTest()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                long b = cb.DrawOutInteger(false);
                //int b = cb.DrawOutNormalizedInteger(-20000, 20000);                
                string s = Math.Abs(b).ToString();
                int key;
                key = s.Length;
                //key = b;
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }
    }
}
