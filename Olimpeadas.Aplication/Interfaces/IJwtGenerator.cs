using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Aplication.Interfaces
{
    public interface IJwtGenerator
    {
        (string Token, DateTime Expiration) GenerateToken(Usuario usuario);
    }
}
