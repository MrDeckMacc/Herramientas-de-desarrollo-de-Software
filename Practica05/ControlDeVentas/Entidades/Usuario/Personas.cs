using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Usuario
{
    public class Personas
    {
        [Key]
        [Required]
        public int IdPersona { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre debe tener mas de tres caracteres, ni mas de 150")]
        public string TipoPersona { get; set; } = string.Empty;
        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150")]
        public string NombrePersona { get; set; } = string.Empty;
        [StringLength(20)]
        public string TipoDocumento { get; set; } = string.Empty;
        [StringLength(20)]
        public string NumeroDocumento { get; set; } = string.Empty;
        [StringLength(150)]
        public string DireccionPersona { get; set; } = string.Empty;
        [StringLength(14)]
        public string TelefonoPersona { get; set; } = string.Empty;
        [StringLength(150)]
         public string EmailPersona { get; set; } = string.Empty;





    }
}
