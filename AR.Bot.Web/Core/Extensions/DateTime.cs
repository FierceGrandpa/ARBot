using System;

namespace AR.Bot
{
    public static class DateTimeExtensions
    {
        public static DateTime ToMoscowTime(this DateTime dt)
        {
            return dt.ToUniversalTime().AddHours(3);
        }
        
    }
}