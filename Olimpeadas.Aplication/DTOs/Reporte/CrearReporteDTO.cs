namespace Olimpeadas.Aplication.DTOs.Reporte
{
    public class CrearReporteDTO
    {
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }
        public string? Calle { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public List<string>? ImagenesUrls { get; set; }
    }
}
