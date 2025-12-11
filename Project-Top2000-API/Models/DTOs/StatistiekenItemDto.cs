namespace TemplateJwtProject.Models.DTOs
{
    public class StatistiekItemDto
    {
        public int Positie { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Artiest { get; set; } = string.Empty;
        public int? Uitgiftejaar { get; set; }
        public int? VorigJaar { get; set; }
        public int? PlaatsenGedaald { get; set; }
        public int? PlaatsenGestegen { get; set; }
        public int? Top2000Jaar { get; set; }
    }
}