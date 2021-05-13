using AR.Bot.Domain;
using Microsoft.EntityFrameworkCore;

namespace AR.Bot.Core.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TelegramUser> Users { get; set; }

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
                    .HasDefaultValue(new MailTime(12, 00));

                entity
                    .HasMany(e => e.Activities)
                    .WithMany(e => e.TelegramUsers);

                entity.ToTable("TelegramUsers");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity
                    .HasMany(e => e.Skills)
                    .WithMany(e => e.Activities);

                entity
                    .HasMany(e => e.TelegramUsers)
                    .WithMany(e => e.Activities);

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

                entity
                    .HasMany(e => e.Activities)
                    .WithMany(e => e.Skills);
                
                entity.ToTable("Skills");
            });

            modelBuilder.Entity<SentActivity>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(e => e.Activity);

                entity.HasOne(e => e.TelegramUser);
                
                entity.ToTable("SentActivities");
            });
        }
    }
}
