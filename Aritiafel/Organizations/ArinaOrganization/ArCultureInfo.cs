using Aritiafel.Organizations.RaeriharUniversity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aritiafel.Organizations.ArinaOrganization
{
    public class ArCultureInfo : CultureInfo
    {
        
        //public ArCultureInfo(CultureInfo baseCultureInfo)
        //{
            
        //}

        public ArCultureInfo()
            : this("ja-JP", true)
        { }        

        internal ArCultureInfo(string name, bool useUserOverride) 
            : base(name, useUserOverride)
        {
            //DateTimeFormatInfo dfi = new DateTimeFormatInfo();
            //dfi.Calendar = new ArNegativeCalendar();            
        }
        public override string Name => "zh-AR";

        public override Calendar Calendar
        {
            get { return new ArNegativeCalendar(); }
        }
            

        //format -x/x/x x:x:x.xxx(xxxx)
    }
}
