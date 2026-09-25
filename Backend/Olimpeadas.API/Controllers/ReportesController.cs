using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Reporte;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.API.Controllers
{
    /* <summary>
     Reportes de incidencias, 
    Los servicios NO validan permisos, lo que permite hacer que cada rol se decide aca
     Los errores (404, 400, 401, 500) los resuelve ExceptionHandlingMiddleware
     </summary> */
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        // POST api/reportes  — cualquier usuario logueado. El autor sale del JWT.
        [HttpPost]
        [ProducesResponseType(typeof(ReporteDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReporteDTO>> Crear([FromBody] CrearReporteDTO dto)
        {
            if (dto.ImagenesUrls?.Any(u => !EsUrlDeImagenValida(u)) == true)
                return UrlDeImagenInvalida(nameof(dto.ImagenesUrls));

            var reporte = await _reporteService.CrearAsync(dto, User.GetUserId());
            return CreatedAtAction(nameof(ObtenerPorId), new { id = reporte.Id }, reporte);
        }

        /* GET api/reportes?estado=Pendiente | ?categoriaId=3 | ?sinAsignar=true
         Se aplica UN filtro por vez (prioridad: estado > categoriaId > sinAsignar).
         Sin filtros devuelve todos */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReporteDTO>>> Listar(
            [FromQuery] Estado? estado,
            [FromQuery] int? categoriaId,
            [FromQuery] bool sinAsignar = false)
        {
            IEnumerable<ReporteDTO> reportes;

            if (estado.HasValue)
                reportes = await _reporteService.GetByEstadoAsync(estado.Value);
            else if (categoriaId.HasValue)
                reportes = await _reporteService.GetByCategoriaAsync(categoriaId.Value);
            else if (sinAsignar)
                reportes = await _reporteService.GetSinAsignarAsync();
            else
                reportes = await _reporteService.GetAllAsync();

            return Ok(reportes);
        }

        // GET api/reportes/mios — los reportes que creó el usuario logueado
        [HttpGet("mios")]
        public async Task<ActionResult<IEnumerable<ReporteDTO>>> ObtenerMios()
        {
            return Ok(await _reporteService.GetByUsuarioAsync(User.GetUserId()));
        }

        // GET api/reportes/asignados — los reportes asignados al empleado logueado
        [HttpGet("asignados")]
        [Authorize(Roles = AppRoles.AdminOEmpleado)]
        public async Task<ActionResult<IEnumerable<ReporteDTO>>> ObtenerAsignados()
        {
            return Ok(await _reporteService.GetByEmpleadoAsync(User.GetUserId()));
        }

        // GET api/reportes/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReporteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReporteDTO>> ObtenerPorId(int id)
        {
            return Ok(await ObtenerOFallarAsync(id));
        }

        // GET api/reportes/5/historial
        [HttpGet("{id:int}/historial")]
        public async Task<ActionResult<IEnumerable<HistorialEstadoDTO>>> ObtenerHistorial(int id)
        {
            return Ok(await _reporteService.GetHistorialAsync(id));
        }

        // PUT api/reportes/5/asignacion — solo Admin asigna empleados
        [HttpPut("{id:int}/asignacion")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<ActionResult<ReporteDTO>> AsignarEmpleado(int id, [FromBody] AsignarEmpleadoDTO dto)
        {
            var reporte = await _reporteService.AsignarEmpleadoAsync(id, dto, User.GetUserId());
            return Ok(reporte);
        }

        // PATCH api/reportes/5/estado
        // Admin: cualquier reporte. Empleado: solo los que tiene asignados.
        [HttpPatch("{id:int}/estado")]
        [Authorize(Roles = AppRoles.AdminOEmpleado)]
        [ProducesResponseType(typeof(ReporteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ReporteDTO>> CambiarEstado(int id, [FromBody] CambiarEstadoReporteDTO dto)
        {
            var usuarioId = User.GetUserId();

            if (User.IsInRole(AppRoles.Empleado))
            {
                var reporte = await ObtenerOFallarAsync(id);
                if (reporte.EmpleadoEncargadoId != usuarioId)
                    return Forbid();
            }

            return Ok(await _reporteService.CambiarEstadoAsync(id, dto, usuarioId));
        }

        // POST api/reportes/5/imagenes — el dueño del reporte, o un Empleado/Admin
        [HttpPost("{id:int}/imagenes")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AgregarImagen(int id, [FromBody] AgregarImagenDTO dto)
        {
            if (!EsUrlDeImagenValida(dto.Url))
                return UrlDeImagenInvalida(nameof(dto.Url));

            var reporte = await ObtenerOFallarAsync(id);

            var esDueno = reporte.UsuarioId == User.GetUserId();
            var esPersonal = User.IsInRole(AppRoles.Admin) || User.IsInRole(AppRoles.Empleado);
            if (!esDueno && !esPersonal)
                return Forbid();

            await _reporteService.AgregarImagenAsync(id, dto.Url);
            return NoContent();
        }

        // ---------------------------------------------------------------
        // Helpers privados
        // ---------------------------------------------------------------

        private async Task<ReporteDTO> ObtenerOFallarAsync(int id)
        {
            return await _reporteService.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Reporte con ID {id} no encontrado.");
        }

        // Solo http/https y hasta 255 caracteres (límite de la columna Imagen.Url).
        private static bool EsUrlDeImagenValida(string? url) =>
            !string.IsNullOrWhiteSpace(url)
            && url.Length <= 255
            && Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

        private ActionResult UrlDeImagenInvalida(string campo)
        {
            ModelState.AddModelError(campo, "Las imágenes deben ser URLs http/https de hasta 255 caracteres.");
            return ValidationProblem(ModelState);
        }
    }
}