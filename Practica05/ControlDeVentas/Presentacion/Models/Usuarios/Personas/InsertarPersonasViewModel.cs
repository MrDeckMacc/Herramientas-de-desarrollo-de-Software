using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models.Usuarios.Personas
{
    public class InsertarPersonasViewModel
    {
        [Required]
        public int IdPersona { get; set; }
        public string TipoPersona { get; set; } = string.Empty;
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre debe tener mas de tres caracteres, ni mas de 150")]
        public string NombrePersona { get; set; } = string.Empty;
        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150")]
        public string Tipodocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string DireccionPersona { get; set; } = string.Empty;
        public string TelefonoPersona { get; set; } = string.Empty;
        public string EmailPersona { get; set; } = string.Empty;
    }
}
