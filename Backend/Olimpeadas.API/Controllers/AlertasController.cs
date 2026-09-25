using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Alerta;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.API.Controllers
{
    /*<summary>
    Alertas de emergencia, cualquier usuario logueado puede emitir una, verlas todas y
    cambiarles el estado es de Empleado o Admin. Un Ciudadano solo ve las que recibio
    </summary>*/
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertaService _alertaService;

        public AlertasController(IAlertaService alertaService)
        {
            _alertaService = alertaService;
        }

        private bool EsPersonal => User.IsInRole(AppRoles.Admin) || User.IsInRole(AppRoles.Empleado);

        /*UsuariosNotificadosIds trae el id de TODOS los vecinos del municipio a los que se les aviso
         personal le sirve; a un ciudadano no, así que se le oculta*/
        private AlertaDTO OcultarDestinatariosSiCorresponde(AlertaDTO alerta)
        {
            if (!EsPersonal)
                alerta.UsuariosNotificadosIds = new List<int>();
            return alerta;
        }

        // POST api/alertas — el emisor sale del JWT, se avisa a los usuarios activos de su municipio
        [HttpPost]
        [ProducesResponseType(typeof(AlertaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AlertaDTO>> Crear([FromBody] CrearAlertaDTO dto)
        {
            var alerta = await _alertaService.CrearAsync(dto, User.GetUserId());
            return CreatedAtAction(nameof(ObtenerPorId), new { id = alerta.Id }, OcultarDestinatariosSiCorresponde(alerta));
        }

        // GET api/alertas/mias — las alertas que recibio (o emitio) el usuario logueado
        [HttpGet("mias")]
        public async Task<ActionResult<IEnumerable<AlertaDTO>>> ObtenerMias()
        {
            var alertas = await _alertaService.GetAlertasPorUsuarioAsync(User.GetUserId());
            return Ok(alertas.Select(OcultarDestinatariosSiCorresponde));
        }

        /* GET api/alertas?estado=Enviada
        ?tipo=Policia — Empleado o Admin
        Un filtro por vez (prioridad: estado > tipo). Sin filtros devuelve todas.*/
        [HttpGet]
        [Authorize(Roles = AppRoles.AdminOEmpleado)]
        public async Task<ActionResult<IEnumerable<AlertaDTO>>> Listar(
            [FromQuery] EstadoAlerta? estado,
            [FromQuery] TipoEmergencia? tipo)
        {
            IEnumerable<AlertaDTO> alertas;

            if (estado.HasValue)
                alertas = await _alertaService.GetByEstadoAsync(estado.Value);
            else if (tipo.HasValue)
                alertas = await _alertaService.GetByTipoAsync(tipo.Value);
            else
                alertas = await _alertaService.GetAllAsync();

            return Ok(alertas);
        }

        // GET api/alertas/5 — personal, o un usuario al que se le notificó esa alerta
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AlertaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AlertaDTO>> ObtenerPorId(int id)
        {
            var alerta = await _alertaService.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Alerta con ID {id} no encontrada.");

            // Primero se comprueba la pertenencia y recién después se oculta la lista.
            if (!EsPersonal && !alerta.UsuariosNotificadosIds.Contains(User.GetUserId()))
                return Forbid();

            return Ok(OcultarDestinatariosSiCorresponde(alerta));
        }

        // PATCH api/alertas/5/estado — body: { "estado": "Atendida" }. Empleado o Admin.
        // Pendiente: hoy el servicio acepta cualquier cambio de estado y no guarda quién lo hizo.
        [HttpPatch("{id:int}/estado")]
        [Authorize(Roles = AppRoles.AdminOEmpleado)]
        public async Task<ActionResult<AlertaDTO>> CambiarEstado(int id, [FromBody] ActualizarAlertaDTO dto)
        {
            return Ok(await _alertaService.CambiarEstadoAsync(id, dto));
        }
    }
}
