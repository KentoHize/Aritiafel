using System;
using System.Collections.Generic;
using System.Text;
using Aritiafel.Items;

namespace Aritiafel.Characters
{
    public class Courier //Message/Object Carrier
    {
        public string Name { get; set; }

        public ArPackage Package { get; set; }
        public string InputResponse { get; set; }
        public List<string> MessageReceived { get; private set; } = new List<string>();

        public Courier()
            : this("")
        { }

        public Courier(InputResponseOptions iro)
            : this("", iro)
        { }

        public Courier(string name, InputResponseOptions iro)
            : this(name, iro.ToString())
        { }

        public Courier(string name, string inputResponse = "")
        {
            Name = name;
            InputResponse = inputResponse;
        }
    }

    public enum InputResponseOptions
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
