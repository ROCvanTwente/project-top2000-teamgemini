namespace TemplateJwtProject.Models.DTOs
{
    public class PlaylistResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PlaylistSongDto> Songs { get; set; } = new();
    }

    public class PlaylistSongDto
    {
        public int SongId { get; set; }
        public string Titel { get; set; }
        public string Artist { get; set; }
    }
}