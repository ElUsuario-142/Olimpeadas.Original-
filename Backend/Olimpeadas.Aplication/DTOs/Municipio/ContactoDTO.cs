using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Municipio
{
    public class ContactoDTO
    {
        public int Id { get; set; }
        public int MunicipioId { get; set; }
        public string? MunicipioNombre { get; set; }
        public TipoEmergencia Tipo { get; set; }
        public required string Numero { get; set; }
    }
}
