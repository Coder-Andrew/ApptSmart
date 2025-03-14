using System;
using System.Collections.Generic;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailableAppointment> AvailableAppointments { get; set; }

    public virtual DbSet<UserAppointment> UserAppointments { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailableAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availabl__3214EC079004A628");

            entity.Property(e => e.DateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAppo__3214EC07A44FA58F");

            entity.Property(e => e.DateTime).HasColumnType("datetime");

            entity.HasOne(d => d.UserInfo).WithMany(p => p.UserAppointments)
                .HasForeignKey(d => d.UserInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk UserAppointments UserInfoId");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC0779A58961");

            entity.ToTable("UserInfo");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AspNetIdentityId).HasMaxLength(450);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.LastName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
