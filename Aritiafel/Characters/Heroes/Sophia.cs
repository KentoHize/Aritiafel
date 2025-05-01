using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Aritiafel.Artifacts;
using Aritiafel.Items;

namespace Aritiafel.Characters.Heroes
{
    public static class Sophia
    {
        public static void SeeThrough(object o)
        {
            Console.WriteLine(Allseer.SeeThrough(o));
        }

        public static void QuickSeeThrough(object o)
        {
            Console.Write(Allseer.SeeThrough(o));
        }
    }
}
