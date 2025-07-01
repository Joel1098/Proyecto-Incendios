using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Forestry.Models;
using Forestry.DTOs;
using Forestry.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Forestry.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncendioController : ControllerBase
    {
        private readonly ContextoBaseDeDatos _context;
        private readonly ILogger<IncendioController> _logger;
        private readonly IEmailService _emailService;

        public IncendioController(ContextoBaseDeDatos context, ILogger<IncendioController> logger, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncendios()
        {
            try
            {
                var incendios = await _context.Incendio
                    .Include(i => i.Etapa)
                    .Include(i => i.UsuarioResponsable)
                    .Include(i => i.Reporte)
                    .ToListAsync();

                return Ok(incendios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo incendios");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncendio(int id)
        {
            try
            {
                var incendio = await _context.Incendio
                    .Include(i => i.Etapa)
                    .Include(i => i.UsuarioResponsable)
                    .Include(i => i.Reporte)
                    .FirstOrDefaultAsync(i => i.idIncendio == id);

                if (incendio == null)
                {
                    return NotFound(new { message = "Incendio no encontrado" });
                }

                return Ok(incendio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo incendio {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<IncendioDTO>> CrearIncendio([FromBody] IncendioCreateSimpleDTO dto)
        {
            try
            {
                var incendio = new Incendio
                {
                    Ubicacion = dto.Ubicacion,
                    Descripcion = dto.Descripcion,
                    Estado = "Activo",
                    idEtapa = 1, // Etapa inicial por defecto
                    NombreDespacho = "Sin asignar",
                    NombreComando = "Sin asignar",
                    FechaIni = DateTime.UtcNow,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Incendio.Add(incendio);
                await _context.SaveChangesAsync();

                // Enviar notificación de nuevo incendio
                try
                {
                    await _emailService.SendIncendioNotificationAsync(incendio.idIncendio, "nuevo");
                    _logger.LogInformation($"Notificación de nuevo incendio enviada para ID: {incendio.idIncendio}");
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning($"No se pudo enviar notificación de incendio: {emailEx.Message}");
                    // No fallar la creación si el email falla
                }

                var response = new IncendioDTO
                {
                    IdIncendio = incendio.idIncendio,
                    Ubicacion = incendio.Ubicacion,
                    Descripcion = incendio.Descripcion,
                    Estado = incendio.Estado,
                    FechaIni = incendio.FechaIni
                };

                return CreatedAtAction(nameof(GetIncendio), new { id = incendio.idIncendio }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncendio(int id, [FromBody] Incendio incendio)
        {
            try
            {
                if (id != incendio.idIncendio)
                {
                    return BadRequest();
                }

                var existingIncendio = await _context.Incendio.FindAsync(id);
                if (existingIncendio == null)
                {
                    return NotFound(new { message = "Incendio no encontrado" });
                }

                existingIncendio.FechaIni = incendio.FechaIni;
                existingIncendio.FechaFin = incendio.FechaFin;
                existingIncendio.Etapa = incendio.Etapa;
                existingIncendio.NombreDespacho = incendio.NombreDespacho;
                existingIncendio.NombreComando = incendio.NombreComando;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando incendio {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncendio(int id)
        {
            try
            {
                var incendio = await _context.Incendio.FindAsync(id);
                if (incendio == null)
                {
                    return NotFound(new { message = "Incendio no encontrado" });
                }

                _context.Incendio.Remove(incendio);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando incendio {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("{id}/personal")]
        public async Task<IActionResult> GetPersonalIncendio(int id)
        {
            try
            {
                var personal = await _context.IncendioPersonal
                    .Where(ip => ip.idIncendio == id)
                    .Include(ip => ip.Trabajador)
                    .Select(ip => ip.Trabajador)
                    .ToListAsync();
                return Ok(personal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo personal del incendio {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("{id}/personal")]
        public async Task<IActionResult> AddPersonalToIncendio(int id, [FromBody] int idTrabajador)
        {
            try
            {
                var incendio = await _context.Incendio.FindAsync(id);
                var trabajador = await _context.Personal.FindAsync(idTrabajador);
                if (incendio == null || trabajador == null)
                {
                    return NotFound(new { message = "Incendio o trabajador no encontrado" });
                }
                var relacion = new IncendioPersonal
                {
                    idIncendio = id,
                    IdTrabajador = idTrabajador,
                    FechaAsignacion = DateTime.UtcNow,
                    Estado = "Activo"
                };
                _context.IncendioPersonal.Add(relacion);
                await _context.SaveChangesAsync();
                return Ok(relacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando personal al incendio {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
