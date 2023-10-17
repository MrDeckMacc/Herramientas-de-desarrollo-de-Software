using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades.Usuario;
using Presentacion.Models.Usuarios.Roles;
using Presentacion.Models.Usuarios.Usuarios;

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

        // METODO SELECCIONAR CATEGORIA  //**********************************************************************************
        [HttpGet("[action]")]
        public async Task<IEnumerable<SeleccionarUsuarioViewModel>> SeleccionarRol()
        {
            var rol = await _context.Rol.Where(a => a.Estado == true).ToArrayAsync();
            return rol.Select(c => new SeleccionarUsuarioViewModel
            {
                IdRol = c.IdRol,
                NombreRol = c.NombreRol,
            });
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entidades.Usuario.Roles>>> GetRoles()
        {
            if (_context.Rol == null)
            {
                return NotFound();
            }
            return await _context.Rol.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entidades.Usuario.Roles>> GetRoles(int id)
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

            return roles;
        }

        // METODO LISTAR //**********************************************************************************
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolesViewModel>> ListarRoles()
        {
            var rol = await _context.Rol.ToListAsync();
            return rol.Select(r => new RolesViewModel
            {
                IdRol = r.IdRol,
                NombreRol = r.NombreRol,
                DescripcionRol = r.DescripcionRol,
                Estado = r.Estado
            });
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoles(int id, Entidades.Usuario.Roles roles)
        {
            if (id != roles.IdRol)
            {
                return BadRequest();
            }

            _context.Entry(roles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolesExists(id))
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

        // MODIFICAR ROL////**********************************************************************************
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarRoles([FromBody] ModificarRolesViewModel modelrol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (modelrol.IdRol < 0)
            {
                return BadRequest(ModelState);
            }

            var rol = await _context.Rol.FirstOrDefaultAsync(r => r.IdRol == modelrol.IdRol);

            if (rol == null)
            {
                return NotFound();
            }

            rol.NombreRol = modelrol.NombreRol;
            rol.DescripcionRol = modelrol.DescripcionRol;

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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entidades.Usuario.Roles>> PostRoles(Entidades.Usuario.Roles roles)
        {
            if (_context.Rol == null)
            {
                return Problem("Entity set 'DBContextSistema.Roles'  is null.");
            }
            _context.Rol.Add(roles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoles", new { id = roles.IdRol }, roles);
        }

        // METODO INSERTAR ROL//**********************************************************************************
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarRoles(InsertarRolesViewModel modelRol)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'DBContextSistema.Roles'  is null.");
            }

            Roles rol = new Roles
            {
                NombreRol = modelRol.NombreRol,
                DescripcionRol = modelRol.DescripcionRol,
                Estado = true
            };
            _context.Rol.Add(rol);
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

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoles(int id)
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
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // BORRAR ROL//**********************************************************************************
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> BorrarRoles(int id)
        {
            if (_context.Rol == null)
            {
                return NotFound();
            }
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Rol.Remove(rol);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        //DESACTIVAR CATEGORIA//**********************************************************************************
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarRoles([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var rol = await _context.Rol.FirstOrDefaultAsync(r => r.IdRol == id);

            if (rol == null)
            {
                return NotFound();
            }

            rol.Estado = false;

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

        //ACTIVAR CATEGORIA//**********************************************************************************
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> ActivarRoles([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var rol = await _context.Rol.FirstOrDefaultAsync(r => r.IdRol == id);

            if (rol == null)
            {
                return NotFound();
            }

            rol.Estado = true;

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

        private bool RolesExists(int id)
        {
            return (_context.Rol?.Any(e => e.IdRol == id)).GetValueOrDefault();
        }
    }
}
