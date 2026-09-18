using Olimpeadas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Olimpeadas.Domain.Entities
{
    public  class Municipio
    {
        public int Id { get; set; }
        
        public required string Nombre { get; set; }

        public Localidad Localidad { get; set; }

        // Relaciones
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<Noticia> Noticias { get; set; } = new List<Noticia>();
        public ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();

    }
}
