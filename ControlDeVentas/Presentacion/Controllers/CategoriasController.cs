using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades.Alamcen;
using Presentacion.Models.Almacen.Categoria;
using Presentacion.Models.Almacen;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly DBContextSistema _context;
        public CategoriasController(DBContextSistema context)
        {
            _context = context;
        }

        #region ListarCategorias
        // GET: api/Categorias/ListarCategorias
        [HttpGet("[action]")]
        public async Task<IEnumerable<CategoriaViewModel>> ListarCategorias()
        {
            var categoria = await _context.Categorias.ToListAsync();
            return categoria.Select(c => new CategoriaViewModel
            {
                IdCategoria = c.IdCategoria,
                NombreCategoria = c.NombreCategoria,
                Descripcion = c.Descripcion,
                Estado = c.Estado
            });
        }
        #endregion

        #region ObtenerCategorias
        // GET: api/Categorias/ObtenerCategoria/5
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> ObtenerCategoria([FromRoute] int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(new CategoriaViewModel
            {
                IdCategoria = categoria.IdCategoria,
                NombreCategoria = categoria.NombreCategoria,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado
            });
        }
        #endregion

        #region ModificarCategoria
        // PUT: api/Categorias/ModificarCategoria/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarCategoria([FromBody] ModificarViewModel modelCategoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (modelCategoria.IdCategoria < 0)
            {
                return BadRequest();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == modelCategoria.IdCategoria);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.NombreCategoria = modelCategoria.NombreCategoria;
            categoria.Descripcion = modelCategoria.Descripcion;

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
        #endregion

        #region InsertarCategoria
        // POST: api/Categorias/InsertarCategoria
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarCategoria(InsertarViewModel modelCategoria)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'DBContextSistema.Categorias'  is null.");
            }

            Categoria categoria = new Categoria
            {
                NombreCategoria = modelCategoria.NombreCategoria,
                Descripcion = modelCategoria.Descripcion,
                Estado = true
            };
            _context.Categorias.Add(categoria);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        #endregion

        #region BorrarCategoria
        // DELETE: api/Categorias/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }
        #endregion

        #region DesactivarCategoria
        // PUT: api/Categorias/DesactivarCategoria/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarCategoria([FromBody] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = false;

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
        #endregion

        #region ActivarCategoria
        // PUT: api/Categorias/ActivarCategoria/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> ActivarCategoria([FromBody] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = true;

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
        #endregion
    }
}
