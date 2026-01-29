using System.Linq;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data; // pas aan indien jouw DbContext-namespace anders is
using TemplateJwtProject.Models; // models voor seeden

namespace Top2000.Testing.TestHelpers
{
    public static class DbContextFactory
    {
        public static AppDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // seed alleen als leeg
            if (!context.Songs.Any())
            {
                var artist = new Artist { ArtistId = 1, Name = "Artist 1" };
                var song = new Song { SongId = 1, Title = "Song 1", ArtistId = 1 };
                var entry = new Top2000Entry { Id = 1, SongId = 1, Year = 2020, Rank = 1 };

                context.Artists.Add(artist);
                context.Songs.Add(song);
                context.Top2000Entries.Add(entry);

                context.SaveChanges();
            }

            return context;
        }
    }
}
