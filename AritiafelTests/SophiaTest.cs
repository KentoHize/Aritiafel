using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aritiafel.Artifacts;
using Aritiafel.Characters.Heroes;
using Aritiafel.Items;
using Aritiafel.Organizations;
using Aritiafel.Organizations.ArinaOrganization;
using Aritiafel.Organizations.RaeriharUniversity;

namespace AritiafelTest
{
    [TestClass]
    public class SophiaTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestArea()
        {
            Allseer.RegisterCustomSeeThroughFunction<int>(m => (m + 3).ToString());
            Allseer.RemoveCustomSeeThroughFunction<int>();
            Sophia.SeeThrough(5);
        }
    }
}
