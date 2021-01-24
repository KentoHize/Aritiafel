using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Characters
{
    public class Bard //Test User
    {
        public string Name { get; set; }
        public IDictionary InputInfomation { get; set; }
        public string Response { get; set; }
        public List<string> MessageReceived { get; set; } = new List<string>();

        public Bard()
            : this("")
        { }

        public Bard(string name)
        {
            Name = name;
            InputInfomation = new Dictionary<string, object>();
        }
    }
}
