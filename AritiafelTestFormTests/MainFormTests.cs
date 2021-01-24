using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using AritiafelTestForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aritiafel.Organizations;
using Aritiafel.Characters;

namespace AritiafelTestForm.Tests
{
    [TestClass]
    public class MainFormTests
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
            => AdventurerAssociation.RegisterMembers();

        [TestCleanup]
        public void TestCleanup()
        { }

        [TestMethod()]
        public void btnMessageBox_ClickTest()
        {
            MainForm mf = new MainForm();
            mf.btnMessageBox_Click(mf, new EventArgs());            
            mf.Close();
            AdventurerAssociation.PrintMessageFromCourier(TestContext);
        }

        [TestMethod]
        public void btnMessageBox2_ClickTest()
        {
            MainForm mf = new MainForm();

            AdventurerAssociation.RegisterMember(new Courier(InputResponseOptions.OK));
            mf.btnMessageBox2_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromCourier(TestContext);
            AdventurerAssociation.RegisterMember(new Courier(InputResponseOptions.Retry));
            mf.btnMessageBox2_Click(mf, new EventArgs());
            mf.Close();
            AdventurerAssociation.PrintMessageFromCourier(TestContext);
        }


        [TestMethod]
        [TestProperty("FileName", @"‪C:\WebSite\GoogleDrive\ArinaQuotes.txt")]
        public void btnOpenFile_ClickTest()
        {
            AdventurerAssociation.RefreshInput(TestContext.Properties);
            MainForm mf = new MainForm();
            mf.btnOpenFile_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromBard(TestContext);

            AdventurerAssociation.RegisterMember(new Bard("FileName", @"‪C:\WebSite\GoogleDrive\ArinaArticles.txt"));
            mf.btnOpenFile_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromBard(TestContext);
            mf.Close();
        }

        [TestMethod]
        public void btnOpenFile_ClickTest2()
        {
            MainForm mf = new MainForm();

            AdventurerAssociation.RegisterMember(new Bard("FileName", @"‪C:\WebSite\GoogleDrive\ArinaQuotes.txt"));
            mf.btnOpenFile_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromBard(TestContext);

            AdventurerAssociation.RegisterMember(new Bard("FileName", @"‪C:\WebSite\GoogleDrive\ArinaArticles.txt"));
            mf.btnOpenFile_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromBard(TestContext);

            mf.Close();
        }



        //[DataTestMethod]
        //[DataRow(1, 2, 3)]
        //[DataRow(2, 3, 5)]
        //[DataRow(3, 5, 8)]
        //[TestProperty("a", "0")]
        //public void AdditionTest(int a, int b, int result)
        //{
        //    Assert.AreEqual(result, a + b + Convert.ToInt32(TestContext.Properties["a"]));
        //}
    }
}