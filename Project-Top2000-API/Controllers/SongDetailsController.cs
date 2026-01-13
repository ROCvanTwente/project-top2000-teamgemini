using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;
using TemplateJwtProject.Models.DTOs;

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

            var chartHistory = await _context.Set<Top2000Entry>()
                .Where(entry => entry.SongId == songId)
                .OrderByDescending(entry => entry.Year)
                .Select(entry => new ChartDto
                {
                    Year = entry.Year,
                    Position = entry.Position
                })
                .ToListAsync();

            var result = new Songsdto
            {
                SongId = song.SongId,
                Titel = song.Titel,
                ArtistName = song.Artist?.Name ?? "",
                Lyrics = song.Lyrics,
                ReleaseYear = song.ReleaseYear,
                ArtistBiography = song.Artist?.Biography,
                ChartHistory = chartHistory
            };

            return Ok(result);
        }
    }
}
