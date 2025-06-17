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

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<UserAppointment> UserAppointments { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3214EC07EB18C5A9");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Company");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3214EC079AF5FB29");

            entity.ToTable("Company");

            entity.HasIndex(e => e.CompanyName, "UQ__Company__9BCE05DC58BF6AA3").IsUnique();

            entity.HasIndex(e => e.CompanySlug, "UQ__Company__9E3A4EAE9C7F75CE").IsUnique();

            entity.Property(e => e.CompanyName).HasMaxLength(150);
            entity.Property(e => e.CompanySlug).HasMaxLength(150);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Owner).WithMany(p => p.Companies)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_UserInfo");
        });

        modelBuilder.Entity<UserAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAppo__3214EC07D2B6AED5");

            entity.HasIndex(e => e.AppointmentId, "UQ_UserAppointments_AppointmentId").IsUnique();

            entity.Property(e => e.BookedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithOne(p => p.UserAppointment)
                .HasForeignKey<UserAppointment>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAppointments_Appointments");

            entity.HasOne(d => d.UserInfo).WithMany(p => p.UserAppointments)
                .HasForeignKey(d => d.UserInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAppointments_UserInfo");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC07977F1668");

            entity.ToTable("UserInfo");

            entity.HasIndex(e => e.AspNetIdentityId, "UQ__UserInfo__CE9D5B260EC16366").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.LastName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
