using AR.Bot.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AR.Bot.Core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TelegramUser> Users { get; set; }

        public DbSet<ActivitySkill> ActivitySkill { get; set; }
        public DbSet<SentActivity> SentActivities { get; set; }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Skill>    Skills { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelegramUser>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .Property(e => e.MailingTime)
                    .HasConversion(
                        e => JsonConvert.SerializeObject(e),
                        e => JsonConvert.DeserializeObject<MailTime>(e))
                    .HasDefaultValue(new MailTime(12, 00));

                entity.HasMany(e => e.SentActivities);

                entity.ToTable("TelegramUsers");
            });

            modelBuilder
                .Entity<Activity>()
                .HasMany(a => a.Skills)
                .WithMany(s => s.Activities)
                .UsingEntity<ActivitySkill>(
                    j => j
                        .HasOne(pt => pt.Skill)
                        .WithMany(p => p.ActivitySkills)
                        .HasForeignKey(pt => pt.SkillId),
                    j => j
                        .HasOne(pt => pt.Activity)
                        .WithMany(t => t.ActivitySkills)
                        .HasForeignKey(pt => pt.ActivityId),
                    j =>
                    {
                        j.Property(pt => pt.ChangeDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                        j.Property(pt => pt.Offset).HasDefaultValue(1);
                        j.HasKey(t => new { t.ActivityId, t.SkillId });
                        j.ToTable("ActivitySkill");
                    }
                );


            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasMany(e => e.SentActivities);

                entity.ToTable("Activities");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .HasMany(e => e.Skills)
                    .WithOne(e => e.Category);
                
                entity.ToTable("Categories");
            });
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Category);
                
                entity.ToTable("Skills");
            });

            modelBuilder.Entity<SentActivity>(entity =>
            {
                entity.HasKey(t => new { t.ActivityId, t.TelegramUserId });

                entity
                    .HasOne(e => e.Activity)
                    .WithMany(p => p.SentActivities)
                    .HasForeignKey(pt => pt.ActivityId);

                entity
                    .HasOne(e => e.TelegramUser)
                    .WithMany(p => p.SentActivities)
                    .HasForeignKey(pt => pt.TelegramUserId);
                
                entity.ToTable("SentActivities");
            });
        }
    }
}
