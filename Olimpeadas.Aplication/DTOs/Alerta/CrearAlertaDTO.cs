using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Alerta
{
    public class CrearAlertaDTO
    {
        public TipoEmergencia Tipo { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        //public UsuarioId lo maneja JWT, no es necesario enviarlo desde el cliente
    }
}
