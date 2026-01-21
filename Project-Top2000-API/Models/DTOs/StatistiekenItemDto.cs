using System.Text.Json.Serialization;

namespace TemplateJwtProject.Models.DTOs
{
    // 1. De Basis (Titel, Artiest, Jaar zitten in ELKE query)
    public class BasisStatistiekDto
    {
        public string Titel { get; set; } = string.Empty;
        public string Artiest { get; set; } = string.Empty;
        public int? Uitgiftejaar { get; set; }
    }

    // 2. Voor lijsten die alleen een simpele 'Positie' toevoegen
    // (Gebruikt door: Nieuwe Binnenkomers, Opnieuw Binnen, Stabiel, Aansluitende Posities)
    public class PositieStatistiekDto : BasisStatistiekDto
    {
        public int Positie { get; set; }
    }

    // 3. Specifiek voor Dalers (Heeft 'PlaatsenGedaald' + VorigJaar)
    public class DalerDto : PositieStatistiekDto
    {
        public int VorigJaar { get; set; }
        public int PlaatsenGedaald { get; set; }
    }

    // 4. Specifiek voor Stijgers (Heeft 'PlaatsenGestegen' + VorigJaar)
    public class StijgerDto : PositieStatistiekDto
    {
        public int VorigJaar { get; set; }
        public int PlaatsenGestegen { get; set; }
    }

    // 5. Specifiek voor Verdwenen (Heeft 'PositieVorigJaar' ipv Positie)
    public class VerdwenenDto : BasisStatistiekDto
    {
        public int PositieVorigJaar { get; set; }
    }

    // 6. Specifiek voor Eenmalige Noteringen (Heeft Top2000Jaar)
    public class EenmaligDto : PositieStatistiekDto
    {
        public int Top2000Jaar { get; set; }
    }
}