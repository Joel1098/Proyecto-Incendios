using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Forestry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Net.WebRequestMethods;
using System.Drawing;
using System.Security.Policy;
using System.Globalization;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Forestry.DTOs;

namespace Forestry.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : Controller
    {
        private readonly ContextoBaseDeDatos _context;
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(ContextoBaseDeDatos context, ILogger<ReportesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetReportes()
        {
            try
            {
                var reportes = await _context.Reporte
                    .Include(r => r.Usuario)
                    .Include(r => r.Incendio)
                    .OrderByDescending(r => r.FechaCreacion)
                    .ToListAsync();

                return Ok(reportes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reportes");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReporte(int id)
        {
            try
            {
                var reporte = await _context.Reporte
                    .Include(r => r.Usuario)
                    .Include(r => r.Incendio)
                    .FirstOrDefaultAsync(r => r.idReporte == id);

                if (reporte == null)
                {
                    return NotFound(new { message = "Reporte no encontrado" });
                }

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReporte([FromBody] CrearReporteDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var reporte = new Reporte
                {
                    Tipo = dto.Tipo,
                    Contenido = dto.Contenido,
                    Lugar = dto.Lugar,
                    Situacion = dto.Situacion,
                    Detalles = dto.Detalles,
                    idIncendio = dto.idIncendio,
                    idUsuario = dto.idUsuario,
                    Fecha = DateTime.UtcNow,
                    FechaCreacion = DateTime.UtcNow,
                    Estado = "Activo"
                };

                _context.Reporte.Add(reporte);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetReporte), new { id = reporte.idReporte }, reporte);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando reporte");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReporte(int id, [FromBody] CrearReporteDTO dto)
        {
            try
            {
                var existingReporte = await _context.Reporte.FindAsync(id);
                if (existingReporte == null)
                {
                    return NotFound(new { message = "Reporte no encontrado" });
                }

                existingReporte.Tipo = dto.Tipo;
                existingReporte.Contenido = dto.Contenido;
                existingReporte.Lugar = dto.Lugar;
                existingReporte.Situacion = dto.Situacion;
                existingReporte.Detalles = dto.Detalles;
                existingReporte.idIncendio = dto.idIncendio;
                existingReporte.idUsuario = dto.idUsuario;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando reporte {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReporte(int id)
        {
            try
            {
                var reporte = await _context.Reporte.FindAsync(id);
                if (reporte == null)
                {
                    return NotFound(new { message = "Reporte no encontrado" });
                }

                _context.Reporte.Remove(reporte);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando reporte {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        public class CambiarEstadoReporteDTO
        {
            public string Estado { get; set; }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoReporte(int id, [FromBody] CambiarEstadoReporteDTO dto)
        {
            try
            {
                var reporte = await _context.Reporte.FindAsync(id);
                if (reporte == null)
                {
                    return NotFound(new { message = "Reporte no encontrado" });
                }

                reporte.Estado = dto.Estado;
                await _context.SaveChangesAsync();

                return Ok(new { message = $"Reporte actualizado a estado '{dto.Estado}' exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error cambiando estado del reporte {id}");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /*---------------CREAR INSTANCIA DE INCENDIO-------------------*/
        [HttpGet("crear-nueva-instancia")]
        public IActionResult CrearNuevaInstancia(int idReporte)
        {
            return Ok(new { message = "Endpoint para crear nueva instancia disponible", idReporte });
        }

        [HttpPost("crear-nueva-instancia")]
        public IActionResult CrearNuevaInstancia([FromBody] CrearInstanciaIncendioDTO dto)
        {
            // Obtén la fecha y hora actuales
            DateTime fechaActual = DateTime.UtcNow;
            DateTime fecha = fechaActual.Date;
            DateTime hora = DateTime.Today.Add(fechaActual.TimeOfDay);
            TimeSpan hora1 = fechaActual.TimeOfDay;

            var reporte = _context.Reporte.FirstOrDefault(u => u.idReporte == dto.IdReporte);
            if (reporte == null)
            {
                return NotFound(new { message = "Reporte no encontrado" });
            }

            // Inicializar la colección de reportes si es null
            if (dto.Incendio.Reporte == null)
            {
                dto.Incendio.Reporte = new HashSet<Reporte>();
            }
            dto.Incendio.Reporte.Add(reporte);

            var etapa1 = _context.Etapas.FirstOrDefault(e => e.idEtapa == 1);
            dto.Incendio.Etapa = etapa1;
            dto.Incendio.FechaIni = hora;
            dto.Incendio.NombreComando = "Sin asignar";
            dto.Incendio.NombreDespacho = "Sin asignar";

            dto.BitacoraMedidaInicial.FechaCreada = hora;
            dto.BitacoraMedidaInicial.CaracterFuego = dto.BitacoraMedidaInicial.CaracterFuego;

            reporte.Estado = "Confirmado";

            _context.Add(dto.Incendio);
            _context.SaveChanges();

            var incendioCreado = _context.Incendio.FirstOrDefault(u => u.Reporte.Contains(reporte));
            dto.BitacoraMedidaInicial.Incendio = incendioCreado;
            _context.Add(dto.BitacoraMedidaInicial);
            _context.SaveChanges();

            // Guardar los datos en la tabla Actualizacion
            if (decimal.TryParse(dto.Latitud, out decimal latitudDecimal) &&
                decimal.TryParse(dto.Longitud, out decimal longitudDecimal))
            {
                dto.Actualizacion.Latitud = Math.Round(latitudDecimal, 6);
                dto.Actualizacion.Longitud = Math.Round(longitudDecimal, 6);
                dto.Actualizacion.Radio = dto.Radio;
                dto.Actualizacion.FechaAccion = DateTime.UtcNow;
                dto.Actualizacion.Accion = "Crear";
                dto.Actualizacion.Tipo = "Inicial";
                dto.Actualizacion.Incendio = incendioCreado;

                _context.Add(dto.Actualizacion);
                _context.SaveChanges();
            }
            else
            {
                return BadRequest(new { message = "Latitud, Longitud o Radio no tienen un formato válido." });
            }

            return Ok(new { message = "Instancia de incendio creada exitosamente", incendio = incendioCreado });
        }

        [HttpPost("asignar-personal")]
        public IActionResult AsignarPersonal(int idDespacho, int idComando, int idIncendio)
        {
            var despacho = _context.Usuarios.FirstOrDefault(u => u.idUsuario == idDespacho);
            var comando = _context.Usuarios.FirstOrDefault(u => u.idUsuario == idComando);
            var incendio = _context.Incendio.FirstOrDefault(u => u.idIncendio == idIncendio);

            if (despacho == null || comando == null || incendio == null)
            {
                return NotFound(new { message = "Usuario o incendio no encontrado" });
            }

            despacho.Estado = "ocupado";
            comando.Estado = "ocupado";

            string DespachoNombre = despacho.Nombre + " " + despacho.ApPaterno + " " + despacho.ApMaterno;
            string ComandoNombre = comando.Nombre + " " + comando.ApPaterno + " " + comando.ApMaterno;

            // Asignar el incendio al usuario responsable
            incendio.UsuarioResponsable = comando;
            var etapa2 = _context.Etapas.FirstOrDefault(e => e.idEtapa == 2);
            incendio.Etapa = etapa2;
            incendio.NombreDespacho = DespachoNombre;
            incendio.NombreComando = ComandoNombre;

            _context.SaveChanges();

            return Ok(new { message = "Personal asignado exitosamente" });
        }

        [HttpGet("incendios")]
        public IActionResult Incendios()
        {
            var enEspera = _context.Incendio
                .Where(c => c.idEtapa == 1)
                .OrderByDescending(r => r.FechaIni)
                .Take(10)
                .Select(i => new { 
                    i.idIncendio, 
                    i.Ubicacion, 
                    i.FechaIni, 
                    i.Estado 
                })
                .ToList();

            var enAtencion = _context.Incendio
                .Where(c => c.idEtapa > 1 && c.idEtapa < 11)
                .OrderByDescending(r => r.FechaIni)
                .Take(10)
                .Select(i => new { 
                    i.idIncendio, 
                    i.Ubicacion, 
                    i.FechaIni, 
                    i.Estado,
                    i.NombreComando 
                })
                .ToList();

            return Ok(new { 
                incendiosEspera = enEspera,
                incendiosAtencion = enAtencion
            });
        }

        [HttpGet("ver-incendio/{idIncendio}")]
        public IActionResult VerIncendio(int idIncendio)
        {
            var incendio = _context.Incendio
                .Include(i => i.Etapa)
                .Include(i => i.UsuarioResponsable)
                .FirstOrDefault(u => u.idIncendio == idIncendio);
                
            if (incendio == null)
                return NotFound(new { message = "Incendio no encontrado" });

            var trabajadores = _context.IncendioPersonal
                .Where(ip => ip.idIncendio == idIncendio)
                .Include(ip => ip.Trabajador)
                .Select(ip => new { 
                    ip.Trabajador.IdTrabajador,
                    ip.Trabajador.Nombre,
                    ip.Trabajador.ApPaterno,
                    ip.Trabajador.ApMaterno,
                    ip.RolEnIncendio,
                    ip.FechaAsignacion
                })
                .ToList();

            return Ok(new {
                incendio = new {
                    incendio.idIncendio,
                    incendio.Ubicacion,
                    incendio.Descripcion,
                    incendio.Estado,
                    incendio.FechaIni,
                    incendio.FechaFin,
                    incendio.NombreDespacho,
                    incendio.NombreComando,
                    etapa = incendio.Etapa?.Nombre,
                    responsable = incendio.UsuarioResponsable != null ? 
                        $"{incendio.UsuarioResponsable.Nombre} {incendio.UsuarioResponsable.ApPaterno}" : null
                },
                trabajadores = trabajadores
            });
        }

        [HttpPost("ver-mapa")]
        public IActionResult VerMapa(int idIncendio)
        {         
            var incendio = _context.Incendio.FirstOrDefault(u => u.idIncendio == idIncendio);
            if (incendio == null)
                return NotFound(new { message = "Incendio no encontrado" });

            // Comentar temporalmente la línea que causa error hasta que se agregue el DbSet
            // var actualizacion = _context.Actualizacion
            //     .Where(u => u.Incendio == incendio && (u.Tipo == "Mapa" || u.Tipo == "Inicial")).ToList();

            return Ok(new { message = "Funcionalidad de mapa temporalmente deshabilitada" });
        }

        [HttpGet("bitacora-chequeo-planeacion")]
        public IActionResult BitacoraChequeoPlaneacion()
        {
            return NoContent();
        }

        [HttpGet("bitacora-status-situacion")]
        public IActionResult BitacoraStatusSituacion()
        {
            return NoContent();
        }
    }
}
