using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("songs")]
    public class SongDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SongDetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{songId}/stats")]
        public async Task<IActionResult> GetSongStats(int songId)
        {
            var song = await _context.Set<Songs>()
                .Include(song => song.Artist)
                .FirstOrDefaultAsync(song => song.SongId == songId);

            if (song == null)
                return NotFound($"Song met id {songId} niet gevonden.");

            var top2000Positions = await _context.Set<Top2000Entry>()
                .Where(entry => entry.SongId == songId)
                .OrderByDescending(entry => entry.Year)
                .Select(entry => new
                {
                    Year = entry.Year,
                    Position = entry.Position
                })
                .ToListAsync();

            var result = new
            {
                SongId = song.SongId,
                Title = song.Titel,
                Artist = song.Artist.Name,
                ReleaseYear = song.ReleaseYear,
                Lyrics = song.Lyrics,
                Top2000Positions = top2000Positions
            };

            return Ok(result);
        }
    }
}
