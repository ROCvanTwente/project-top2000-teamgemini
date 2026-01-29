using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("api/artist")]
    public class ArtistDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArtistDetailsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/artist/{id}
        /// Haalt een specifieke artiest op via ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtistById(int id)
        {
            try
            {
                var artist = await _context.Set<TemplateJwtProject.Models.Artist>()
                    .Include(a => a.Songs)
                    .ThenInclude(s => s.Top2000Entries)
                    .FirstOrDefaultAsync(a => a.ArtistId == id);

                if (artist == null)
                {
                    return NotFound($"Artist with ID {id} not found");
                }

                var result = new
                {
                    ArtistId = artist.ArtistId,
                    Name = artist.Name,
                    Wiki = artist.Wiki,
                    Biography = artist.Biography,
                    Photo = artist.Photo
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// GET: api/artist/{id}/songs
        /// Haalt alle nummers van een specifieke artiest op
        /// </summary>
        [HttpGet("{id}/songs")]
        public async Task<IActionResult> GetArtistSongs(int id)
        {
            try
            {
                var artist = await _context.Set<TemplateJwtProject.Models.Artist>()
                    .Include(a => a.Songs)
                    .ThenInclude(s => s.Top2000Entries)
                    .FirstOrDefaultAsync(a => a.ArtistId == id);

                if (artist == null)
                {
                    return NotFound($"Artist with ID {id} not found");
                }

                var songs = artist.Songs.Select(s => new
                {
                    songId = s.SongId,
                    title = s.Titel,
                    artist = artist.Name,
                    releaseYear = s.ReleaseYear,
                    youtubeLink = s.Youtube,
                    timesListed = s.Top2000Entries.Count(),
                    highestPosition = s.Top2000Entries.Any() ? s.Top2000Entries.Min(e => e.Position) : (int?)null
                }).ToList();

                return Ok(songs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
