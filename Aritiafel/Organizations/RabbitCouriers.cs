using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using Aritiafel.Items;

namespace Aritiafel.Organizations
{
    /// <summary>
    /// 兔子快遞，快速傳達包裹、訊息及取得在地化翻譯
    /// </summary>
    public static class RabbitCouriers
    {
        public static ResourceManager ResourceManager { get; private set; }
        public static CultureInfo CultureInfo { get; private set; }

        private static Dictionary<string, string> languagePairs;

        //第一種方法
        public static void RegisterRMAndCI(ResourceManager rm, CultureInfo ci)
        {
            languagePairs = null;
            ResourceManager = rm;
            CultureInfo = ci;
        }

        //第二種方法
        public static void RegisterLaguageFolderAndCI(string languageFilesFolder, CultureInfo ci)
        {
            ResourceManager = null;
            CultureInfo = ci;
            if (!Directory.Exists(languageFilesFolder))
                throw new DirectoryNotFoundException(languageFilesFolder);
            string file = Path.Combine(languageFilesFolder, $"{ci.Name}.json");
            if (!File.Exists(file))
                throw new CultureNotFoundException(ci.Name);

            using (StreamReader sr = new StreamReader(file))
            {
                List<KeyValuePair<string, string>> lkv = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(sr.ReadToEnd());
                languagePairs = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> kvp in lkv)
                    languagePairs.Add(kvp.Key.ToUpper(), kvp.Value);
            }
        }

        public static string GetMessage(string resourceKey, params object[] args)
        {
            if (languagePairs != null)
                return string.Format(languagePairs[resourceKey], args);
            else if (ResourceManager != null)
                return string.Format(ResourceManager.GetString(resourceKey, CultureInfo), args);
            else
                throw new NullReferenceException("Register a resource manager or a language folder first.");
        }

        public static DialogResult SentInformationByResource(string key, string title, params object[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, ChoiceOptions.OK, LevelOfEergency.Information));

        public static DialogResult SentWarningMessageByResource(string key, string title, params object[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, ChoiceOptions.OK, LevelOfEergency.Warning));

        public static DialogResult SentErrorMessageByResource(string key, string title, params object[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, ChoiceOptions.OK, LevelOfEergency.Error));

        public static DialogResult SentNormalQuestionByResource(string key, string title, params string[] args)
            => SentNormalQuestionByResource(key, title, ChoiceOptions.OKCancel, 1, args);

        public static DialogResult SentNormalQuestionByResource(string key, string title, ChoiceOptions co, params string[] args)
            => SentNormalQuestionByResource(key, title, co, 1, args);

        public static DialogResult SentNormalQuestionByResource(string key, string title, ChoiceOptions co, byte defaultResponse, params object[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, co, LevelOfEergency.Question, defaultResponse));

        public static DialogResult SentWarningQuestionByResource(string key, string title, params string[] args)
            => SentWarningQuestionByResource(key, title, ChoiceOptions.OKCancel, 1, args);

        public static DialogResult SentWarningQuestionByResource(string key, string title, ChoiceOptions co, params string[] args)
            => SentWarningQuestionByResource(key, title, co, 1, args);

        public static DialogResult SentWarningQuestionByResource(string key, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1, params string[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, co, LevelOfEergency.Warning, defaultResponse));

        public static DialogResult SentErrorQuestionByResource(string key, string title, params string[] args)
            => SentErrorQuestionByResource(key, title, ChoiceOptions.OKCancel, 1, args);

        public static DialogResult SentErrorQuestionByResource(string key, string title, ChoiceOptions co, params string[] args)
            => SentErrorQuestionByResource(key, title, co, 1, args);

        public static DialogResult SentErrorQuestionByResource(string key, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1, params string[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, co, LevelOfEergency.Error, defaultResponse));

        //Message

        public static DialogResult SentInformation(string message, string title = "", string id = "")
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, id, ChoiceOptions.OK, LevelOfEergency.Information));

        public static DialogResult SentWarningMessage(string message, string title = "", string id = "")
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, id, ChoiceOptions.OK, LevelOfEergency.Warning));

        public static DialogResult SentErrorMessage(string message, string title = "", string id = "")
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, id, ChoiceOptions.OK, LevelOfEergency.Error));

        public static DialogResult SentNormalQuestion(string message, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => SentNormalQuestion(message, title, null, co, defaultResponse);

        public static DialogResult SentNormalQuestion(string message, string title = "", string id = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, id, co, LevelOfEergency.Question, defaultResponse));

        public static DialogResult SentWarningQuestion(string message, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => SentWarningQuestion(message, title, null, co, defaultResponse);

        public static DialogResult SentWarningQuestion(string message, string title = "", string id = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, id, co, LevelOfEergency.Warning, defaultResponse));

        public static DialogResult SentErrorQuestion(string message, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => SentErrorQuestion(message, title, null, co, defaultResponse);

        public static DialogResult SentErrorQuestion(string message, string title = "", string id = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, id, co, LevelOfEergency.Error, defaultResponse));

    }
}
