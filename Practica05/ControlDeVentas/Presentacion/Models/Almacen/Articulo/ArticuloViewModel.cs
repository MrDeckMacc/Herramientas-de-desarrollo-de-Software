using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentacion.Models.Almacen.Articulo
{
    public class ArticuloViewModel
    {
        [Key]
        public int IdArticulo { get; set; }
        [ForeignKey("IdCategoria")]
        public int IdCategoria { get; set; }
        public string Categoria { get; set;} = string.Empty;
        public string CodigoArticulo { get; set;} = string.Empty;
        public string NombreArticulo { get; set;} = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public string DescripcionArticurlo { get; set; } = string.Empty;
        public bool Estado { get; set; }

    }
}
