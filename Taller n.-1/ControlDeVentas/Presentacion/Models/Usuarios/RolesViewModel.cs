
namespace Presentacion.Models.Usuarios
{
    public class RolesViewModel
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string DescripcionRol { get; set; } = string.Empty;
        public bool Estado { get; set; } = false;
    }
}
