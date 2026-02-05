using System;
using System.Linq;
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
    public class ArtistDetailsControllerTests
    {
        [Fact]
        public async Task GetArtistById_ReturnsNotFound_WhenMissing()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using var context = new AppDbContext(options);

            var controller = new ArtistDetailsController(context);

            var result = await controller.GetArtistById(999);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetArtistSongs_ReturnsSongsWithStats()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using var context = new AppDbContext(options);

            var artist = new Artist { ArtistId = 20, Name = "Artist X" };
            context.Artists.Add(artist);
            context.Songs.Add(new Songs { SongId = 20, ArtistId = 20, Titel = "Song X" });
            context.Top2000Entries.Add(new Top2000Entry { SongId = 20, Year = 2018, Position = 10 });
            context.SaveChanges();

            var controller = new ArtistDetailsController(context);

            var result = await controller.GetArtistSongs(20);

            result.Should().BeOfType<OkObjectResult>();
            var ok = result as OkObjectResult;
            ok!.Value.Should().NotBeNull();

            var list = ok.Value as System.Collections.IEnumerable;
            list.Should().NotBeNull();
        }
    }
}
