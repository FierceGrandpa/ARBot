using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class SentActivity
    {
        public SentActivity()
        {
            // TODO: Replace with Ext
            SentDate = DateTime.UtcNow.AddHours(3);
        }

        public SentActivity(Guid userId, Guid activityId) : this()
        {
            TelegramUserId = userId;
            ActivityId = activityId;
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
