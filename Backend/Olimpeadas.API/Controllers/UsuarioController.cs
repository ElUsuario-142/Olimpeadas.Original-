using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Olimpeadas.API.Extensions;
using Olimpeadas.Aplication.DTOs.Usuario;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.API.Controllers
{
	/* <summary>
	 Gestión de usuarios, Casi todo es exclusivo de Admin: UsuarioDTO expone DNI, teléfono y email.
	 La única excepción es PUT api/usuarios/me (editar el propio perfil).
	 [Authorize(Roles=...)] a nivel método se SUMA al [Authorize] de la clase, no lo remplazamos,
	 por eso el rol Admin se pide método por método y no en la clase
	 </summary>*/
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class UsuariosController : ControllerBase
	{
		private readonly IUsuarioService _usuarioService;

		public UsuariosController(IUsuarioService usuarioService)
		{
			_usuarioService = usuarioService;
		}

		// POST api/usuarios — el Admin crea un usuario con el rol que elija (así se dan de alta los Empleados)
		[HttpPost]
		[Authorize(Roles = AppRoles.Admin)]
		[ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status201Created)]
		public async Task<ActionResult<UsuarioDTO>> Crear([FromBody] CrearUsuarioDTO dto)
		{
			var usuario = await _usuarioService.CrearAsync(dto);
			return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.Id }, usuario);
		}

		/* GET api/usuarios?rol=Empleado 
		 *  ?municipioId=1 
		 *  ?soloActivos=true
		 Un filtro por vez (prioridad: rol > municipioId > soloActivos). Sin filtros, devuelve todos.
		 El front lo tiene que usar para llenar el selector "asignar empleado" por ejemplo (?rol=Empleado).*/
		[HttpGet]
		[Authorize(Roles = AppRoles.Admin)]
		public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Listar(
			[FromQuery] Rol? rol,
			[FromQuery] int? municipioId,
			[FromQuery] bool soloActivos = false)
		{
			IEnumerable<UsuarioDTO> usuarios;

			if (rol.HasValue)
				usuarios = await _usuarioService.GetByRolAsync(rol.Value);
			else if (municipioId.HasValue)
				usuarios = await _usuarioService.GetByMunicipioAsync(municipioId.Value);
			else if (soloActivos)
				usuarios = await _usuarioService.GetActivosAsync();
			else
				usuarios = await _usuarioService.GetAllAsync();

			return Ok(usuarios);
		}

		// GET api/usuarios/5
		[HttpGet("{id:int}")]
		[Authorize(Roles = AppRoles.Admin)]
		[ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<UsuarioDTO>> ObtenerPorId(int id)
		{
			var usuario = await _usuarioService.GetByIdAsync(id)
				?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
			return Ok(usuario);
		}

		/* PUT api/usuarios/me — cualquier usuario logueado edita SU PROPIO perfil
	       Se arma un ActualizarUsuarioDTO copiando solo los 4 campos seguros: Rol, Activo y
		   MunicipioId quedan en null, así que el servicio no los toca aunque el cliente los mande.*/

		[HttpPut("me")]
		public async Task<ActionResult<UsuarioDTO>> ActualizarMiPerfil([FromBody] ActualizarPerfilDTO dto)
		{
			var cambios = new ActualizarUsuarioDTO
			{
				Nombre = dto.Nombre,
				Apellido = dto.Apellido,
				Telefono = dto.Telefono,
				Email = dto.Email
			};

			return Ok(await _usuarioService.UpdateAsync(User.GetUserId(), cambios));
		}

		// PUT api/usuarios/5 — el Admin puede cambiar rol, activo y municipio de cualquiera
		[HttpPut("{id:int}")]
		[Authorize(Roles = AppRoles.Admin)]
		public async Task<ActionResult<UsuarioDTO>> Actualizar(int id, [FromBody] ActualizarUsuarioDTO dto)
		{
			// Evita que el único Admin se quite el permiso o se desactive y deje el sistema sin administracion por que si no tecnicamente se rompe el sistema 
			var seQuitaAdmin = dto.Rol.HasValue && dto.Rol.Value != Rol.Admin;
			var seDesactiva = dto.Activo == false;
			if (id == User.GetUserId() && (seQuitaAdmin || seDesactiva))
				throw new InvalidOperationException("Un administrador no puede quitarse su propio rol ni desactivarse a sí mismo.");

			return Ok(await _usuarioService.UpdateAsync(id, dto));
		}

		// PATCH api/usuarios/5/activo — body: { "activo": false }
		[HttpPatch("{id:int}/activo")]
		[Authorize(Roles = AppRoles.Admin)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> CambiarActivo(int id, [FromBody] CambiarActivoDTO dto)
		{
			if (id == User.GetUserId() && !dto.Activo)
				throw new InvalidOperationException("Un administrador no puede desactivarse a sí mismo.");

			await _usuarioService.CambiarEstadoActivoAsync(id, dto.Activo);
			return NoContent();
		}
	}
}
