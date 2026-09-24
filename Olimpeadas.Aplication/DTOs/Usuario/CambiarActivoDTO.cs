namespace Olimpeadas.Aplication.DTOs.Usuario
{
    public class CambiarActivoDTO
    {
        // "required": si el body no trae el campo, devuelve 400 en vez de asumir false
        // (que desactivaría al usuario sin querer).
        public required bool Activo { get; set; }
    }
}