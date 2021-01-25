using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aritiafel.Characters;
using Aritiafel.Organizations;

namespace AritiafelTestForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
        }

        public void btnMessageBox_Click(object sender, EventArgs e)
        {
            DialogResult dr = RabbitCouriers.SentNoramlQuestion("點選OK或Cancel", "Test", Aritiafel.Items.ResponseOption.OKCancel, 2);

            if (dr == DialogResult.Cancel)
                Console.WriteLine(1);
            else
                Console.WriteLine(2);
        }

        public void btnMessageBox2_Click(object sender, EventArgs e)
        {
            Var.VarA = Var.VarA += "T";

            RabbitCouriers.SentInformation(Var.VarA);
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        public void btnOpenFile_Click(object sender, EventArgs e)
        {   
            openFileDialog1.ShowDialogOrSetResult(this);
        }

        public void btnInputForm_Click(object sender, EventArgs e)
        {
            string result = frmInputBox.Show(this);
            RabbitCouriers.SentInformation(result);            
        }
    }
}
