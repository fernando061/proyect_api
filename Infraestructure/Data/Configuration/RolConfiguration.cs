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
                  //.HasDefaultValueSql("NEWID()")
                  //.HasDefaultValueSql("uuid()")//.HasDefaultValueSql("UUID()")    
                .IsRequired();

        builder.HasKey(k => k.Id)
               .HasName("PrimaryKey_IdRol");

        builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
       
        builder.HasData(
           new Rol()
           {
               Id = Guid.Parse("9dad84bd-48e3-4004-9c22-5d8b9c34b5dc"),
               Name = "Administrador",
           },
           new Rol()
           {
               Id = Guid.Parse("acfb60ee-9ab7-4674-a358-c25a2e2f4fbd"),
               Name = "Gerente",
           },
           new Rol()
           {
               Id = Guid.Parse("4abe9c57-d1ae-483c-850b-312ca04e5a78"),
               Name = "Empleado",
           }
       );
    }
}
