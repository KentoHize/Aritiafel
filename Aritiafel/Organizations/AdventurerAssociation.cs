using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Aritiafel.Characters;

namespace Aritiafel.Organizations
{
    public static class AdventurerAssociation
    {
        public static Bard Bard { get; set; }

        public static bool Registered { get; private set; }

        public static void RegisterMember()
            => RegisterMember(new Bard("Claire"));

        public static void RegisterMember(Bard member)
        {
            Bard = member;
            Registered = true;
        }

        public static void RegisterMemberAndRefreshInput(IDictionary inputInformation)
        {
            if (!Registered)
                RegisterMember();
            Bard.InputInfomation = inputInformation;
        }

        public static DialogResult NewMessage(string message)
        {
            if(!Registered)
                return MessageBox.Show(message);

            //Test
            Bard.MessageReceived.Add("NewMessage:" + message);
            return DialogResult.Cancel;
        }

        public static DialogResult ShowDialog2(this OpenFileDialog ofd)
            => ShowDialog2(ofd, null);

        public static DialogResult ShowDialog2(this OpenFileDialog ofd, IWin32Window owner)
        {   
            if(!Registered)
                return ofd.ShowDialog(owner);
            
            ////Test
            //if (Bard.InputInfomation["FileName"] == null)
            //    Bard.MessageReceived.Add("FileName is null");
            //else
            ofd.FileName = Bard.InputInfomation["FileName"].ToString();
            Bard.MessageReceived.Add("ShowFileDialog => Set FileName to" + ofd.FileName);
            return DialogResult.OK;
        }
    }
}
