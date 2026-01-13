namespace TemplateJwtProject.Models.DTOs
{
    public class Songsdto
    {
        public int SongId { get; set; }
        public string Titel { get; set; }
        public string ArtistName { get; set; }
        public string Lyrics { get; set; }
        public int? ReleaseYear { get; set; }
        public string ArtistPhoto { get; set; }
        public string ArtistBiography { get; set; }
        public List<ChartDto> ChartHistory { get; set; } = new();
    }

}
