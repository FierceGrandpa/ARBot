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

            // TODO: Refactor
            DateAdded = DateTime.UtcNow.AddHours(3);
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
