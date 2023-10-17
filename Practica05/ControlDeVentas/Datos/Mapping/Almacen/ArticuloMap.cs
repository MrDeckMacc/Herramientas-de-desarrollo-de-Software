using Entidades.Almacen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Mapping.Almacen
{
    public class ArticuloMap : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder) 
        {
            builder.ToTable("Articulos").HasKey(c => c.IdArticulo);
            builder.HasOne(c => c.IdCategoriaNavigation).WithMany(a => a.Articulos).HasForeignKey(d => d.IdCategoria);
            builder.Property(c => c.CodigoArticulo).HasMaxLength(50);
            builder.Property(c => c.NombreArticulo).HasMaxLength(150);
            builder.Property(c => c.PrecioVenta);
            builder.Property(c => c.Stock);
            builder.Property(c => c.DescripcionArticulo).HasMaxLength(250);
            builder.Property(c => c.Estado).HasDefaultValue(false);
        }
    }
}
