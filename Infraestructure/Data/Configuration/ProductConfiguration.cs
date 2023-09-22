using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configuration;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("PRODUCT", "dbo");

        builder.Property(k => k.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

        builder.HasKey(k => k.Id)
               .HasName("PrimaryKey_IdProduct");

        builder.Property(p => p.Name)
              .HasColumnType("varchar()")
              .HasMaxLength(40)
              .IsRequired();

        builder.Property(p => p.Price)
              .HasColumnType("decimal(10,2)")
              .IsRequired();
        builder.Property(p => p.Amount)
              .HasColumnType("int")
              .IsRequired();
    }
}

