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
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IReporteService, ReporteService>();
            services.AddScoped<IAlertaService, AlertaService>();
            services.AddScoped<INoticiaService, NoticiaService>();
            services.AddScoped<IMunicipioService, MunicipioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();

            return services;
        }
    }
}
