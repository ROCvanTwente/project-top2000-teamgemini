using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Models
{
    public class Songs
    {
        [Key]
        public int SongId { get; set; }
        public int ArtistId { get; set; }

        public string Titel { get; set; } = null!;
        public int? ReleaseYear { get; set; }
        public string? ImgUrl { get; set; }
        public string? Lyrics { get; set; }
        public string? Youtube { get; set; }

        public Artist Artist { get; set; } = null!;
        public ICollection<Top2000Entry> Top2000Entries { get; set; }
            = new List<Top2000Entry>();

        public ICollection<PlaylistSong> PlayListSongs { get; set; } = new List<PlaylistSong>();
    }

}