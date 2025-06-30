using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forestry.Models;
using Forestry.DTOs;
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

        public AuthController(ContextoBaseDeDatos context)
        {
            _context = context;
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
                    Contrasena = usuario.Contrasena, // En producción, no enviar la contraseña
                    TrabajoInicio = usuario.TrabajoInicio,
                    TrabajoFin = usuario.TrabajoFin,
                    FechaCreacion = usuario.FechaCreacion
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
                    Rol = usuarioDto.Rol,
                    Estado = "Activo",
                    Nombre = usuarioDto.Nombre,
                    ApPaterno = usuarioDto.ApPaterno,
                    ApMaterno = usuarioDto.ApMaterno,
                    NumeTel = usuarioDto.NumeTel,
                    Usuario = usuarioDto.Usuario,
                    Contrasena = usuarioDto.Contrasena, // En producción, hashear la contraseña
                    TrabajoInicio = usuarioDto.TrabajoInicio,
                    TrabajoFin = usuarioDto.TrabajoFin,
                    FechaCreacion = DateTime.Now
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var response = new UsuarioDTO
                {
                    IdUsuario = usuario.idUsuario,
                    Rol = usuario.Rol,
                    Estado = usuario.Estado,
                    Nombre = usuario.Nombre,
                    ApPaterno = usuario.ApPaterno,
                    ApMaterno = usuario.ApMaterno,
                    NumeTel = usuario.NumeTel,
                    Usuario = usuario.Usuario,
                    Contrasena = usuario.Contrasena,
                    TrabajoInicio = usuario.TrabajoInicio,
                    TrabajoFin = usuario.TrabajoFin,
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

                // Verificar si el nuevo nombre de usuario ya existe (si cambió)
                if (usuario.Usuario != usuarioDto.Usuario)
                {
                    var usuarioExistente = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Usuario == usuarioDto.Usuario);

                    if (usuarioExistente != null)
                    {
                        return BadRequest(new { message = "El nombre de usuario ya existe" });
                    }
                }

                usuario.Rol = usuarioDto.Rol;
                usuario.Nombre = usuarioDto.Nombre;
                usuario.ApPaterno = usuarioDto.ApPaterno;
                usuario.ApMaterno = usuarioDto.ApMaterno;
                usuario.NumeTel = usuarioDto.NumeTel;
                usuario.Usuario = usuarioDto.Usuario;
                usuario.TrabajoInicio = usuarioDto.TrabajoInicio;
                usuario.TrabajoFin = usuarioDto.TrabajoFin;

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