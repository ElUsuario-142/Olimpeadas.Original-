namespace Olimpeadas.Aplication.DTOs.Noticia
{
    public class CrearNoticiaDTO
    {
        public int MunicipioId { get; set; }
        public required string Titulo { get; set; }
        public string? Contenido { get; set; }
        public List<int>? ReportesIds { get; set; }
    }

    public class VincularReporteDTO
    {
        public int ReporteId { get; set; }
        public string? Comentario { get; set; }
    }
}
