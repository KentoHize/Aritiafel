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

        public ArCultureInfo(string name, bool useUserOverride) 
            : base(name, useUserOverride)
        { }
        public override string Name => "zh-AA";

        //public override Calendar Calendar
        //{
        //    get { return new ArNegativeCalendar(); }
        //}
            

        //format -x/x/x x:x:x.xxx(xxxx)
    }
}
