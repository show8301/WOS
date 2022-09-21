using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WOS_Test.Models
{
    public partial class WOSContext : DbContext
    {
        public WOSContext()
        {
        }

        public WOSContext(DbContextOptions<WOSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserDatum> UserData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDatum>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserData__CB9A1CDF1E425CF6");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("userID");

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
