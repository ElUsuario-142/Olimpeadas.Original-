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
            return Ok(await _authService.LoginAsync(loginDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CrearUsuarioDTO registroDto)
        {

            var response = await _authService.RegisterAsync(registroDto);
            return CreatedAtAction(nameof(ObtenerPerfilAutenticado), response);

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
