using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QAWebsite.Models;
using QAWebsite.Models.QuestionModels;

namespace QAWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Tag>().HasIndex(x => x.Name).IsUnique();
            builder.Entity<QuestionTag>().HasKey(qt => new { qt.QuestionId, qt.TagId });

            builder.Entity<QuestionTag>()
                .HasOne(q => q.Question)
                .WithMany(qt => qt.QuestionTags)
                .HasForeignKey(q => q.QuestionId);

            builder.Entity<QuestionTag>()
                .HasOne(t => t.Tag)
                .WithMany(qt => qt.QuestionTags)
                .HasForeignKey(t => t.TagId);
        }

        public DbSet<Question> Question { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<QuestionTag> QuestionTag { get; set; }
        public DbSet<QAWebsite.Models.Flag> Flag { get; set; }
    }
}
