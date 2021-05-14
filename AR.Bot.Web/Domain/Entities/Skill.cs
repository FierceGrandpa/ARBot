using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Domain
{
    public class Skill : EntityBase
    {
        public Skill() => Activities = new HashSet<Activity>();

        public string Title  { get; set; }
        public bool   Status { get; set; }
        
        #region Category

        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        #endregion
        
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<ActivitySkill> ActivitySkills { get; set; }
    }
}
