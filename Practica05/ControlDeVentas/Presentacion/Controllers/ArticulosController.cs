using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades.Almacen;
using Presentacion.Models.Almacen.Articulo;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public ArticulosController(DBContextSistema context)
        {
            _context = context;
        }

        #region Listar Articulos
        // GET: api/Articulos/ListarArticulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarArticulos()
        {
            var articulo = await _context.Articulos.Include(a => a.IdCategoriaNavegation).ToListAsync();
            return articulo.Select(a => new ArticuloViewModel
            {
                IdArticulo = a.IdArticulo,
                IdCategoria = a.IdCategoria,
                CodigoArticulo = a.CodigoArticulo,
                NombreArticulo = a.NombreArticulo,
                PrecioVenta = a.PrecioVenta,
                Stock = a.Stock,
                DescripcionArticurlo = a.DescripcionArticulo,
                Estado = a.Estado,

                Categoria = a.IdCategoriaNavegation.NombreCategoria
            });
        }
        #endregion

        #region Obtener Articulo
        // GET: api/Articulos/ObtenerArticulo/5
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> ObtenerArticulo([FromRoute]int id)
        {
            var articulo = await _context.Articulos.Include(a => a.IdCategoriaNavegation).SingleOrDefaultAsync(a => a.IdArticulo == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                IdArticulo = articulo.IdArticulo,
                IdCategoria = articulo.IdCategoria,
                CodigoArticulo = articulo.CodigoArticulo,
                NombreArticulo = articulo.NombreArticulo,
                DescripcionArticurlo = articulo.DescripcionArticulo,
                Stock = articulo.Stock,
                PrecioVenta = articulo.PrecioVenta,

                Categoria = articulo.IdCategoriaNavegation.NombreCategoria
            });
        }
        #endregion


        // PUT: api/Articulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticulo(int id, Articulo articulo)
        {
            if (id != articulo.IdArticulo)
            {
                return BadRequest();
            }

            _context.Entry(articulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticuloExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Articulo>> PostArticulo(Articulo articulo)
        {
          if (_context.Articulos == null)
          {
              return Problem("Entity set 'DBContextSistema.Articulos'  is null.");
          }
            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticulo", new { id = articulo.IdArticulo }, articulo);
        }

        // DELETE: api/Articulos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticulo(int id)
        {
            if (_context.Articulos == null)
            {
                return NotFound();
            }
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }

            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // OUT: api/Articulos/ModificarArticulo
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarArticulos([FromBody] ModificarArticuloViewModel modelArticulo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (modelArticulo.IdArticulo < 0)
            {
                return BadRequest(ModelState);
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.IdArticulo == modelArticulo.IdArticulo);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.IdCategoria = modelArticulo.IdCategoria;
            articulo.CodigoArticulo = modelArticulo.CodigoArticulo;
            articulo.NombreArticulo = modelArticulo.NombreArticulo;
            articulo.PrecioVenta = modelArticulo.PrecioVenta;
            articulo.Stock = modelArticulo.Stock;
            articulo.DescripcionArticulo = modelArticulo.DescripcionArticulo;
            articulo.Estado = modelArticulo.Estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarArticulos(InsertarArticuloViewModel modelArticulo)
        {
            if (_context.Articulos == null)
            {
                return Problem("Entity set 'DBContextSistema.Roles'  is null.");
            }

            Articulo articulo = new Articulo
            {
                IdCategoria = modelArticulo.IdCategoria,
                CodigoArticulo = modelArticulo.CodigoArticulo,
                NombreArticulo = modelArticulo.NombreArticulo,
                PrecioVenta = modelArticulo.PrecioVenta,
                Stock = modelArticulo.Stock,
                DescripcionArticulo = modelArticulo.DescripcionArticulo,
                Estado = modelArticulo.Estado
            };
            _context.Articulos.Add(articulo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarArticulos([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.Estado = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> ActivarArticulos([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.Estado = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }


        private bool ArticuloExists(int id)
        {
            return (_context.Articulos?.Any(e => e.IdArticulo == id)).GetValueOrDefault();
        }
    }
}
