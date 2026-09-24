using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Olimpeadas.Aplication.Interfaces;
using Olimpeadas.Domain.Entities;
using Olimpeadas.Domain.Enums;


namespace Olimpeadas.Infraestructure.Data.Migrations
{
    /* <summary>
    Carga los datos iniciales que el sistema necesita para funcionar
    Cada paso pregunta primero "ya existe?", así que se puede ejecutar en cada
    arranque sin duplicar nada
    </summary>*/
    public class DbSeeder
    {
        private readonly OlimpeadasDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbSeeder> _logger;

        public DbSeeder(
            OlimpeadasDbContext context,
            IPasswordHasher passwordHasher,
            IConfiguration configuration,
            ILogger<DbSeeder> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            // Crea la base y aplica las migraciones pendientes (no hace nada si ya está al día).
            await _context.Database.MigrateAsync();

            await SeedMunicipiosAsync();
            await SeedCategoriasAsync();
            await SeedContactosAsync();
            await SeedAdminAsync();
        }

        private async Task SeedMunicipiosAsync()
        {
            if (await _context.Municipios.AnyAsync()) return;

            _context.Municipios.AddRange(
                new Municipio { Nombre = "Morón", Localidad = Localidad.Moron },
                new Municipio { Nombre = "Castelar", Localidad = Localidad.Castelar },
                new Municipio { Nombre = "El Palomar", Localidad = Localidad.El_Palomar },
                new Municipio { Nombre = "Villa Sarmiento", Localidad = Localidad.Villa_Sarmiento },
                new Municipio { Nombre = "Haedo", Localidad = Localidad.Haedo });

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seed: municipios cargados.");
        }

        private async Task SeedCategoriasAsync()
        {
            if (await _context.Categorias.AnyAsync()) return;

            // Categoria.Nombre admite hasta 30 caracteres.
            _context.Categorias.AddRange(
                new Categoria { Nombre = "Bache" },
                new Categoria { Nombre = "Alumbrado público" },
                new Categoria { Nombre = "Recolección de residuos" },
                new Categoria { Nombre = "Arbolado" },
                new Categoria { Nombre = "Señalización vial" },
                new Categoria { Nombre = "Veredas" },
                new Categoria { Nombre = "Espacios verdes" });

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seed: categorías cargadas.");
        }

        private async Task SeedContactosAsync()
        {
            if (await _context.Contactos.AnyAsync()) return;

            // Números de ejemplo (Argentina). Cámbienlos por los reales del municipio si hace falta.
            var municipiosIds = await _context.Municipios.Select(m => m.Id).ToListAsync();
            foreach (var municipioId in municipiosIds)
            {
                _context.Contactos.AddRange(
                    new Contacto { MunicipioId = municipioId, Tipo = TipoEmergencia.Policia, Numero = "911" },
                    new Contacto { MunicipioId = municipioId, Tipo = TipoEmergencia.Ambulancia, Numero = "107" },
                    new Contacto { MunicipioId = municipioId, Tipo = TipoEmergencia.Bombero, Numero = "100" });
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seed: contactos de emergencia cargados.");
        }

        private async Task SeedAdminAsync()
        {
            if (await _context.Usuarios.AnyAsync(u => u.Rol == Rol.Admin)) return;

            // Las credenciales NO están en el código ni en appsettings.json:
            // salen de user-secrets (local) o de variables de entorno (Docker: Seed__AdminEmail).
            var email = _configuration["Seed:AdminEmail"];
            var password = _configuration["Seed:AdminPassword"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning(
                    "No existe ningún Admin y faltan Seed:AdminEmail / Seed:AdminPassword. No se creó el admin inicial.");
                return;
            }

            var municipioId = await _context.Municipios.OrderBy(m => m.Id).Select(m => m.Id).FirstAsync();

            _context.Usuarios.Add(new Usuario
            {
                Nombre = "Administrador",
                Apellido = "Sistema",
                DNI = _configuration["Seed:AdminDni"] ?? "00000000",
                Telefono = "0000000000",
                Email = email.Trim(),
                ContraseñaHash = _passwordHasher.HashPassword(password),
                Rol = Rol.Admin,
                Activo = true,
                MunicipioId = municipioId
            });

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seed: administrador inicial creado ({Email}).", email.Trim());
        }
    }
}
