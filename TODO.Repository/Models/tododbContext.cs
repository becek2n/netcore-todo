using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TODO.Repository.Models
{
    public partial class tododbContext : DbContext
    {
        public tododbContext()
        {
        }

        public tododbContext(DbContextOptions<tododbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=(local);user=sa;password=1q2w3e;database=todo-db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("tasks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("created_by");

                entity.Property(e => e.DateStarted)
                    .HasColumnType("datetime")
                    .HasColumnName("date_started")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpdated)
                    .HasColumnType("datetime")
                    .HasColumnName("date_updated");

                entity.Property(e => e.Day)
                    .HasColumnType("date")
                    .HasColumnName("day");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("updated_by");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasColumnType("text")
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<Task>().HasData(
                new Task() { Id = 1, Name = "task 1", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 2, Name = "task 2", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 3, Name = "task 3", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 4, Name = "task 4", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 5, Name = "task 5", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 6, Name = "task 6", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 7, Name = "task 7", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 8, Name = "task 8", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 9, Name = "task 9", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 10, Name = "task 10", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 11, Name = "task 11", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 12, Name = "task 12", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 13, Name = "task 13", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 14, Name = "task 14", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 15, Name = "task 15", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 16, Name = "task 16", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 17, Name = "task 17", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 18, Name = "task 18", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 19, Name = "task 19", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 20, Name = "task 20", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 21, Name = "task 21", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 22, Name = "task 22", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 23, Name = "task 23", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 24, Name = "task 24", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 25, Name = "task 25", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 26, Name = "task 26", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 27, Name = "task 27", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 28, Name = "task 28", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 29, Name = "task 29", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 30, Name = "task 30", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 31, Name = "task 31", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 32, Name = "task 32", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 33, Name = "task 33", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 34, Name = "task 34", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 35, Name = "task 35", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 36, Name = "task 36", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 37, Name = "task 37", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 38, Name = "task 38", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 39, Name = "task 39", Day = DateTime.Parse("2021-01-01"), Status = false, },
                new Task() { Id = 40, Name = "task 40", Day = DateTime.Parse("2021-01-01"), Status = false, }


            );

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
