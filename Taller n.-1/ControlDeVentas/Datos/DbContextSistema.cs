using Datos.Mapping.Usuarios;
using Entidades;
using Entidades.Usuarios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DBContextSistema : DbContext
    {
        public DbSet<Roles> Rol { get; set; } = null!;
        public DBContextSistema() { }
        public DBContextSistema(DbContextOptions <DBContextSistema> options) : base(options) {}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Conexion");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RolesMap());
        }
    }
}
