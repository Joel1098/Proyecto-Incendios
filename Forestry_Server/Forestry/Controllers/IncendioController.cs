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
    public class IncendioController : ControllerBase
    {
        private readonly ContextoBaseDeDatos _context;
        private readonly ILogger<IncendioController> _logger;

        public IncendioController(ContextoBaseDeDatos context, ILogger<IncendioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncendios()
        {
            try
            {
                var incendios = await _context.Incendio
                    .Include(i => i.Personal)
                    .Include(i => i.ReporteNavigation)
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
                    .Include(i => i.Personal)
                    .Include(i => i.ReporteNavigation)
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
        public async Task<IActionResult> CreateIncendio([FromBody] Incendio incendio)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Incendio.Add(incendio);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetIncendio), new { id = incendio.idIncendio }, incendio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando incendio");
                return StatusCode(500, new { message = "Error interno del servidor" });
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
                var personal = await _context.Personal
                    .Where(p => p.IdIncendio == id)
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
        public async Task<IActionResult> AddPersonalToIncendio(int id, [FromBody] Personal personal)
        {
            try
            {
                var incendio = await _context.Incendio.FindAsync(id);
                if (incendio == null)
                {
                    return NotFound(new { message = "Incendio no encontrado" });
                }

                personal.IdIncendio = id;
                personal.FechaCreada = DateTime.Now;

                _context.Personal.Add(personal);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPersonalIncendio), new { id }, personal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando personal al incendio {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}
