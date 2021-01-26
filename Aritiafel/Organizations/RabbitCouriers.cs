﻿using Aritiafel.Items;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;



namespace Aritiafel.Organizations
{
    public static class RabbitCouriers
    {
        public static ResourceManager ResourceManager { get; private set; }
        public static CultureInfo CultureInfo { get; private set; }

        public static void RegisterRMAndCI(ResourceManager rm, CultureInfo ci)
        {
            ResourceManager = rm;
            CultureInfo = ci;
        }

        public static string GetMessage(string resourceKey, params object[] args)
               => string.Format(ResourceManager.GetString(resourceKey, CultureInfo), args);

        //By Resource

        public static DialogResult SentInformationByResource(string key, string title, params object[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, ChoiceOptions.OK, LevelOfEergency.Information));

        public static DialogResult SentWarningMessageByResource(string key, string title, params object[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, ChoiceOptions.OK, LevelOfEergency.Warning));

        public static DialogResult SentErrorMessageByResource(string key, string title, params object[] args)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(GetMessage(key, args), title, key, ChoiceOptions.OK, LevelOfEergency.Error));

        public static DialogResult SentNoramlQuestionByResource(string key, string title, params string[] args)
            => SentNoramlQuestionByResource(key, title, ChoiceOptions.OKCancel, 1, args);

        public static DialogResult SentNoramlQuestionByResource(string key, string title, ChoiceOptions co, params string[] args)
            => SentNoramlQuestionByResource(key, title, co, 1, args);

        public static DialogResult SentNoramlQuestionByResource(string key, string title, ChoiceOptions co, byte defaultResponse, params object[] args)
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

        public static DialogResult SentInformation(string message, string title = "")
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, null, ChoiceOptions.OK, LevelOfEergency.Information));

        public static DialogResult SentWarningMessage(string message, string title = "")
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, null, ChoiceOptions.OK, LevelOfEergency.Warning));

        public static DialogResult SentErrorMessage(string message, string title = "")
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, null, ChoiceOptions.OK, LevelOfEergency.Error));

        public static DialogResult SentNoramlQuestion(string message, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, null, co, LevelOfEergency.Question, defaultResponse));

        public static DialogResult SentWarningQuestion(string message, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, null, co, LevelOfEergency.Warning, defaultResponse));

        public static DialogResult SentErrorQuestion(string message, string title = "", ChoiceOptions co = ChoiceOptions.OKCancel, byte defaultResponse = 1)
            => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, null, co, LevelOfEergency.Error, defaultResponse));
    }
}
