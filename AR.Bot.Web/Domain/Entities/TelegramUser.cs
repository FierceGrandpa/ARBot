using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class TelegramUser : EntityBase
    {
        public TelegramUser() => SentActivities = new HashSet<SentActivity>();

        public TelegramUser(long chatId) : this()
        {
            ChatId = chatId;

            MailingTime = new MailTime(12, 00);
        }

        public long ChatId { get; set; }

        // TODO: Enum
        public SubscribeStatus SubscribeStatus { get; set; }
        // TODO: Telegram User Model
        public MailTime MailingTime { get; set; }
        public SendingTimeMode MailingMode { get; set; }

        public virtual ICollection<SentActivity> SentActivities { get; set; }
    }
}
