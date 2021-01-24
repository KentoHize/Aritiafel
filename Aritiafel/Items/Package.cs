using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public abstract class Package
    {
        public string Title { get; set; }

        protected Package(string title)
        {
            Title = title;
        }
    }
}
