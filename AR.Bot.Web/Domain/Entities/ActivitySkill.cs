using System;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class ActivitySkill : EntityBase
    {
        public Guid ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public Guid SkillId { get; set; }
        public virtual Skill Skill { get; set; }

        public double Offset { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
