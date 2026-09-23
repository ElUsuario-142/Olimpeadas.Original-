using Microsoft.EntityFrameworkCore;
using Olimpeadas.Domain.Entities;

namespace Olimpeadas.Infraestructure.Data
{
    public class OlimpeadasDbContext : DbContext
    {
        public OlimpeadasDbContext(DbContextOptions<OlimpeadasDbContext> options)
            : base(options)
        {
        }

        // Un DbSet por cada tabla. Esto es lo que usás para consultar/insertar/etc.
        public DbSet<Municipio> Municipios => Set<Municipio>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Direccion> Direcciones => Set<Direccion>();
        public DbSet<Reporte> Reportes => Set<Reporte>();
        public DbSet<Imagen> Imagenes => Set<Imagen>();
        public DbSet<HistorialEstado> HistorialesEstado => Set<HistorialEstado>();
        public DbSet<Noticia> Noticias => Set<Noticia>();
        public DbSet<Registro> Registros => Set<Registro>();
        public DbSet<Contacto> Contactos => Set<Contacto>();
        public DbSet<Alerta> Alertas => Set<Alerta>();
        public DbSet<UsuarioAlerta> UsuariosAlertas => Set<UsuarioAlerta>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----------------------------------------------------------------
            // MUNICIPIO
            // ----------------------------------------------------------------
            modelBuilder.Entity<Municipio>(e =>
            {
                e.Property(m => m.Nombre).HasMaxLength(50);

                // Enum guardado como texto ("Moron", "Castelar"...) en vez de un
                // número (0, 1, 2...) para que la tabla se entienda a simple vista.
                e.Property(m => m.Localidad).HasConversion<string>().HasMaxLength(30);
            });

            // ----------------------------------------------------------------
            // CATEGORIA
            // ----------------------------------------------------------------
            modelBuilder.Entity<Categoria>(e =>
            {
                e.Property(c => c.Nombre).HasMaxLength(30);
            });

            // ----------------------------------------------------------------
            // USUARIO
            // ----------------------------------------------------------------
            modelBuilder.Entity<Usuario>(e =>
            {
                e.Property(u => u.Nombre).HasMaxLength(20);
                e.Property(u => u.Apellido).HasMaxLength(20);
                e.Property(u => u.DNI).HasMaxLength(14);
                e.Property(u => u.Telefono).HasMaxLength(14);
                e.Property(u => u.Email).HasMaxLength(255);
                e.Property(u => u.ContraseñaHash).HasMaxLength(255);
                e.Property(u => u.Rol).HasConversion<string>().HasMaxLength(20);

                // Equivalente a [unique] del DBML: no puede haber dos filas
                // con el mismo Email o el mismo DNI.
                e.HasIndex(u => u.Email).IsUnique();
                e.HasIndex(u => u.DNI).IsUnique();

                e.HasOne(u => u.Municipio)
                 .WithMany(m => m.Usuarios)
                 .HasForeignKey(u => u.MunicipioId)
                 .OnDelete(DeleteBehavior.Restrict); // no borres municipios con usuarios
            });

            // ----------------------------------------------------------------
            // DIRECCION
            // ----------------------------------------------------------------
            modelBuilder.Entity<Direccion>(e =>
            {
                e.Property(d => d.Calle).HasMaxLength(100);

                // decimal(9,6): hasta 3 dígitos enteros + 6 decimales,
                // precisión suficiente para coordenadas GPS (~11cm de error).
                e.Property(d => d.Latitud).HasColumnType("decimal(9,6)");
                e.Property(d => d.Longitud).HasColumnType("decimal(9,6)");
            });

            // ----------------------------------------------------------------
            // REPORTE — la entidad con más relaciones, la que necesita más ayuda
            // ----------------------------------------------------------------
            modelBuilder.Entity<Reporte>(e =>
            {
                e.Property(r => r.Titulo).HasMaxLength(50);
                e.Property(r => r.Estado).HasConversion<string>().HasMaxLength(20);

                e.HasOne(r => r.Categoria)
                 .WithMany(c => c.Reportes)
                 .HasForeignKey(r => r.CategoriaId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Direccion)
                 .WithMany(d => d.Reportes)
                 .HasForeignKey(r => r.DireccionId)
                 .OnDelete(DeleteBehavior.Restrict);

                // FK #1 a Usuario: quién CREÓ el reporte (obligatorio)
                e.HasOne(r => r.Usuario)
                 .WithMany(u => u.Reportes)
                 .HasForeignKey(r => r.UsuarioId)
                 .OnDelete(DeleteBehavior.Restrict);

                // FK #2 a Usuario: el EMPLEADO ENCARGADO (opcional).
                // Como ya hay una relación Usuario->Reporte de arriba, EF Core
                // no puede adivinar cuál FK va con cuál navegación sin que se
                // lo digamos de forma explícita, así que acá se lo aclaramos.
                e.HasOne(r => r.EmpleadoEncargado)
                 .WithMany(u => u.ReportesAsignados)
                 .HasForeignKey(r => r.EmpleadoEncargadoId)
                 .OnDelete(DeleteBehavior.Restrict) // evita "multiple cascade paths"
                 .IsRequired(false);
            });

