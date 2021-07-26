using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Aritiafel.Artifacts;
using System.Text;
using System;
using System.Threading;
using Aritiafel.Organizations.RaeriharUniversity;

namespace AritiafelTest
{
    [TestClass]
    public class ChaosBoxTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SmoothTestNextIntTest()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);

            SortedList<int, int> result = new SortedList<int, int>();
            for (int i = 0; i < 10000; i++)
            {
                int r = rnd.Next(30, 101);
                if (result.ContainsKey(r))
                    result[r]++;
                else
                    result.Add(r, 1);
            }

            foreach (KeyValuePair<int, int> kvp in result)
                TestContext.WriteLine($"{kvp.Key}:{kvp.Value}");
        }

        [TestMethod]
        public void SmoothTestRandomTest()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            int a = 0, b = 0, t = 0;
            for (int j = 0; j < 100; j++)
            {
                a = 0; b = 0;
                for (int i = 0; i < 100000; i++)
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

        //[TestMethod]
        //public void RandomStringDoubleMinMaxTest()
        //{
        //    ChaosBox cb = new ChaosBox();
        //    SortedList<int, int> test = new SortedList<int, int>();
        //    int wrongNumber = 0;
        //    for (int i = 1; i < 10000; i++)
        //    {
        //        double a = cb.DrawOutDiversityDouble();
        //        double b = cb.DrawOutDiversityDouble();
        //        double c;
        //        if (a > b)
        //        { c = a; a = b; b = c; }

        //        //a = 5.665465E-150;
        //        //b = 0.076789387490;
        //        //a = -0.08782316379783744;
        //        //a = -0.01;
        //        //b = 8.353550460771968E-50;


        //        string s = cb.RandomMinMaxValue(a.ToString(), b.ToString());
        //        try
        //        {
        //            double d = double.Parse(s);
        //            if (d < a || d > b)
        //                throw new Exception();
        //            int key;
        //            if (s.Contains('E'))
        //                key = int.Parse(s.Substring(s.IndexOf('E') + 1));
        //            else
        //                key = cb.GetNumberStringPowOf10(s);
        //            if (!test.ContainsKey(key))
        //                test.Add(key, 1);
        //            else
        //                test[key]++;
        //        }
        //        catch
        //        {
        //            TestContext.WriteLine($"{a} :A");
        //            TestContext.WriteLine($"{b} :B");
        //            TestContext.WriteLine($"Wrong:");
        //            TestContext.WriteLine(s);
        //            TestContext.WriteLine(s.Length.ToString());
        //            wrongNumber++;
        //        }
        //    }
        //    Console.WriteLine(wrongNumber);
        //    //Assert.IsTrue(wrongNumber == 0);
        //    foreach (KeyValuePair<int, int> kv in test)
        //        TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        //}

        //[TestMethod]
        //public void RandomStringIntegerMinMaxTest()
        //{
        //    ChaosBox cb = new ChaosBox();
        //    SortedList<int, int> test = new SortedList<int, int>();
        //    int wrongNumber = 0;
        //    for (int i = 0; i < 10000; i++)
        //    {
        //        int a = cb.DrawOutInteger();
        //        int b = cb.DrawOutInteger();
        //        int c;

        //        if (a > b)
        //        { c = a; a = b; b = c; }
        //        //a = 100;
        //        //c = 10000;
        //        //a = 0.0267987465;
        //        //b = 300.3;
        //        //b = 1000.654989;
        //        a = 0;
        //        b = 1500;
        //        string s = cb.RandomMinMaxValue(a.ToString(), b.ToString());
        //        try
        //        {
        //            int d = int.Parse(s);
        //            if (d < a || d > b)
        //                throw new Exception();
        //            int key = (d >= 0 ? 1 : -1) * Math.Abs(d).ToString().Length;

        //            if (!test.ContainsKey(key))
        //                test.Add(key, 1);
        //            else
        //                test[key]++;                    
        //        }
        //        catch
        //        {
        //            TestContext.WriteLine($"{a} :A");
        //            TestContext.WriteLine($"{b} :B");
        //            TestContext.WriteLine($"Wrong:");
        //            TestContext.WriteLine(s);
        //            TestContext.WriteLine(s.Length.ToString());
        //            wrongNumber++;
        //            //TestContext.WriteLine(cb.RandomMinMaxValue(a.ToString(), b.ToString(), out int _));
        //        }

        //    }
        //    Assert.IsTrue(wrongNumber == 0);
        //    foreach (KeyValuePair<int, int> kv in test)
        //        TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        //}

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
        public void SmoothTestLimitedInteger()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                int b = cb.DrawOutInteger(0, 33);
                if (!test.ContainsKey(b))
                    test.Add(b, 1);
                else
                    test[b]++;
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

        //[TestMethod]
        //public void DrawOutStringTest()
        //{
        //    ChaosBox cb = new ChaosBox();
        //    for (int i = 1; i < 10; i++)
        //        TestContext.WriteLine(cb.DrawOutString(i));
        //    TestContext.WriteLine("----------");
        //    for (int i = 1; i < 20; i++)
        //        TestContext.WriteLine(cb.DrawOutRandomLengthString(100, Encoding.Unicode));
        //}

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

        //[TestMethod]
        //public void DrawOutNormalizedByteTest()
        //{
        //    //Is Normalize?
        //    ChaosBox cb = new ChaosBox();
        //    SortedList<byte, int> test = new SortedList<byte, int>();
        //    for (int i = 1; i < 10000; i++)
        //    {
        //        byte b = cb.DrawOutNormalizedByte();
        //        if (!test.ContainsKey(b))
        //            test.Add(b, 0);
        //        else
        //            test[b]++;
        //    }
        //    foreach (KeyValuePair<byte, int> kv in test)
        //        TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        //}


        //[TestMethod]
        //public void DrawOutNormalizedIntegerTest()
        //{
        //    ChaosBox cb = new ChaosBox();
        //    SortedList<int, int> test = new SortedList<int, int>();
        //    for (int i = 1; i < 10000; i++)
        //    {
        //        int b = cb.DrawOutNormalizedInteger(false);
        //        //int b = cb.DrawOutNormalizedInteger(-20000, 20000);                
        //        string s = Math.Abs(b).ToString();
        //        int key;
        //        key = Math.Abs((b - int.MaxValue / 2)).ToString().Length;
        //        //key = b;
        //        if (!test.ContainsKey(key))
        //            test.Add(key, 1);
        //        else
        //            test[key]++;
        //    }
        //    foreach (KeyValuePair<int, int> kv in test)
        //        TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        //}

        //[TestMethod]
        //public void DrawOutNormalizedByteTest2()
        //{
        //    ChaosBox cb = new ChaosBox();
        //    SortedList<byte, int> test = new SortedList<byte, int>();
        //    for (int i = 1; i < 10000; i++)
        //    {
        //        byte b = cb.DrawOutNormalizedByte(3, 5);
        //        if (!test.ContainsKey(b))
        //            test.Add(b, 0);
        //        else
        //            test[b]++;
        //    }
        //    foreach (KeyValuePair<byte, int> kv in test)
        //        TestContext.WriteLine($"{kv.Key}:{kv.Value}");
        //}

        [TestMethod]
        public void DrawOutDecimal()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, int> test = new SortedList<int, int>();
            for (int i = 1; i < 10000; i++)
            {
                decimal b = cb.DrawOutDecimal(true);
                //TestContext.WriteLine(b.ToString());
                int key;
                key = Mathematics.GetIntegerDigitsCount(b.ToString());
                if (!test.ContainsKey(key))
                    test.Add(key, 1);
                else
                    test[key]++;
            }
            foreach (KeyValuePair<int, int> kv in test)
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
                int b = cb.DrawOutInteger();
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
        public void DrawOutDateTimeTest()
        {
            ChaosBox cb = new ChaosBox();

            for (int i = 0; i < 10; i++)
            {
                TestContext.WriteLine(cb.DrawOutDateTime(DateTime.Now, DateTime.Now.AddHours(2)).ToString());
            }

        }
    
        [TestMethod]
        public void DrawOutUnrepetableIntegers()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<int, long> resultCount = new SortedList<int, long>();
            for (int i = 0; i < 1000; i++)
            {
                int[] result;
                result = cb.DrawOutIntegers(100, 500, 1020, false);
                for (int j = 0; j < result.Length; j++)
                {
                    if (resultCount.ContainsKey(result[j]))
                        resultCount[result[j]]++;
                    else
                        resultCount.Add(result[j], 1);
                }
            }

            //平穩度測試
            TestContext.WriteLine("StableTable:");
            for (int i = 0; i < resultCount.Count; i++)
                TestContext.WriteLine($"{resultCount.Keys[i]}:{resultCount.Values[i]}");
        }

        [TestMethod]
        public void DrawOutUnrepetableDoubles()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<double, long> resultCount = new SortedList<double, long>();
            for (int i = 0; i < 1000; i++)
            {
                double[] result;
                result = cb.DrawOutDoubles(100, -4623484.54813, 999994559.5484, false);
                for (int j = 0; j < result.Length; j++)
                {
                    if (resultCount.ContainsKey(result[j]))
                        resultCount[result[j]]++;
                    else
                        resultCount.Add(result[j], 1);
                }
            }

            //平穩度測試
            TestContext.WriteLine("StableTable:");
            for (int i = 0; i < resultCount.Count; i++)
                TestContext.WriteLine($"{resultCount.Keys[i]}:{resultCount.Values[i]}");
        }


        [TestMethod]
        public void DrawOutUnrepetableDecimals()
        {
            ChaosBox cb = new ChaosBox();
            SortedList<decimal, long> resultCount = new SortedList<decimal, long>();
            for (int i = 0; i < 1000; i++)
            {
                decimal[] result;
                result = cb.DrawOutDecimals(100, (decimal)123484.54813, (decimal)9994559.5484, false);
                for (int j = 0; j < result.Length; j++)
                {
                    if (resultCount.ContainsKey(result[j]))
                        resultCount[result[j]]++;
                    else
                        resultCount.Add(result[j], 1);
                }
            }

            //平穩度測試
            TestContext.WriteLine("StableTable:");
            for (int i = 0; i < resultCount.Count; i++)
                TestContext.WriteLine($"{resultCount.Keys[i]}:{resultCount.Values[i]}");
        }



        private int[] IdealC(int count, int minValue, int maxValue)
        {
            ChaosBox cb = new ChaosBox();
            //第一件事
            //做第一次隨機分count然後找出第一個差距確認
            int maxValue2 = 0;
            int minValue2 = count;
            int r = 0;
            for (int i = minValue; i < maxValue; i++)
            {
                r = cb.DrawOutInteger(0, count - 1);
                if (r < minValue2)
                    minValue2 = r;
                //minValue越高，代表最後一張牌的位置越近
            }
            maxValue -= minValue2;
            //TestContext.WriteLine(maxValue.ToString());
            //已知最後一張牌位置
            SortedList<int, int> choiceList = new SortedList<int, int>();
            for (int i = 0; i < count; i++)
                choiceList.Add(i, 0);

            for (int i = minValue; i < maxValue; i++)
            {
                r = cb.DrawOutInteger(0, count - 1);
                choiceList[r]++;
                //得到間距
            }

            List<int> reList = new List<int>(choiceList.Values);
            int[] result = new int[count];
            reList.Sort();

            int currentNumber = minValue;
            for (int i = 0; i < reList.Count; i++)
            {
                currentNumber += reList[i];
                result[i] = currentNumber;
            }
            return result;
        }

        [TestMethod]
        public void MathCount()
        {
            int count = 0;
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4 - i; j++)
                {
                    for (int k = 0; k < 4 - j - i; k++)
                    {
                        for (int l = 0; l < 4 - i - j - k; l++)
                        {
                            for(int m = 0; m < 4 - i - j - k - l; m++)
                            {
                                count++;
                                TestContext.WriteLine($"{count}:{i + 1}{j + 1}{k + 1}{l + 1}{m + 1}");
                            }
                        }
                    }
                }
            }
            
        }

        [TestMethod]
        public void MathTest()
        {            
            SortedList<int, int> AllNumbers = new SortedList<int, int>();
            for(int i = 0; i < 100; i++)
            {
                int[] result = IdealC(20, 30, 90);
                for (int j = 0; j < result.Length; j++)
                {
                    if (AllNumbers.ContainsKey(result[j]))
                        AllNumbers[result[j]] += 1;
                    else
                        AllNumbers.Add(result[j], 1);
                }
            }

            foreach(KeyValuePair<int, int> kvp in AllNumbers)
                TestContext.WriteLine($"{kvp.Key}:{kvp.Value}");        
        }
    }
}