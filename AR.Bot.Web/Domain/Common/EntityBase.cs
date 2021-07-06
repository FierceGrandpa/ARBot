 using System;
 using System.ComponentModel.DataAnnotations;

// ReSharper disable once CheckNamespace
 namespace AR.Bot.Domain
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid();
            DateAdded = DateTime.UtcNow.ToMoscowTime();
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
