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
using Aritiafel.Items;
using System.IO;
using System.Diagnostics;
using Aritiafel.Locations;
using Aritiafel.Organizations.RaeriharUniversity;

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
            DialogResult dr = RabbitCouriers.SentNormalQuestion("點選OK或Cancel", "Test", Aritiafel.Items.ChoiceOptions.OKCancel, 2);

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
            RabbitCouriers.RegisterRMAndCI(Resources.Res.ResourceManager, new System.Globalization.CultureInfo("zh-TW"));
        }

        public void btnOpenFile_Click(object sender, EventArgs e)
        {   
            openFileDialog1.ShowDialogOrSetResult(this);
        }

        public void btnInputForm_Click(object sender, EventArgs e)
        {
            string result = frmInputBox.MyShow(this);
            RabbitCouriers.SentInformation(result);            
        }

        public void btnShowMessageByResource_Click(object sender, EventArgs e)
        {
            DialogResult dr = RabbitCouriers.SentNormalQuestionByResource("QuestionString", "Q1", ChoiceOptions.YesNo, "Male", "Female");
            DialogResult dr2 = RabbitCouriers.SentNormalQuestionByResource("QuestionString2", "Q2", ChoiceOptions.YesNo, "15", "16");

            if (dr == DialogResult.No && dr2 == DialogResult.Yes)
                RabbitCouriers.SentInformationByResource("AnswerString", "Answer");
        }

        private void btnTranslateZHCN_Click(object sender, EventArgs e)
        {
            string result;

            //byte[] bytes = Encoding.Default.GetBytes(txtText.Text);
            //result = Encoding.UTF8.GetString(bytes);

            result = WizardGuild.TranslateTextFromTraditionalChineseToSimplifiedChinese(txtText.Text);
                       //UnicodeEncoding.GetEncoding("UTF-8").GetEncoder();


            txtOutput.Text = result;
        }

        private void btnSaveUTF8_Click(object sender, EventArgs e)
        {
            string outputFile = Path.Combine(Application.StartupPath, "UTF-8.txt");

            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(txtOutput.Text);
                }
            }

            Process.Start(outputFile);
        }

        private void btnScript1_Click(object sender, EventArgs e)
        {
            //"C:\Programs\Standard\Aritiafel\Aritiafel"
            string file = @"C:\Programs\Standard\Aritiafel\Aritiafel\CFamilyNames.txt";
            string buffer;
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    buffer = sr.ReadToEnd();
                }
            }
            
            string outfile = @"C:\Programs\Standard\Aritiafel\Aritiafel\Data\SurnameOfChinese.csv";

            using (FileStream fs = new FileStream(outfile, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    int pos = 0;
                    while (pos < buffer.Length)
                    {
                        pos = buffer.IndexOf("php", pos + 1);
                        if (pos == -1)
                            break;
                        //MessageBox.Show(buffer.Substring(pos + 9, 1));
                        sw.WriteLine(buffer.Substring(pos + 5, 1));
                    }
                }
            }
            //" / word/"
        }

        private static class Settings
        {
            public static string aa { get; set; }
            public static DateTime DateTime { get; set; }
            public static Color Color { get; set; }
        }

        private void btnTestSettingShop_Click(object sender, EventArgs e)
        {
            //ArSettingGroup arSettingGroup = new ArSettingGroup();            
            //arSettingGroup.Add("aa", "bb", "Test");

            //Color.Red.ToArString();

            //AppLocation
            //Settings.aa = "dsadsa";
            //Settings.Color = Color.Red;
            //Settings.DateTime = DateTime.Now;
            //SettingShop.SaveIniFile(typeof(Settings));
            SettingShop.LoadIniFile(typeof(Settings));
            MessageBox.Show(Settings.DateTime.ToString());
        }

        private void btnShowMessageFromLanguageFile_Click(object sender, EventArgs e)
        {
            RabbitCouriers.RegisterLaguageFolderAndCI(@"C:\Programs\Aritiafel\AritiafelTestForm\Language", new System.Globalization.CultureInfo("zh-TW"));
            RabbitCouriers.SentInformationByResource("RES_APPLE", "Test");
            RabbitCouriers.SentInformationByResource("RES_BAT", "Test");
        }

        private void btnTestDateTime_Click(object sender, EventArgs e)
        {
            ArDateTime ad = ArDateTime.Parse(txtDateTime.Text, null, System.Globalization.DateTimeStyles.None);
            txtOutput.Text = ad.ToString();
        }
    }
}
