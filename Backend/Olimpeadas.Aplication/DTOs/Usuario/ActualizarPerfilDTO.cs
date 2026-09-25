using System.ComponentModel.DataAnnotations;

namespace Olimpeadas.Aplication.DTOs.Usuario
{
    /* Lo que un usuario puede cambiar de SÍ MISMO. No tiene Rol, Activo ni MunicipioId
    a propósito, por que si estuvieran aca, cualquiera podría hacerse Admin con un PUT y eso seria desastrozo,
    entonces los campos que vienen null no se modifican.*/
    public class ActualizarPerfilDTO
    {
        [StringLength(20, MinimumLength = 2)]
        public string? Nombre { get; set; }

        [StringLength(20, MinimumLength = 2)]
        public string? Apellido { get; set; }

        [StringLength(14, MinimumLength = 6)]
        public string? Telefono { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string? Email { get; set; }
    }
}