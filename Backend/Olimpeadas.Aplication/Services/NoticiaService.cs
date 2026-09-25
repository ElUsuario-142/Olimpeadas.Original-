using Olimpeadas.Aplication.DTOs.Noticia;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Interfaces;

namespace Olimpeadas.Aplication.Services
{
    public class NoticiaService : INoticiaService
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IMunicipioRepository _municipioRepository;
        private readonly IReporteRepository _reporteRepository;
        private readonly IRegistroRepository _registroRepository;

        public NoticiaService(
            INoticiaRepository noticiaRepository,
            IMunicipioRepository municipioRepository,
            IReporteRepository reporteRepository,
            IRegistroRepository registroRepository)
        {
            _noticiaRepository = noticiaRepository;
            _municipioRepository = municipioRepository;
            _reporteRepository = reporteRepository;
            _registroRepository = registroRepository;
        }

        public async Task<NoticiaDTO> CrearAsync(CrearNoticiaDTO dto)
        {
            var municipio = await _municipioRepository.GetByIdAsync(dto.MunicipioId)
                ?? throw new KeyNotFoundException($"Municipio con ID {dto.MunicipioId} no encontrado.");

            var noticia = new Noticia
            {
                MunicipioId = dto.MunicipioId,
                Titulo = dto.Titulo,
                Contenido = dto.Contenido,
                Fecha = DateTime.UtcNow
            };

            await _noticiaRepository.AddAsync(noticia);
            await _noticiaRepository.SaveChangesAsync();

            if (dto.ReportesIds != null && dto.ReportesIds.Count > 0)
            {
                foreach (var reporteId in dto.ReportesIds)
                {
                    if (await _reporteRepository.ExistsAsync(reporteId))
                    {
                        var registro = new Registro
                        {
                            NoticiaId = noticia.Id,
                            ReporteId = reporteId,
                            Fecha = DateTime.UtcNow,
                            Comentario = "Vinculado al momento de crear la noticia."
                        };
                        await _registroRepository.AddAsync(registro);
                    }
                }
                await _registroRepository.SaveChangesAsync();
            }

            var noticiaCompleta = await _noticiaRepository.GetWithRegistrosAsync(noticia.Id);
            return MapToDTO(noticiaCompleta ?? noticia);
        }

        public async Task<NoticiaDTO?> GetByIdAsync(int id)
        {
            var noticia = await _noticiaRepository.GetWithRegistrosAsync(id);
            return noticia == null ? null : MapToDTO(noticia);
        }

        public async Task<IEnumerable<NoticiaDTO>> GetAllAsync()
        {
            var noticias = await _noticiaRepository.GetAllAsync();
            return noticias.Select(MapToDTO);
        }

        public async Task<IEnumerable<NoticiaDTO>> GetByMunicipioAsync(int municipioId)
        {
            var noticias = await _noticiaRepository.GetByMunicipioIdAsync(municipioId);
            return noticias.Select(MapToDTO);
        }

        public async Task<RegistroDTO> VincularReporteAsync(int noticiaId, VincularReporteDTO dto)
        {
            var noticiaExiste = await _noticiaRepository.ExistsAsync(noticiaId);
            if (!noticiaExiste)
                throw new KeyNotFoundException($"Noticia con ID {noticiaId} no encontrada.");

            var reporte = await _reporteRepository.GetByIdAsync(dto.ReporteId)
                ?? throw new KeyNotFoundException($"Reporte con ID {dto.ReporteId} no encontrado.");

            var yaVinculado = await _registroRepository.ExisteVinculoAsync(dto.ReporteId, noticiaId);
            if (yaVinculado)
                throw new InvalidOperationException("Este reporte ya se encuentra vinculado a la noticia.");

            var registro = new Registro
            {
                NoticiaId = noticiaId,
                ReporteId = dto.ReporteId,
                Fecha = DateTime.UtcNow,
                Comentario = dto.Comentario
            };

            await _registroRepository.AddAsync(registro);
            await _registroRepository.SaveChangesAsync();

            return new RegistroDTO
            {
                Id = registro.Id,
                ReporteId = registro.ReporteId,
                ReporteTitulo = reporte.Titulo,
                NoticiaId = registro.NoticiaId,
                Fecha = registro.Fecha,
                Comentario = registro.Comentario
            };
        }

        public async Task<bool> DesvincularReporteAsync(int noticiaId, int reporteId)
        {
            var registros = await _registroRepository.FindAsync(r => r.NoticiaId == noticiaId && r.ReporteId == reporteId);
            var registro = registros.FirstOrDefault()
                ?? throw new KeyNotFoundException($"No se encontró la vinculación entre la noticia {noticiaId} y el reporte {reporteId}.");

            _registroRepository.Delete(registro);
            await _registroRepository.SaveChangesAsync();
            return true;
        }

        private static NoticiaDTO MapToDTO(Noticia n) => new()
        {
            Id = n.Id,
            MunicipioId = n.MunicipioId,
            MunicipioNombre = n.Municipio?.Nombre,
            Titulo = n.Titulo,
            Contenido = n.Contenido,
            Fecha = n.Fecha,
            RegistrosVinculados = n.Registros?.Select(r => new RegistroDTO
            {
                Id = r.Id,
                ReporteId = r.ReporteId,
                ReporteTitulo = r.Reporte?.Titulo,
                NoticiaId = r.NoticiaId,
                Fecha = r.Fecha,
                Comentario = r.Comentario
            }).ToList() ?? new List<RegistroDTO>()
        };
    }
}
