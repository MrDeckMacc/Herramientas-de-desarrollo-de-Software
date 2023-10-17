using Entidades.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Mapping.Usuario
{
    public class UsuariosMap : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios>builder) 
        {
            builder.ToTable("Usuarios").HasKey(u => u.IdUsuario);
            builder.HasOne(r => r.IdRolNavigation).WithMany(u => u.Usuarios).HasForeignKey(d => d.IdRol);
            builder.Property(u => u.NombreUsuario).HasMaxLength(150);
            builder.Property(u => u.TipoDocumento).HasMaxLength(20);
            builder.Property(u => u.NumeroDocumento).HasMaxLength(20);
            builder.Property(u => u.NombreUsuario).HasMaxLength(150);
            builder.Property(u => u.Direccion).HasMaxLength(14);
            builder.Property(u => u.Telefono).HasMaxLength(150);
            builder.Property(u => u.PasswordHash);
            builder.Property(u => u.PasswordHash);

            builder.Property(u => u.Estado).HasDefaultValue(false);

        }  
    }
}
