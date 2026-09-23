using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Municipio
{
    public class CrearContactoDTO
    {
        public int MunicipioId { get; set; }
        public TipoEmergencia Tipo { get; set; }
        public required string Numero { get; set; }
    }
}
