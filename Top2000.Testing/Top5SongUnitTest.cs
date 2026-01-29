using TemplateJwtProject.Models.DTOs;
using Xunit;

namespace TemplateJwtProject.Tests
{
    public class Top5SongUnitTest
    {
        [Fact]
        public void Top5SongDto_Should_Set_Properties_Correctly()
        {
            var dto = new Top5SongDto
            {
                SongId = 1,
                Position = 1,
                Title = "Bohemian Rhapsody",
                Artist = "Queen",
                ReleaseYear = 1975
            };

            Assert.Equal(1, dto.SongId);
            Assert.Equal(1, dto.Position);
            Assert.Equal("Bohemian Rhapsody", dto.Title);
            Assert.Equal("Queen", dto.Artist);
            Assert.Equal(1975, dto.ReleaseYear);
        }

        [Fact]
        public void Top5SongDto_ReleaseYear_Can_Be_Null()
        {
            var dto = new Top5SongDto
            {
                SongId = 2,
                Position = 2,
                Title = "Unknown Song",
                Artist = "Unknown Artist",
                ReleaseYear = null
            };

            Assert.Null(dto.ReleaseYear);
        }
    }
}
