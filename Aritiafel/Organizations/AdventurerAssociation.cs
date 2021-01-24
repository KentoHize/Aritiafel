using Aritiafel.Characters;
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace Aritiafel.Organizations
{
    public static class AdventurerAssociation
    {
        public static Bard Bard { get; set; }

        public static Courier Courier { get; private set; } = new Courier();

        public static bool Registered { get; private set; }

        public static void RegisterMember()
        { 
            if (!Registered)
                RegisterMember(new Bard());
        }

        public static void RegisterMember(Bard member)
        {
            Bard = member;
            Registered = true;
        }

        public static void RegisterMemberAndRefreshInput(IDictionary inputInformation)
        {
            if (!Registered)
                RegisterMember();
            RefreshInput(inputInformation);
        }

        public static void RefreshInput(IDictionary inputInformation)
            => Bard.InputInformation = inputInformation;        

        /// <summary>
        /// 從詩人身上傾倒逐行訊息
        /// </summary>
        /// <param name="writeLineObject">可以WriteLine的任意Object</param>
        public static void PrintMessageFromBard(object writeLineObject)
        {
            foreach(string message in Bard.MessageReceived)
            writeLineObject.GetType().InvokeMember("WriteLine", BindingFlags.InvokeMethod, 
                null, writeLineObject, new object[] { message });
            Bard.MessageReceived.Clear();
        }

        public static DialogResult NewMessage(string message)
        {
            if (!Registered)
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
            
            if (Bard.InputInformation == null)
                throw new ArgumentNullException("Text Context Connected Failed");

            string dialogTypeName = cd.GetType().Name;
            DialogResult dr = DialogResult.OK;
            PropertyInfo[] cdProps = cd.GetType().GetProperties();

            foreach (PropertyInfo pi in cdProps)
            {
                object value = Bard.InputInformation[$"{cd.GetType().Name}.{pi.Name}"] ??
                    Bard.InputInformation[pi.Name];

                if (value != null)
                {
                    pi.SetValue(cd, value);
                    Bard.MessageReceived.Add($"{dialogTypeName}.{pi.Name} = {value}");
                }
            }

            if (Bard.InputInformation["DialogResult"] != null)
            {
                dr = (DialogResult)Bard.InputInformation["DialogResult"];
                Bard.MessageReceived.Add($"DialogResult = {dr}");
            }
            return dr;
        }
    }
}
