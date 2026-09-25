using Olimpeadas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Olimpeadas.Aplication.DTOs.Alerta
{
    public class CrearAlertaDTO
    {
        //usuarioId no es necesario, se obtiene del JWT
        public required TipoEmergencia Tipo { get; set; }

        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90 grados.")]
        public required decimal Latitud { get; set; }

        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180 grados.")]
        public required decimal Longitud { get; set; }
        //public UsuarioId lo maneja JWT, no es necesario enviarlo desde el cliente
    }
}
