using System.ComponentModel.DataAnnotations;

namespace Olimpeadas.Aplication.DTOs.Categoria
{
    public class CrearCategoriaDTO
    {
        //indicamos que 30 como maximo y 3 como mínimo, y un mensaje de error personalizado.
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 30 caracteres.")]
        public required string Nombre { get; set; }
    }
}
