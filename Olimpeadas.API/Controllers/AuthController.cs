using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.Aplication.DTOs.Auth;
using Olimpeadas.Aplication.DTOs.Usuario;
using Olimpeadas.Aplication.Interfaces;

namespace Olimpeadas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado.", detalle = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CrearUsuarioDTO registroDto)
        {
            try
            {
                var response = await _authService.RegisterAsync(registroDto);
                return CreatedAtAction(nameof(Login), new { id = response.UsuarioId }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado al registrar el usuario.", detalle = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("perfil")]
        public IActionResult ObtenerPerfilAutenticado()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var nombre = User.FindFirstValue(ClaimTypes.Name);
            var rol = User.FindFirstValue(ClaimTypes.Role);
            var municipioId = User.FindFirstValue("municipioId");
            var dni = User.FindFirstValue("dni");

            return Ok(new
            {
                id = userId,
                nombre,
                email,
                rol,
                municipioId,
                dni
            });
        }
    }
}
