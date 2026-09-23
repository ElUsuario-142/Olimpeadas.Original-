using Olimpeadas.Aplication.DTOs.Alerta;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;

namespace Olimpeadas.Aplication.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly IAlertaRepository _alertaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioAlertaRepository _usuarioAlertaRepository;

        public AlertaService(
            IAlertaRepository alertaRepository,
            IUsuarioRepository usuarioRepository,
            IUsuarioAlertaRepository usuarioAlertaRepository)
        {
            _alertaRepository = alertaRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioAlertaRepository = usuarioAlertaRepository;
        }

        public async Task<AlertaDTO> CrearAsync(CrearAlertaDTO dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId)
                ?? throw new KeyNotFoundException($"Usuario emisor con ID {dto.UsuarioId} no encontrado.");

            if (!usuario.Activo)
                throw new InvalidOperationException("El usuario se encuentra inactivo.");

            var alerta = new Alerta
            {
                Tipo = dto.Tipo,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                Fecha = DateTime.UtcNow,
                Estado = EstadoAlerta.Enviada
            };

            await _alertaRepository.AddAsync(alerta);
            await _alertaRepository.SaveChangesAsync();

            // Vincular al usuario emisor
            await _usuarioAlertaRepository.AddAsync(new UsuarioAlerta
            {
                UsuarioId = usuario.Id,
                AlertaId = alerta.Id
            });

            // Difundir la alerta a todos los usuarios activos del municipio
            var usuariosMunicipio = await _usuarioRepository.GetByMunicipioIdAsync(usuario.MunicipioId);
            foreach (var u in usuariosMunicipio.Where(u => u.Activo && u.Id != usuario.Id))
            {
                await _usuarioAlertaRepository.AddAsync(new UsuarioAlerta
                {
                    UsuarioId = u.Id,
                    AlertaId = alerta.Id
                });
            }

            await _usuarioAlertaRepository.SaveChangesAsync();

            var alertaConUsuarios = await _alertaRepository.GetWithUsuariosAsync(alerta.Id);
            return MapToDTO(alertaConUsuarios ?? alerta);
        }

        public async Task<AlertaDTO?> GetByIdAsync(int id)
        {
            var alerta = await _alertaRepository.GetWithUsuariosAsync(id);
            return alerta == null ? null : MapToDTO(alerta);
        }

        public async Task<IEnumerable<AlertaDTO>> GetAllAsync()
        {
            var alertas = await _alertaRepository.GetAllAsync();
            return alertas.Select(MapToDTO);
        }

        public async Task<IEnumerable<AlertaDTO>> GetByEstadoAsync(EstadoAlerta estado)
        {
            var alertas = await _alertaRepository.GetByEstadoAsync(estado);
            return alertas.Select(MapToDTO);
        }

        public async Task<IEnumerable<AlertaDTO>> GetByTipoAsync(TipoEmergencia tipo)
        {
            var alertas = await _alertaRepository.GetByTipoAsync(tipo);
            return alertas.Select(MapToDTO);
        }

        public async Task<AlertaDTO> CambiarEstadoAsync(int id, ActualizarAlertaDTO dto)
        {
            var alerta = await _alertaRepository.GetWithUsuariosAsync(id)
                ?? throw new KeyNotFoundException($"Alerta con ID {id} no encontrada.");

            alerta.Estado = dto.Estado;
            _alertaRepository.Update(alerta);
            await _alertaRepository.SaveChangesAsync();

            return MapToDTO(alerta);
        }

        public async Task<IEnumerable<AlertaDTO>> GetAlertasPorUsuarioAsync(int usuarioId)
        {
            var usuarioAlertas = await _usuarioAlertaRepository.GetByUsuarioIdAsync(usuarioId);
            return usuarioAlertas.Select(ua => MapToDTO(ua.Alerta));
        }

        private static AlertaDTO MapToDTO(Alerta a) => new()
        {
            Id = a.Id,
            Tipo = a.Tipo,
            Latitud = a.Latitud,
            Longitud = a.Longitud,
            Fecha = a.Fecha,
            Estado = a.Estado,
            UsuariosNotificadosIds = a.UsuariosAlertas?.Select(ua => ua.UsuarioId).ToList() ?? new List<int>()
        };
    }
}
