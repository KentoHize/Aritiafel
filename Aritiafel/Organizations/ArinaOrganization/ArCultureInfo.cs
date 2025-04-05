using Aritiafel.Organizations.RaeriharUniversity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

//目前為可消除Class
namespace Aritiafel.Organizations.ArinaOrganization
{
    public class ArCultureInfo : CultureInfo
    {
     
        //public ArCultureInfo(CultureInfo baseCultureInfo)
        //{
            
        //}

        public ArCultureInfo()
            : this("")
        { }
        public ArCultureInfo(string name)
            : base(name)
        {
            //this.DateTimeFormat
            DateTimeFormatInfo dfi = new DateTimeFormatInfo();
            dfi.FirstDayOfWeek = DayOfWeek.Monday;
            this.DateTimeFormat = dfi;
            //dfi.
            //Console.WriteLine(OptionalCalendars.Length);
            //for(int i = 0; i < OptionalCalendars.Length; i++)
                //Console.WriteLine(this.OptionalCalendars[i].ToString());
            //dfi.Calendar = new ArNegativeCalendar();
            DateTimeFormat = dfi;
        }
    }
}
