using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Categoria;
using Olimpeadas.Aplication.Interfaces;

namespace Olimpeadas.API.Controllers
{
    /* <summary>
     Categorías de reportes (bache, alumbrado, residuos,etc)
     Leer es público (el formulario de "nuevo reporte" las necesita), crear es solo de Admin
    </summary>*/
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET api/categorias
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Listar()
        {
            return Ok(await _categoriaService.GetAllAsync());
        }

        // GET api/categorias/3
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDTO>> ObtenerPorId(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Categoría con ID {id} no encontrada.");
            return Ok(categoria);
        }

        // POST api/categorias — solo Admin Si el nombre ya existe, el servicio responde 400
        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<CategoriaDTO>> Crear([FromBody] CrearCategoriaDTO dto)
        {
            var categoria = await _categoriaService.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = categoria.Id }, categoria);
        }
    }
}
