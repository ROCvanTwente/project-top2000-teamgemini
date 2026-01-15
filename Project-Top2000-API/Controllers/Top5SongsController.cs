using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("top5songs")]
    public class Top5SongsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Top5SongsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> GetTop5Songs(int year)
        {
            var entries = await _context.Set<Top2000Entry>()
                .Where(entry => entry.Year == year)
                .Include(entry => entry.Songs)
                    .ThenInclude(song => song.Artist)
                .OrderBy(entry => entry.Position)
                .Take(5)
                .Select(entry => new
                {
                    SongId = entry.Songs.SongId,
                    Position = entry.Position,
                    Title = entry.Songs.Titel,
                    Artist = entry.Songs.Artist.Name,
                    ReleaseYear = entry.Songs.ReleaseYear,
                })
                .ToListAsync();

            if (!entries.Any())
                return NotFound($"Geen Top 5 gevonden voor jaar {year}.");

            return Ok(entries);
        }
    }
}
