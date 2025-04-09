using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aritiafel.Items;
using Aritiafel.Organizations;
using Aritiafel.Organizations.ArinaOrganization;
using Aritiafel.Organizations.RaeriharUniversity;



namespace AritiafelTest
{
    [TestClass]
    public class RyabrarFactoryTest
    {
        public TestContext TestContext { get; set; }


        //[+-]?\d+(?:\.\d+)?
        //[+-]?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?
        [TestMethod]
        public void TestArea()
        {
            //ArCultureInfo ci = RyabrarFactory.CreateOrGet<ArCultureInfo>();
            //TestContext.WriteLine(RyabrarFactory.Exists(typeof(ArCultureInfo)).ToString());
            //RyabrarFactory.Remove(new ArProductInfo(typeof(ArCultureInfo)));
            //TestContext.WriteLine(RyabrarFactory.Exists(typeof(ArCultureInfo)).ToString());
            //TestContext.WriteLine(ci.DisplayName);
            //TestContext.WriteLine(ci.Name);
            string s = "100";
            Stopwatch sw = Stopwatch.StartNew();
            Match m;
            for(int i = 0; i < 1000; i++)
                m = Regex.Match(s, "^100");
            sw.Stop();
            TestContext.WriteLine(sw.ToString());
            sw.Reset();
            sw.Start();
            for (int i = 0; i < 1000; i++)
                s.StartsWith("100");
            sw.Stop();
            TestContext.WriteLine(sw.ToString());

            //MatchCollection mc = Regex.Matches("202020.33", "[+-]?\\d+(?:\\.\\d+)?");
            //MatchCollection mc = Regex.Matches("202020.33", "[+-]?\\d+(\\.\\d+)?");            
            //foreach (Match m in mc)
            //{
            //    TestContext.WriteLine(m.Value);
            //}
            //CultureInfo ct = RyabrarFactory.CreateOrGet<CultureSupportNegativeYear>(
            //    new ArProductInfo(typeof(CultureSupportNegativeYear), CultureInfo.CurrentCulture.Name));

            //Console.WriteLine( ct.OptionalCalendars[0].ToString());
            //ct.Calendar = new ArNegativeCalendar();
            //ArProductInfo pi = new ArProductInfo(typeof(ArCultureInfo), "zh-AA", true);
            //ci = (ArCultureInfo)RyabrarFactory.CreateOrGet(pi);
            //TestContext.WriteLine(ci.DisplayName);
        }
    }
}
