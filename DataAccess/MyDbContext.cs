using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WindMill.DataAccess;

public partial class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TurbineAlert> TurbineAlerts { get; set; }

    public virtual DbSet<TurbineCommandHistory> TurbineCommandHistories { get; set; }

    public virtual DbSet<TurbineSettingsHistory> TurbineSettingsHistories { get; set; }

    public virtual DbSet<TurbineTelemetry> TurbineTelemetries { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<TurbineAlert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("turbine_alerts_pkey");

            entity.ToTable("turbine_alerts");

            entity.HasIndex(e => new { e.TurbineId, e.Timestamp }, "idx_alerts_turbine_time").IsDescending(false, true);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Severity).HasColumnName("severity");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.TurbineId).HasColumnName("turbine_id");
        });

        modelBuilder.Entity<TurbineCommandHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("turbine_command_history_pkey");

            entity.ToTable("turbine_command_history");

            entity.HasIndex(e => new { e.TurbineId, e.CreatedAt }, "idx_command_turbine_time").IsDescending(false, true);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.Angle).HasColumnName("angle");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.TurbineId).HasColumnName("turbine_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.User).WithMany(p => p.TurbineCommandHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_command_user");
        });

        modelBuilder.Entity<TurbineSettingsHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("turbine_settings_history_pkey");

            entity.ToTable("turbine_settings_history");

            entity.HasIndex(e => new { e.TurbineId, e.CreatedAt }, "idx_settings_turbine_time").IsDescending(false, true);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Settings)
                .HasColumnType("jsonb")
                .HasColumnName("settings");
            entity.Property(e => e.TurbineId).HasColumnName("turbine_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TurbineSettingsHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_settings_user");
        });

        modelBuilder.Entity<TurbineTelemetry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("turbine_telemetry_pkey");

            entity.ToTable("turbine_telemetry");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AmbientTemperature).HasColumnName("ambient_temperature");
            entity.Property(e => e.BladePitch).HasColumnName("blade_pitch");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.GearboxTemp).HasColumnName("gearbox_temp");
            entity.Property(e => e.GeneratorTemp).HasColumnName("generator_temp");
            entity.Property(e => e.IsRunning).HasColumnName("is_running");
            entity.Property(e => e.NacelleDirection).HasColumnName("nacelle_direction");
            entity.Property(e => e.PowerOutput).HasColumnName("power_output");
            entity.Property(e => e.RotorSpeed).HasColumnName("rotor_speed");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.TurbineId).HasColumnName("turbine_id");
            entity.Property(e => e.Vibration).HasColumnName("vibration");
            entity.Property(e => e.WindDirection).HasColumnName("wind_direction");
            entity.Property(e => e.WindSpeed).HasColumnName("wind_speed");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "idx_users_username");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
