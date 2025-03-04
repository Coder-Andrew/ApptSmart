using System;
using System.Collections.Generic;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailableAppointment> AvailableAppointments { get; set; }

    public virtual DbSet<UserAppointment> UserAppointments { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=AppConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailableAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availabl__3214EC07F2447C31");

            entity.Property(e => e.Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAppo__3214EC072D70BA79");

            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.UserInfo).WithMany(p => p.UserAppointments)
                .HasForeignKey(d => d.UserInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk UserAppointments UserInfoId");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC07A59AD28C");

            entity.ToTable("UserInfo");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AspNetIdentityId).HasMaxLength(450);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
