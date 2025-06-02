using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Task4.Models;

public partial class PizzasStoreContext : DbContext
{
    public PizzasStoreContext()
    {
    }

    public PizzasStoreContext(DbContextOptions<PizzasStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DoughType> DoughTypes { get; set; }

    public virtual DbSet<Pizza> Pizzas { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoughType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DoughTyp__3214EC07170F028E");

            entity.HasIndex(e => e.Name, "UQ__DoughTyp__737584F693538072").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Pizza>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pizzas__3214EC0748709BFC");

            entity.HasIndex(e => e.Name, "UQ__Pizzas__737584F6BF059A07").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Weight).HasDefaultValueSql("((100))");

            entity.HasMany(d => d.DoughTypes).WithMany(p => p.Pizzas)
                .UsingEntity<Dictionary<string, object>>(
                    "PizzaDoughType",
                    r => r.HasOne<DoughType>().WithMany()
                        .HasForeignKey("DoughTypeId")
                        .HasConstraintName("FK__PizzaDoug__Dough__49C3F6B7"),
                    l => l.HasOne<Pizza>().WithMany()
                        .HasForeignKey("PizzaId")
                        .HasConstraintName("FK__PizzaDoug__Pizza__48CFD27E"),
                    j =>
                    {
                        j.HasKey("PizzaId", "DoughTypeId").HasName("PK__PizzaDou__E904E5436B9043DE");
                        j.ToTable("PizzaDoughTypes");
                    });

            entity.HasMany(d => d.Sizes).WithMany(p => p.Pizzas)
                .UsingEntity<Dictionary<string, object>>(
                    "PizzaSize",
                    r => r.HasOne<Size>().WithMany()
                        .HasForeignKey("SizeId")
                        .HasConstraintName("FK__PizzaSize__SizeI__45F365D3"),
                    l => l.HasOne<Pizza>().WithMany()
                        .HasForeignKey("PizzaId")
                        .HasConstraintName("FK__PizzaSize__Pizza__44FF419A"),
                    j =>
                    {
                        j.HasKey("PizzaId", "SizeId").HasName("PK__PizzaSiz__B35BC24A3A7854DD");
                        j.ToTable("PizzaSizes");
                    });
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sizes__3214EC0708CFDB33");

            entity.HasIndex(e => e.Value, "UQ__Sizes__07D9BBC2F93AC73B").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
