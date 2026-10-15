using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Auth;
using Olimpeadas.Aplication.DTOs.Usuario;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Interfaces;

namespace Olimpeadas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthController(IAuthService authService, IUsuarioRepository usuarioRepository)
        {
            _authService = authService;
            _usuarioRepository = usuarioRepository;
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
        public async Task<IActionResult> ObtenerPerfilAutenticado()
        {
            var usuario = await _usuarioRepository.GetByIdAsync(User.GetUserId())
                ?? throw new KeyNotFoundException("Usuario no encontrado.");

            return Ok(new
            {
                id = usuario.Id,
                nombre = usuario.Nombre,
                apellido = usuario.Apellido,
                email = usuario.Email,
                dni = usuario.DNI,
                rol = usuario.Rol.ToString(),
                municipioId = usuario.MunicipioId,
                telefono = usuario.Telefono,
                activo = usuario.Activo
            });
        }
    }
}
