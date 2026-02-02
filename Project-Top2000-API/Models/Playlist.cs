using System.ComponentModel.DataAnnotations;

namespace TemplateJwtProject.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; }

        public ICollection<PlaylistSong> PlayListSongs { get; set; }
            = new List<PlaylistSong>();
    }


}
