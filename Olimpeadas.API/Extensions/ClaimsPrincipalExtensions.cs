using System.Security.Claims;
using Olimpeadas.Domain.Enums;

namespace Olimpeadas.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /* <summary>
         Id del usuario logueado, leído del JWT (claim NameIdentifier)
         Los controllers usan esto en vez de recibir el usuario por body.
         </summary>*/
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(value, out var id))
                throw new UnauthorizedAccessException("El token no contiene un identificador de usuario válido.");
            return id;
        }
    }

    /* <summary>
     Nombres de rol para usar en [Authorize(Roles = ...)]. Salen de nameof(Rol.X),
     que es lo mismo que JwtGenerator escribe en el claim (Rol.ToString()),
     así no hay strings "mágicos" que se puedan desincronizar.
     </summary> */
    public static class AppRoles
    {
        public const string Ciudadano = nameof(Rol.Ciudadano);
        public const string Empleado = nameof(Rol.Empleado);
        public const string Admin = nameof(Rol.Admin);
        public const string AdminOEmpleado = Admin + "," + Empleado;
    }
}
