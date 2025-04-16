using Aritiafel.Artifacts;
using Aritiafel.Items;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

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
