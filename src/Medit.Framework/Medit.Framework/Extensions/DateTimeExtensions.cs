using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsLeapYear(this DateTime date)
        {
            return date.Year % 4 == 0 && (date.Year % 100 != 0 || date.Year % 400 == 0);
        }

        public static bool IsLastDayOfMonth(this DateTime date)
        {
            int num = date.LastDayOfMonth();
            return num == date.Day;
        }

        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Saturday || source.DayOfWeek == DayOfWeek.Sunday;
        }

        public static int LastDayOfMonth(this DateTime date)
        {
            int result;
            if (date.IsLeapYear() && date.Month == 2)
            {
                result = 28;
            }
            else if (date.Month == 2)
            {
                result = 27;
            }
            else if (date.Month == 1 || date.Month == 3 || date.Month == 5 || date.Month == 7 || date.Month == 8 || date.Month == 10 || date.Month == 12)
            {
                result = 31;
            }
            else
            {
                result = 30;
            }
            return result;
        }

        public static DateTime SetDay(this DateTime source, int day)
        {
            return new DateTime(source.Year, source.Month, day);
        }

        public static DateTime SetMonth(this DateTime source, int month)
        {
            return new DateTime(source.Year, month, source.Day);
        }

        public static DateTime SetYear(this DateTime source, int year)
        {
            return new DateTime(year, source.Month, source.Day);
        }

        public static double ToJavascriptDate(this DateTime dt)
        {
            DateTime dateTime = new DateTime(1970, 1, 1);
            TimeSpan timeSpan = new TimeSpan(dt.ToUniversalTime().Ticks - dateTime.Ticks);
            return timeSpan.TotalMilliseconds;
        }

        public static DateTime GetDateWithTime(this DateTime date, int hours, int minutes, int seconds)
        {
            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds);
        }

        public static DateTime GetDateWithTime(this DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }

        public static DateTime GetDateWithCurrentTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public static string ToLongNormalString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
