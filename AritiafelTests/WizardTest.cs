using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod]
        public void ResourceFileTranslateToZHCN()
        {
            WizardGuild.ProduceSimplifiedChineseResourceFile(@"C:\Programs\WinForm\JsonEditorV2\JsonEditorV2\Resources\Res.resx");
        }
    }
}
