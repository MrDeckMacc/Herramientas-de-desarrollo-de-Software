using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades.Usuario;
using Presentacion.Models.Usuarios.Usuarios;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public UsuariosController(DBContextSistema context)
        {
            _context = context;
        }
        // Metodo listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuarioViewModel>> ListarUsuarios()
        {
            var usuarios = await _context.Usuario.Include(a => a.IdRolNavigation).ToListAsync();
            return usuarios.Select(u => new UsuarioViewModel
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdRol,
                NombreUsuario = u.NombreUsuario,
                TipoDocumento = u.TipoDocumento,
                NumeroDocumento = u.NumeroDocumento,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Estado = u.Estado,

                Rol = u.IdRolNavigation.NombreRol
            }) ;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuario()
        {
          if (_context.Usuario == null)
          {
              return NotFound();
          }
            return await _context.Usuario.ToListAsync();
        }

        //Metodo Modificar 
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarUsuarios([FromBody] ModificarUsuarioViewModel modelUsuario)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'DBContextSistema.Usuarios'  is null.");
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == modelUsuario.IdUsuario);



            var email = modelUsuario.Email.ToUpper();
            if (await _context.Usuario.AnyAsync(u => u.Email == email && u.IdUsuario != modelUsuario.IdUsuario))
            {
                return BadRequest("El Email de este usuario ya existe");
            }
            if (usuario == null)
            {
                return NotFound();
            }


            usuario.IdRol = modelUsuario.IdRol;
            usuario.NombreUsuario = modelUsuario.NombreUsuario;
            usuario.TipoDocumento = modelUsuario.TipoDocumento;
            usuario.NumeroDocumento = modelUsuario.NumeroDocumento;
            usuario.Direccion = modelUsuario.Direccion;
            usuario.Telefono = modelUsuario.Telefono;
            usuario.Email = modelUsuario.Email;

            if (modelUsuario.ActualizarPassword == true)
            {
                CreaPassword(modelUsuario.Password, out byte[] passwordHash, out byte[] passwordSalt);
                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string Error = e.Message;
                var inner = e.InnerException;
                return BadRequest();
            }
            return Ok();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuarios(int id)
        {
          if (_context.Usuario == null)
          {
              return NotFound();
          }
            var usuarios = await _context.Usuario.FindAsync(id);

            if (usuarios == null)
            {
                return NotFound();
            }

            return usuarios;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarios(int id, Usuarios usuarios)
        {
            if (id != usuarios.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
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

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuarios(Usuarios usuarios)
        {
          if (_context.Usuario == null)
          {
              return Problem("Entity set 'DBContextSistema.Usuario'  is null.");
          }
            _context.Usuario.Add(usuarios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuarios.IdUsuario }, usuarios);
        }
        // METODO CREAR PASSWORD
        public static void CreaPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
         //Metodo Insertar
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarUsuarios(InsertarUsuarioViewModel modelUsuario)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'DBContextSistema.Usuarios'  is null.");
            }

            CreaPassword(modelUsuario.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var email = modelUsuario.Email.ToUpper();
            if (await _context.Usuario.AnyAsync(u => u.Email == email))
            {
                return BadRequest("El Email de este usuario ya existe"); //Función para validar que no se repita un Email
            }
            Usuarios usuario = new Usuarios
            {
                IdRol = modelUsuario.IdRol,
                NombreUsuario = modelUsuario.NombreUsuario,
                TipoDocumento = modelUsuario.TipoDocumento,
                NumeroDocumento = modelUsuario.NumeroDocumento,
                Direccion = modelUsuario.Direccion,
                Telefono = modelUsuario.Telefono,
                Email = modelUsuario.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Estado = modelUsuario.Estado
            };
            _context.Usuario.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string Error = e.Message;
                var inner = e.InnerException;
                return BadRequest();
            }
            return Ok();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuarios(int id)
        {
            if (_context.Usuario == null)
            {
                return NotFound();
            }
            var usuarios = await _context.Usuario.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuarios);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        //Metodo Desactivar Categoria
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarUsuarios([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = false;

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

        // Metodo Activar Categoria
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> ActivarUsuarios([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = true;

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



        private bool UsuariosExists(int id)
        {
            return (_context.Usuario?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }
    }
}
