using Aritiafel.Characters;
using Aritiafel.Items;
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;

namespace Aritiafel.Organizations
{
    public static class AdventurerAssociation
    {
        public static Bard Bard { get; set; }

        public static Courier Courier { get; set; }

        public static bool Registered { get; private set; }

        public static void RegisterMember()
        {
            if (!Registered)
                RegisterMember(new Bard(), new Courier());
        }

        public static void RegisterMember(Bard bard, Courier courier)
        {
            Bard = bard;
            Courier = courier;
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
            foreach (string message in Bard.MessageReceived)
                writeLineObject.GetType().InvokeMember("WriteLine", BindingFlags.InvokeMethod,
                    null, writeLineObject, new object[] { message });
            Bard.MessageReceived.Clear();
        }

        /// <summary>
        /// 從信使身上傾倒逐行訊息
        /// </summary>
        /// <param name="writeLineObject">可以WriteLine的任意Object</param>
        public static void PrintMessageFromCourier(object writeLineObject)
        {
            foreach (string message in Courier.MessageReceived)
                writeLineObject.GetType().InvokeMember("WriteLine", BindingFlags.InvokeMethod,
                    null, writeLineObject, new object[] { message });
            Courier.MessageReceived.Clear();
        }

        /// <summary>
        /// 顯示訊息視窗或設置結果(測試時)
        /// </summary>
        /// <param name="message">訊息</param>
        /// <returns>結果</returns>
        public static DialogResult ShowNewMessageOrSetResult(ArMessage message)
            => ShowNewMessageOrSetResult(message, null);

        /// <summary>
        /// 顯示訊息視窗或設置結果(測試時)
        /// </summary>
        /// <param name="message">訊息</param>
        /// <param name="owner">父表單</param>
        /// <returns>結果</returns>
        public static DialogResult ShowNewMessageOrSetResult(ArMessage message, IWin32Window owner)
        {
            if (!Registered)
                if (owner == null)
                    return MessageBox.Show(message.Content, message.Title,
                        (MessageBoxButtons)(byte)message.ResponseOption,
                        (MessageBoxIcon)(byte)message.LevelOfEergency,
                        (MessageBoxDefaultButton)((message.DefaultResponse - 1) * 256));
                else
                    return MessageBox.Show(owner, message.Content, message.Title,
                        (MessageBoxButtons)(byte)message.ResponseOption,
                        (MessageBoxIcon)(byte)message.LevelOfEergency,
                        (MessageBoxDefaultButton)((message.DefaultResponse - 1) * 256));

            DialogResult dr = DialogResult.Cancel;
            Courier.MessageReceived.Add(message.ToString());
            if (Courier.InputResponse != null)
            {
                dr = (DialogResult)Enum.Parse(typeof(DialogResult), Courier.InputResponse);
                Courier.MessageReceived.Add($"DialogResult = {dr}");
            }            
            return dr;
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
        /// <param name="owner">父表單</param>
        /// <returns>結果</returns>
        public static DialogResult ShowDialogOrSetResult(this CommonDialog cd, IWin32Window owner)
        {
            if (!Registered)
                if (owner == null)
                    return cd.ShowDialog();
                else
                    return cd.ShowDialog(owner);


            if (Bard.InputInformation == null)
                throw new ArgumentNullException("Text Context Connected Failed");

            string dialogTypeName = cd.GetType().Name;
            DialogResult dr = DialogResult.OK;
            PropertyInfo[] cdProps = cd.GetType().GetProperties();

            Bard.MessageReceived.Add($"Open Dialog: \"{cd.GetType().Name}\"");

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
