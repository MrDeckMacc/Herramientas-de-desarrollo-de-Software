

using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Entidades.Almacen
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        [Required]
        [StringLength(100,MinimumLength =3, ErrorMessage ="El Nombre no debe tener más de 100 caracteres")]
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
