using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TemplateJwtProject.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; } = null!;
        public string? Wiki { get; set; }
        public string? Biography { get; set; }
        public string? Photo { get; set; }

        [NotMapped]
        public string? SpotifyLink { get; set; }

        [NotMapped]
        public string? YoutubeLink { get; set; }



        // Navigatie-eigenschap (EF Core)
        public ICollection<Songs> Songs { get; set; } = new List<Songs>();
    }
}
