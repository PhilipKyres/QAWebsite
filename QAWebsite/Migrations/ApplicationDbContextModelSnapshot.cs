﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using QAWebsite.Data;
using QAWebsite.Models.Enums;
using System;

namespace QAWebsite.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.Answer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("AuthorId")
                        .HasMaxLength(450);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EditDate");

                    b.Property<string>("QuestionId")
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.AnswerComment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("AuthorId")
                        .HasMaxLength(450);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("FkId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FkId");

                    b.ToTable("AnswerComment");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.AnswerRating", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("FkId");

                    b.Property<string>("RatedBy")
                        .HasMaxLength(450);

                    b.Property<int>("RatingValue");

                    b.HasKey("Id");

                    b.ToTable("AnswerRating");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.Flag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("AuthorId")
                        .HasMaxLength(450);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("QuestionId")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<int>("Reason");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Flag");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.Question", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(8);

                    b.Property<string>("AuthorId")
                        .HasMaxLength(450);

                    b.Property<string>("BestAnswerId")
                        .HasMaxLength(36);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("EditDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.QuestionComment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("AuthorId")
                        .HasMaxLength(450);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("FkId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FkId");

                    b.ToTable("QuestionComment");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.QuestionEdit", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EditDate");

                    b.Property<string>("EditorId");

                    b.Property<string>("NewContent");

                    b.Property<string>("NewTitle");

                    b.Property<string>("QuestionId");

                    b.HasKey("Id");

                    b.ToTable("QuestionEdits");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.QuestionRating", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("FkId");

                    b.Property<string>("RatedBy")
                        .HasMaxLength(450);

                    b.Property<int>("RatingValue");

                    b.HasKey("Id");

                    b.ToTable("QuestionRating");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.QuestionTag", b =>
                {
                    b.Property<string>("QuestionId")
                        .HasMaxLength(8);

                    b.Property<string>("TagId")
                        .HasMaxLength(36);

                    b.HasKey("QuestionId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("QuestionTag");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.Tag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("QAWebsite.Models.UserModels.Achievement", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("AchievementImage");

                    b.Property<int>("Comparator");

                    b.Property<string>("Description");

                    b.Property<int>("Threshold");

                    b.Property<string>("Title");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Achievement");
                });

            modelBuilder.Entity("QAWebsite.Models.UserModels.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("QAWebsite.Models.UserModels.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AboutMe");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsEnabled");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<byte[]>("UserImage");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("QAWebsite.Models.UserModels.ApplicationUserAchievements", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AchievementId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAchievements");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("QAWebsite.Models.UserModels.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("QAWebsite.Models.UserModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("QAWebsite.Models.UserModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("QAWebsite.Models.UserModels.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QAWebsite.Models.UserModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("QAWebsite.Models.UserModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.Answer", b =>
                {
                    b.HasOne("QAWebsite.Models.QuestionModels.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.AnswerComment", b =>
                {
                    b.HasOne("QAWebsite.Models.QuestionModels.Answer", "Answer")
                        .WithMany("Comments")
                        .HasForeignKey("FkId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.Flag", b =>
                {
                    b.HasOne("QAWebsite.Models.QuestionModels.Question", "Question")
                        .WithMany("Flags")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.Question", b =>
                {
                    b.HasOne("QAWebsite.Models.UserModels.ApplicationUser", "Author")
                        .WithMany("Questions")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.QuestionComment", b =>
                {
                    b.HasOne("QAWebsite.Models.QuestionModels.Question", "Question")
                        .WithMany("Comments")
                        .HasForeignKey("FkId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QAWebsite.Models.QuestionModels.QuestionTag", b =>
                {
                    b.HasOne("QAWebsite.Models.QuestionModels.Question", "Question")
                        .WithMany("QuestionTags")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QAWebsite.Models.QuestionModels.Tag", "Tag")
                        .WithMany("QuestionTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QAWebsite.Models.UserModels.ApplicationUserAchievements", b =>
                {
                    b.HasOne("QAWebsite.Models.UserModels.Achievement", "Achievement")
                        .WithMany("UserAchievements")
                        .HasForeignKey("AchievementId");

                    b.HasOne("QAWebsite.Models.UserModels.ApplicationUser", "User")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
