using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NAUReviewApplication.Models
{
    public partial class NAUcountryContext : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Participant> Participant { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Survey> Survey { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public DbSet<SurveyResponse> SurveyResponse { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=NAUcountry;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.GroupId)
                    .HasColumnName("GroupID")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GroupID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.SurveyId);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("ID");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GroupId)
                    .HasColumnName("GroupID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.Type)
                    .IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creationDate")
                    .HasColumnType("date");

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<SurveyQuestion>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.HasKey(s => new { s.SurveyId, s.QuestionId });
                /*
                entity.HasOne(d => d.Question)
                    .WithMany(m => m.SurveyQuestions)
                    .HasForeignKey(d => d.QuestionId);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.SurveyId);
                */
                ///*
                entity.HasOne(d => d.Question)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.QuestionId);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.SurveyId); //*/
            });

            modelBuilder.Entity<SurveyResponse>(entity =>
            {
                entity.HasKey(e => e.ResponseId);

                entity.Property(e => e.ResponseId).HasColumnName("ResponseID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(d => d.QuestionId);

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyResponse)
                    .HasForeignKey(d => d.SurveyId);
            });
        }
    }
}
