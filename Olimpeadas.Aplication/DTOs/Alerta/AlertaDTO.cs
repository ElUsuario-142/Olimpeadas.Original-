using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Alerta
{
    public class AlertaDTO
    {
        public int Id { get; set; }
        public TipoEmergencia Tipo { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoAlerta Estado { get; set; }
    }
}
