namespace Olimpeadas.Domain.Entities
{
    // La restricción de unicidad (Usuario, Alerta) tambien las vamos a declarar en el DbContext
    
    public class UsuarioAlerta
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public int AlertaId { get; set; }

        // Relaciones
        public Usuario Usuario { get; set; } = null!;
        public Alerta Alerta { get; set; } = null!;
    }
}
