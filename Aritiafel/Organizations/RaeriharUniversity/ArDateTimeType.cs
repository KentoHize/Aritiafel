using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public enum ArDateTimeType
    {
        DateTime = 0,
        Date,       //d
        Time,      //T
        ShortTime, //t
        LongDate, //D
        System //A (Without Era)
    }
    
    // AR  0001/01/01 00:00:00.000          對齊模式(可比)
    // AR -0001/01/01 23:59:59.999          對齊模式
    //AR -1/1/1 1:1:1
    // AR 19999/01/01                       日期對齊模式
    // 00:00:00.000                         時間對齊模式
}
