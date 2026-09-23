using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Municipio
{
    public class MunicipioDTO
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public Localidad Localidad { get; set; }
    }
}
