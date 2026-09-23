using Olimpeadas.Aplication.DTOs.Usuario;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;

namespace Olimpeadas.Aplication.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMunicipioRepository _municipioRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IMunicipioRepository municipioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _municipioRepository = municipioRepository;
        }

        public async Task<UsuarioDTO?> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return usuario == null ? null : MapToDTO(usuario);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(MapToDTO);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetByMunicipioAsync(int municipioId)
        {
            var usuarios = await _usuarioRepository.GetByMunicipioIdAsync(municipioId);
            return usuarios.Select(MapToDTO);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetByRolAsync(Rol rol)
        {
            var usuarios = await _usuarioRepository.GetByRolAsync(rol);
            return usuarios.Select(MapToDTO);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetActivosAsync()
        {
            var usuarios = await _usuarioRepository.GetActivosAsync();
            return usuarios.Select(MapToDTO);
        }

        public async Task<UsuarioDTO> UpdateAsync(int id, ActualizarUsuarioDTO dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
                usuario.Nombre = dto.Nombre;

            if (!string.IsNullOrWhiteSpace(dto.Apellido))
                usuario.Apellido = dto.Apellido;

            if (!string.IsNullOrWhiteSpace(dto.Telefono))
                usuario.Telefono = dto.Telefono;

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != usuario.Email)
            {
                var emailExistente = await _usuarioRepository.GetByEmailAsync(dto.Email);
                if (emailExistente != null && emailExistente.Id != id)
                {
                    throw new InvalidOperationException("El correo electrónico ya está registrado por otro usuario.");
                }
                usuario.Email = dto.Email;
            }

            if (dto.Rol.HasValue)
                usuario.Rol = dto.Rol.Value;

            if (dto.Activo.HasValue)
                usuario.Activo = dto.Activo.Value;

            if (dto.MunicipioId.HasValue && dto.MunicipioId.Value != usuario.MunicipioId)
            {
                var municipio = await _municipioRepository.GetByIdAsync(dto.MunicipioId.Value)
                    ?? throw new KeyNotFoundException($"Municipio con ID {dto.MunicipioId.Value} no encontrado.");
                usuario.MunicipioId = municipio.Id;
            }

            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            return MapToDTO(usuario);
        }

        public async Task<bool> CambiarEstadoActivoAsync(int id, bool activo)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

            usuario.Activo = activo;
            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveChangesAsync();

            return true;
        }

        private static UsuarioDTO MapToDTO(Usuario u) => new()
        {
            Id = u.Id,
            MunicipioId = u.MunicipioId,
            MunicipioNombre = u.Municipio?.Nombre,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            DNI = u.DNI,
            Telefono = u.Telefono,
            Email = u.Email,
            Rol = u.Rol,
            Activo = u.Activo
        };
    }
}
