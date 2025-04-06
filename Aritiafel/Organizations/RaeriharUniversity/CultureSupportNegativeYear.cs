using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

//可刪除
namespace Aritiafel.Organizations.RaeriharUniversity
{   
    public class CultureSupportNegativeYear : CultureInfo
    {
        Calendar _Calendar;
        public CultureSupportNegativeYear()
            : this("en-US")
        { }

        public CultureSupportNegativeYear(string name)
            : base(name, true)
        { }

        public override Calendar Calendar => _Calendar ??= new ArNegativeCalendar();

        public override Calendar[] OptionalCalendars => new Calendar[] { new ArNegativeCalendar() };
    }
}
