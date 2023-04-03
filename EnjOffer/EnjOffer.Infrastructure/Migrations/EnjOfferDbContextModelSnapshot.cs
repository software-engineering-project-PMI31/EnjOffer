﻿// <auto-generated />
using System;
using EnjOffer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EnjOffer.Infrastructure.Migrations
{
    [DbContext(typeof(EnjOfferDbContext))]
    partial class EnjOfferDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.Advice", b =>
                {
                    b.Property<Guid>("AdviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("advice_id");

                    b.Property<string>("AdviceContent")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Advice isn't supplied")
                        .HasColumnName("advice_content");

                    b.Property<int>("AdviceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("advice_number");

                    b.HasKey("AdviceId");

                    b.HasIndex("AdviceNumber")
                        .IsUnique();

                    b.ToTable("advice", (string)null);
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.Books", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("book_id");

                    b.Property<string>("Author")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasDefaultValue("The author isn't supplied")
                        .HasColumnName("book_author");

                    b.Property<string>("Content")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Book content isn't supplied")
                        .HasColumnName("book_content");

                    b.Property<string>("Description")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Book description isn't supplied")
                        .HasColumnName("book_description");

                    b.Property<string>("ImageSrc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("imgNotFound.png")
                        .HasColumnName("book_image_src");

                    b.Property<string>("Title")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasDefaultValue("Book title isn't supplied")
                        .HasColumnName("book_title");

                    b.HasKey("BookId");

                    b.ToTable("books", (string)null);
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.DefaultWords", b =>
                {
                    b.Property<Guid>("DefaultWordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("default_word_id");

                    b.Property<string>("ImageSrc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("imgNotFound.png")
                        .HasColumnName("default_word_image_src");

                    b.Property<string>("Word")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)")
                        .HasDefaultValue("")
                        .HasColumnName("default_word_word");

                    b.Property<string>("WordTranslation")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)")
                        .HasDefaultValue("")
                        .HasColumnName("default_word_word_translation");

                    b.HasKey("DefaultWordId");

                    b.HasIndex("Word")
                        .IsUnique();

                    b.ToTable("default_words", (string)null);
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.UserStatistics", b =>
                {
                    b.Property<Guid>("UserStatisticsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_statistic_id");

                    b.Property<DateTime?>("AnswerDate")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(5678))
                        .HasColumnName("user_statistic_answer_date");

                    b.Property<int>("CorrectAnswersCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("user_statistic_correct_answer_count");

                    b.Property<int>("IncorrectAnswersCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("user_statistic_incorrect_answer_count");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("UserStatisticsId");

                    b.HasIndex("UserId");

                    b.ToTable("user_statistics", (string)null);
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.UserWords", b =>
                {
                    b.Property<Guid>("UserWordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_word_id");

                    b.Property<int>("CorrectEnteredCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("user_correct_entered_count");

                    b.Property<int>("IncorrectEnteredCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("user_incorrect_entered_count");

                    b.Property<DateTime?>("LastTimeEntered")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 4, 2, 19, 3, 3, 289, DateTimeKind.Local).AddTicks(2699))
                        .HasColumnName("user_last_time_entered");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(5)
                        .HasColumnName("user_word_priority");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Word")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)")
                        .HasDefaultValue("")
                        .HasColumnName("user_word_word");

                    b.Property<string>("WordTranslation")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)")
                        .HasDefaultValue("")
                        .HasColumnName("user_word_word_translation");

                    b.HasKey("UserWordId");

                    b.HasIndex("UserId");

                    b.HasIndex("Word")
                        .IsUnique();

                    b.ToTable("user_words", (string)null);
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.Users", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("varchar(320)")
                        .HasColumnName("user_email");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("user_password");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("SuperAdmin")
                        .HasColumnName("user_role");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.UsersDefaultWords", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DefaultWordId")
                        .HasColumnType("uuid");

                    b.Property<int>("CorrectEnteredCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("correct_entered_count");

                    b.Property<int>("IncorrectEnteredCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("incorrect_entered_count");

                    b.Property<DateTime?>("LastTimeEntered")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValue(new DateTime(2023, 4, 2, 19, 3, 3, 291, DateTimeKind.Local).AddTicks(3650))
                        .HasColumnName("last_time_entered");

                    b.Property<int>("Priority")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(5)
                        .HasColumnName("priority");

                    b.HasKey("UserId", "DefaultWordId");

                    b.HasIndex("DefaultWordId");

                    b.ToTable("users_default_words", (string)null);
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.UserStatistics", b =>
                {
                    b.HasOne("EnjOffer.Core.Domain.Entities.Users", "User")
                        .WithMany("UserStatistics")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.UserWords", b =>
                {
                    b.HasOne("EnjOffer.Core.Domain.Entities.Users", "User")
                        .WithMany("UserWords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.UsersDefaultWords", b =>
                {
                    b.HasOne("EnjOffer.Core.Domain.Entities.DefaultWords", "DefaultWord")
                        .WithMany("UsersDefaultWords")
                        .HasForeignKey("DefaultWordId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("EnjOffer.Core.Domain.Entities.Users", "User")
                        .WithMany("UsersDefaultWords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("DefaultWord");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.DefaultWords", b =>
                {
                    b.Navigation("UsersDefaultWords");
                });

            modelBuilder.Entity("EnjOffer.Core.Domain.Entities.Users", b =>
                {
                    b.Navigation("UserStatistics");

                    b.Navigation("UserWords");

                    b.Navigation("UsersDefaultWords");
                });
#pragma warning restore 612, 618
        }
    }
}