            // ----------------------------------------------------------------
            // IMAGEN
            // ----------------------------------------------------------------
            modelBuilder.Entity<Imagen>(e =>
            {
                e.Property(i => i.Url).HasMaxLength(255);

                e.HasOne(i => i.Reporte)
                 .WithMany(r => r.Imagenes)
                 .HasForeignKey(i => i.ReporteId)
                 .OnDelete(DeleteBehavior.Cascade); // si se borra el reporte, se borran sus fotos
            });

            // ----------------------------------------------------------------
            // HISTORIAL DE ESTADO
            // ----------------------------------------------------------------
            modelBuilder.Entity<HistorialEstado>(e =>
            {
                e.Property(h => h.EstadoAnterior).HasConversion<string>().HasMaxLength(20);
                e.Property(h => h.EstadoNuevo).HasConversion<string>().HasMaxLength(20);

                e.HasOne(h => h.Reporte)
                 .WithMany(r => r.HistorialesEstado)
                 .HasForeignKey(h => h.ReporteId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(h => h.Usuario)
                 .WithMany(u => u.HistorialesEstado)
                 .HasForeignKey(h => h.UsuarioId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ----------------------------------------------------------------
            // NOTICIA
            // ----------------------------------------------------------------
            modelBuilder.Entity<Noticia>(e =>
            {
                e.Property(n => n.Titulo).HasMaxLength(200);

                e.HasOne(n => n.Municipio)
                 .WithMany(m => m.Noticias)
                 .HasForeignKey(n => n.MunicipioId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ----------------------------------------------------------------
            // REGISTRO — tabla intermedia Reporte <-> Noticia (N:M)
            // ----------------------------------------------------------------
            modelBuilder.Entity<Registro>(e =>
            {
                e.HasOne(r => r.Reporte)
                 .WithMany(rep => rep.Registros)
                 .HasForeignKey(r => r.ReporteId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(r => r.Noticia)
                 .WithMany(n => n.Registros)
                 .HasForeignKey(r => r.NoticiaId)
                 .OnDelete(DeleteBehavior.Cascade);

                // No permite vincular dos veces el mismo Reporte con la misma Noticia
                e.HasIndex(r => new { r.ReporteId, r.NoticiaId }).IsUnique();
            });

            // ----------------------------------------------------------------
            // CONTACTO
            // ----------------------------------------------------------------
            modelBuilder.Entity<Contacto>(e =>
            {
                e.Property(c => c.Numero).HasMaxLength(20);
                e.Property(c => c.Tipo).HasConversion<string>().HasMaxLength(20);

                e.HasOne(c => c.Municipio)
                 .WithMany(m => m.Contactos)
                 .HasForeignKey(c => c.MunicipioId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ----------------------------------------------------------------
            // ALERTA
            // ----------------------------------------------------------------
            modelBuilder.Entity<Alerta>(e =>
            {
                e.Property(a => a.Tipo).HasConversion<string>().HasMaxLength(20);
                e.Property(a => a.Estado).HasConversion<string>().HasMaxLength(20);
                e.Property(a => a.Latitud).HasColumnType("decimal(9,6)");
                e.Property(a => a.Longitud).HasColumnType("decimal(9,6)");
            });

            // ----------------------------------------------------------------
            // USUARIOALERTA — tabla intermedia Usuario <-> Alerta (N:M)
            // ----------------------------------------------------------------
            modelBuilder.Entity<UsuarioAlerta>(e =>
            {
                e.HasOne(ua => ua.Usuario)
                 .WithMany(u => u.UsuariosAlertas)
                 .HasForeignKey(ua => ua.UsuarioId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ua => ua.Alerta)
                 .WithMany(a => a.UsuariosAlertas)
                 .HasForeignKey(ua => ua.AlertaId)
                 .OnDelete(DeleteBehavior.Cascade);

                // No permite vincular dos veces el mismo Usuario con la misma Alerta
                e.HasIndex(ua => new { ua.UsuarioId, ua.AlertaId }).IsUnique();
            });
        }
    }
}
