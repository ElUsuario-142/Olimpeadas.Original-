using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Aplication.DTOs.Municipio
{
    public class CrearMunicipioDTO
    {
        public required string Nombre { get; set; }
        public Localidad Localidad { get; set; }
    }
}
