using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data.Configuration;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Rol");
       
        builder.Property(k => k.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

        builder.HasKey(k => k.Id)
               .HasName("PrimaryKey_IdRol");

        builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
    }
}
