using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("top2000")]
    public class JaarOverzichtController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JaarOverzichtController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> GetTop2000ByYear(int year)
        {
            var entries = await _context.Set<Top2000Entry>()
                .Where(entry => entry.Year == year)
                .Include(entry => entry.Songs)
                    .ThenInclude(song => song.Artist)
                .OrderBy(entry => entry.Position)
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
                return NotFound($"Geen Top2000 entries gevonden voor jaar {year}.");

            return Ok(entries);
        }
    }
}
