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
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Forestry.Models;
//using Forestry.Models.ViewModels;
//using Forestry.Repositories;
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
using Microsoft.VisualStudio.Web.CodeGeneration;
using PagedList;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq;
using Forestry.ViewModels;

namespace Forestry.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
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

                reporte.Fecha = DateTime.Now;
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
        [HttpGet]
        public IActionResult CrearReporte()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            ViewBag.idUsuario = idUsuario;

            return View("CrearReporte");
        }

        [HttpPost]
        public IActionResult CrearReporte(Reporte reportes, Usuarios _Usuario, string Lugar, string Situacion, string Detalles)
        {
            // Obtén la fecha y hora actuales
            DateTime fechaActual = DateTime.Now;

            // Guarda la fecha en una variable
            DateTime fecha = fechaActual.Date;

            DateTime hora = DateTime.Today.Add(fechaActual.TimeOfDay);


            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            var usuario = _context._Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);

            // var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);


            reportes.idUsuario = usuario;
            reportes.Fecha = hora;
            reportes.Estado = "Reportado";
            reportes.Lugar = Lugar;
            reportes.Situacion = Situacion;
            reportes.Detalles = Detalles;

            _context.Add(reportes);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cuenta creada con éxito.";


            var user = HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var roles = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                ViewBag.Rol = roles;
                // Ahora tienes una lista de roles del usuario autenticado
                // Puedes hacer lo que necesites con esta lista
            }

            // Obtener los últimos 10 reportes ordenados por fecha de creación descendente
            var model = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Reportado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            var model1 = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Confirmado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            var model2 = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Rechazado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            ViewBag.ReportesConfirmados = model1;
            ViewBag.ReportesRechazados = model2;

            var cantidadUsuarios = _context._Usuarios.Count();

            ViewBag.CantidadUsuarios = cantidadUsuarios;
            ViewBag.PagInicio = 1;
            ViewBag.PagFin = 10;

            return View("Reportes", model);
        }


        /*----------------VISUALIZAR REPORTES------------------------*/

        [HttpGet]
        public IActionResult Reportes(Reporte reportes)
        {
            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var user = HttpContext.User;

            var UsuarioLogeado = _context._Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);

            if (user.Identity.IsAuthenticated)
            {
                var roles = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                ViewBag.Rol = roles;
                // Ahora tienes una lista de roles del usuario autenticado
                // Puedes hacer lo que necesites con esta lista
            }



            // Consulta para obtener los primeros 10 registros
            //  var model = _context._Usuarios.Where(u => u.idUsuario >= 1 && u.idUsuario <= 10).ToList();

            // Obtener los últimos 10 reportes ordenados por fecha de creación descendente
            var model = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Reportado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            var model1 = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Confirmado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            var model2 = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Rechazado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            if (UsuarioLogeado.Rol == "Jefe")
            {
                model = _context.Reporte.Where(c => c.Estado == "Reportado")
                               .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                               .Take(10)
                               .ToList();

                model1 = _context.Reporte.Where(c => c.Estado == "Confirmado")
                    .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                    .Take(10)
                    .ToList();

                model2 = _context.Reporte.Where(c => c.Estado == "Rechazado")
                    .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                    .Take(10)
                    .ToList();
            }

            ViewBag.ReportesConfirmados = model1;
            ViewBag.ReportesRechazados = model2;

            var cantidadUsuarios = _context._Usuarios.Count();

            ViewBag.CantidadUsuarios = cantidadUsuarios;
            ViewBag.PagInicio = 1;
            ViewBag.PagFin = 10;

            return View("Reportes", model);
        }

        [HttpPost]
        public IActionResult Reportes(Reporte reportes, int idReporte, Usuarios usuarios)
        {
            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var user = HttpContext.User;

            var UsuarioLogeado = _context._Usuarios.FirstOrDefault(u => u.idUsuario == idUsuario);

            if (user.Identity.IsAuthenticated)
            {
                var roles = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);
                ViewBag.Rol = roles;
                // Ahora tienes una lista de roles del usuario autenticado
                // Puedes hacer lo que necesites con esta lista
            }

            Console.WriteLine("El valor de la variable es: " + idReporte);
            Debug.WriteLine("El valor de la variable es: " + idReporte);

            //    var reporte = _context.Reporte.Where(u => u.idReporte == idReporte);

            var reporte = _context.Reporte.FirstOrDefault(r => r.idReporte == idReporte);

            Console.WriteLine("El valor de la variable es: " + reporte);
            Debug.WriteLine("El valor de la variable es: " + reporte);

            if (reporte != null)
            {
                // Modificar el valor de Estado a "Rechazado"
                reporte.Estado = "Rechazado";

                // Guardar los cambios en la base de datos
                _context.SaveChanges();
            }

            // Consulta para obtener los primeros 10 registros
            //  var model = _context._Usuarios.Where(u => u.idUsuario >= 1 && u.idUsuario <= 10).ToList();

            // Obtener los últimos 10 reportes ordenados por fecha de creación descendente
            var model = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Reportado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            var model1 = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Confirmado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            var model2 = _context.Reporte.Where(c => c.idUsuario.idUsuario == idUsuario && c.Estado == "Rechazado")
                .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            if(UsuarioLogeado.Rol == "Jefe")
            {
                model = _context.Reporte.Where(c => c.Estado == "Reportado")
                               .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                               .Take(10)
                               .ToList();

                model1 = _context.Reporte.Where(c => c.Estado == "Confirmado")
                    .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                    .Take(10)
                    .ToList();

                model2 = _context.Reporte.Where(c => c.Estado == "Rechazado")
                    .OrderByDescending(r => r.Fecha) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                    .Take(10)
                    .ToList();
            }

            ViewBag.ReportesConfirmados = model1;
            ViewBag.ReportesRechazados = model2;

            var cantidadUsuarios = _context._Usuarios.Count();

            ViewBag.CantidadUsuarios = cantidadUsuarios;
            ViewBag.PagInicio = 1;
            ViewBag.PagFin = 10;

            return View("Reportes", model);
        }

        /*---------------CREAR INSTANCIA DE INCENDIO-------------------*/
        [HttpGet]
        public IActionResult CrearNuevaInstancia(int idReporte)
        {
            ViewBag.idReporte = idReporte;

            /*
            UsuariosReporteViewModel viewModel = new UsuariosReporteViewModel
            {
                incendio = incendio,
                bitacoraMedidaInicial = bitacoraMedidaInicial
            };
            */


            // return View("CrearNuevaInstancia", viewModel);
            return View("CrearNuevaInstancia");
        }

        [HttpPost]

        [HttpPost]
        public IActionResult CrearNuevaInstancia(Incendio incendio, BitacoraMedidaInicial bitacoraMedidaInicial, int idReporte, Reporte reportes, Actualizacion actualizacion, string Latitud, string Longitud, float Radio)
        {

            /*
            var enEspera = _context.Incendio.Where(c => c.Estado == "En espera")
               .OrderByDescending(r => r.FechaIni) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
               .Take(10)
               .ToList();

            var enAtencion = _context.Incendio.Where(c => c.Estado == "En atencion")
                .OrderByDescending(r => r.FechaIni) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            ViewBag.IncendiosEspera = enEspera;
            ViewBag.IncendiosAtencion = enAtencion;

            */

            ViewBag.idReporte = idReporte;


            // Debug.WriteLine("El valor de la variable es: " + viewModel.incendio.Latitud);

            // Obtén la fecha y hora actuales
            DateTime fechaActual = DateTime.Now;

            // Guarda la fecha en una variable
            DateTime fecha = fechaActual.Date;

            DateTime hora = DateTime.Today.Add(fechaActual.TimeOfDay);

            TimeSpan hora1 = fechaActual.TimeOfDay;

            /*
            UsuariosReporteViewModel viewModel = new UsuariosReporteViewModel
            {
                incendio = incendio,
                bitacoraMedidaInicial = bitacoraMedidaInicial
            };
            */

            var reporte = _context.Reporte.FirstOrDefault(u => u.idReporte == idReporte);

            incendio.Reporte = reporte;
            // incendio.idReporte = reporte.idReporte;
            incendio.FechaIni = hora;
            incendio.Etapa = 1;
            incendio.NombreComando = "Sin asignar";
            incendio.NombreDespacho = "Sin asignar";

            bitacoraMedidaInicial.FechaCreada = hora;
            bitacoraMedidaInicial.CaracterFuego = bitacoraMedidaInicial.CaracterFuego;

            reporte.Estado = "Confirmado";

            _context.Add(incendio);
            _context.SaveChanges();

            var incendioCreado = _context.Incendio.FirstOrDefault(u => u.Reporte == reporte);

            bitacoraMedidaInicial.Incendio = incendioCreado;

            _context.Add(bitacoraMedidaInicial);
            _context.SaveChanges();

            // Guardar los datos en la tabla Actualizacion

            // Convertir y guardar los datos en la tabla Actualizacion
            if (decimal.TryParse(Latitud, out decimal latitudDecimal) &&
                decimal.TryParse(Longitud, out decimal longitudDecimal))
            {
                actualizacion.Latitud = Math.Round(latitudDecimal, 6); ;
                actualizacion.Longitud = Math.Round(longitudDecimal, 6);
                actualizacion.Radio = Radio;
                actualizacion.FechaAccion = DateTime.Now;
                actualizacion.Accion = "Crear";
                actualizacion.Tipo = "Inicial";
                actualizacion.Incendio = incendioCreado;

                _context.Add(actualizacion);
                _context.SaveChanges();
            }
            else
            {
                // Manejo de error en caso de conversión fallida
                ModelState.AddModelError(string.Empty, "Latitud, Longitud o Radio no tienen un formato válido.");
                return View("CrearNuevaInstancia"); // Asegúrate de que esta vista exista
            }

            return RedirectToAction("Incendios");
            // return View("Incendios");
        }

        [HttpPost]

        public IActionResult AsignarPersonal(int idDespacho, int idComando, int idIncendio)
        {
            var despacho = _context._Usuarios.FirstOrDefault(u => u.idUsuario == idDespacho);
            var comando = _context._Usuarios.FirstOrDefault(u => u.idUsuario == idComando);
            var incendio = _context.Incendio.FirstOrDefault(u => u.idIncendio == idIncendio);

            despacho.Estado = "ocupado";
            comando.Estado = "ocupado";

            string DespachoNombre = despacho.Nombre + " " + despacho.ApPaterno + " " + despacho.ApMaterno;
            string ComandoNombre = comando.Nombre + " " + comando.ApPaterno + " " + comando.ApMaterno;

            despacho.Incendio = incendio;
            comando.Incendio = incendio;
            incendio.Etapa = 2;
            incendio.NombreDespacho = DespachoNombre;
            incendio.NombreComando = ComandoNombre;

            _context.SaveChanges();

            return RedirectToAction("Incendios");
           // return View("Incendios");
        }

        [HttpGet]
        public IActionResult Incendios(Incendio incendios)
        {

            var enEspera = _context.Incendio.Where(c => c.Etapa == 1)
                .OrderByDescending(r => r.FechaIni) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            var enAtencion = _context.Incendio.Where(c => c.Etapa > 1 && c.Etapa < 11)
                .OrderByDescending(r => r.FechaIni) // Suponiendo que "FechaCreacion" es el campo que indica cuándo se creó el reporte
                .Take(10)
                .ToList();

            DayOfWeek diaActual = DateTime.Today.DayOfWeek;

            TimeSpan horaActual = DateTime.Now.TimeOfDay;

            var despacho = _context._Usuarios
            .Where(u => u.Rol == "Despacho" && u.Estado == "disponible"
                        && u.DiasLaborales.Contains(diaActual.ToString())
                        && u.TrabajoInicio <= horaActual
                        && horaActual <= u.TrabajoFin)
            .ToList();

            var comando = _context._Usuarios
            .Where(u => u.Rol == "Comandos" && u.Estado == "disponible"
                        && u.DiasLaborales.Contains(diaActual.ToString())
                        && u.TrabajoInicio <= horaActual
                        && horaActual <= u.TrabajoFin)
            .ToList();
            ViewBag.Despacho = despacho;
            ViewBag.Comando = comando;

            ViewBag.IncendiosEspera = enEspera;
            ViewBag.IncendiosAtencion = enAtencion;

          /*  var cantidadUsuarios = _context._Usuarios.Count();

            ViewBag.CantidadUsuarios = cantidadUsuarios;
            ViewBag.PagInicio = 1;
            ViewBag.PagFin = 10;
          */

            return View("Incendios");
        }

        [HttpGet]
        public IActionResult HistoricoIncendios()
        {
            var incendios = _context.Incendio.Where(u => u.Etapa > 10)
                .OrderByDescending(r => r.FechaFin);

            ViewBag.IncendiosConcluidos = incendios;
            return View("HistoricoIncendios");
        }

        [HttpPost]
        public IActionResult VerIncendio(int idIncendio)
        {
            var incendio = _context.Incendio.FirstOrDefault(u => u.idIncendio == idIncendio);
            var comando = _context._Usuarios.FirstOrDefault(u => u.Incendio == incendio && u.Rol == "Comandos");
            var comando1 = incendio.NombreComando;

            Debug.WriteLine(comando1);

            var RecursosTotales = _context.Recursos.Where(u => u.Incendio == null);
            var actRecursos = _context.ActualizacionRecursos.Where(u => u.Incendio == incendio);
            var RecursosPrestados = _context.Recursos.Where(u => u.Incendio == incendio);
            var Trabajadores = _context.Personal.Where(u => u.Usuarios.Nombre + " " + u.Usuarios.ApPaterno + " " + u.Usuarios.ApMaterno == comando1);

            foreach(var trabajo in Trabajadores) { 
                Debug.WriteLine(trabajo.Nombre);
            }

            var BitacoraChequeoPlaneacion = _context.BitacoraChequeoYPlaneacion.FirstOrDefault(u => u.Incendio == incendio) ?? new BitacoraChequeoYPlaneacion();
            var BitacoraStatusSitiacion = _context.BitacoraStatus.FirstOrDefault(u => u.Incendio == incendio) ?? new BitacoraStatus();
            var BitacoraTamanoIncendio = _context.BitacoraTamanoIncendio.FirstOrDefault(u => u.Incendio == incendio) ?? new BitacoraTamanoIncendio();
            var BitacoraVerficacionCI = _context.BitacoraVerificacionCI.FirstOrDefault(u => u.Incendio == incendio) ?? new BitacoraVerificacionCI();
            var BitacoraMedidaInicial = _context.BitacoraMedidaInicial.FirstOrDefault(u => u.Incendio == incendio) ?? new BitacoraMedidaInicial();

            var BitacoraActualizacion = _context.Actualizacion.Where(u => u.Incendio == incendio && u.Tipo == "accion").ToList();
            var BitacoraRecursos = _context.BitacoraRecursos.Where(u => u.Incendio == incendio).ToList();
            var BitacoraRelacionTrabajo = _context.BitacoraRelacionTrabajo.Where(u => u.Incendio == incendio).ToList();
            var RecursosSolicitados = _context.RecursosSolicitados?.Where(u => u.Incendio == incendio);

            ViewBag.RecursosSolicitados = RecursosSolicitados;
            ViewBag.CatalogoTrabajadores = Trabajadores;
            ViewBag.CatalogoRecursos = RecursosPrestados;
            ViewBag.ActRecursos = actRecursos;
            ViewBag.RecursosTotales = RecursosTotales;
            ViewBag.Incendio = incendio;
            ViewBag.BitacoraChequeoPlaneacion = BitacoraChequeoPlaneacion;
            ViewBag.BitacoraStatusSitiacion = BitacoraStatusSitiacion;
            ViewBag.BitacoraTamanoIncendio = BitacoraTamanoIncendio;
            ViewBag.BitacoraVerficacionCI = BitacoraVerficacionCI;
            ViewBag.BitacoraMedidaInicial = BitacoraMedidaInicial;

            ViewBag.BitacoraActualizacion = BitacoraActualizacion;
            ViewBag.BitacoraRecursos = BitacoraRecursos;
            ViewBag.BitacoraRelacionTrabajo = BitacoraRelacionTrabajo;

            return View("VerIncendio");
        }


        [HttpPost]
        public IActionResult VerMapa(int idIncendio)
        {         
            var incendio = _context.Incendio.FirstOrDefault(u => u.idIncendio == idIncendio);
                  
            var actualizacion = _context.Actualizacion
            .Where(u => u.Incendio == incendio && (u.Tipo == "Mapa" || u.Tipo == "Inicial")).ToList();

            ViewBag.Actualizacion = actualizacion;

            return View("VerMapa");
        }
    }
}
