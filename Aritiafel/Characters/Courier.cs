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
        public Dictionary<string, List<string>> Responses { get; private set; } = new Dictionary<string, List<string>>();
        public List<string> MessageReceived { get; private set; } = new List<string>();

        public void AddResponse(string messageID, string response)
        {
            if (!Responses.ContainsKey(messageID))
                Responses.Add(messageID, new List<string>());

            if (Responses[messageID] == null)
                Responses[messageID] = new List<string>();

            Responses[messageID].Add(response);
        }

        public void AddResponses(string messageID, List<string> responses)        
            => Responses[messageID] = responses;

        public void ClearResponses(string messageID)
        {
            if (Responses[messageID] != null)
                Responses[messageID].Clear();
        }

        public void ClearAllResponses()
            => Responses.Clear();

        //public Courier()
        //    : this("")
        //{ }

        //public Courier(InputResponseOptions iro)
        //    : this("", iro)
        //{ }

        //public Courier(string name, InputResponseOptions iro)
        //    : this(name, iro.ToString())
        //{ }

        //public Courier(string name, string inputResponse = "")
        //{
        //    Name = name;
        //    InputResponse = inputResponse;
        //}

        public Courier(ResponseOptions ro)
        {

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
