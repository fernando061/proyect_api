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
                  //.HasDefaultValueSql("NEWID()")
                  //.HasDefaultValueSql("uuid()")//.HasDefaultValueSql("UUID()")
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

        builder
        .HasMany(p => p.Roles)
        .WithMany(p => p.User)
        .UsingEntity<UsersRoles>(
            j => j
                .HasOne(pt => pt.Rol)
                .WithMany(t => t.UsersRoles)
                .HasForeignKey(pt => pt.RolId),
            j => j
                .HasOne(pt => pt.User)
                .WithMany(p => p.UsersRoles)
                .HasForeignKey(pt => pt.UserId),
            j =>
            {
                j.HasKey(t => new { t.UserId, t.RolId });
            });

        builder.HasMany(p => p.RefreshTokens)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

    }
}
