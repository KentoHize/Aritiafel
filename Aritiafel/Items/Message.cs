using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class Message : Package
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

        public Message(string content, string title = "", ResponseOption ro = ResponseOption.OK, LevelOfEergency loe = LevelOfEergency.None, byte defaultResponse = 1)
            : base(title)
        {
            Content = content;
            ResponseOption = ro;
            LevelOfEergency = LevelOfEergency;           
            DefaultResponse = defaultResponse;
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
