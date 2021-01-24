using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using AritiafelTestForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aritiafel.Organizations;

namespace AritiafelTestForm.Tests
{
    [TestClass]
    public class MainFormTests
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
            => AdventurerAssociation.RegisterMemberAndRefreshInput(TestContext.Properties);

        [TestCleanup]
        public void TestCleanup()
        {   
            foreach (string s in AdventurerAssociation.Bard.MessageReceived)
                TestContext.WriteLine(s);
            AdventurerAssociation.Bard.MessageReceived.Clear();
        }

        [TestMethod]
        public void btnMessageBox2_ClickTest()
        {
            MainForm mf = new MainForm();

            mf.btnMessageBox2_Click(mf, new EventArgs());
            mf.btnMessageBox2_Click(mf, new EventArgs());
            mf.Close();         
        }

            
        [TestMethod]
        [TestProperty("FileName", @"‪C:\WebSite\GoogleDrive\ArinaQuotes.txt")]
        public void btnOpenFile_ClickTest()
        {   
            MainForm mf = new MainForm();           
            mf.btnOpenFile_Click(mf, new EventArgs());

            TestContext.Properties["FileName"] = @"C:\WebSite\GoogleDrive\ArinaArticles.txt";
            mf.btnOpenFile_Click(mf, new EventArgs());            
            mf.Close();           
        }

        [TestMethod]
        [TestProperty("FileName", @"‪C:\WebSite\GoogleDrive\ArinaQuotes.txt")]
        public void btnOpenFile_ClickTest2()
        {
            MainForm mf = new MainForm();
            mf.btnOpenFile_Click(mf, new EventArgs());

            TestContext.Properties["FileName"] = @"C:\WebSite\GoogleDrive\ArinaArticles.txt";
            mf.btnOpenFile_Click(mf, new EventArgs());
            mf.Close();
        }

        [DataTestMethod]
        [DataRow(1, 2, 3)]
        [DataRow(2, 3, 5)]
        [DataRow(3, 5, 8)]
        [TestProperty("a", "0")]
        public void AdditionTest(int a, int b, int result)
        {
            Assert.AreEqual(result, a + b + Convert.ToInt32(TestContext.Properties["a"]));
        }
    }
}