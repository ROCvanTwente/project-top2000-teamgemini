namespace TemplateJwtProject.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? UserId { get; set; }       // (?) omdat we nog geen gebruikers hebben
        public ApplicationUser? User { get; set; }

        public ICollection<PlaylistSong> PlayListSongs { get; set; }
            = new List<PlaylistSong>();
    }


}
