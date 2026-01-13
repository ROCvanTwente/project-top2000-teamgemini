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

        // 1. DALERS -> Gebruikt nu DalerDto
        [HttpGet("dalers/{jaar}")]
        public async Task<ActionResult<IEnumerable<DalerDto>>> GetDalers(int jaar, int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<DalerDto>($"EXEC GetDalers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 2. STIJGERS -> Gebruikt nu StijgerDto
        [HttpGet("stijgers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StijgerDto>>> GetStijgers(int jaar, int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StijgerDto>($"EXEC GetStijgers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 3. ALLE EDITIES -> Gebruikt BaseStatistiekDto (heeft alleen Titel/Artiest/Jaar nodig)
        [HttpGet("alle-edities")]
        public async Task<ActionResult<IEnumerable<BaseStatistiekDto>>> GetAlleEdities(int aantal = 50)
        {
            var result = await _context.Database
                .SqlQuery<BaseStatistiekDto>($"EXEC GetAlleEdities @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 4. NIEUWE BINNENKOMERS -> Gebruikt StatistiekMetPositieDto
        [HttpGet("nieuwe-binnenkomers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekMetPositieDto>>> GetNieuweBinnenkomers(int jaar, int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekMetPositieDto>($"EXEC GetNieuweBinnenkomers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 5. VERDWENEN -> Gebruikt VerdwenenDto (PositieVorigJaar)
        [HttpGet("verdwenen/{jaar}")]
        public async Task<ActionResult<IEnumerable<VerdwenenDto>>> GetVerdwenen(int jaar, int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<VerdwenenDto>($"EXEC GetVerdwenen @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 6. OPNIEUW BINNEN -> Gebruikt StatistiekMetPositieDto
        [HttpGet("opnieuw-binnen/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekMetPositieDto>>> GetOpnieuwBinnen(int jaar, int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekMetPositieDto>($"EXEC GetOpnieuwBinnen @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 7. STABIEL -> Gebruikt StatistiekMetPositieDto
        [HttpGet("stabiel/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekMetPositieDto>>> GetStabiel(int jaar, int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekMetPositieDto>($"EXEC GetStabiel @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 8. AANSLUITENDE POSITIES -> Gebruikt StatistiekMetPositieDto
        [HttpGet("aansluitende-posities/{jaar}")]
        public async Task<ActionResult<IEnumerable<StatistiekMetPositieDto>>> GetAansluitendePosities(int jaar, int aantal = 10)
        {
            var result = await _context.Database
                .SqlQuery<StatistiekMetPositieDto>($"EXEC GetAansluitendePosities @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 9. EENMALIGE NOTERINGEN -> Gebruikt EenmaligeNoteringDto
        [HttpGet("eenmalige-noteringen")]
        public async Task<ActionResult<IEnumerable<EenmaligeNoteringDto>>> GetEenmaligeNoteringen(int aantal = 20)
        {
            var result = await _context.Database
                .SqlQuery<EenmaligeNoteringDto>($"EXEC GetEenmaligeNoteringen @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }

        // 10. TOP ARTIESTEN PER JAAR -> Deze bleef hetzelfde (gebruikte al een eigen DTO)
        [HttpGet("top-artiesten/{jaar}")]
        public async Task<ActionResult<IEnumerable<ArtiestStatistiekDto>>> GetTopArtiestenPerJaar(int jaar, int aantal = 3)
        {
            var result = await _context.Database
                .SqlQuery<ArtiestStatistiekDto>($"EXEC GetTopArtiestenPerJaar @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();
            return Ok(result);
        }
    }
}