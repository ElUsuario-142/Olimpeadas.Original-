using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Noticia;
using Olimpeadas.Aplication.Interfaces;

namespace Olimpeadas.API.Controllers
{
    /* <summary>
    Noticias del municipio y su vínculo con reportes
    "Registro" en este proyecto es el VÍNCULO Noticia-Reporte (con fecha y comentario),
    no el historial de cambios de un reporte. Por eso vincular/desvincular vive acá, y el historial de
    estados está en GET api/reportes/{id}/historial. No hace falta un RegistrosController aparte
    Leer es público; crear y vincular es de Empleado o Admin.
    </summary>*/
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly INoticiaService _noticiaService;

        public NoticiasController(INoticiaService noticiaService)
        {
            _noticiaService = noticiaService;
        }

        // GET api/noticias  |  GET api/noticias?municipioId=1
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NoticiaDTO>>> Listar([FromQuery] int? municipioId)
        {
            var noticias = municipioId.HasValue
                ? await _noticiaService.GetByMunicipioAsync(municipioId.Value)
                : await _noticiaService.GetAllAsync();

            return Ok(noticias);
        }

        // GET api/noticias/5 — incluye los reportes vinculados
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(NoticiaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoticiaDTO>> ObtenerPorId(int id)
        {
            var noticia = await _noticiaService.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Noticia con ID {id} no encontrada.");
            return Ok(noticia);
        }

        // POST api/noticias — Empleado o Admin. Puede traer "reportesIds" para vincular al crear
        [HttpPost]
        [Authorize(Roles = AppRoles.AdminOEmpleado)]
        [ProducesResponseType(typeof(NoticiaDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<NoticiaDTO>> Crear([FromBody] CrearNoticiaDTO dto)
        {
            var noticia = await _noticiaService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = noticia.Id }, noticia);
        }

        /*POST api/noticias/5/reportes — body: { "reporteId": 12, "comentario": "..." }
        Si el reporte ya estaba vinculado, el servicio responde 400*/
        [HttpPost("{id:int}/reportes")]
        [Authorize(Roles = AppRoles.AdminOEmpleado)]
        [ProducesResponseType(typeof(RegistroDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<RegistroDTO>> VincularReporte(int id, [FromBody] VincularReporteDTO dto)
        {
            var registro = await _noticiaService.VincularReporteAsync(id, dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id }, registro);
        }

        // DELETE api/noticias/5/reportes/12
        [HttpDelete("{id:int}/reportes/{reporteId:int}")]
        [Authorize(Roles = AppRoles.AdminOEmpleado)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DesvincularReporte(int id, int reporteId)
        {
            await _noticiaService.DesvincularReporteAsync(id, reporteId);
            return NoContent();
        }
    }
}