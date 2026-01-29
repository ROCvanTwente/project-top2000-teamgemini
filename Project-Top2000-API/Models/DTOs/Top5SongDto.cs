namespace TemplateJwtProject.Models.DTOs
{
    public class Top5SongDto
    {
        public int SongId { get; set; }
        public int Position { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int? ReleaseYear { get; set; }
    }
}