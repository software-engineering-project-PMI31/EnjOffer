using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnjOffer.Infrastructure
{
    public class EnjOfferDbContext : DbContext
    {
        public DbSet<Advice>? Advice { get; set; }

        public DbSet<Books>? Books { get; set; }

        public DbSet<DefaultWords>? DefaultWords { get; set; }

        public DbSet<UserWords>? UserWords { get; set; }

        public DbSet<Users>? Users { get; set; }

        public DbSet<UserStatistics>? UserStatistics { get; set; }

        public DbSet<UsersBooks>? UsersBooks { get; set; }

        public DbSet<UsersAdvice>? UsersAdvice { get; set; }

        public DbSet<UsersDefaultWords>? UsersDefaultWords { get; set; }


        public EnjOfferDbContext(DbContextOptions options) : base(options)
        {

        }

        // TO-DO: Switch off cascade deleting
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Advice>().ToTable("advice");
            modelBuilder.Entity<Books>().ToTable("books");
            modelBuilder.Entity<DefaultWords>().ToTable("default_words");
            modelBuilder.Entity<UserWords>().ToTable("user_words");
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<UserStatistics>().ToTable("user_statistics");

            modelBuilder.Entity<Advice>().HasKey(t => t.AdviceId);
            modelBuilder.Entity<Advice>().HasIndex(t => t.AdviceNumber).IsUnique();
            modelBuilder.Entity<Advice>().Property(t => t.AdviceId).HasColumnName("advice_id").HasColumnType("uuid");
            modelBuilder.Entity<Advice>().Property(t => t.AdviceNumber).HasColumnName("advice_number").HasColumnType("integer").IsRequired();
            modelBuilder.Entity<Advice>().Property(t => t.AdviceContent).HasColumnName("advice_content").HasColumnType("text").IsRequired();

            modelBuilder.Entity<Users>().HasKey(t => t.UserId);
            modelBuilder.Entity<Users>().HasIndex(t => t.Email).IsUnique();
            modelBuilder.Entity<Users>().Property(t => t.UserId).HasColumnName("user_id").HasColumnType("uuid");
            modelBuilder.Entity<Users>().Property(t => t.Email).HasColumnName("user_email").HasColumnType("varchar(320)").HasMaxLength(320).IsRequired();
            modelBuilder.Entity<Users>().Property(t => t.Password).HasColumnName("user_password").HasColumnType("bytea").IsRequired();
            modelBuilder.Entity<Users>().Property(t => t.Role).HasConversion<string>().HasColumnName("user_role").IsRequired();

            modelBuilder.Entity<Books>().HasKey(t => t.BookId);
            modelBuilder.Entity<Books>().Property(t => t.BookId).HasColumnName("book_id").HasColumnType("uuid");
            modelBuilder.Entity<Books>().Property(t => t.Title).HasColumnName("book_title").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Books>().Property(t => t.Description).HasColumnName("book_description").HasColumnType("text").IsRequired();
            modelBuilder.Entity<Books>().Property(t => t.Author).HasColumnName("book_author").HasColumnType("varchar(200)").HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Books>().Property(t => t.Content).HasColumnName("book_content").HasColumnType("text").IsRequired();
            modelBuilder.Entity<Books>().Property(t => t.ImageSrc).HasColumnName("book_image_src").HasColumnType("text");

            modelBuilder.Entity<DefaultWords>().HasKey(t => t.DefaultWordId);
            modelBuilder.Entity<DefaultWords>().HasIndex(t => t.Word).IsUnique();
            modelBuilder.Entity<DefaultWords>().Property(t => t.DefaultWordId).HasColumnName("default_word_id").HasColumnType("uuid");
            modelBuilder.Entity<DefaultWords>().Property(t => t.Word).HasColumnName("default_word_word").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired();
            modelBuilder.Entity<DefaultWords>().Property(t => t.WordTranslation).HasColumnName("default_word_word_translation").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired();
            modelBuilder.Entity<DefaultWords>().Property(t => t.Priority).HasColumnName("default_word_priority").HasColumnType("integer").IsRequired();
            modelBuilder.Entity<DefaultWords>().Property(t => t.ImageSrc).HasColumnName("default_word_image_src").HasColumnType("text");

            modelBuilder.Entity<UserWords>().HasKey(t => t.UserWordId);
            modelBuilder.Entity<UserWords>().HasIndex(t => t.Word).IsUnique();
            modelBuilder.Entity<UserWords>().Property(t => t.UserWordId).HasColumnName("user_word_id").HasColumnType("uuid");
            modelBuilder.Entity<UserWords>().Property(t => t.Word).HasColumnName("user_word_word").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired();
            modelBuilder.Entity<UserWords>().Property(t => t.WordTranslation).HasColumnName("user_word_word_translation").HasColumnType("varchar(400)").HasMaxLength(400).IsRequired();
            modelBuilder.Entity<UserWords>().Property(t => t.Priority).HasColumnName("user_word_priority").HasColumnType("integer").IsRequired();

            modelBuilder.Entity<UserStatistics>().HasKey(t => t.UserStatisticsId);
            modelBuilder.Entity<UserStatistics>().Property(t => t.UserStatisticsId).HasColumnName("user_statistic_id").HasColumnType("uuid");
            modelBuilder.Entity<UserStatistics>().Property(t => t.AnswerDate).HasColumnName("user_statistic_answer_date").HasColumnType("date").IsRequired();
            modelBuilder.Entity<UserStatistics>().Property(t => t.CorrectAnswersCount).HasColumnName("user_statistic_correct_answer_count").HasColumnType("integer").IsRequired();
            modelBuilder.Entity<UserStatistics>().Property(t => t.IncorrectAnswersCount).HasColumnName("user_statistic_incorrect_answer_count").HasColumnType("integer").IsRequired();

            modelBuilder.Entity<Users>().HasMany(u => u.UserStatistics).WithOne(us => us.User).HasForeignKey(us => us.UserId);
            modelBuilder.Entity<Users>().HasMany(u => u.UserWords).WithOne(uw => uw.User).HasForeignKey(uw => uw.UserId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasMany(u => u.Books).WithMany(b => b.Users).UsingEntity<UsersBooks>(
                j => j
                .HasOne(pt => pt.Book)
                .WithMany(t => t.UsersBooks)
                .HasForeignKey(pt => pt.BookId),
                j => j
                .HasOne(pt => pt.User)
                .WithMany(t => t.UsersBooks)
                .HasForeignKey(pt => pt.UserId),
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.BookId });
                    j.ToTable("users_books");
                }
                );
            modelBuilder.Entity<Users>().HasMany(u => u.DefaultWords).WithMany(dw => dw.Users).UsingEntity<UsersDefaultWords>(
                j => j
                .HasOne(pt => pt.DefaultWord)
                .WithMany(t => t.UsersDefaultWords)
                .HasForeignKey(pt => pt.DefaultWordId),
                j => j
                .HasOne(pt => pt.User)
                .WithMany(t => t.UsersDefaultWords)
                .HasForeignKey(pt => pt.UserId),
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.DefaultWordId });
                    j.ToTable("users_default_words");
                }
                );

            modelBuilder.Entity<Users>().HasMany(u => u.Advice).WithMany(a => a.Users).UsingEntity<UsersAdvice>(
                j => j
                .HasOne(pt => pt.Advice)
                .WithMany(t => t.UsersAdvice)
                .HasForeignKey(pt => pt.AdviceId),
                j => j
                .HasOne(pt => pt.User)
                .WithMany(t => t.UsersAdvice)
                .HasForeignKey(pt => pt.UserId),
                j =>
                {
                    j.HasKey(t => new { t.UserId, t.AdviceId });
                    j.ToTable("users_advice");
                }
                );
        }
    }
}
