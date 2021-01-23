using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using AritiafelTestForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritiafelTestForm.Tests
{
    [TestClass]
    public class MainFormTests
    {
        public TestContext TestContext { get; set; }
        
        [TestMethod]
        public void btnMessageBox2_ClickTest()
        {
            MainForm mf = new MainForm();
            
            mf.btnMessageBox2_Click(mf, new EventArgs());
            mf.btnMessageBox2_Click(mf, new EventArgs());

            TestContext.WriteLine(Var.VarA);
            Console.WriteLine("aaaaa");
            //aaa = "dsdas";
            //?Var b
            //Assert.Fail();
        }

        [TestMethod]
        public void Test2()
        {
            Assert.Fail();
        }
    }
}