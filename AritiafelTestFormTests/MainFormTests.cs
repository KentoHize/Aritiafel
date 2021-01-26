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
using System.IO;

namespace AritiafelTestForm.Tests
{
    [TestClass]
    public class MainFormTests
    {
        public TestContext TestContext { get; set; }

        static FileStream fs;

        MemoryStream ms;

        [TestInitialize]
        public void TestInitialize()
        {
            if (!AdventurerAssociation.Registered)
            {
                RabbitCouriers.RegisterRMAndCI(Resources.Res.ResourceManager, new System.Globalization.CultureInfo("zh-TW"));

                fs = new FileStream(@"C:\Programs\TestArea\TestOutput2.txt", FileMode.Create);
                //ms = new MemoryStream();
                AdventurerAssociation.RegisterMembers(fs);
                AdventurerAssociation.Form_Start += AdventurerAssociation_Form_Start;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //MessageBox.Show((AdventurerAssociation.Archivist.Stream.Length).ToString());
            //MessageBox.Show(ms.ToString());

        }

        [ClassCleanup]
        public static void CleanUp()
        {
            if (fs != null)
                fs.Close();
        }

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

            AdventurerAssociation.RegisterMember(new Courier(ResponseOptions.OK));
            mf.btnMessageBox2_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromCourier(TestContext);
            AdventurerAssociation.RegisterMember(new Courier(ResponseOptions.OK));
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

        [TestMethod]
        public void btnInputForm_ClickTest()
        {
            MainForm mf = new MainForm();
            AdventurerAssociation.Archivist.ClearRecords();
            mf.btnInputForm_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromArchivist(TestContext);

            mf.Close();
        }

        private DialogResult AdventurerAssociation_Form_Start(Form newForm)
        {
            AdventurerAssociation.PrintMessageFromArchivist(TestContext);
            if (newForm.Name == "frmInputBox")
            {
                frmInputBox frmInputBox = newForm as frmInputBox;
                (frmInputBox.Controls.Find("txtInputbox", false)[0] as TextBox).Text = "A New Record";
                TestContext.WriteLine("this");
                frmInputBox.btnOK_Click(frmInputBox, new EventArgs());
                return newForm.DialogResult;
            }
            return DialogResult.None;
        }

        [TestMethod]
        public void btnShowMessageByResource_ClickTest()
        {
            AdventurerAssociation.Archivist.ClearRecords();            
            MainForm mf = new MainForm();

            Courier courier = new Courier();

            courier.AddResponse(ResponseOptions.Yes, "QuestionString2");
            courier.AddResponse(ResponseOptions.No, "QuestionString");
            courier.AddResponse(ResponseOptions.Yes, "QuestionString");
            courier.AddResponse(ResponseOptions.OK);

            AdventurerAssociation.RegisterMember(courier);
            mf.btnShowMessageByResource_Click(mf, new EventArgs());
            AdventurerAssociation.PrintMessageFromArchivist(TestContext);
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