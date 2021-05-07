﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aritiafel.Organizations;


namespace AritiafelTest
{
    [TestClass]
    public class WizardTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ResourceFileTranslateToZHCN()
        {
            WizardGuild.ProduceSimplifiedChineseResourceFile(@"C:\Programs\WinForm\JsonEditorV2\JsonEditorV2\Resources\Res.resx");
        }

        [TestMethod]
        public void RandomName()
        {
            for(int i = 0; i < 100; i++)
                TestContext.WriteLine(WizardGuild.RandomChineseFemaleName());
        }
    }
}
