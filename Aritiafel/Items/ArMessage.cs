using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArMessage : ArPackage
    {
        public string Content { get; set; }
        public LevelOfEergency LevelOfEergency { get; set; }
        public ResponseOption ResponseOption { get; set;}
        public byte DefaultResponse
        {
            get => _DefaultRespose;
            set
            {
                if (value == 0 || value > 2)
                    throw new ArgumentOutOfRangeException("DefaultRespone");
                _DefaultRespose = value;
            }
        }
        private byte _DefaultRespose;

        public ArMessage(string content, string title = "", ResponseOption ro = ResponseOption.OK, LevelOfEergency loe = LevelOfEergency.None, byte defaultResponse = 1)
            : base(title)
        {
            Content = content;
            ResponseOption = ro;
            LevelOfEergency = loe;           
            DefaultResponse = defaultResponse;
        }

        private string GetResponseOptionString()
        {
            string result;
            switch(ResponseOption)
            {
                case ResponseOption.OK:
                    result = "[OK]";
                    break;
                case ResponseOption.OKCancel:
                    result = "[OK][Cancel]";
                    break;
                case ResponseOption.RetryCancel:
                    result = "[Retry][Cancel]";
                    break;
                case ResponseOption.YesNo:
                    result = "[Yes][No]";
                    break;
                case ResponseOption.YesNoCancel:
                    result = "[Yes][No][Cancel]";
                    break;
                case ResponseOption.AbortRetryIgnore:
                    result = "[Abort][Retry][Ignore]";
                    break;
                default:
                    return null;                    
            }
            return result.Insert(result.IndexOf('[', 0, DefaultResponse), "*");
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

    public enum ResponseOption
    {
        OK = 0,
        OKCancel,
        AbortRetryIgnore,        
        YesNoCancel,        
        YesNo,        
        RetryCancel
    }
}
