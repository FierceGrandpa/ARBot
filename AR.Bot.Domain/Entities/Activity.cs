using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class Activity : EntityBase
    {
        public Activity()
        {
            Skills = new HashSet<Skill>();

            TelegramUsers = new HashSet<TelegramUser>();
        }
        
        public string Title       { get; set; }
        public string Description { get; set; }

        public bool   Status      { get; set; }

        public int    MaxAge      { get; set; }
        public int    MinAge      { get; set; }

        public ICollection<Skill> Skills { get; set; }
        public ICollection<TelegramUser> TelegramUsers { get; set; }
    }
}
