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

            ArDateTime.TicksToDate(adt._data, out int year, out int month, out int day);            
            long n1 = adt._data * -1 % 864000000000L;
            if (n1 == 0)
                n1 = 864000000000L;
            DateTime dt = new DateTime(year * - 1, month, day).AddTicks(864000000000L - n1);
            return $"(-){dt.ToString(format, formatProvider)}";
        }
    }
}
