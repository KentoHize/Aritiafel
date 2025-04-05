using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

//目前為可消除Class
namespace Aritiafel.Organizations.RaeriharUniversity
{
    public class ArNegativeCalendar : Calendar
    {
        internal Calendar Calendar { get; set; }
        public ArNegativeCalendar()
            => Calendar = CultureInfo.CurrentCulture.Calendar;
        public override int[] Eras => Calendar.Eras;
        public override DateTime AddMonths(DateTime time, int months)
            => Calendar.AddMonths(time, months);
        public override DateTime AddYears(DateTime time, int years)
            => Calendar.AddYears(time, years);
        public override int GetDayOfMonth(DateTime time)
            => Calendar.GetDayOfMonth(time);
        public override DayOfWeek GetDayOfWeek(DateTime time)
        {
            Console.WriteLine(7 + time.Ticks % 6048000000000L);
            return (DayOfWeek)(7 + time.Ticks % 6048000000000L);
        }
            
        public override int GetDayOfYear(DateTime time)
            => Calendar.GetDayOfYear(time);
        public override int GetDaysInMonth(int year, int month, int era)
            => Calendar.GetDaysInMonth(year, month, era);
        public override int GetDaysInYear(int year, int era)
            => Calendar.GetDaysInYear(year, era);
        public override int GetEra(DateTime time)
            => Calendar.GetEra(time);
        public override int GetMonth(DateTime time)
            => Calendar.GetMonth(time);
        public override int GetMonthsInYear(int year, int era)
            => Calendar.GetMonthsInYear(year, era);
        public override int GetYear(DateTime time)
            => Calendar.GetYear(time);
        public override bool IsLeapDay(int year, int month, int day, int era)
            => Calendar.IsLeapDay(year, month, day, era);
        public override bool IsLeapMonth(int year, int month, int era)
            => Calendar.IsLeapMonth(year, month, era);
        public override bool IsLeapYear(int year, int era)
            => Calendar.IsLeapYear(year, era);
        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
            => Calendar.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
    }
}
