using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;

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
            var list = await _context.Set<TemplateJwtProject.Models.Top2000Entry>()
                .Where(t => t.Year == year)
                .Include(t => t.Songs)
                    .ThenInclude(s => s.Artist)
                .OrderBy(t => t.Position)
                .Select(t => new
                {
                    SongId = t.Songs.SongId,
                    Position = t.Position,
                    Title = t.Songs.Titel,
                    Artist = t.Songs.Artist.Name,
                    ReleaseYear = t.Songs.ReleaseYear
                })
                .ToListAsync();

            if (!list.Any())
                return NotFound($"Geen Top2000 entries gevonden voor jaar {year}.");

            return Ok(list);
        }
    }
}
