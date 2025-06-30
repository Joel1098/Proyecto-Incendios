using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Forestry.Models
{
    public class ContextoBaseDeDatos : DbContext
    {
        public ContextoBaseDeDatos(DbContextOptions<ContextoBaseDeDatos> opt) : base(opt) { }

        public ContextoBaseDeDatos() { }

        // DbSets for all entities
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Etapas> Etapas { get; set; }
        public DbSet<Incendio> Incendio { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Reporte> Reporte { get; set; }
        public DbSet<IncendioPersonal> IncendioPersonal { get; set; }
        public DbSet<BitacoraMedidaInicial> BitacoraMedidaInicial { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Esta configuración solo se usa para herramientas de diseño (migraciones, etc.)
                // En producción, se usa la configuración del Program.cs
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=forestrydb;Username=postgres;Password=forestry123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names to match PostgreSQL conventions
            modelBuilder.Entity<Usuarios>().ToTable("Usuarios");
            modelBuilder.Entity<Etapas>().ToTable("Etapas");
            modelBuilder.Entity<Incendio>().ToTable("Incendio");
            modelBuilder.Entity<Personal>().ToTable("Personal");
            modelBuilder.Entity<Reporte>().ToTable("Reporte");
            modelBuilder.Entity<IncendioPersonal>().ToTable("IncendioPersonal");
            modelBuilder.Entity<BitacoraMedidaInicial>().ToTable("BitacoraMedidaInicial");

            // Configure composite key for IncendioPersonal
            modelBuilder.Entity<IncendioPersonal>()
                .HasKey(ip => new { ip.idIncendio, ip.IdTrabajador });

            // Configure relationships
            modelBuilder.Entity<Incendio>()
                .HasOne(i => i.Etapa)
                .WithMany(e => e.Incendio)
                .HasForeignKey(i => i.idEtapa)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incendio>()
                .HasOne(i => i.UsuarioResponsable)
                .WithMany(u => u.IncendiosResponsable)
                .HasForeignKey(i => i.idUsuarioResponsable)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Incendio)
                .WithMany(i => i.Reporte)
                .HasForeignKey(r => r.idIncendio)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reporte)
                .HasForeignKey(r => r.idUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IncendioPersonal>()
                .HasOne(ip => ip.Incendio)
                .WithMany()
                .HasForeignKey(ip => ip.idIncendio)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IncendioPersonal>()
                .HasOne(ip => ip.Trabajador)
                .WithMany()
                .HasForeignKey(ip => ip.IdTrabajador)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes for better performance
            modelBuilder.Entity<Usuarios>()
                .HasIndex(u => u.Usuario)
                .IsUnique();

            modelBuilder.Entity<Usuarios>()
                .HasIndex(u => u.Estado);

            modelBuilder.Entity<Etapas>()
                .HasIndex(e => e.Orden);

            modelBuilder.Entity<Incendio>()
                .HasIndex(i => i.idEtapa);

            modelBuilder.Entity<Incendio>()
                .HasIndex(i => i.idUsuarioResponsable);

            modelBuilder.Entity<Incendio>()
                .HasIndex(i => i.Estado);

            modelBuilder.Entity<Personal>()
                .HasIndex(p => p.Estado);

            modelBuilder.Entity<Reporte>()
                .HasIndex(r => r.idIncendio);

            modelBuilder.Entity<Reporte>()
                .HasIndex(r => r.idUsuario);

            modelBuilder.Entity<Reporte>()
                .HasIndex(r => r.Fecha);

            // Configure default values
            modelBuilder.Entity<Usuarios>()
                .Property(u => u.Estado)
                .HasDefaultValue("Activo");

            modelBuilder.Entity<Etapas>()
                .Property(e => e.Estado)
                .HasDefaultValue("Activo");

            modelBuilder.Entity<Etapas>()
                .Property(e => e.Color)
                .HasDefaultValue("#007bff");

            modelBuilder.Entity<Incendio>()
                .Property(i => i.Estado)
                .HasDefaultValue("Activo");

            modelBuilder.Entity<Personal>()
                .Property(p => p.Estado)
                .HasDefaultValue("Activo");

            modelBuilder.Entity<Reporte>()
                .Property(r => r.Estado)
                .HasDefaultValue("Activo");

            modelBuilder.Entity<IncendioPersonal>()
                .Property(ip => ip.Estado)
                .HasDefaultValue("Activo");
        }
    }
}

