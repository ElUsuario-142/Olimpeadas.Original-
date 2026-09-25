using Olimpeadas.Aplication.DTOs.Reporte;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;
using Olimpeadas.Domain.Interfaces;

namespace Olimpeadas.Aplication.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IDireccionRepository _direccionRepository;
        private readonly IImagenRepository _imagenRepository;
        private readonly IHistorialEstadoRepository _historialEstadoRepository;

        public ReporteService(
            IReporteRepository reporteRepository,
            IUsuarioRepository usuarioRepository,
            ICategoriaRepository categoriaRepository,
            IDireccionRepository direccionRepository,
            IImagenRepository imagenRepository,
            IHistorialEstadoRepository historialEstadoRepository)
        {
            _reporteRepository = reporteRepository;
            _usuarioRepository = usuarioRepository;
            _categoriaRepository = categoriaRepository;
            _direccionRepository = direccionRepository;
            _imagenRepository = imagenRepository;
            _historialEstadoRepository = historialEstadoRepository;
        }

        public async Task<ReporteDTO> CrearAsync(CrearReporteDTO dto, int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId)
                ?? throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

            if (!usuario.Activo)
                throw new InvalidOperationException("El usuario se encuentra inactivo y no puede generar reportes.");

            var categoria = await _categoriaRepository.GetByIdAsync(dto.CategoriaId)
                ?? throw new KeyNotFoundException($"Categoría con ID {dto.CategoriaId} no encontrada.");

            // Buscar dirección por coordenadas o crear una nueva
            var direccion = await _direccionRepository.GetByCoordenadasAsync(dto.Latitud, dto.Longitud);
            if (direccion == null)
            {
                direccion = new Direccion
                {
                    Calle = dto.Calle,
                    Latitud = dto.Latitud,
                    Longitud = dto.Longitud
                };
                await _direccionRepository.AddAsync(direccion);
                await _direccionRepository.SaveChangesAsync();
            }

            var now = DateTime.UtcNow;

            var reporte = new Reporte
            {
                UsuarioId = usuarioId,
                CategoriaId = dto.CategoriaId,
                DireccionId = direccion.Id,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Estado = Estado.Pendiente,
                FechaCreacion = now,
                FechaActualizacion = now
            };

            await _reporteRepository.AddAsync(reporte);
            await _reporteRepository.SaveChangesAsync();

            // Adjuntar imágenes si existen
            if (dto.ImagenesUrls != null && dto.ImagenesUrls.Count > 0)
            {
                foreach (var url in dto.ImagenesUrls)
                {
                    var imagen = new Imagen
                    {
                        ReporteId = reporte.Id,
                        Url = url
                    };
                    await _imagenRepository.AddAsync(imagen);
                }
                await _imagenRepository.SaveChangesAsync();
            }

            // Crear registro inicial en el historial de estados
            var historialInicial = new HistorialEstado
            {
                UsuarioId = usuarioId,
                ReporteId = reporte.Id,
                EstadoAnterior = null,
                EstadoNuevo = Estado.Pendiente,
                Comentario = "Reporte creado e ingresado como Pendiente.",
                Fecha = now
            };

            await _historialEstadoRepository.AddAsync(historialInicial);
            await _historialEstadoRepository.SaveChangesAsync();

            var reporteCompleto = await _reporteRepository.GetWithDetallesAsync(reporte.Id);
            return MapToDTO(reporteCompleto ?? reporte);
        }

        public async Task<ReporteDTO?> GetByIdAsync(int id)
        {
            var reporte = await _reporteRepository.GetWithDetallesAsync(id);
            return reporte == null ? null : MapToDTO(reporte);
        }

        public async Task<IEnumerable<ReporteDTO>> GetAllAsync()
        {
            var reportes = await _reporteRepository.GetAllAsync();
            return reportes.Select(MapToDTO);
        }

        public async Task<IEnumerable<ReporteDTO>> GetByUsuarioAsync(int usuarioId)
        {
            var reportes = await _reporteRepository.GetByUsuarioIdAsync(usuarioId);
            return reportes.Select(MapToDTO);
        }

        public async Task<IEnumerable<ReporteDTO>> GetByEstadoAsync(Estado estado)
        {
            var reportes = await _reporteRepository.GetByEstadoAsync(estado);
            return reportes.Select(MapToDTO);
        }

        public async Task<IEnumerable<ReporteDTO>> GetByCategoriaAsync(int categoriaId)
        {
            var reportes = await _reporteRepository.GetByCategoriaIdAsync(categoriaId);
            return reportes.Select(MapToDTO);
        }

        public async Task<IEnumerable<ReporteDTO>> GetByEmpleadoAsync(int empleadoId)
        {
            var reportes = await _reporteRepository.GetByEmpleadoIdAsync(empleadoId);
            return reportes.Select(MapToDTO);
        }

        public async Task<IEnumerable<ReporteDTO>> GetSinAsignarAsync()
        {
            var reportes = await _reporteRepository.GetSinAsignarAsync();
            return reportes.Select(MapToDTO);
        }

        public async Task<ReporteDTO> AsignarEmpleadoAsync(int reporteId, AsignarEmpleadoDTO dto,int asignadorUsuarioId)
        {
            var reporte = await _reporteRepository.GetWithDetallesAsync(reporteId)
                ?? throw new KeyNotFoundException($"Reporte con ID {reporteId} no encontrado.");

            var empleado = await _usuarioRepository.GetByIdAsync(dto.EmpleadoId)
                ?? throw new KeyNotFoundException($"Empleado con ID {dto.EmpleadoId} no encontrado.");

            if (empleado.Rol != Rol.Empleado && empleado.Rol != Rol.Admin)
                throw new InvalidOperationException("El usuario asignado debe tener rol de Empleado o Admin.");

            
            reporte.EmpleadoEncargadoId = empleado.Id;
            reporte.FechaActualizacion = DateTime.UtcNow;

            _reporteRepository.Update(reporte);

            // Registro en historial
            var historial = new HistorialEstado
            {
                UsuarioId = asignadorUsuarioId,
                ReporteId = reporte.Id,
                EstadoAnterior = reporte.Estado,
                EstadoNuevo = reporte.Estado,
                Comentario = $"Asignado al empleado: {empleado.Nombre} {empleado.Apellido} (ID: {empleado.Id})",
                Fecha = DateTime.UtcNow
            };

            await _historialEstadoRepository.AddAsync(historial);
            await _reporteRepository.SaveChangesAsync();

            var actualizado = await _reporteRepository.GetWithDetallesAsync(reporteId);
            return MapToDTO(actualizado ?? reporte);
        }

        public async Task<ReporteDTO> CambiarEstadoAsync(int reporteId, CambiarEstadoReporteDTO dto, int usuarioId)
        {
            var reporte = await _reporteRepository.GetWithDetallesAsync(reporteId)
                ?? throw new KeyNotFoundException($"Reporte con ID {reporteId} no encontrado.");

            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId)
                ?? throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

            var estadoAnterior = reporte.Estado;
            reporte.Estado = dto.NuevoEstado;
            reporte.FechaActualizacion = DateTime.UtcNow;

            _reporteRepository.Update(reporte);

            var historial = new HistorialEstado
            {
                UsuarioId = usuarioId,
                ReporteId = reporte.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = dto.NuevoEstado,
                Comentario = dto.Comentario,
                Fecha = DateTime.UtcNow
            };

            await _historialEstadoRepository.AddAsync(historial);
            await _reporteRepository.SaveChangesAsync();

            var actualizado = await _reporteRepository.GetWithDetallesAsync(reporteId);
            return MapToDTO(actualizado ?? reporte);
        }

        public async Task<bool> AgregarImagenAsync(int reporteId, string url)
        {
            var existe = await _reporteRepository.ExistsAsync(reporteId);
            if (!existe)
                throw new KeyNotFoundException($"Reporte con ID {reporteId} no encontrado.");

            var imagen = new Imagen
            {
                ReporteId = reporteId,
                Url = url
            };

            await _imagenRepository.AddAsync(imagen);
            await _imagenRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<HistorialEstadoDTO>> GetHistorialAsync(int reporteId)
        {
            var historiales = await _historialEstadoRepository.GetByReporteIdAsync(reporteId);
            return historiales.Select(h => new HistorialEstadoDTO
            {
                Id = h.Id,
                UsuarioId = h.UsuarioId,
                UsuarioNombre = h.Usuario != null ? $"{h.Usuario.Nombre} {h.Usuario.Apellido}" : null,
                EstadoAnterior = h.EstadoAnterior,
                EstadoNuevo = h.EstadoNuevo,
                Comentario = h.Comentario,
                Fecha = h.Fecha
            });
        }

        private static ReporteDTO MapToDTO(Reporte r) => new()
        {
            Id = r.Id,
            UsuarioId = r.UsuarioId,
            UsuarioNombre = r.Usuario != null ? $"{r.Usuario.Nombre} {r.Usuario.Apellido}" : null,
            CategoriaId = r.CategoriaId,
            CategoriaNombre = r.Categoria?.Nombre,
            DireccionId = r.DireccionId,
            DireccionCalle = r.Direccion?.Calle,
            Latitud = r.Direccion?.Latitud ?? 0,
            Longitud = r.Direccion?.Longitud ?? 0,
            EmpleadoEncargadoId = r.EmpleadoEncargadoId,
            EmpleadoEncargadoNombre = r.EmpleadoEncargado != null ? $"{r.EmpleadoEncargado.Nombre} {r.EmpleadoEncargado.Apellido}" : null,
            Titulo = r.Titulo,
            Descripcion = r.Descripcion,
            Estado = r.Estado,
            FechaCreacion = r.FechaCreacion,
            FechaActualizacion = r.FechaActualizacion,
            ImagenesUrls = r.Imagenes?.Select(i => i.Url).ToList() ?? new List<string>(),
            HistorialEstados = r.HistorialesEstado?.Select(h => new HistorialEstadoDTO
            {
                Id = h.Id,
                UsuarioId = h.UsuarioId,
                UsuarioNombre = h.Usuario != null ? $"{h.Usuario.Nombre} {h.Usuario.Apellido}" : null,
                EstadoAnterior = h.EstadoAnterior,
                EstadoNuevo = h.EstadoNuevo,
                Comentario = h.Comentario,
                Fecha = h.Fecha
            }).ToList() ?? new List<HistorialEstadoDTO>()
        };
    }
}
