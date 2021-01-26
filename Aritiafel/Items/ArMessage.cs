using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArMessage : ArPackage
    {
        public string Content { get; set; }
        public LevelOfEergency LevelOfEergency { get; set; }
        public ChoiceOptions ChoiceOption { get; set; }
        public byte DefaultResponse
        {
            get => _DefaultRespose;
            set
            {
                if (value == 0 || value > 2)
                    throw new ArgumentOutOfRangeException("DefaultResponse");
                _DefaultRespose = value;
            }
        }
        private byte _DefaultRespose;

        public string GetDefaultResponse()
        {
            string[] sArray = GetResponseOptionString().Split(']');
            for (int i = 0; i < sArray.Length; i++)
                if (sArray[i][0] == '*')
                    return sArray[i].Substring(2);
            return null;
        }

        public ArMessage(string content, string title = "", string id = null, ChoiceOptions co = ChoiceOptions.OK, LevelOfEergency loe = LevelOfEergency.None, byte defaultResponse = 1)
            : base(id, title)
        {   
            Content = content;
            ChoiceOption = co;
            LevelOfEergency = loe;           
            DefaultResponse = defaultResponse;
        }

        private string GetResponseOptionString()
        {
            string result;
            switch(ChoiceOption)
            {
                case ChoiceOptions.OK:
                    result = "[OK]";
                    break;
                case ChoiceOptions.OKCancel:
                    result = "[OK][Cancel]";
                    break;
                case ChoiceOptions.RetryCancel:
                    result = "[Retry][Cancel]";
                    break;
                case ChoiceOptions.YesNo:
                    result = "[Yes][No]";
                    break;
                case ChoiceOptions.YesNoCancel:
                    result = "[Yes][No][Cancel]";
                    break;
                case ChoiceOptions.AbortRetryIgnore:
                    result = "[Abort][Retry][Ignore]";
                    break;
                default:
                    return null;                    
            }
            int index = 0;
            byte time = 0;
            while(true)
            {
                index = result.IndexOf('[', index);
                time++;                

                if (time == DefaultResponse)
                    break;
                index++;
            }
            return result.Insert(index, "*");
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("Title:\"{0}\"\n", Title);
            result.AppendFormat("Level:{0}\n", LevelOfEergency);
            result.AppendFormat("Content:\"{0}\"\n", Content);
            result.AppendFormat("Response options:{0}", GetResponseOptionString());
            return result.ToString();
        }
    }

    public enum LevelOfEergency
    {
        None = 0,
        Error = 16,
        Question = 32,
        Warning = 48,
        Information = 64,
    }

    public enum ChoiceOptions
    {
        OK = 0,
        OKCancel,
        AbortRetryIgnore,        
        YesNoCancel,        
        YesNo,        
        RetryCancel
    }
}
