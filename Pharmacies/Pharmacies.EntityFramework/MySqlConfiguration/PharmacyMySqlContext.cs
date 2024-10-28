using Microsoft.EntityFrameworkCore;
using Pharmacies.Model;
using Pharmacies.Model.Reference;

namespace Pharmacies.EntityFramework.MySqlConfiguration;

public class PharmacyMySqlContext : DbContext
{
    public DbSet<Pharmacy> Pharmacies { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<ProductGroup> ProductGroups { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<PharmaceuticalGroup> PharmaceuticalGroups { get; set; }
    public DbSet<PharmaceuticalGroupReference> PharmaceuticalGroupReferences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
            "Server=mysql_database;Database=pharmacies;User=your_user;Password=your_password;",
            new MySqlServerVersion(new Version(8, 0, 21))
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Position
        modelBuilder.Entity<Position>()
            .HasKey(p => p.Code);

        modelBuilder.Entity<Position>()
            .HasOne(p => p.Pharmacy)
            .WithMany()
            .HasForeignKey("PharmacyId");

        modelBuilder.Entity<Position>()
            .HasOne(p => p.Price)
            .WithMany()
            .HasForeignKey("PriceId");
        
        modelBuilder.Entity<Position>()
            .HasOne(p => p.ProductGroup)
            .WithMany()
            .HasForeignKey("ProductGroupId");
        
        // PharmaceuticalGroupReference
        modelBuilder.Entity<PharmaceuticalGroupReference>()
            .HasKey(pgr => pgr.Id);

        modelBuilder.Entity<PharmaceuticalGroupReference>()
            .HasOne(pgr => pgr.PharmaceuticalGroup)
            .WithMany()
            .HasForeignKey("PharmaceuticalGroupId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PharmaceuticalGroupReference>()
            .HasOne(pgr => pgr.Position)
            .WithMany()
            .HasForeignKey("PositionId")
            .OnDelete(DeleteBehavior.Cascade);

        // Pharmacy
        modelBuilder.Entity<Pharmacy>()
            .HasKey(ph => ph.Number);

        // ProductGroup
        modelBuilder.Entity<ProductGroup>()
            .HasKey(pg => pg.Id);

        // Price
        modelBuilder.Entity<Price>()
            .HasKey(pr => pr.Id);

        // PharmaceuticalGroup
        modelBuilder.Entity<PharmaceuticalGroup>()
            .HasKey(pg => pg.Id);
    }
}