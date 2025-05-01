namespace Aritiafel.Organizations.ArinaOrganization
{
    public enum ArStandardDateTimeType
    {
        DateTime = 0, //A
        ShortDateTime, //a
        Date, // B
        ShortDate, //b
        Time,  // C   
        ShortTime, // c

        DateTimeExtension, //Z
        ShortDateTimeExtension, //z
        DateExtension, //X
        ShortDateExtension //x

    }

    // AR 00001/01/01 00:00:00.0000000      A 標準時間日期(可比) 32位
    // AR -0001/01/01 23:59:59.9999999      A 標準時間日期(負數，可比需先判斷正負) 32位
    // AR -0001/01/01 01:01:01              a 短時間日期 24位
    // AR 19999/01/01                       B 日期對齊模式 15位
    //09999/01/01                           b 短日期對齊模式 11位
    //00:00:00.0000000                      C 時間對齊模式 16位
    //12:00:00                              c 短時間對齊模式 8位

    //  AR000001/01/01 00:00:00.0000000     Z 標準時間日期擴充 32位
    //  AR-00001/01/01 00:00:00.0000000     z 短時間日期擴充 24位
    //  AR000001/01/01                      X 日期擴充 15位
    //000001/01/01                          x 短日期擴充 12位
}
