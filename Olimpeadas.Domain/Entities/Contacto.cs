using System;
using System.Collections.Generic;
using System.Text;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Entities
{
    public class Contacto
    {
        public int Id { get; set; }
        public int MunicipioId { get; set; }
        public TipoEmergencia Tipo { get; set; }
        public required string Numero { get; set; }

        // Relación
        public Municipio Municipio { get; set; } = null!;

    }
}
