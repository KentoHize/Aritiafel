using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Items;

namespace Aritiafel.Characters
{
    public class Courier //Message/Mail/Crate Carrier
    {
        public string Name { get; set; }

        public ArPackage Package { get; set; }
        public Dictionary<string, List<string>> Responses
        {
            get
                => _Responses;
            set
            {
                if(value != null && value.ContainsKey(""))
                    _Responses = value;
                throw new ArgumentException("Respones must exist and has a empty string key.");
            }
        }
        private Dictionary<string, List<string>> _Responses = new Dictionary<string, List<string>> { { "", new List<string>() } };
        public List<string> MessageReceived { get; private set; } = new List<string>();

        public void AddResponse(string response)
            => AddResponse(response, null);

        public void AddResponse(string response, string messageID)
        {
            if (string.IsNullOrEmpty(messageID))
            {
                _Responses[""].Add(response);
                return;
            }

            if (!_Responses.ContainsKey(messageID))
                _Responses.Add(messageID, new List<string>());

            if (_Responses[messageID] == null)
                _Responses[messageID] = new List<string>();

            _Responses[messageID].Add(response);
        }

        public void AddResponses(List<string> responses)
            => AddResponses(responses, null);

        public void AddResponses(List<string> responses, string messageID)
        {
            if (string.IsNullOrEmpty(messageID))
                _Responses[""] = responses;
            else
                _Responses[messageID] = responses;
        }

        public string GetResponse(string messageID = "", bool notDefault = false)
        {
            string result;
            if (messageID == null)
                messageID = "";

            if (_Responses.ContainsKey(messageID) && _Responses[messageID].Count != 0)
            {
                result = _Responses[messageID][0];
                _Responses[messageID].RemoveAt(0);
                return result;
            }
            else if(_Responses[""].Count != 0 && !notDefault)
            {
                result = _Responses[""][0];
                _Responses[""].RemoveAt(0);
                return result;
            }
            return null;
        }

        public void ClearResponses(string messageID = null)
        {
            if (string.IsNullOrEmpty(messageID))
                _Responses[""].Clear();            
            else if (_Responses[messageID] != null)
                _Responses[messageID].Clear();
        }

        public void ClearAllResponses()
        {
            _Responses.Clear();
            _Responses.Add("", new List<string>());
        }

        public Courier()
            : this("")
        { }

        public Courier(ResponseOptions ro, string messageID = null)
            : this("", ro, messageID)
        { }

        public Courier(string name, ResponseOptions ro, string messageID = null)
        {
            Name = name;            
            AddResponse(ro.ToString(), messageID);
        }

        public Courier(string name, Dictionary<string, List<string>> responses = null)
        {
            Name = name;
            if(responses != null)
                Responses = responses;
        }
    }

    public enum ResponseOptions
    {
        None = 0,
        OK,
        Cancel,
        Abort,
        Retry,
        Ignore,
        Yes,
        No
    }   
}
