using System.Text.Json.Serialization;

namespace TemplateJwtProject.Models.DTOs
{
    public class Top2000Entrydto
    {
        public int SongId { get; set; }
        public int Year { get; set; }
        public int Position { get; set; }
        public int? PositionLastYear { get; set; }
        public int? verschil { get; set; }
        public string Titel { get; set; } = null!;
        public int ArtistId { get; set; }
        public string ArtistName { get; set; } = null!;
        public int? ReleaseYear { get; set; }



    }
}
