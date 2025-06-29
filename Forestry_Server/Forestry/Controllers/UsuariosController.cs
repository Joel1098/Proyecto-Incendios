using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Forestry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forestry.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ContextoBaseDeDatos _context;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ContextoBaseDeDatos context, ILogger<UsuariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var usuarios = await _context._Usuarios
                    .Include(u => u.Incendio)
                    .Include(u => u.Reporte)
                    .ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuarios");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            try
            {
                var usuario = await _context._Usuarios
                    .Include(u => u.Incendio)
                    .Include(u => u.Reporte)
                    .FirstOrDefaultAsync(u => u.idUsuario == id);

                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo usuario {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuarios usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si el usuario ya existe
                var existingUsuario = await _context._Usuarios
                    .FirstOrDefaultAsync(u => u.Usuario == usuario.Usuario);

                if (existingUsuario != null)
                {
                    return BadRequest(new { message = "El nombre de usuario ya existe" });
                }

                // Encriptar contraseña (implementar método de encriptación)
                // usuario.Contrasena = Encriptar.EncriptarPassword(usuario.Contrasena);

                _context._Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.idUsuario }, usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando usuario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuarios usuario)
        {
            try
            {
                if (id != usuario.idUsuario)
                {
                    return BadRequest();
                }

                var existingUsuario = await _context._Usuarios.FindAsync(id);
                if (existingUsuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                existingUsuario.Nombre = usuario.Nombre;
                existingUsuario.ApPaterno = usuario.ApPaterno;
                existingUsuario.ApMaterno = usuario.ApMaterno;
                existingUsuario.Rol = usuario.Rol;
                existingUsuario.Estado = usuario.Estado;
                existingUsuario.NumeTel = usuario.NumeTel;
                existingUsuario.DiasLaborales = usuario.DiasLaborales;
                existingUsuario.TrabajoInicio = usuario.TrabajoInicio;
                existingUsuario.TrabajoFin = usuario.TrabajoFin;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando usuario {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _context._Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                _context._Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando usuario {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetPersonal()
        {
            try
            {
                var personal = await _context.Personal
                    .Include(p => p.Incendio)
                    .ToListAsync();

                return Ok(personal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo personal");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("personal/{id}")]
        public async Task<IActionResult> GetPersonalById(int id)
        {
            try
            {
                var personal = await _context.Personal
                    .Include(p => p.Incendio)
                    .FirstOrDefaultAsync(p => p.IdTrabajador == id);

                if (personal == null)
                {
                    return NotFound(new { message = "Personal no encontrado" });
                }

                return Ok(personal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo personal {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("personal")]
        public async Task<IActionResult> CreatePersonal([FromBody] Personal personal)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                personal.FechaCreada = DateTime.Now;

                _context.Personal.Add(personal);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPersonalById), new { id = personal.IdTrabajador }, personal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando personal");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPut("personal/{id}")]
        public async Task<IActionResult> UpdatePersonal(int id, [FromBody] Personal personal)
        {
            try
            {
                if (id != personal.IdTrabajador)
                {
                    return BadRequest();
                }

                var existingPersonal = await _context.Personal.FindAsync(id);
                if (existingPersonal == null)
                {
                    return NotFound(new { message = "Personal no encontrado" });
                }

                existingPersonal.Nombre = personal.Nombre;
                existingPersonal.ApPaterno = personal.ApPaterno;
                existingPersonal.ApMaterno = personal.ApMaterno;
                existingPersonal.Turno = personal.Turno;
                existingPersonal.IdIncendio = personal.IdIncendio;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando personal {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpDelete("personal/{id}")]
        public async Task<IActionResult> DeletePersonal(int id)
        {
            try
            {
                var personal = await _context.Personal.FindAsync(id);
                if (personal == null)
                {
                    return NotFound(new { message = "Personal no encontrado" });
                }

                _context.Personal.Remove(personal);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando personal {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
