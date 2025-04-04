using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    internal static class ArDateTimeFormat 
    {

        //Parse
        //Format
        //+(-)
        public static string Format(string format, ArDateTime adt, IFormatProvider formatProvider)
        {
            if (adt._data >= 0)
                return new DateTime(adt._data).ToString(format, formatProvider);

            long n1, n2;            
            n1 = Math.DivRem(adt._data, 126227808000000000L, out n2);
            n2 += 126227808000000000L;
            DateTime dt = new DateTime(n2);
            dt = dt.AddYears((int)(-n1 * 400) + (401 - dt.Year * 2));

            //Console.WriteLine((int)(-n1 * 400) + (401 - dt.Year));
            //Console.WriteLine(-n1 * 400);
            //dt = dt.AddYears((int)(-(401 - dt.Year) + (-n1 * 400)));
            //Console.WriteLine(dt.ToString());
            //adt.Year
            //dt = dt.AddTicks((n1 - 1) * 126227808000000000L);
            return $"(-){dt.ToString(format, formatProvider)}";
        }
    }
}
