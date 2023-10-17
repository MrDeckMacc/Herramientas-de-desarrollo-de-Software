using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Usuario
{
    public class Roles
    {
        [Key]
        public int IdRol { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El nombre no debe tener menos de 5 caracteres, ni mas de 30")]
        public string NombreRol { get; set; } = string.Empty;
        [StringLength(100)]
        public string DescripcionRol { get; set; } = string.Empty;
        public bool Estado { get; set; } = false;
        [ForeignKey("IdRol")]
        public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
    }
}
