using Datos.Mapping.Almacen;
using Datos.Mapping.Usuario;
using Entidades;
using Entidades.Almacen;
using Entidades.Usuario;
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
        public DbSet<Personas> Persona { get; set; } = null!;
        public DbSet<Roles> Rol { get; set; } = null!;
        public DbSet<Usuarios> Usuario { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Articulo> Articulos { get; set; } = null!;
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
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new RolesMap());
            modelBuilder.ApplyConfiguration(new UsuariosMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
        }
    }
}
