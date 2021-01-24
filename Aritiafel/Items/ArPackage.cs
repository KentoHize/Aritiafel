using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Items
{
    public abstract class ArPackage
    {
        public string Title { get; set; }

        protected ArPackage(string title)
        {
            Title = title;
        }
    }
}
