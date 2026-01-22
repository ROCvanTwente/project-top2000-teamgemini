namespace TemplateJwtProject.Models
{
    public class PlaylistSong
    {
        public int PlayListId { get; set; }
        public Playlist PlayList { get; set; }

        public int SongId { get; set; }
        public Songs Song { get; set; }

        public DateTime AddedAT { get; set; } = DateTime.UtcNow;
    }

}
