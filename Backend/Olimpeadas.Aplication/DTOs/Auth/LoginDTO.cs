namespace Olimpeadas.Aplication.DTOs.Auth
{
    public class LoginDTO
    {
        public required string Email { get; set; }//unique
        public required string Contraseña { get; set; }
    }
}
