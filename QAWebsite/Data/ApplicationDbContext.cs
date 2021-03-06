﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Models.QuestionModels;
using QAWebsite.Models.UserModels;

namespace QAWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Question>()
                .HasIndex(x => x.Title)
                .IsUnique();

            builder.Entity<Question>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.AuthorId);

            builder.Entity<Question>()
                .HasMany(x => x.Answers)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);

            builder.Entity<Question>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.FkId);

            builder.Entity<Question>()
                .HasMany(x => x.Flags)
                .WithOne(x => x.Question)
                .HasForeignKey(x => x.QuestionId);

            builder.Entity<Tag>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<QuestionTag>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            builder.Entity<QuestionTag>()
                .HasOne(q => q.Question)
                .WithMany(qt => qt.QuestionTags)
                .HasForeignKey(q => q.QuestionId);

            builder.Entity<QuestionTag>()
                .HasOne(t => t.Tag)
                .WithMany(qt => qt.QuestionTags)
                .HasForeignKey(t => t.TagId);

            builder.Entity<Answer>()
               .HasMany(x => x.Comments)
               .WithOne(x => x.Answer)
               .HasForeignKey(x => x.FkId);

            builder.Entity<ApplicationUser>()
               .HasMany(x => x.UserAchievements)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);

            builder.Entity<ApplicationUserAchievements>()
               .HasOne(x => x.Achievement)
               .WithMany(x => x.UserAchievements)
               .HasForeignKey(x => x.AchievementId);
        }

        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionRating> QuestionRating { get; set; }
        public DbSet<AnswerRating> AnswerRating { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<QuestionTag> QuestionTag { get; set; }
        public DbSet<Flag> Flag { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<QuestionEdit> QuestionEdits { get; set; }
        public DbSet<AnswerComment> AnswerComment { get; set; }
        public DbSet<QuestionComment> QuestionComment { get; set; }
        public DbSet<ApplicationUserAchievements> UserAchievements { get; set; }
        public DbSet<Achievement> Achievement { get; set; }

    }
}
