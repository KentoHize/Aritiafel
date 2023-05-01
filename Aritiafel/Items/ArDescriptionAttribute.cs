using System;

namespace Aritiafel.Items
{
    public class ArDescriptionAttribute : Attribute
    {
        public string Text { get; set; }
        public ArDescriptionAttribute(string text)
        {
            Text = text;
        }
    }
}
