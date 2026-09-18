using System;
using System.Collections.Generic;
using System.Text;

namespace Olimpeadas.Domain.Entities
{
    public class Noticia
    {
        public int Id { get; set; }

        public int MunicipioId { get; set; }

        public required string Titulo { get; set; }

        public string? Contenido { get; set; }

        public DateTime Fecha { get; set; }

        // Relación
        public Municipio Municipio { get; set; } = null!;

        public ICollection<Registro> Registros { get; set; } = new List<Registro>();

    }
}
