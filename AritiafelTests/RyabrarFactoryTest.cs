using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        [TestMethod]
        public void TestArea()
        {
            ArCultureInfo ci = RyabrarFactory.CreateOrGet<ArCultureInfo>();
            TestContext.WriteLine(RyabrarFactory.Exists(typeof(ArCultureInfo)).ToString());
            RyabrarFactory.Remove(new ArProductInfo(typeof(ArCultureInfo)));
            TestContext.WriteLine(RyabrarFactory.Exists(typeof(ArCultureInfo)).ToString());
            TestContext.WriteLine(ci.DisplayName);
            TestContext.WriteLine(ci.Name);

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
