using System;
using System.Collections.Generic;
using System.Text;

namespace Aritiafel.Organizations.RaeriharUniversity
{
    public enum ArStandardDateTimeType
    {
        DateTime = 0, //A
        ShortDateTime, //a
        Date, // B
        ShortDate, //b
        Time,  // C   
        ShortTime, // c
    }

    // AR 00001/01/01 00:00:00.0000000      標準時間日期(可比) 32位
    // AR -0001/01/01 23:59:59.9999999      標準時間日期(負數，可比需先判斷正負) 32位
    // AR -0001/01/01 01:01:01              短時間日期 24位
    // AR 19999/01/01                       日期對齊模式 15位
    // 9999/01/01                           短日期對齊模式 11位
    //00:00:00.0000000                      時間對齊模式 16位
    //12:00:00                              短時間對齊模式 8位
}
