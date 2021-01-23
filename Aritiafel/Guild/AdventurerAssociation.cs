using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Aritiafel.Guild
{
    public static class AdventurerAssociation
    {
        public static string UserName { get; set; }

        public static void RegisterAsAMember(string name)
        {
            UserName = name;
        }

        public static DialogResult NewMessage(string message)
        {
            if(string.IsNullOrEmpty(UserName))
            {
                //MessageBox.Show("This is a Test");
                return DialogResult.Cancel;
            }   
            else
            {   
                MessageBox.Show(message);
                return DialogResult.OK;
            }
        }
    }
}
