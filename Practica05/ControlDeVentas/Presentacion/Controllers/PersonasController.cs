using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades.Usuario;
using Presentacion.Models.Usuarios.Personas;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public PersonasController(DBContextSistema context)
        {
            _context = context;
        }

        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personas>>> GetPersona()
        {
          if (_context.Persona == null)
          {
              return NotFound();
          }
            return await _context.Persona.ToListAsync();
        }

        // GET: api/Personas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Personas>> GetPersonas(int id)
        {
          if (_context.Persona == null)
          {
              return NotFound();
          }
            var personas = await _context.Persona.FindAsync(id);

            if (personas == null)
            {
                return NotFound();
            }

            return personas;
        }

        // PUT: api/Personas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonas(int id, Personas personas)
        {
            if (id != personas.IdPersona)
            {
                return BadRequest();
            }

            _context.Entry(personas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonasExists(id))
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

        // POST: api/Personas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Personas>> PostPersonas(Personas personas)
        {
          if (_context.Persona == null)
          {
              return Problem("Entity set 'DBContextSistema.Persona'  is null.");
          }
            _context.Persona.Add(personas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonas", new { id = personas.IdPersona }, personas);
        }

        // DELETE: api/Personas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonas(int id)
        {
            if (_context.Persona == null)
            {
                return NotFound();
            }
            var personas = await _context.Persona.FindAsync(id);
            if (personas == null)
            {
                return NotFound();
            }

            _context.Persona.Remove(personas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #region InserterPersonas
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarPersona(PersonasViewModel modelPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            if (_context.Persona == null)
            {
                return Problem("Entity set 'Context.persona is null");
            }
            var email = modelPersona.EmailPersona.ToUpper();
            if (await _context.Persona.AnyAsync(p => p.EmailPersona == email))
            {
                return BadRequest("El Email de la persona ya existe");
            }
            Personas persona = new()
            {
                TipoPersona = modelPersona.TipoPersona,
                NombrePersona = modelPersona.NombrePersona,
                TipoDocumento = modelPersona.Tipodocumento,
                NumeroDocumento = modelPersona.NumeroDocumento,
                DireccionPersona = modelPersona.DireccionPersona,
                TelefonoPersona = modelPersona.TelefonoPersona,
                EmailPersona = modelPersona.EmailPersona.ToUpper(),
            };
            _context.Persona.Add(persona);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
                var inner = ex.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion

        #region ModificarPersonas
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarPersona(ModificarPersonasViewModel modelPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            if (modelPersona.IdPersona <= 0)
            {
                return Problem("Entity set 'Context.persona is null");
            }
            var persona = await _context.Persona.FirstOrDefaultAsync(p => p.IdPersona == modelPersona.IdPersona);
            var email = modelPersona.EmailPersona.ToUpper();
            if (await _context.Persona.AnyAsync(p => p.EmailPersona == email && p.IdPersona != modelPersona.IdPersona))
            {
                return BadRequest("El Email de la persona ya existe");
            }
            if (persona == null)
            {
                return NotFound();
            }
            persona.IdPersona = modelPersona.IdPersona;
            persona.TipoPersona = modelPersona.TipoPersona;
            persona.NombrePersona = modelPersona.NombrePersona;
            persona.TipoDocumento = modelPersona.Tipodocumento;
            persona.NumeroDocumento = modelPersona.NumeroDocumento;
            persona.DireccionPersona = modelPersona.DireccionPersona;
            persona.TelefonoPersona = modelPersona.TelefonoPersona;
            persona.EmailPersona = modelPersona.EmailPersona.ToUpper();
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
                var inner = ex.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion

        #region ListarPersonas
        //GET: api/Personas/ListarCllientes
        [HttpGet("[action]")]
        public async Task<IEnumerable<ModificarPersonasViewModel>> ListarCliente()
        {
            var persona = await _context.Persona.Where(p => p.TipoPersona == "Cliente").ToListAsync();
            return persona.Select(p => new ModificarPersonasViewModel
            {
                IdPersona = p.IdPersona,
                TipoPersona = p.TipoPersona,
                NombrePersona = p.NombrePersona,
                Tipodocumento = p.TipoDocumento,
                NumeroDocumento = p.NumeroDocumento,
                DireccionPersona = p.DireccionPersona,
                TelefonoPersona = p.TelefonoPersona,
                EmailPersona = p.EmailPersona
            });
        }
        #endregion

        #region ListarProvedores
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonasViewModel>> ListarProvedores()
        {
            var persona = await _context.Persona.Where(p => p.TipoPersona == "Provedor").ToListAsync();
            return persona.Select(p => new PersonasViewModel
            {
                IdPersona = p.IdPersona,
                TipoPersona = p.TipoPersona,
                NombrePersona = p.NombrePersona,
                Tipodocumento = p.TipoDocumento,
                NumeroDocumento = p.NumeroDocumento,
                DireccionPersona = p.DireccionPersona,
                TelefonoPersona = p.TelefonoPersona,
                EmailPersona = p.EmailPersona
            });

        }
        #endregion

        private bool PersonasExists(int id)
        {
            return (_context.Persona?.Any(e => e.IdPersona == id)).GetValueOrDefault();
        }
    }
}
