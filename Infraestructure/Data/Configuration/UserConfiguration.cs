using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USER", "dbo");

        builder.Property(k => k.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

        builder.HasKey(k => k.Id)
               .HasName("PrimaryKey_IdUser");

        builder.Property(p => p.Name)
                  .HasColumnType("varchar()")
                  .HasMaxLength(40)
                  .IsRequired();

        builder.Property(p => p.Email)
                  .HasColumnType("varchar()")
                  .HasMaxLength(40)
                  .IsRequired();

        builder.Property(p => p.Password)
                  .HasColumnType("varchar()")
                  .HasMaxLength(255)
                  .IsRequired();

        builder.Property(p => p.LastName)
                  .HasColumnType("varchar()")
                  .HasMaxLength(40)
                  .IsRequired();
        builder.Property(p => p.MiddleName)
                 .HasColumnType("varchar()")
                 .HasMaxLength(40)
                 .IsRequired();

    }
}
