using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Municipio;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.API.Controllers
{
    /* <summary>
    Teléfonos de emergencia por municipio. Consultarlos es público (son datos de utilidad
    pública y hay que poder verlos aunque no se haya iniciado sesión), cargarlos es de Admin
    La lógica vive en IMunicipioService, que ya maneja los contactos
    </summary>*/
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IMunicipioService _municipioService;

        public ContactosController(IMunicipioService municipioService)
        {
            _municipioService = municipioService;
        }

        /* GET api/contactos?municipioId=1
        GET api/contactos?tipo=Policia
        Hace falta UN filtro (prioridad: municipioId > tipo): no hay un "listar todos" en el servicio*/
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ContactoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ContactoDTO>>> Listar(
            [FromQuery] int? municipioId,
            [FromQuery] TipoEmergencia? tipo)
        {
            if (municipioId.HasValue)
                return Ok(await _municipioService.GetContactosByMunicipioAsync(municipioId.Value));

            if (tipo.HasValue)
                return Ok(await _municipioService.GetContactosByTipoAsync(tipo.Value));

            throw new InvalidOperationException("Indicá un filtro: ?municipioId=... o ?tipo=Policia|Ambulancia|Bombero.");
        }

        // POST api/contactos — solo Admin
        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        [ProducesResponseType(typeof(ContactoDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<ContactoDTO>> Crear([FromBody] CrearContactoDTO dto)
        {
            var contacto = await _municipioService.CrearContactoAsync(dto);
            return StatusCode(StatusCodes.Status201Created, contacto);
        }
    }
}

