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
                    .OrderByDescending(r => r.Fecha)
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
        public async Task<IActionResult> CreateReporte([FromBody] Reporte reporte)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                reporte.Fecha = DateTime.UtcNow;
                reporte.Estado = "Reportado";

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
        public async Task<IActionResult> UpdateReporte(int id, [FromBody] Reporte reporte)
        {
            try
            {
                if (id != reporte.idReporte)
                {
                    return BadRequest();
                }

                var existingReporte = await _context.Reporte.FindAsync(id);
                if (existingReporte == null)
                {
                    return NotFound(new { message = "Reporte no encontrado" });
                }

                existingReporte.Lugar = reporte.Lugar;
                existingReporte.Situacion = reporte.Situacion;
                existingReporte.Detalles = reporte.Detalles;
                existingReporte.Estado = reporte.Estado;

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

        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarReporte(int id)
        {
            try
            {
                var reporte = await _context.Reporte.FindAsync(id);
                if (reporte == null)
                {
                    return NotFound(new { message = "Reporte no encontrado" });
                }

                reporte.Estado = "Confirmado";
                await _context.SaveChangesAsync();

                return Ok(new { message = "Reporte confirmado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirmando reporte {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("{id}/rechazar")]
        public async Task<IActionResult> RechazarReporte(int id)
        {
            try
            {
                var reporte = await _context.Reporte.FindAsync(id);
                if (reporte == null)
                {
                    return NotFound(new { message = "Reporte no encontrado" });
                }

                reporte.Estado = "Rechazado";
                await _context.SaveChangesAsync();

                return Ok(new { message = "Reporte rechazado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando reporte {Id}", id);
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /*---------CREAR REPORTES---------------------------------------------*/
        [HttpGet("crear-reporte")]
        public IActionResult CrearReporte()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            return Ok(new { message = "Endpoint para crear reporte disponible" });
        }

        [HttpPost("crear")]
        public IActionResult CrearReporte([FromBody] CrearReporteDTO dto)
        {
            int idUsuario = dto.IdUsuario;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);

            if (usuario == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            }

            var nuevoReporte = new Reporte
            {
                Lugar = dto.Lugar,
                Situacion = dto.Situacion,
                Detalles = dto.Detalles,
                Fecha = DateTime.UtcNow,
                Estado = "Reportado",
                Usuario = usuario
            };

            _context.Reporte.Add(nuevoReporte);
            _context.SaveChanges();

            return Ok(new { message = "Reporte creado exitosamente", reporte = nuevoReporte });
        }

        [HttpPost("reportes")]
        public IActionResult Reportes([FromBody] Reporte reportes, [FromQuery] int? idUsuario = null)
        {
            var reporte = _context.Reporte.FirstOrDefault(r => r.idReporte == reportes.idReporte);
            if (reporte == null)
            {
                return NotFound(new { message = "Reporte no encontrado" });
            }

            // Actualizar el reporte
            reporte.Lugar = reportes.Lugar;
            reporte.Situacion = reportes.Situacion;
            reporte.Detalles = reportes.Detalles;
            reporte.Estado = reportes.Estado;

            _context.SaveChanges();

            return Ok(new { message = "Reporte actualizado exitosamente", reporte });
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
        public IActionResult Incendios(Incendio incendios)
        {
            var enEspera = _context.Incendio.Where(c => c.idEtapa == 1)
                .OrderByDescending(r => r.FechaIni)
                .Take(10)
                .ToList();

            var enAtencion = _context.Incendio.Where(c => c.idEtapa > 1 && c.idEtapa < 11)
                .OrderByDescending(r => r.FechaIni)
                .Take(10)
                .ToList();

            DayOfWeek diaActual = DateTime.Today.DayOfWeek;
            TimeSpan horaActual = DateTime.UtcNow.TimeOfDay;

            var despacho = _context.Usuarios
                .Where(u => u.Rol == "Despacho" && u.Estado == "disponible"
                            && u.DiasLaborales.Contains(diaActual.ToString())
                            && u.TrabajoInicio <= horaActual
                            && horaActual <= u.TrabajoFin)
                .ToList();

            var comando = _context.Usuarios
                .Where(u => u.Rol == "Comandos" && u.Estado == "disponible"
                            && u.DiasLaborales.Contains(diaActual.ToString())
                            && u.TrabajoInicio <= horaActual
                            && horaActual <= u.TrabajoFin)
                .ToList();

            return Ok(new { 
                message = "Endpoint para incendios disponible",
                incendiosEspera = enEspera,
                incendiosAtencion = enAtencion,
                despacho = despacho,
                comando = comando
            });
        }

        [HttpGet("historico-incendios")]
        public IActionResult HistoricoIncendios()
        {
            var incendios = _context.Incendio.Where(u => u.idEtapa > 10)
                .OrderByDescending(r => r.FechaFin)
                .ToList();
            return Ok(incendios);
        }

        [HttpPost("ver-incendio")]
        public IActionResult VerIncendio(int idIncendio)
        {
            var incendio = _context.Incendio.FirstOrDefault(u => u.idIncendio == idIncendio);
            if (incendio == null)
                return NotFound(new { message = "Incendio no encontrado" });

            var comando = _context.Usuarios.FirstOrDefault(u => u.idUsuario == incendio.idUsuarioResponsable && u.Rol == "Comandos");
            var comando1 = incendio.NombreComando;

            // Obtener personal relacionado con el incendio
            var trabajadores = _context.IncendioPersonal
                .Where(ip => ip.idIncendio == idIncendio)
                .Include(ip => ip.Trabajador)
                .Select(ip => ip.Trabajador)
                .ToList();

            // Filtrar trabajadores por el nombre del comando
            var trabajadoresFiltrados = trabajadores
                .Where(t => $"{t.Nombre} {t.ApPaterno} {t.ApMaterno}" == comando1)
                .ToList();

            return Ok(new {
                incendio,
                comando,
                trabajadores = trabajadoresFiltrados
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
