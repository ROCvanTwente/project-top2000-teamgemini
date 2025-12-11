using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplateJwtProject.Data;
using TemplateJwtProject.Models.DTOs;

namespace TemplateJwtProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatistiekenOverzichtController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatistiekenOverzichtController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Dalers
        [HttpGet("dalers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetDalers(int jaar, [FromQuery] int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetDalers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 2. Stijgers
        [HttpGet("stijgers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetStijgers(int jaar, [FromQuery] int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetStijgers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 3. Alle Edities
        [HttpGet("alle-edities")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetAlleEdities([FromQuery] int aantal = 50)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetAlleEdities @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 4. Nieuwe Binnenkomers
        [HttpGet("nieuwe-binnenkomers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetNieuweBinnenkomers(int jaar, [FromQuery] int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetNieuweBinnenkomers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 5. Verdwenen
        [HttpGet("verdwenen/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetVerdwenen(int jaar, [FromQuery] int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetVerdwenen @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 6. Opnieuw Binnen
        [HttpGet("opnieuw-binnen/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetOpnieuwBinnen(int jaar, [FromQuery] int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetOpnieuwBinnen @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 7. Stabiel
        [HttpGet("stabiel/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetStabiel(int jaar, [FromQuery] int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetStabiel @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 8. Aansluitende Posities
        [HttpGet("aansluitende-posities/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetAansluitendePosities(int jaar, [FromQuery] int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetAansluitendePosities @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 9. Eenmalige Noteringen
        [HttpGet("eenmalige-noteringen")]
        public async Task<ActionResult<IEnumerable<StatistiekItemDto>>> GetEenmaligeNoteringen([FromQuery] int aantal = 20)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekItemDto>($"EXEC GetEenmaligeNoteringen @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 10. Top Artiesten Per Jaar (Gebruikt een andere DTO!)
        [HttpGet("top-artiesten/{jaar}")]
        public async Task<ActionResult<IEnumerable<TopArtiestDto>>> GetTopArtiesten(int jaar, [FromQuery] int aantal = 3)
        {
            var result = await _context.Database
                .SqlQuery<TopArtiestDto>($"EXEC GetTopArtiestenPerJaar @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }
    }

    // Extra DTO specifiek voor statistiek 10
    public class TopArtiestDto
    {
        public string Naam { get; set; } = string.Empty;
        public int AantalLiedjes { get; set; }
        public int? GemiddeldePositie { get; set; } // Kan null zijn als SQL AVG int/null teruggeeft
        public int HoogsteNotering { get; set; }
    }
}