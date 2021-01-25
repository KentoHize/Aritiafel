using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public class ArCrate : ArPackage
    {
        public object Content { get; set; }

        public ArCrate()
            : this(null)
        { }

        public ArCrate(object content, string title = "")
            : base(title)
        {
            Content = content;
        }
    }
}
