using System;
using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public sealed record MailTime([Range(0, 23)] int Hour, [Range(0, 63)] int Minutes)
    {
        public bool IsNowSend
        {
            get
            {
                var time = RemoveSeconds(GetMoscowTime());

                return time.AddMinutes(-1.5) < NextParcelDate && NextParcelDate < time.AddMinutes(1.5);
            }
        }

        public DateTime NextParcelDate => DateTime.UtcNow.Date.AddHours(Hour).AddMinutes(Minutes);

        public override string ToString() => $"{Format(Hour)}:{Format(Minutes)}";

        // TODO: Make Ext
        private static string Format(int value) => value.ToString().PadLeft(2, '0');

        private static DateTime GetMoscowTime() => DateTime.UtcNow.AddHours(3); // (GMT+3)

        private static DateTime RemoveSeconds(DateTime value) => value.AddSeconds(-value.Second);
    }
}