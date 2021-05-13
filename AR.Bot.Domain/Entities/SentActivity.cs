using System;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class SentActivity
    {
        public SentActivity(Guid userId, Guid activityId)
        {
            TelegramUserId = userId;
            ActivityId = activityId;
            // TODO: Replace with Ext
            SentDate = DateTime.UtcNow.AddHours(3);
        }

        #region Activity

        public Guid ActivityId { get; set; }
        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; }

        #endregion

        #region TelegramUser

        public Guid TelegramUserId { get; set; }
        [ForeignKey(nameof(TelegramUserId))]
        public TelegramUser TelegramUser { get; set; }

        #endregion

        #region DateTime

        public DateTime SentDate { get; set; } = DateTime.UtcNow.AddHours(3); // TODO: Refactor

        #endregion
    }
}
