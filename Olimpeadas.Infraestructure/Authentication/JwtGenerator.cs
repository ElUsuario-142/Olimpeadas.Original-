using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Infraestructure.Authentication
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime Expiration) GenerateToken(Usuario usuario)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"] 
                ?? throw new InvalidOperationException("JwtSettings:SecretKey no está configurado.");
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var durationInMinutes = int.Parse(_configuration["JwtSettings:DurationInMinutes"] ?? "120");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new(ClaimTypes.Email, usuario.Email),
                new(ClaimTypes.Role, usuario.Rol.ToString()),
                new("municipioId", usuario.MunicipioId.ToString()),
                new("dni", usuario.DNI)
            };

            var expiration = DateTime.UtcNow.AddMinutes(durationInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return (tokenString, expiration);
        }
    }
}
