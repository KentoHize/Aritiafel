using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Characters
{
    public class Bard //TestingUser
    {
        public string Name { get; set; }
        public IDictionary InputInfomation { get; set; }
        public List<string> MessageReceived { get; set; } = new List<string>();

        public Bard(string name)
        {
            Name = name;
            InputInfomation = new Dictionary<string, object>();
        }
    }
}
