using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olimpeadas.Domain.Interfaces;
using Olimpeadas.Infraestructure.Data;
using Olimpeadas.Infraestructure.Repositories;

namespace Olimpeadas.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<OlimpeadasDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Registro de Repositorio genérico y específicos
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IMunicipioRepository, MunicipioRepository>();
            services.AddScoped<IReporteRepository, ReporteRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IDireccionRepository, DireccionRepository>();
            services.AddScoped<IImagenRepository, ImagenRepository>();
            services.AddScoped<IHistorialEstadoRepository, HistorialEstadoRepository>();
            services.AddScoped<INoticiaRepository, NoticiaRepository>();
            services.AddScoped<IRegistroRepository, RegistroRepository>();
            services.AddScoped<IContactoRepository, ContactoRepository>();
            services.AddScoped<IAlertaRepository, AlertaRepository>();
            services.AddScoped<IUsuarioAlertaRepository, UsuarioAlertaRepository>();

            // Autenticación y Seguridad (BCrypt y JWT)
            services.AddSingleton<Olimpeadas.Aplication.Interfaces.IPasswordHasher, Authentication.BCryptPasswordHasher>();
            services.AddScoped<Olimpeadas.Aplication.Interfaces.IJwtGenerator, Authentication.JwtGenerator>();

            return services;
        }
    }
}
