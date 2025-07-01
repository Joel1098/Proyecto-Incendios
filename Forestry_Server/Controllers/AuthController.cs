using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Forestry.Models;
using Forestry.DTOs;
using Forestry.Services;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Forestry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ContextoBaseDeDatos _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ContextoBaseDeDatos context, IEmailService emailService, ILogger<AuthController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Usuario == loginDto.Usuario && u.Estado == "Activo");

                if (usuario == null)
                {
                    return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
                }

                // En un entorno real, deberías usar hashing de contraseñas
                if (usuario.Contrasena != loginDto.Contrasena)
                {
                    return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
                }

                var response = new
                {
                    idUsuario = usuario.idUsuario,
                    usuario = usuario.Usuario,
                    rol = usuario.Rol,
                    nombre = $"{usuario.Nombre} {usuario.ApPaterno} {usuario.ApMaterno}",
                    estado = usuario.Estado
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/auth/profile/{id}
        [HttpGet("profile/{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetProfile(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                var usuarioDto = new UsuarioDTO
                {
                    IdUsuario = usuario.idUsuario,
                    Rol = usuario.Rol,
                    Estado = usuario.Estado,
                    Nombre = usuario.Nombre,
                    ApPaterno = usuario.ApPaterno,
                    ApMaterno = usuario.ApMaterno,
                    NumeTel = usuario.NumeTel,
                    Usuario = usuario.Usuario,
                    TrabajoInicio = usuario.TrabajoInicio,
                    TrabajoFin = usuario.TrabajoFin,
                };

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDTO>> Register([FromBody] UsuarioCreateDTO usuarioDto)
        {
            try
            {
                // Verificar si el usuario ya existe
                var usuarioExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Usuario == usuarioDto.Usuario);

                if (usuarioExistente != null)
                {
                    return BadRequest(new { message = "El nombre de usuario ya existe" });
                }

                var usuario = new Usuarios
                {
                    Usuario = usuarioDto.Usuario,
                    Contrasena = usuarioDto.Contrasena, // En producción, hashear la contraseña
                    Nombre = usuarioDto.Nombre,
                    ApPaterno = usuarioDto.ApPaterno,
                    ApMaterno = usuarioDto.ApMaterno,
                    Rol = "Personal", // O el rol por defecto que desees
                    Estado = "Activo",
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Enviar email de bienvenida si se proporciona un email
                if (!string.IsNullOrEmpty(usuario.NumeTel) && usuario.NumeTel.Contains("@"))
                {
                    try
                    {
                        await _emailService.SendUserRegistrationAsync(usuario.NumeTel, usuario.Usuario, usuario.Contrasena);
                        _logger.LogInformation($"Email de bienvenida enviado a {usuario.NumeTel}");
                    }
                    catch (Exception emailEx)
                    {
                        _logger.LogWarning($"No se pudo enviar email de bienvenida: {emailEx.Message}");
                        // No fallar el registro si el email falla
                    }
                }

                var response = new UsuarioDTO
                {
                    IdUsuario = usuario.idUsuario,
                    Usuario = usuario.Usuario,
                    Rol = usuario.Rol,
                    Nombre = usuario.Nombre,
                    ApPaterno = usuario.ApPaterno,
                    ApMaterno = usuario.ApMaterno,
                    Estado = usuario.Estado,
                    FechaCreacion = usuario.FechaCreacion
                };

                return CreatedAtAction(nameof(GetProfile), new { id = usuario.idUsuario }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // PUT: api/auth/profile/{id}
        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UsuarioUpdateDTO usuarioDto)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                usuario.Nombre = usuarioDto.Nombre;
                usuario.ApPaterno = usuarioDto.ApPaterno;
                usuario.ApMaterno = usuarioDto.ApMaterno;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
} 