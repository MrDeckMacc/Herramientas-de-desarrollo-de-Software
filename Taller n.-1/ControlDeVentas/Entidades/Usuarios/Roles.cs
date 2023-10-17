using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Usuarios
{
    public class Roles
    {
        public int IdRol { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Elnombre no debe tener menos de tres caracteres, ni mas de 100")]
        public string NombreRol { get; set; } = string.Empty;
        [StringLength(30)]
        public string DescripcionRol { get; set; } = string.Empty;
        [StringLength(100)]
        public bool Estado { get; set; } = false;


    }
}
