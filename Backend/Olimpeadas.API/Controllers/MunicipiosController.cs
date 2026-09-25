using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Municipio;
using Olimpeadas.Aplication.Interfaces;

namespace Olimpeadas.API.Controllers
{
    /* <summary>
    Municipios / localidades
    Leer es PÚBLICO a propósito, ya que el formulario de registro necesita el selector de municipio
    antes de que el usuario tenga cuenta (y por lo tanto token). Crear es solo de Admin
    </summary>*/
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipiosController : ControllerBase
    {
        private readonly IMunicipioService _municipioService;

        public MunicipiosController(IMunicipioService municipioService)
        {
            _municipioService = municipioService;
        }

        // GET api/municipios
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MunicipioDTO>>> Listar()
        {
            return Ok(await _municipioService.GetAllMunicipiosAsync());
        }

        // GET api/municipios/1
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MunicipioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MunicipioDTO>> ObtenerPorId(int id)
        {
            var municipio = await _municipioService.GetMunicipioByIdAsync(id)
                ?? throw new KeyNotFoundException($"Municipio con ID {id} no encontrado.");
            return Ok(municipio);
        }

        // POST api/municipios — solo Admin
        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        [ProducesResponseType(typeof(MunicipioDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<MunicipioDTO>> Crear([FromBody] CrearMunicipioDTO dto)
        {
            var municipio = await _municipioService.CrearMunicipioAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = municipio.Id }, municipio);
        }
    }
}
