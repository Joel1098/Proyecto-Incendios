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

        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return Ok(new { message = "Privacy Policy" });
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
                    .FirstOrDefaultAsync(u => u.Usuario == request.Username);

                if (usuario == null)
                {
                    return Unauthorized(new { message = "Usuario no encontrado" });
                }

                // Verificar contraseña (asumiendo que está encriptada)
                if (usuario.Contrasena != request.Password) // En producción usar hash
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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logout exitoso" });
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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

        /*-----------------AGENTE TELEFÓNICO----------------------*/

        /*-----------------OTROS-----------------------*/
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
