using System;
using System.Collections.Generic;
using System.Text;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Entities
{
    public class Alerta
    {
        public int Id { get; set; }
        public TipoEmergencia Tipo { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoAlerta Estado { get; set; }

        // Relación muchos a muchos mediante UsuarioAlerta (N:N)
        public ICollection<UsuarioAlerta> UsuariosAlertas { get; set; } = new List<UsuarioAlerta>();


    }
}
