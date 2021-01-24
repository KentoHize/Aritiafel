using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Characters
{
    public class Bard //Test User
    {
        public string Name { get; set; }
        public IDictionary InputInformation { get; set; }
        public List<string> MessageReceived { get; private set; } = new List<string>();

        public Bard()
            : this("")
        { }

        public Bard(string name)
            : this("", null, null)
        { }

        public Bard(string informationKey, object informationValue)
           : this("", informationKey, informationValue)
        { }

        public Bard(string name, string informationKey, object informationValue)
        {
            Name = name;
            InputInformation = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(informationKey))
                InputInformation.Add(informationKey, informationValue);
        }

        public Bard(IDictionary informationDictionary)
            : this("", informationDictionary)
        { }

        public Bard(string name, IDictionary informationDictionary)
        {
            Name = name;
            InputInformation = informationDictionary;
        }
    }
}
