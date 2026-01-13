namespace TemplateJwtProject.Models.DTOs
{
    public class StatistiekItemDto
    {
        // Basis velden (zitten in bijna alle resultaten)
        public string Titel { get; set; } = string.Empty;
        public string Artiest { get; set; } = string.Empty;
        public int? Uitgiftejaar { get; set; }

        // Positie (voor o.a. Nieuw, Stabiel, Dalers, Stijgers)
        public int Positie { get; set; }

        // Specifiek voor Dalers & Stijgers
        public int VorigJaar { get; set; }
        public int PlaatsenGedaald { get; set; }
        public int PlaatsenGestegen { get; set; }

        // Specifiek voor Verdwenen (gebruikt vaak PositieVorigJaar ipv Positie)
        public int PositieVorigJaar { get; set; }

        // Specifiek voor Eenmalige Noteringen
        public int Top2000Jaar { get; set; }
    }
}