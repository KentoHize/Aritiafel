using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    internal static class CultureSupportNegativeYearFactory
    {
        static CultureSupportNegativeYear _CultureSupportNegativeYear;
        public static CultureSupportNegativeYear CultureSupportNegativeYear => _CultureSupportNegativeYear ?? new CultureSupportNegativeYear();
    }

    internal class CultureSupportNegativeYear : CultureInfo
    {
        Calendar _Calendar;
        public CultureSupportNegativeYear()
            : base("en-US", true)
        { }

        public override Calendar Calendar => _Calendar ?? new ArNegativeCalendar();
    }
}
