using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Controllers;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;
using Xunit;

namespace Top2000.Testing
{
    public class NummersOverzichtControllerTests
    {
        [Fact]
        public async Task GetNummersOverzicht_ReturnsOk_WithExpectedShape()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using var context = new AppDbContext(options);

            context.Artists.Add(new Artist { ArtistId = 1, Name = "Artist 1" });
            context.Songs.Add(new Songs { SongId = 1, ArtistId = 1, Titel = "Song 1", ReleaseYear = 1999, ImgUrl = "img", Spotify = "sp" });
            context.Top2000Entries.Add(new Top2000Entry { SongId = 1, Year = 2020, Position = 2 });
            context.Top2000Entries.Add(new Top2000Entry { SongId = 1, Year = 2019, Position = 5 });
            context.SaveChanges();

            var controller = new NummersOverzichtController(context);

            var result = await controller.GetNummersOverzicht();

            result.Should().BeOfType<OkObjectResult>();
            var ok = result as OkObjectResult;
            ok!.Value.Should().NotBeNull();

            var list = ok.Value as System.Collections.IEnumerable;
            list.Should().NotBeNull();

            var enumerator = list!.GetEnumerator();
            enumerator.MoveNext().Should().BeTrue();
            var first = enumerator.Current!;

            var propSongId = first.GetType().GetProperty("SongId");
            propSongId.Should().NotBeNull();
            propSongId.GetValue(first).Should().Be(1);

            var propTitle = first.GetType().GetProperty("Title");
            propTitle.Should().NotBeNull();
            propTitle.GetValue(first).Should().Be("Song 1");

            var propArtist = first.GetType().GetProperty("Artist");
            propArtist.Should().NotBeNull();
            propArtist.GetValue(first).Should().Be("Artist 1");

            var propTimesListed = first.GetType().GetProperty("TimesListed");
            propTimesListed.Should().NotBeNull();
            propTimesListed.GetValue(first).Should().Be(2);

            var propHighest = first.GetType().GetProperty("HighestPosition");
            propHighest.Should().NotBeNull();
            propHighest.GetValue(first).Should().Be(2);
        }
    }
}
