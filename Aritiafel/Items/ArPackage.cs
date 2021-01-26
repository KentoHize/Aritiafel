using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public abstract class ArPackage
    {
        public string ID { get; set; }
        public string Title { get; set; }

        protected ArPackage(string id = null, string title = "")
        {
            ID = id;
            Title = title;
        }
    }
}
