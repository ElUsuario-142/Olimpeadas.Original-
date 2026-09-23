namespace Olimpeadas.Aplication.DTOs.Noticia
{
    public class NoticiaDTO
    {
        public int Id { get; set; }
        public int MunicipioId { get; set; }
        public string? MunicipioNombre { get; set; }
        public required string Titulo { get; set; }
        public string? Contenido { get; set; }
        public DateTime Fecha { get; set; }
        public List<RegistroDTO> RegistrosVinculados { get; set; } = new();
    }

    public class RegistroDTO
    {
        public int Id { get; set; }
        public int ReporteId { get; set; }
        public string? ReporteTitulo { get; set; }
        public int NoticiaId { get; set; }
        public DateTime Fecha { get; set; }
        public string? Comentario { get; set; }
    }
}
