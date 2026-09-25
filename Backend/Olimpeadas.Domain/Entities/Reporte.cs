using Olimpeadas.Domain.Enums;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;// Using para la clase MediaTypeNames, que contiene constantes para tipos MIME comunes(facilita uso de tipos de contenido en la web, como "image/jpeg" o "application/json")

namespace Olimpeadas.Domain.Entities
{
    public class Reporte
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int CategoriaId { get; set; }

        public int DireccionId { get; set; }

        public int? EmpleadoEncargadoId { get; set; } // Nullable hasta que se asigne un empleado

        public required string Titulo { get; set; }

        public required string Descripcion { get; set; }

        public Estado Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        // Usuario que creó el reporte (usualmente un ciudadano)
        public Usuario Usuario { get; set; } = null!;

        // Categoría del reporte (Bache, Alumbrado, etc.)
        public Categoria Categoria { get; set; } = null!;

        // Dirección del reporte
        public Direccion Direccion { get; set; } = null!;

        // Empleado encargado del reporte (null si aún no se asignó)
        public Usuario? EmpleadoEncargado { get; set; }

        // Relaciones 1:N con Imagen, HistorialEstado y Registro
        public ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();
        public ICollection<HistorialEstado> HistorialesEstado { get; set; } = new List<HistorialEstado>();
        public ICollection<Registro> Registros { get; set; } = new List<Registro>();
    }
}
