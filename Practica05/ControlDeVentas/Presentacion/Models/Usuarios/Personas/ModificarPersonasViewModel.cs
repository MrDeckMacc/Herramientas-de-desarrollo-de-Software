namespace Presentacion.Models.Usuarios.Personas
{
    public class ModificarPersonasViewModel
    {
        public int IdPersona { get; set; }
        public string TipoPersona { get; set; } = string.Empty;
        public string NombrePersona { get; set; } = string.Empty;
        public string Tipodocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string DireccionPersona { get; set; } = string.Empty;
        public string TelefonoPersona { get; set; } = string.Empty;
        public string EmailPersona { get; set; } = string.Empty;
    }
}

