using Aritiafel.Characters;
using Aritiafel.Items;
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace Aritiafel.Organizations
{
    /// <summary>
    /// 冒險者公會，主要處理表單測試問題
    /// </summary>
    public static class AdventurerAssociation
    {
        public static Bard Bard { get; private set; }

        public static Courier Courier { get; private set; }

        public static Archivist Archivist { get; private set; }

        public static bool Registered { get; private set; }

        public delegate DialogResult FormStartDelegate (Form newForm);

        public static event FormStartDelegate Form_Start;

        /// <summary>
        /// 冒險者公會成員註冊
        /// </summary>
        public static void RegisterMembers()
            => RegisterMembers(null);

        /// <summary>
        /// 冒險者公會成員註冊
        /// </summary>
        /// <param name="output">輸出流</param>
        public static void RegisterMembers(Stream output)
        {
            if (!Registered)
                RegisterMembers(new Bard(), new Courier(), new Archivist(output));
        }

        /// <summary>
        /// 冒險者公會成員解散
        /// </summary>
        public static void DismissMembers()
        {
            Bard = null;
            Courier = null;            
            Archivist = null;
            Registered = false;
        }

        private static void UpdateRegisteredState()
            => Registered = Bard != null && Courier != null && Archivist != null;

        /// <summary>
        /// 註冊詩人、信使與文件管理員
        /// </summary>
        /// <param name="bard">新詩人</param>
        /// <param name="courier">新信使</param>
        /// <param name="archivist">新文件管理員</param>
        public static void RegisterMembers(Bard bard, Courier courier, Archivist archivist)
        {
            RegisterMember(bard);
            RegisterMember(courier);
            RegisterMember(archivist);
            UpdateRegisteredState();
        }

        /// <summary>
        /// 註冊詩人
        /// </summary>
        /// <param name="newcomer">新詩人</param>
        public static void RegisterMember(Bard newcomer)
        {
            if (newcomer != null)
                Bard = newcomer;
            UpdateRegisteredState();
        }

        /// <summary>
        /// 註冊信使
        /// </summary>
        /// <param name="newcomer">新信使</param>
        public static void RegisterMember(Courier newcomer)
        {
            if (newcomer != null)
                Courier = newcomer;
            UpdateRegisteredState();
        }

        /// <summary>
        /// 註冊文件管理員
        /// </summary>
        /// <param name="newcomer">新文件管理員</param>
        public static void RegisterMember(Archivist newcomer)
        {
            if (newcomer != null)
                Archivist = newcomer;
            UpdateRegisteredState();
        }

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
        /// 從文件管理員上傾倒逐行訊息(全部流程)
        /// </summary>
        /// <param name="writeLineObject">可以WriteLine的任意Object</param>
        public static void PrintMessageFromArchivist(object writeLineObject)
        {
            foreach (string record in Archivist.Records)
                writeLineObject.GetType().InvokeMember("WriteLine", BindingFlags.InvokeMethod,
                    null, writeLineObject, new object[] { record });
            Archivist.ClearRecords();
        }

        /// <summary>
        /// 顯示訊息視窗或設置結果(測試時)，從信使身上讀取輸入資料
        /// </summary>
        /// <param name="message">訊息</param>
        /// <returns>結果</returns>
        public static DialogResult ShowNewMessageOrSetResult(ArMessage message)
            => ShowNewMessageOrSetResult(message, null);

        /// <summary>
        /// 顯示訊息視窗或設置結果(測試時)，從信使身上讀取輸入資料
        /// </summary>
        /// <param name="message">訊息</param>
        /// <param name="owner">父表單</param>
        /// <returns>結果</returns>
        public static DialogResult ShowNewMessageOrSetResult(ArMessage message, IWin32Window owner)
        {
            if (!Registered)
                if (owner == null)
                    return MessageBox.Show(message.Content, message.Title,
                        (MessageBoxButtons)(byte)message.ChoiceOption,
                        (MessageBoxIcon)(byte)message.LevelOfEergency,
                        (MessageBoxDefaultButton)((message.DefaultResponse - 1) * 256));
                else
                    return MessageBox.Show(owner, message.Content, message.Title,
                        (MessageBoxButtons)(byte)message.ChoiceOption,
                        (MessageBoxIcon)(byte)message.LevelOfEergency,
                        (MessageBoxDefaultButton)((message.DefaultResponse - 1) * 256));

            DialogResult dr = (DialogResult)Enum.Parse(typeof(DialogResult), message.GetDefaultResponse());
            string record;

            Courier.Package = message;
            record = message.ToString();
            Courier.MessageReceived.Add(record);
            Archivist.WriteRecord(record);

            string response = Courier.GetResponse(message.ID);
            if (!string.IsNullOrEmpty(response))
                dr = (DialogResult)Enum.Parse(typeof(DialogResult), response);                

            if (!message.ChoiceOption.ToString().Contains(dr.ToString()))
                throw new InvalidCastException("DialogResult");

            record = $"DialogResult = [{dr}]";
            Courier.MessageReceived.Add(record);
            Archivist.WriteRecord(record);            
                
            return dr;
        }

        /// <summary>
        /// 顯示一般對話視窗或是設置結果(測試時)，從詩人身上讀取輸入資料
        /// </summary>
        /// <param name="cd">對話視窗實體</param>
        /// <returns>結果</returns>
        public static DialogResult ShowDialogOrSetResult(this CommonDialog cd)
            => cd.ShowDialogOrSetResult(null);

        /// <summary>
        /// 顯示一般對話視窗或是設置結果(測試時)，從詩人身上讀取輸入資料
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
            string record;
            DialogResult dr = DialogResult.OK;
            PropertyInfo[] cdProps = cd.GetType().GetProperties();

            record = $"Open Dialog: \"{cd.GetType().Name}\"";
            Bard.MessageReceived.Add(record);
            Archivist.WriteRecord(record);

            foreach (PropertyInfo pi in cdProps)
            {
                object value = Bard.InputInformation[$"{cd.GetType().Name}.{pi.Name}"] ??
                    Bard.InputInformation[pi.Name];

                if (value != null)
                {
                    pi.SetValue(cd, value);
                    record = $"{dialogTypeName}.{pi.Name} = {value}";
                    Bard.MessageReceived.Add(record);
                    Archivist.WriteRecord(record);
                }
            }

            if (Bard.InputInformation["DialogResult"] != null)
                dr = (DialogResult)Bard.InputInformation["DialogResult"];
            else if (Bard.InputInformation[$"{dialogTypeName}.DialogResult"] != null)
                dr = (DialogResult)Bard.InputInformation[$"{dialogTypeName}.DialogResult"];

            if (dr != DialogResult.OK && dr != DialogResult.Cancel)
                throw new InvalidCastException("DialogResult");

            record = $"DialogResult = [{dr}]";
            Bard.MessageReceived.Add(record);
            Archivist.WriteRecord(record);
            return dr;
        }

        /// <summary>
        /// 顯示表單或是觸發事件(測試時)，從詩人身上讀取輸入資料
        /// </summary>
        /// <param name="form">顯示的表單</param>
        public static void ShowOrCallEvent(this Form form)
            => ShowOrCallEvent(form, null, false);

        /// <summary>
        /// 顯示表單或是觸發事件(測試時)，從詩人身上讀取輸入資料
        /// </summary>
        /// <param name="form">顯示的表單</param>
        /// <param name="owner">父表單</param>
        public static void ShowOrCallEvent(this Form form, IWin32Window owner)
            => ShowOrCallEvent(form, owner, false);

        /// <summary>
        /// 顯示表單或是觸發事件(測試時)，從詩人身上讀取輸入資料
        /// </summary>
        /// <param name="form">顯示的表單</param>
        /// <returns>結果</returns>
        public static DialogResult ShowDialogOrCallEvent(this Form form)
            => ShowOrCallEvent(form, null, true);

        /// <summary>
        /// 顯示表單或是觸發事件(測試時)，從詩人身上讀取輸入資料
        /// </summary>
        /// <param name="form">顯示的表單</param>
        /// <param name="owner">父表單</param>
        /// <returns>結果</returns>
        public static DialogResult ShowDialogOrCallEvent(this Form form, IWin32Window owner)
            => ShowOrCallEvent(form, owner, true);

        //Implement
        private static DialogResult ShowOrCallEvent(Form form, IWin32Window owner, bool asDialog)
        {            
            if (!Registered)
            { 
                if(asDialog)
                {
                    if (owner == null)
                        return form.ShowDialog();
                    else
                        return form.ShowDialog(owner);
                }
                else
                {
                    if (owner == null)
                        form.Show();
                    else
                        form.Show(owner);
                    return DialogResult.None;
                }
            }
            DialogResult dr = form.DialogResult;
            PropertyInfo[] formProps = form.GetType().GetProperties();
            string record;

            record = $"Show Form: \"{form.Name}\"";
            Bard.MessageReceived.Add(record);
            Archivist.WriteRecord(record);

            foreach (PropertyInfo pi in formProps)
            {
                object value = Bard.InputInformation[$"{form.Name}.{pi.Name}"] ??
                    Bard.InputInformation[pi.Name];

                if (value != null)
                {
                    pi.SetValue(form, value);
                    record = $"{form.Name}.{pi.Name} = {value}";
                    Bard.MessageReceived.Add(record);
                    Archivist.WriteRecord(record);
                }
            }

            if (Bard.InputInformation["DialogResult"] != null)
                dr = (DialogResult)Bard.InputInformation["DialogResult"];
            else if (Bard.InputInformation[$"{form.Name}.DialogResult"] != null)
                dr = (DialogResult)Bard.InputInformation[$"{form.Name}.DialogResult"];

            if (Form_Start != null)
                dr = Form_Start(form);

            record = $"DialogResult = [{dr}]";
            Bard.MessageReceived.Add(record);
            Archivist.WriteRecord(record);

            return dr;
        }
    }
}
