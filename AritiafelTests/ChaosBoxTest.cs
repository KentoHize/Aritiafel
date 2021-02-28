using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Aritiafel.Artifacts;
using System.Text;
using System;

namespace AritiafelTestFormTests
{
    [TestClass]
    public class ChaosBoxTest
    {
        public TestContext TestContext { get; set; }

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
            foreach(KeyValuePair<byte, int> kv in test)
                TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        }


        [TestMethod]
        public void DrawOutNormalizedIntegerTest()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                //int b = cb.DrawOutNormalizedInteger(-2, 2);
                int b = cb.DrawOutInteger(-20000, 20000);
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
