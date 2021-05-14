using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class Category : EntityBase
    {
        public Category() => Skills = new HashSet<Skill>();

        public string Title  { get; set; }
        public bool   Status { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}