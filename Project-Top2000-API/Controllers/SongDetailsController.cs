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


        [HttpGet("{songId}")]
        public async Task<IActionResult> GetSongDetails(int songId)
        {
            try
            {
                var song = await _context.Set<Songs>()
                              .Include(s => s.Artist)
                              .Include(s => s.Top2000Entries)
                              .FirstOrDefaultAsync(s => s.SongId == songId);

                if (song == null)
                    return NotFound($"Song met id {songId} niet gevonden.");

                var positions = song.Top2000Entries
                    .OrderByDescending(e => e.Year)
                    .Select(e => new
                    {
                        Year = e.Year,
                        Position = e.Position
                    })
                    .ToList();

                var result = new
                {
                    SongId = song.SongId,
                    Title = song.Titel,
                    Artist = song.Artist.Name,
                    ReleaseYear = song.ReleaseYear,
                    ImgUrl = song.ImgUrl,
                    Youtube = song.Youtube,
                    Lyrics = song.Lyrics,

                    Stats = new
                    {
                        TimesListed = positions.Count,
                        HighestPosition = positions.Any()
            ? (int?)positions.Min(p => p.Position)
            : null,
                        FirstYear = positions.LastOrDefault()?.Year,
                        LastYear = positions.FirstOrDefault()?.Year
                    },


                    Top2000Positions = positions
                };

                return Ok(result);
            } catch (Exception ex)
            {
                return Ok(ex.Message);
            }
          
        }
    }
}
