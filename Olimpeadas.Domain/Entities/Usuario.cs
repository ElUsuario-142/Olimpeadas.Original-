using System.Collections.Generic;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.Domain.Entities
{
    // Las restricciones [unique] de Email y DNI la vamos a declaran en el DbContext
    public class Usuario
    {
        public int Id { get; set; }

        public int MunicipioId { get; set; }

        public required string Nombre { get; set; }

        public required string Apellido { get; set; }

        public required string DNI { get; set; }

        public required string Telefono { get; set; }

        public required string Email { get; set; }

        /* Nunca se guarda la contraseña en texto plano aca,
        Esto almacena el resultado de BCrypt.HashPassword(contraseñaPlana),
        calculado en la capa de servicios ANTES de asignar esta propiedad, despues 
        Para el login usariamos BCrypt.Verify(contraseñaIngresada, ContraseñaHash),
        nunca se "desencripta" este valor para no comprometer la seguridad del sistema/usuario.*/
        public required string ContraseñaHash { get; set; }

        public Rol Rol { get; set; }

        public bool Activo { get; set; }

        // Relación con Municipio
        public Municipio Municipio { get; set; } = null!;

        // Reportes creados por este usuario
        public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

        // Reportes asignados a este usuario como empleado
        public ICollection<Reporte> ReportesAsignados { get; set; } = new List<Reporte>();

        // Cambios de estado realizados por este usuario
        public ICollection<HistorialEstado> HistorialesEstado { get; set; } = new List<HistorialEstado>();

        // Relación muchos a muchos con Alerta
        public ICollection<UsuarioAlerta> UsuariosAlertas { get; set; } = new List<UsuarioAlerta>();
    }
}
