﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Aritiafel.Items;

namespace Aritiafel.Organizations
{
    public static class RabbitCouriers
    {
        public static DialogResult SentInformation(string message, string title = "")
                => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, ResponseOption.OK, LevelOfEergency.Information));

        public static DialogResult SentWarningMessage(string message, string title = "")
                => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, ResponseOption.OK, LevelOfEergency.Warning));

        public static DialogResult SentErrorMessage(string message, string title = "")
                => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, ResponseOption.OK, LevelOfEergency.Error));

        public static DialogResult SentNoramlQuestion(string message, string title = "", ResponseOption ro = ResponseOption.OKCancel, byte defaultResponse = 1)
                => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, ro, LevelOfEergency.Question, defaultResponse));

        public static DialogResult SentWarningQuestion(string message, string title = "", ResponseOption ro = ResponseOption.OKCancel, byte defaultResponse = 1)
                => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, ro, LevelOfEergency.Warning, defaultResponse));

        public static DialogResult SentErrorQuestion(string message, string title = "", ResponseOption ro = ResponseOption.OKCancel, byte defaultResponse = 1)
                => AdventurerAssociation.ShowNewMessageOrSetResult(new ArMessage(message, title, ro, LevelOfEergency.Error, defaultResponse));
    }
}
