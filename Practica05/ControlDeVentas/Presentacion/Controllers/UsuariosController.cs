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
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DBContextSistema _context;
        private readonly IConfiguration _configuracion;

        public UsuariosController(DBContextSistema context, IConfiguration configuracion)
        {
            _context = context;
            _configuracion = configuracion;
        }

        #region Metodo Verificar Password
        //Metodo para Verificar Password
        private bool VerificaPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var nuevoPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHash).SequenceEqual(new ReadOnlySpan<byte>(nuevoPasswordHash));
            }
        }
        #endregion

        #region Metodo Crear Token
        //Metodo para crear token
        private string GenerarToken(List<Claim> claims) 
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:Key"]));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuracion["Jnt:Issuer"],
                _configuracion["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credenciales,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Metodo Login
        // Metodo de Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var email = model.Email.ToUpper();
            var usuario = await _context.Usuario.Where(u => u.Estado == true).Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return NotFound();
            }
            var IsValido = VerificaPassword(model.Password, usuario.PasswordHash, usuario.PasswordSalt);
            if (!IsValido)
            {
                return BadRequest();
            }

            var claim = new List<Claim>
            {
                //Claim utilizadaos en el BackEnd
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, usuario.Rol.NombreRol),
                //Claim utilizandas en el FrontEnd
                new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                new Claim("Rol", usuario.Rol.NombreRol),
                new Claim("NombreUsuario", usuario.NombreUsuario)
            };

            return Ok(
                new { token = GenerarToken(claim) }
                );
        }
        #endregion

        #region Metodo Listar
        // Metodo listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuarioViewModel>> ListarUsuarios()
        {
            var usuarios = await _context.Usuario.Include(a => a.Rol).ToListAsync();
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
                Rol = u.Rol.NombreRol
            }) ;
        }
        #endregion

        #region Metodo Modificar
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
            usuario.Email = modelUsuario.Email.ToUpper();

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
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return Ok();
        }
        #endregion

        #region Metodo Crear Password
        // METODO CREAR PASSWORD
        public static void CreaPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        #endregion

        #region Metodo Insertar
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
                Email = modelUsuario.Email.ToUpper(),
                PasswordHash = passwordHash,
                Estado = true,
                PasswordSalt = passwordSalt,
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
        #endregion

        #region Metodo Desactivar
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
        #endregion

        #region Metodo Activar
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
        #endregion


        private bool UsuariosExists(int id)
        {
            return (_context.Usuario?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }
    }
}
