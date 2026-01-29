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
    }
}
