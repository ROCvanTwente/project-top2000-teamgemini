namespace TemplateJwtProject.Models.DTOs
{
    public class ArtiestStatistiekDto
    {
        public string Naam { get; set; } = string.Empty;
        public int AantalLiedjes { get; set; }
        public int? GemiddeldePositie { get; set; }
        public int? HoogsteNotering { get; set; }
    }
}