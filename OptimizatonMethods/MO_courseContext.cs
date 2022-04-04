using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OptimizatonMethods
{
    public partial class MO_courseContext : DbContext
    {
        public MO_courseContext()
        {
        }

        public MO_courseContext(DbContextOptions<MO_courseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Method> Methods { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=MO_course.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Method>(entity =>
            {
                entity.ToTable("Method");

                entity.HasIndex(e => e.Id, "IX_Method_Id")
                    .IsUnique();
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.HasIndex(e => e.Id, "IX_Task_Id")
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Id, "IX_User_Id")
                    .IsUnique();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
