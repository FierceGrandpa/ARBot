using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class TelegramUser : EntityBase
    {
        public TelegramUser() => Activities = new HashSet<Activity>();

        public TelegramUser(long chatId) : this()
        {
            ChatId = chatId;

            MailingTime = new MailTime(12, 00);
        }

        public long ChatId { get; set; }

        // TODO: Enum
        public bool SubscribeStatus { get; set; }
        // TODO: Telegram User Model
        public MailTime MailingTime { get; set; }
        public SendingTimeMode MailingMode { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
}
