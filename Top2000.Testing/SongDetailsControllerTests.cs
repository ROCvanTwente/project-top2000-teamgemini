using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TemplateJwtProject.Controllers;
using Top2000.Testing.TestHelpers;
using Xunit;

namespace Top2000.Testing
{
    public class SongDetailsControllerTests
    {
        [Fact]
        public async Task GetSongDetails_ReturnsOkWithResult_WhenSongExists()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = DbContextFactory.Create(dbName);
            var controller = new SongDetailsController(context);

            var result = await controller.GetSongDetails(1);

            result.Should().BeOfType<OkObjectResult>();
            var ok = result as OkObjectResult;
            ok.Value.Should().NotBeNull();
            var prop = ok.Value!.GetType().GetProperty("SongId");
            prop.Should().NotBeNull();
            prop.GetValue(ok.Value).Should().Be(1);
        }

        [Fact]
        public async Task GetSongDetails_ReturnsNotFound_WhenSongDoesNotExist()
        {
            var dbName = Guid.NewGuid().ToString();
            using var context = DbContextFactory.Create(dbName);
            var controller = new SongDetailsController(context);

            var result = await controller.GetSongDetails(999);

            result.Should().BeOfType<NotFoundObjectResult>();
            var notFound = result as NotFoundObjectResult;
            notFound!.Value.Should().Be($"Song met id 999 niet gevonden.");
        }

        [Fact]
        public async Task GetSongDetails_ReturnsOkWithExceptionMessage_WhenContextDisposed()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = DbContextFactory.Create(dbName);
            var controller = new SongDetailsController(context);

            context.Dispose();

            var result = await controller.GetSongDetails(1);

            result.Should().BeOfType<OkObjectResult>();
            var ok = result as OkObjectResult;
            ok!.Value.Should().BeOfType<string>();
            ((string)ok.Value).Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GetSongDetails_ReturnsStatsAndPositions_WhenSongHasEntries()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using var context = new AppDbContext(options);

            var artist = new Artist { ArtistId = 10, Name = "Test Artist" };
            var song = new Songs { SongId = 10, ArtistId = 10, Titel = "Test Song", ReleaseYear = 2000 };

            context.Artists.Add(artist);
            context.Songs.Add(song);
            context.Top2000Entries.Add(new Top2000Entry { SongId = 10, Year = 2021, Position = 3 });
            context.Top2000Entries.Add(new Top2000Entry { SongId = 10, Year = 2020, Position = 5 });
            context.SaveChanges();

            var controller = new SongDetailsController(context);

            var result = await controller.GetSongDetails(10);

            result.Should().BeOfType<OkObjectResult>();
            var ok = result as OkObjectResult;
            ok!.Value.Should().NotBeNull();

            var value = ok.Value!;
            var propStats = value.GetType().GetProperty("Stats");
            propStats.Should().NotBeNull();
            var stats = propStats.GetValue(value);
            stats.Should().NotBeNull();

            var timesListedProp = stats!.GetType().GetProperty("TimesListed");
            timesListedProp.Should().NotBeNull();
            timesListedProp.GetValue(stats).Should().Be(2);

            var highestProp = stats.GetType().GetProperty("HighestPosition");
            highestProp.Should().NotBeNull();
            highestProp.GetValue(stats).Should().Be(3);

            var topPositionsProp = value.GetType().GetProperty("Top2000Positions");
            topPositionsProp.Should().NotBeNull();
            var positions = topPositionsProp.GetValue(value) as System.Collections.IEnumerable;
            positions.Should().NotBeNull();

            var en = positions!.GetEnumerator();
            en.MoveNext().Should().BeTrue();
            var first = en.Current!;
            var yearProp = first.GetType().GetProperty("Year");
            yearProp.Should().NotBeNull();
            yearProp.GetValue(first).Should().Be(2021);
        }
    }
}
