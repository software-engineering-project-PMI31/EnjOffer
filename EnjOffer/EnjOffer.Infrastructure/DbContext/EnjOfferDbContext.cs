using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Domain.IdentityEntities;
using EnjOffer.Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EnjOffer.Infrastructure
{
    public class EnjOfferDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Advice>? Advice { get; set; }

        public virtual DbSet<Books>? Books { get; set; }

        public virtual DbSet<DefaultWords>? DefaultWords { get; set; }

        public virtual DbSet<UserWords>? UserWords { get; set; }

        public virtual DbSet<UserStatistics>? UserStatistics { get; set; }

        public virtual DbSet<UsersDefaultWords>? UsersDefaultWords { get; set; }


        public EnjOfferDbContext(DbContextOptions options) : base(options)
        {

        }

        // TO-DO: Switch off cascade deleting
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var converter = new EnumToStringConverter<UserRole>();

            modelBuilder.Entity<Advice>().ToTable("advice");
            modelBuilder.Entity<Books>().ToTable("books");
            modelBuilder.Entity<DefaultWords>().ToTable("default_words");
            modelBuilder.Entity<UserWords>().ToTable("user_words");
            //modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<UserStatistics>().ToTable("user_statistics");

            modelBuilder.Entity<Advice>().HasKey(t => t.AdviceId);
            modelBuilder.Entity<Advice>().HasIndex(t => t.AdviceNumber).IsUnique();
            modelBuilder.Entity<Advice>().Property(t => t.AdviceId).HasColumnName("advice_id").HasColumnType("uuid");
            modelBuilder.Entity<Advice>().Property(t => t.AdviceNumber).HasColumnName("advice_number").HasColumnType("integer").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Advice>().Property(t => t.AdviceContent).HasColumnName("advice_content").HasColumnType("text").IsRequired().HasDefaultValue("Advice isn't supplied");

            /*modelBuilder.Entity<Users>().HasKey(t => t.UserId);
            modelBuilder.Entity<Users>().HasIndex(t => t.Email).IsUnique();
            modelBuilder.Entity<Users>().Property(t => t.UserId).HasColumnName("user_id").HasColumnType("uuid");
            modelBuilder.Entity<Users>().Property(t => t.Email).HasColumnName("user_email").HasColumnType("varchar(320)").HasMaxLength(320).IsRequired();
            modelBuilder.Entity<Users>().Property(t => t.Password).HasColumnName("user_password").HasColumnType("bytea").IsRequired();
            modelBuilder.Entity<Users>().Property(t => t.Role).HasConversion(converter).HasColumnName("user_role").IsRequired().HasDefaultValue<UserRole>(UserRole.SuperAdmin);*/

            modelBuilder.Entity<Books>().HasKey(t => t.BookId);
            modelBuilder.Entity<Books>().Property(t => t.BookId).HasColumnName("book_id").HasColumnType("uuid");
            modelBuilder.Entity<Books>().Property(t => t.Title).HasColumnName("book_title").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired().HasDefaultValue("Book title isn't supplied");
            modelBuilder.Entity<Books>().Property(t => t.Description).HasColumnName("book_description").HasColumnType("text").IsRequired().HasDefaultValue("Book description isn't supplied");
            modelBuilder.Entity<Books>().Property(t => t.Author).HasColumnName("book_author").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired().HasDefaultValue("The author isn't supplied");
            modelBuilder.Entity<Books>().Property(t => t.Content).HasColumnName("book_content").HasColumnType("text").IsRequired().HasDefaultValue("Book content isn't supplied");
            modelBuilder.Entity<Books>().Property(t => t.ImageSrc).HasColumnName("book_image_src").HasColumnType("text").HasDefaultValue("imgNotFound.png");

            modelBuilder.Entity<DefaultWords>().HasKey(t => t.DefaultWordId);
            modelBuilder.Entity<DefaultWords>().HasIndex(t => new { t.Word, t.WordTranslation }).IsUnique();
            modelBuilder.Entity<DefaultWords>().Property(t => t.DefaultWordId).HasColumnName("default_word_id").HasColumnType("uuid");
            modelBuilder.Entity<DefaultWords>().Property(t => t.Word).HasColumnName("default_word_word").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired().HasDefaultValue(string.Empty);
            modelBuilder.Entity<DefaultWords>().Property(t => t.WordTranslation).HasColumnName("default_word_word_translation").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired().HasDefaultValue(string.Empty);
            modelBuilder.Entity<DefaultWords>().Property(t => t.ImageSrc).HasColumnName("default_word_image_src").HasColumnType("text").HasDefaultValue("imgNotFound.png");

            modelBuilder.Entity<UserWords>().HasKey(t => t.UserWordId);
            modelBuilder.Entity<UserWords>().HasIndex(t => t.Word).IsUnique();
            modelBuilder.Entity<UserWords>().Property(t => t.UserWordId).HasColumnName("user_word_id").HasColumnType("uuid");
            modelBuilder.Entity<UserWords>().Property(t => t.Word).HasColumnName("user_word_word").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired().HasDefaultValue(string.Empty);
            modelBuilder.Entity<UserWords>().Property(t => t.WordTranslation).HasColumnName("user_word_word_translation").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired().HasDefaultValue(string.Empty);
            modelBuilder.Entity<UserWords>().Property(t => t.LastTimeEntered).HasColumnName("user_last_time_entered").HasColumnType("date");
            modelBuilder.Entity<UserWords>().Property(t => t.CorrectEnteredCount).HasColumnName("user_correct_entered_count").HasColumnType("integer").IsRequired();
            modelBuilder.Entity<UserWords>().Property(t => t.IncorrectEnteredCount).HasColumnName("user_incorrect_entered_count").HasColumnType("integer").IsRequired();

            modelBuilder.Entity<UserStatistics>().HasKey(t => t.UserStatisticsId);
            modelBuilder.Entity<UserStatistics>().Property(t => t.UserStatisticsId).HasColumnName("user_statistic_id").HasColumnType("uuid");
            modelBuilder.Entity<UserStatistics>().Property(t => t.AnswerDate).HasColumnName("user_statistic_answer_date").HasColumnType("date").IsRequired();
            modelBuilder.Entity<UserStatistics>().Property(t => t.CorrectAnswersCount).HasColumnName("user_statistic_correct_answer_count").HasColumnType("integer").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<UserStatistics>().Property(t => t.IncorrectAnswersCount).HasColumnName("user_statistic_incorrect_answer_count").HasColumnType("integer").IsRequired().HasDefaultValue(0);
            //modelBuilder.Entity<Users>().HasMany(u => u.UserStatistics).WithOne(us => us.User).HasForeignKey(us => us.UserId).OnDelete(DeleteBehavior.SetNull);
            //modelBuilder.Entity<Users>().HasMany(u => u.UserWords).WithOne(uw => uw.User).HasForeignKey(uw => uw.UserId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ApplicationUser>().HasMany(t => t.UserStatistics).WithOne(t => t.User).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>().HasMany(t => t.UserWords).WithOne(t => t.User).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.DefaultWords).WithMany(dw => dw.Users)
                .UsingEntity<UsersDefaultWords>(
                j => j
                .HasOne(pt => pt.DefaultWord)
                .WithMany(t => t.UsersDefaultWords)
                .HasForeignKey(pt => pt.DefaultWordId).OnDelete(DeleteBehavior.Cascade),
                j => j
                .HasOne(pt => pt.User)
                .WithMany(t => t.UsersDefaultWords)
                .HasForeignKey(pt => pt.UserId).OnDelete(DeleteBehavior.Cascade),

                j =>
                {
                    j.HasKey(t => new { t.UserId, t.DefaultWordId });

                    j
                    .Property(j => j.LastTimeEntered)
                    .HasColumnType("date")
                    .HasColumnName("last_time_entered");

                    j
                    .Property(j => j.CorrectEnteredCount)
                    .HasColumnType("integer")
                    .HasColumnName("correct_entered_count")
                    .IsRequired();

                    j
                    .Property(j => j.IncorrectEnteredCount)
                    .HasColumnType("integer")
                    .HasColumnName("incorrect_entered_count")
                    .IsRequired();

                    j.ToTable("users_default_words");
                }
                );
        }
    }
}
