// ReSharper disable once CheckNamespace

using System;

namespace AR.Bot.Domain
{
    [Flags]
    public enum SendingTimeMode
    {
        // 9:00
        Morning = 1,
        // 12:00
        Default = 2,
        // 15:00
        Lunch = 4,
        // 19:00
        Evening = 8,
        Custom = 0
    }
}