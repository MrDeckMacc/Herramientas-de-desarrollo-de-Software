using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entidades.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datos.Mapping.Usuario
{
    internal class PersonaMap : IEntityTypeConfiguration<Personas>
    {
        public void Configure (EntityTypeBuilder<Personas> builder)
        {
            builder.ToTable("Personas").HasKey(u => u.IdPersona);
        }
    }
}
