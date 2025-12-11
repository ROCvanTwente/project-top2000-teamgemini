using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TemplateJwtProject.Models
{
    public class Top2000Entry
    {
        // Composite key: SongId + Year
        [Key]
        [ForeignKey("Songs")]
        public int SongId { get; set; }
        public int Year { get; set; }
        public int Position { get; set; }

        [JsonIgnore]
        public Songs Songs { get; set; } = null!;
    }

}
