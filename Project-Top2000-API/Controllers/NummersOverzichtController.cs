using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("songs")]
    public class NummersOverzichtController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NummersOverzichtController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetNummersOverzicht()
        {
            var songs = await _context.Set<Songs>()
                .Include(s => s.Artist)
                .Include(s => s.Top2000Entries)
                .Select(s => new
                {
                    SongId = s.SongId,
                    Title = s.Titel,
                    Artist = s.Artist.Name,
                    ReleaseYear = s.ReleaseYear,
                    ImageUrl = s.ImageUrl,
                    SpotifyLink = s.SpotifyLink,
                    TimesListed = s.Top2000Entries.Count,
                    HighestPosition = s.Top2000Entries.Any()
                        ? (int?)s.Top2000Entries.Min(e => e.Position)
                        : null
                })
                .OrderBy(s => s.Title)
                .ToListAsync();

            return Ok(songs);
        }
    }
}
