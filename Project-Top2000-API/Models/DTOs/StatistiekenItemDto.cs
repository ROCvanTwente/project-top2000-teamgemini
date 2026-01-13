namespace TemplateJwtProject.Models.DTOs
{
    // Basis klasse met velden die (bijna) iedereen heeft
    public class BaseStatistiekDto
    {
        public string Titel { get; set; } = string.Empty;
        public string Artiest { get; set; } = string.Empty;
        public int? Uitgiftejaar { get; set; }
    }

    // Voor lijsten die gewoon een positie hebben (Nieuw, Stabiel, etc.)
    public class StatistiekMetPositieDto : BaseStatistiekDto
    {
        public int Positie { get; set; }
    }

    // Specifiek voor Dalers (heeft PlaatsenGedaald & VorigJaar extra)
    public class DalerDto : StatistiekMetPositieDto
    {
        public int VorigJaar { get; set; }
        public int PlaatsenGedaald { get; set; }
    }

    // Specifiek voor Stijgers (heeft PlaatsenGestegen & VorigJaar extra)
    public class StijgerDto : StatistiekMetPositieDto
    {
        public int VorigJaar { get; set; }
        public int PlaatsenGestegen { get; set; }
    }

    // Specifiek voor Verdwenen (heeft PositieVorigJaar in plaats van Positie)
    public class VerdwenenDto : BaseStatistiekDto
    {
        public int PositieVorigJaar { get; set; }
    }

    // Specifiek voor Eenmalige Noteringen (heeft Top2000Jaar extra)
    public class EenmaligeNoteringDto : BaseStatistiekDto
    {
        public int Positie { get; set; }
        public int Top2000Jaar { get; set; }
    }
}