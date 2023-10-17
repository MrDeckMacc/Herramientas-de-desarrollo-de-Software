using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entidades.Almacen;
using Microsoft.EntityFrameworkCore;

namespace Datos.Mapping.Almacen
{
    public class CategoriaMap:IEntityTypeConfiguration<Categoria>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias").HasKey(t => t.IdCategoria);
            builder.Property(t  => t.NombreCategoria).HasMaxLength(100);
            builder.Property(t => t.Descripcion).HasMaxLength(250);
            builder.Property(t => t.Estado).HasDefaultValue(false);
        }
    }
}
