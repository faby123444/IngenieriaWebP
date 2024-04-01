using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IngenieriaWebP.Models;

public partial class IngenieriaWebContext : DbContext
{
    public IngenieriaWebContext()
    {
    }

    public IngenieriaWebContext(DbContextOptions<IngenieriaWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=ProOS10;Initial Catalog=IngenieriaWeb;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC072A061D7F");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A196FBFB6DA").IsUnique();

            entity.HasIndex(e => e.Cedula, "UQ__Usuario__B4ADFE38EF96FD86").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(50);
            entity.Property(e => e.Cedula).HasMaxLength(20);
            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Contrasenia).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
