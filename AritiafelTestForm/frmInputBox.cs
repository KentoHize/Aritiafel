using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aritiafel.Organizations;


namespace AritiafelTestForm
{
    public partial class frmInputBox : Form
    {
        public frmInputBox()
        {
            InitializeComponent();
        }

        public static string Show(IWin32Window owner)
        {
            frmInputBox frmInputBox = new frmInputBox();
            frmInputBox.ShowDialogOrCallEvent(owner);
            return frmInputBox.txtInputBox.Text;
        }

        public void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            txtInputBox.Text = "";
            DialogResult = DialogResult.Cancel;
        }
    }
}
