
using Olimpeadas.Aplication.DTOs.Auth;
using Olimpeadas.Aplication.DTOs.Usuario;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Domain.Enums;
namespace Olimpeadas.Aplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher passwordHasher,
            IJwtGenerator jwtGenerator)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            if (usuario == null)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            if (!usuario.Activo)
            {
                throw new InvalidOperationException("La cuenta de usuario se encuentra inactiva.");
            }

            var passwordValido = _passwordHasher.VerifyPassword(loginDto.Contraseña, usuario.ContraseñaHash);
            if (!passwordValido)
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            var (token, expiracion) = _jwtGenerator.GenerateToken(usuario);

            return new AuthResponseDTO
            {
                Token = token,
                UsuarioId = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Rol = usuario.Rol,
                MunicipioId = usuario.MunicipioId,
                Expiracion = expiracion
            };
        }

        public async Task<AuthResponseDTO> RegisterAsync(CrearUsuarioDTO registroDto)
        {
            var usuarioExistenteEmail = await _usuarioRepository.GetByEmailAsync(registroDto.Email);
            if (usuarioExistenteEmail != null)
            {
                throw new InvalidOperationException("El correo electrónico ya se encuentra registrado.");
            }

            var usuarioExistenteDni = await _usuarioRepository.GetByDNIAsync(registroDto.DNI);
            if (usuarioExistenteDni != null)
            {
                throw new InvalidOperationException("El DNI ya se encuentra registrado.");
            }

            var passwordHash = _passwordHasher.HashPassword(registroDto.Contraseña);

            var nuevoUsuario = new Usuario
            {
                Nombre = registroDto.Nombre,
                Apellido = registroDto.Apellido,
                DNI = registroDto.DNI,
                Telefono = registroDto.Telefono,
                Email = registroDto.Email,
                ContraseñaHash = passwordHash,
                Rol = Rol.Ciudadano,
                MunicipioId = registroDto.MunicipioId,
                Activo = true
            };

            await _usuarioRepository.AddAsync(nuevoUsuario);
            await _usuarioRepository.SaveChangesAsync();

            var (token, expiracion) = _jwtGenerator.GenerateToken(nuevoUsuario);

            return new AuthResponseDTO
            {
                Token = token,
                UsuarioId = nuevoUsuario.Id,
                Nombre = nuevoUsuario.Nombre,
                Apellido = nuevoUsuario.Apellido,
                Email = nuevoUsuario.Email,
                Rol = nuevoUsuario.Rol,
                MunicipioId = nuevoUsuario.MunicipioId,
                Expiracion = expiracion
            };
        }
    }
}
