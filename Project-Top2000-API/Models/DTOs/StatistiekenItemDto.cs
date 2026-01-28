using System.Text.Json.Serialization;

namespace TemplateJwtProject.Models.DTOs
{
    // 1. De Basis (Nu met ID's voor klikbare linkjes!)
    public class BasisStatistiekDto
    {
        public int SongId { get; set; }
        public int ArtistId { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Artiest { get; set; } = string.Empty;
        public int? Uitgiftejaar { get; set; }
    }

    // 2. Voor lijsten die alleen een simpele 'Positie' toevoegen
    public class PositieStatistiekDto : BasisStatistiekDto
    {
        public int Positie { get; set; }
    }

    // 3. Specifiek voor Dalers
    public class DalerDto : PositieStatistiekDto
    {
        public int VorigJaar { get; set; }
        public int PlaatsenGedaald { get; set; }
    }

    // 4. Specifiek voor Stijgers
    public class StijgerDto : PositieStatistiekDto
    {
        public int VorigJaar { get; set; }
        public int PlaatsenGestegen { get; set; }
    }

    // 5. Eenmalige noteringen (heeft ook een Top2000Jaar)
    public class EenmaligDto : BasisStatistiekDto
    {
        public int Positie { get; set; }
        public int Top2000Jaar { get; set; }
    }

    // 6. Verdwenen uit de lijst (Heeft PositieVorigJaar ipv huidige positie)
    public class VerdwenenDto : BasisStatistiekDto
    {
        public int PositieVorigJaar { get; set; }
    }
}