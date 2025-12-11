using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("artist")]
    public class ArtiestenOverzichtController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArtiestenOverzichtController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/ArtiestenOverzicht
        /// Haalt alle artiesten op, gesorteerd op naam (alfabetisch)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllArtists()
        {
            var artists = await _context.Set<TemplateJwtProject.Models.Artist>()
                .Include(a => a.Songs)
                .OrderBy(a => a.Name)
                .ToListAsync();
            return Ok(artists);
        }

        /// <summary>
        /// GET: api/ArtiestenOverzicht/filter?searchTerm=Queen&minSongs=3&hasBiography=true
        /// Filtert artiesten op meerdere criteria:
        /// - searchTerm: Zoekt in artiestenaam
        /// - minSongs: Minimum aantal nummers
        /// - hasBiography: Of artiest een biografie heeft (true/false)
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredArtists(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? minSongs = null,
            [FromQuery] bool? hasBiography = null)
        {
            // Start met alle artiesten
            var query = _context.Set<TemplateJwtProject.Models.Artist>()
                .Include(a => a.Songs)
                .AsQueryable();

            // Filter op naam als searchTerm is opgegeven
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Name.Contains(searchTerm));
            }

            // Filter op minimum aantal nummers
            if (minSongs.HasValue)
            {
                query = query.Where(a => a.Songs.Count >= minSongs.Value);
            }

            // Filter op wel/geen biografie
            if (hasBiography.HasValue)
            {
                if (hasBiography.Value)
                {
                    query = query.Where(a => !string.IsNullOrEmpty(a.Biography));
                }
                else
                {
                    query = query.Where(a => string.IsNullOrEmpty(a.Biography));
                }
            }

            var artists = await query.ToListAsync();
            return Ok(artists);
        }

        /// <summary>
        /// GET: api/ArtiestenOverzicht/with-songs
        /// Haalt alleen artiesten op die minimaal 1 nummer hebben
        /// </summary>
        [HttpGet("with-songs")]
        public async Task<IActionResult> GetArtistsWithSongs()
        {
            var artists = await _context.Set<TemplateJwtProject.Models.Artist>()
                .Include(a => a.Songs)
                .Where(a => a.Songs.Any())
                .ToListAsync();
            return Ok(artists);
        }

        /// <summary>
        /// GET: api/ArtiestenOverzicht/alphabetical
        /// Haalt alle artiesten op, alfabetisch gesorteerd op naam
        /// </summary>
        [HttpGet("alphabetical")]
        public async Task<IActionResult> GetArtistsAlphabetical()
        {
            var artists = await _context.Set<TemplateJwtProject.Models.Artist>()
                .Include(a => a.Songs)
                .OrderBy(a => a.Name)
                .ToListAsync();
            return Ok(artists);
        }

        public static string GetControllerName()
        {
            return "ArtiestenOverzicht";
        }
    }
}
