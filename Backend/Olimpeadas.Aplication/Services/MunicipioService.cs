using Olimpeadas.Aplication.DTOs.Municipio;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;

namespace Olimpeadas.Aplication.Services
{
    public class MunicipioService : IMunicipioService
    {
        private readonly IMunicipioRepository _municipioRepository;
        private readonly IContactoRepository _contactoRepository;

        public MunicipioService(
            IMunicipioRepository municipioRepository,
            IContactoRepository contactoRepository)
        {
            _municipioRepository = municipioRepository;
            _contactoRepository = contactoRepository;
        }

        public async Task<IEnumerable<MunicipioDTO>> GetAllMunicipiosAsync()
        {
            var municipios = await _municipioRepository.GetAllAsync();
            return municipios.Select(m => new MunicipioDTO
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Localidad = m.Localidad
            });
        }

        public async Task<MunicipioDTO?> GetMunicipioByIdAsync(int id)
        {
            var municipio = await _municipioRepository.GetByIdAsync(id);
            return municipio == null ? null : new MunicipioDTO
            {
                Id = municipio.Id,
                Nombre = municipio.Nombre,
                Localidad = municipio.Localidad
            };
        }

        public async Task<MunicipioDTO> CrearMunicipioAsync(CrearMunicipioDTO dto)
        {
            var municipio = new Municipio
            {
                Nombre = dto.Nombre,
                Localidad = dto.Localidad
            };

            await _municipioRepository.AddAsync(municipio);
            await _municipioRepository.SaveChangesAsync();

            return new MunicipioDTO
            {
                Id = municipio.Id,
                Nombre = municipio.Nombre,
                Localidad = municipio.Localidad
            };
        }

        public async Task<IEnumerable<ContactoDTO>> GetContactosByMunicipioAsync(int municipioId)
        {
            var contactos = await _contactoRepository.GetByMunicipioIdAsync(municipioId);
            return contactos.Select(c => new ContactoDTO
            {
                Id = c.Id,
                MunicipioId = c.MunicipioId,
                MunicipioNombre = c.Municipio?.Nombre,
                Tipo = c.Tipo,
                Numero = c.Numero
            });
        }

        public async Task<IEnumerable<ContactoDTO>> GetContactosByTipoAsync(TipoEmergencia tipo)
        {
            var contactos = await _contactoRepository.GetByTipoAsync(tipo);
            return contactos.Select(c => new ContactoDTO
            {
                Id = c.Id,
                MunicipioId = c.MunicipioId,
                MunicipioNombre = c.Municipio?.Nombre,
                Tipo = c.Tipo,
                Numero = c.Numero
            });
        }

        public async Task<ContactoDTO> CrearContactoAsync(CrearContactoDTO dto)
        {
            var municipio = await _municipioRepository.GetByIdAsync(dto.MunicipioId)
                ?? throw new KeyNotFoundException($"Municipio con ID {dto.MunicipioId} no encontrado.");

            var contacto = new Contacto
            {
                MunicipioId = dto.MunicipioId,
                Tipo = dto.Tipo,
                Numero = dto.Numero
            };

            await _contactoRepository.AddAsync(contacto);
            await _contactoRepository.SaveChangesAsync();

            return new ContactoDTO
            {
                Id = contacto.Id,
                MunicipioId = contacto.MunicipioId,
                MunicipioNombre = municipio.Nombre,
                Tipo = contacto.Tipo,
                Numero = contacto.Numero
            };
        }
    }
}
