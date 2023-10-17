using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades.Usuarios;
using Presentacion.Models.Usuarios;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public RolesController(DBContextSistema context)
        {
            _context = context;
        }

        // GET: api/Roles/ListarRoles
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolesViewModel>> ListarRoles()
        {
            var rol = await _context.Rol.ToListAsync();
            return rol.Select(c => new RolesViewModel
            {
                IdRol = c.IdRol,
                NombreRol = c.NombreRol,
                DescripcionRol = c.DescripcionRol,
                Estado = c.Estado
            });
        }

        // GET: api/Roles/ObtenerRol/5
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> ObtenerRol([FromRoute] int id)
        {
            var rol = await _context.Rol.FindAsync(id);
          if (rol == null)
          {
              return NotFound();
          }
          return Ok(new RolesViewModel
            {
                IdRol = rol.IdRol,
                NombreRol = rol.NombreRol,
                DescripcionRol = rol.DescripcionRol,
                Estado = rol.Estado
            });
        }

        // PUT: api/Roles/ModificarRol/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarRol([FromBody] ModificarViewModel modelRol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (modelRol.IdRol < 0) 
            {
                return BadRequest();
            }

            var rol = await _context.Rol.FirstOrDefaultAsync(c => c.IdRol == modelRol.IdRol);

            if(rol == null)
            {
                return NotFound();
            }

            rol.NombreRol = modelRol.NombreRol;
            rol.DescripcionRol = modelRol.DescripcionRol;

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

        // POST: api/Roles/InsertarRol
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarRol(InsertarViewModel modelRol)
        {
          if (_context.Rol == null)
          {
              return Problem("Entity set 'DBContextSistema.Rol'  is null.");
          }

            Roles roles = new Roles
            {
                NombreRol= modelRol.NombreRol,
                DescripcionRol = modelRol.DescripcionRol,
                Estado = true
            };
            
            _context.Rol.Add(roles);
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

        // DELETE: api/Roles/BorrarRol/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> BorrarRol(int id)
        {
            if (_context.Rol == null)
            {
                return NotFound();
            }
            var roles = await _context.Rol.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }

            _context.Rol.Remove(roles);
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

        // PUT: api/Roles/DesactivarRol/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarRol([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var roles = await _context.Rol.FirstOrDefaultAsync(c => c.IdRol == id);

            if (roles == null)
            {
                return NotFound();
            }

            roles.Estado = false;

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

        // PUT: api/Roles/ActivarRol/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> ActivarRol([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var roles = await _context.Rol.FirstOrDefaultAsync(c => c.IdRol == id);

            if (roles == null)
            {
                return NotFound();
            }

            roles.Estado = true;

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

        private bool RolesExists(int id)
        {
            return (_context.Rol?.Any(e => e.IdRol == id)).GetValueOrDefault();
        }
    }
}
