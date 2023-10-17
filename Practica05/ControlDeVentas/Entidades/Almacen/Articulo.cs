using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Almacen
{
    public class Articulo
    {
        [Key]
        public int IdArticulo { get; set; }
        [Required]
        [ForeignKey("IdCategoria")]
        public int IdCategoria { get; set; }
        [Required]
        public Categoria IdCategoriaNavigation { get; set; }
        public string CodigoArticulo { get; set; }
        [Required]
        [StringLength (150, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener más de 150 caracteres, ni menos de caracteres.")]
        public string NombreArticulo { get; set; }
        [Required]
        public decimal PrecioVenta { get; set; }
        [Required]
        public int Stock { get; set; }
        public string DescripcionArticulo { get; set; }
        public bool Estado { get; set; }

        public Categoria IdCategoriaNavegation { get; set; }

    }
}
