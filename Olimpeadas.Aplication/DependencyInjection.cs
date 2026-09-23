using Microsoft.Extensions.DependencyInjection;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Aplication.Services;

namespace Olimpeadas.Aplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
