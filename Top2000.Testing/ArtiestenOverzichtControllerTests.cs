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
    public class ArtiestenOverzichtControllerTests
    {
        [Fact]
        public async Task GetAllArtists_ReturnsOk_WithArtistsOrdered()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using var context = new AppDbContext(options);

            context.Artists.Add(new Artist { ArtistId = 1, Name = "Zebra" });
            context.Artists.Add(new Artist { ArtistId = 2, Name = "Alpha" });
            context.Songs.Add(new Songs { SongId = 1, ArtistId = 2, Titel = "A Song" });
            context.SaveChanges();

            var controller = new ArtiestenOverzichtController(context);

            var result = await controller.GetAllArtists();

            result.Should().BeOfType<OkObjectResult>();
            var ok = result as OkObjectResult;
            ok!.Value.Should().NotBeNull();

            var list = ok.Value as System.Collections.Generic.List<Artist>;
            list.Should().NotBeNull();
            list![0].Name.Should().Be("Alpha");
            list[1].Name.Should().Be("Zebra");
        }
    }
}
