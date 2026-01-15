namespace TemplateJwtProject.Models.DTOs
{
    public class UpdateSongDto
    {
        public int SongId { get; set; }
        public int? ArtistId { get; set; }
        public string? Titel { get; set; }
        public int? ReleaseYear { get; set; }
        public string? ImgUrl { get; set; }
        public string? Lyrics { get; set; }
        public string? Youtube { get; set; }
    }

}
