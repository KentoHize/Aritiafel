using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using Aritiafel.Characters;

namespace Aritiafel.Organizations
{
    public static class AdventurerAssociation
    {
        public static Bard Bard { get; private set; }

        public static Courier Courier { get; private set; } = new Courier();

        public static bool Registered { get; private set; }

        public static void RegisterMember()
            => RegisterMember(new Bard());

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

        private static void CheckInputInformationIsNew()
        {
            if (Bard.InputInfomation == null)
                throw new ArgumentNullException("Text Context Connected Failed");
        }   

        public static DialogResult NewMessage(string message)
        {
            if(!Registered)
                return MessageBox.Show(message);

            //Test
            Bard.MessageReceived.Add("NewMessage:" + message);
            return DialogResult.Cancel;
        }

        /// <summary>
        /// 顯示對話視窗或是設置結果(測試時)
        /// </summary>
        /// <param name="cd">對話視窗實體</param>
        /// <returns>結果</returns>
        public static DialogResult ShowDialogOrSetResult(this CommonDialog cd)
            => cd.ShowDialogOrSetResult(null);

        /// <summary>
        /// 顯示對話視窗或是設置結果(測試時)
        /// </summary>
        /// <param name="cd">對話視窗實體</param>
        /// <param name="owner">父視窗</param>
        /// <returns>結果</returns>
        public static DialogResult ShowDialogOrSetResult(this CommonDialog cd, IWin32Window owner)
        {
            if (!Registered)
                if (owner != null)
                    return cd.ShowDialog(owner);
                else
                    return cd.ShowDialog();

            CheckInputInformationIsNew();

            string dialogTypeName = cd.GetType().Name;
            DialogResult dr = DialogResult.OK;
            PropertyInfo[] cdProps = cd.GetType().GetProperties();

            foreach (PropertyInfo pi in cdProps)
            {
                if (Bard.InputInfomation[pi.Name] != null)
                { 
                    pi.SetValue(cd, Bard.InputInfomation[pi.Name]);
                    Bard.MessageReceived.Add($"{dialogTypeName} {pi.Name} = {Bard.InputInfomation[pi.Name]}");
                }
            }

            if (Bard.InputInfomation["DialogResult"] != null)
            {
                dr = (DialogResult)Bard.InputInfomation["DialogResult"];
                Bard.MessageReceived.Add($"DialogResult = {dr}");
            }
            return dr;
        }
    }
}
