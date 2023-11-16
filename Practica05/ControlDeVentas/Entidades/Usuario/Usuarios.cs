using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Usuario
{
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required]
        [ForeignKey("IdRol")]
        public int IdRol { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El Nombre del usario no debe tener menos de 3 carasteres ni mas de 150")]
        public string NombreUsuario { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public bool Estado { get; set; } = true;
        public Roles Rol { get; set; }
    }
}
