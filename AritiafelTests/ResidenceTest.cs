using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Locations;

namespace AritiafelTest
{
    [TestClass]
    public class ResidenceTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SaveProjectTest()
        {
            string sourceDir = @"C:\Programs\Standard\Aritiafel";
            string targetDir = @"E:\Backup";

            Residence rs = new Residence(targetDir);
            rs.SaveVSSolution(sourceDir, false);

        }
    }
}
