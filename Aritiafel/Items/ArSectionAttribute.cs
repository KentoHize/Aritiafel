using System;

namespace Aritiafel.Items
{
    public class ArSectionAttribute : Attribute
    {
        public string Name { get; set; }
        public ArSectionAttribute(string name)
        {
            Name = name;
        }
    }
}
