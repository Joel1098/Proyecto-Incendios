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
using Forestry.DTOs;

namespace Forestry.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ContextoBaseDeDatos _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ILogger<HomeController> logger, ContextoBaseDeDatos context, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new { message = "Forestry API is running" });
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Ok(new { message = "Login endpoint" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Usuario == request.Usuario);

                if (usuario == null)
                {
                    return Unauthorized(new { message = "Usuario no encontrado" });
                }

                // Verificar contraseña (en producción usar hash seguro)
                if (usuario.Contrasena != request.Contrasena)
                {
                    return Unauthorized(new { message = "Contraseña incorrecta" });
                }

                // Configurar sesión
                HttpContext.Session.SetInt32("IdUsuario", usuario.idUsuario);
                HttpContext.Session.SetString("Rol", usuario.Rol);

                return Ok(new {
                    message = "Login exitoso",
                    usuario = new {
                        id = usuario.idUsuario,
                        nombre = usuario.Nombre,
                        rol = usuario.Rol
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return StatusCode(500, new { message = "Error interno del servidor" });
        }

        /*-----------------MÉTODOS DEL JEFE DE DESPACHO--------------------*/

        /*---------------------------MÉTODOS DEL PERSONAL DE DESPACHO-----------------------------*/
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

        [HttpGet("bitacora-tamano-incendio")]
        public IActionResult BitacoraTamanoIncendio()
        {
            return NoContent();
        }

        [HttpGet("bitacora-verificacion-ci")]
        public IActionResult BitacoraVerificacionCI()
        {
            return NoContent();
        }

        [HttpGet("bitacora-revision-posterior")]
        public IActionResult BitacoraRevisionPosterior()
        {
            return NoContent();
        }

        [Authorize]
        [HttpGet("recursos")]
        public IActionResult Recursos()
        {
            return NoContent();
        }

        [HttpGet("mapa")]
        public IActionResult Mapa()
        {
            return NoContent();
        }

        /*-----------------OTROS-----------------------*/

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = new[] { "Administrador", "Jefe", "Despacho", "Comando", "Personal" };
            return Ok(roles);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Validar si el usuario ya existe
                var existe = await _context.Usuarios.AnyAsync(u => u.Usuario == request.Usuario);
                if (existe)
                {
                    return BadRequest(new { message = "El usuario ya existe" });
                }

                var nuevoUsuario = new Usuarios
                {
                    Usuario = request.Usuario,
                    Contrasena = request.Contrasena, // En producción usar hash seguro
                    Nombre = request.Nombre,
                    ApPaterno = request.ApPaterno,
                    ApMaterno = request.ApMaterno,
                    Rol = request.Rol,
                    NumeTel = request.NumeTel,
                    DiasLaborales = request.DiasLaborales,
                    Estado = request.Estado,
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuario registrado exitosamente", usuario = nuevoUsuario });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en registro");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }
}
