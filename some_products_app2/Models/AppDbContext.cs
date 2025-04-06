using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace some_products_app2.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<PartnerImport> PartnerImports { get; set; }

    public virtual DbSet<PartnerType> PartnerTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=Mama20199!;database=some_products", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.DirectorId).HasName("PRIMARY");

            entity.ToTable("director");

            entity.Property(e => e.DirectorId).HasColumnName("director_id");
            entity.Property(e => e.DirectorEmail)
                .HasMaxLength(30)
                .HasColumnName("director_email");
            entity.Property(e => e.DirectorName)
                .HasMaxLength(45)
                .HasColumnName("director_name");
            entity.Property(e => e.DirectorPatronymic)
                .HasMaxLength(45)
                .HasColumnName("director_patronymic");
            entity.Property(e => e.DirectorPhone)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("director_phone");
            entity.Property(e => e.DirectorSurname)
                .HasMaxLength(45)
                .HasColumnName("director_surname");
        });

        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.MaterialTypeId).HasName("PRIMARY");

            entity.ToTable("material_type");

            entity.Property(e => e.MaterialTypeId).HasColumnName("material_type_id");
            entity.Property(e => e.MaterialType1)
                .HasMaxLength(45)
                .HasColumnName("material_type");
            entity.Property(e => e.MaterialTypeBrakPercent).HasColumnName("material_type_brak_percent");
        });

        modelBuilder.Entity<PartnerImport>(entity =>
        {
            entity.HasKey(e => e.PartnerImportId).HasName("PRIMARY");

            entity.ToTable("partner_import");

            entity.HasIndex(e => e.DirectorId, "director_fk_idx");

            entity.HasIndex(e => e.PartnerTypeId, "partner_type_fk_idx");

            entity.Property(e => e.PartnerImportId).HasColumnName("partner_import_id");
            entity.Property(e => e.DirectorId).HasColumnName("director_id");
            entity.Property(e => e.PartnerImport1)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("partner_import");
            entity.Property(e => e.PartnerImportCity)
                .HasMaxLength(45)
                .HasColumnName("partner_import_city");
            entity.Property(e => e.PartnerImportHouse)
                .HasMaxLength(10)
                .HasColumnName("partner_import_house");
            entity.Property(e => e.PartnerImportIndex)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("partner_import_index");
            entity.Property(e => e.PartnerImportName)
                .HasMaxLength(45)
                .HasColumnName("partner_import_name");
            entity.Property(e => e.PartnerImportOblast)
                .HasMaxLength(80)
                .HasColumnName("partner_import_oblast");
            entity.Property(e => e.PartnerImportRating).HasColumnName("partner_import_rating");
            entity.Property(e => e.PartnerImportStreet)
                .HasMaxLength(45)
                .HasColumnName("partner_import_street");
            entity.Property(e => e.PartnerTypeId).HasColumnName("partner_type_id");

            entity.HasOne(d => d.Director).WithMany(p => p.PartnerImports)
                .HasForeignKey(d => d.DirectorId)
                .HasConstraintName("director_fk");

            entity.HasOne(d => d.PartnerType).WithMany(p => p.PartnerImports)
                .HasForeignKey(d => d.PartnerTypeId)
                .HasConstraintName("partner_type_fk");
        });

        modelBuilder.Entity<PartnerType>(entity =>
        {
            entity.HasKey(e => e.PartnerTypeId).HasName("PRIMARY");

            entity.ToTable("partner_type");

            entity.Property(e => e.PartnerTypeId).HasColumnName("partner_type_id");
            entity.Property(e => e.PartnerTypeName)
                .HasMaxLength(10)
                .HasColumnName("partner_type_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Article).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductTypeId, "product_type_fk_idx");

            entity.Property(e => e.Article)
                .ValueGeneratedNever()
                .HasColumnName("article");
            entity.Property(e => e.ProductMinPrice).HasColumnName("product_min_price");
            entity.Property(e => e.ProductName)
                .HasColumnType("text")
                .HasColumnName("product_name");
            entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("product_type_fk");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PRIMARY");

            entity.ToTable("product_type");

            entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
            entity.Property(e => e.ProductTypeCoeff).HasColumnName("product_type_coeff");
            entity.Property(e => e.ProductTypeName)
                .HasMaxLength(45)
                .HasColumnName("product_type_name");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PRIMARY");

            entity.ToTable("sale");

            entity.HasIndex(e => e.PartnerImportId, "partner_fk_idx");

            entity.HasIndex(e => e.Article, "product_fk_idx");

            entity.Property(e => e.SaleId).HasColumnName("sale_id");
            entity.Property(e => e.Article).HasColumnName("article");
            entity.Property(e => e.PartnerImportId).HasColumnName("partner_import_id");
            entity.Property(e => e.ProductCount).HasColumnName("product_count");
            entity.Property(e => e.SaleDate).HasColumnName("sale_date");

            entity.HasOne(d => d.ArticleNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.Article)
                .HasConstraintName("product_fk");

            entity.HasOne(d => d.PartnerImport).WithMany(p => p.Sales)
                .HasForeignKey(d => d.PartnerImportId)
                .HasConstraintName("partner_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
