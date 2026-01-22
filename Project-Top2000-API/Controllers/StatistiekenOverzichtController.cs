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

        // =================================================================
        // 1. DALERS
        // =================================================================
        [HttpGet("dalers/{jaar}")]
        public async Task<ActionResult<IEnumerable<DalerDto>>> GetDalers(int jaar, [FromQuery] int aantal = 10)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<DalerDto>($"EXEC GetDalers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen dalers gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // 2. STIJGERS
        // =================================================================
        [HttpGet("stijgers/{jaar}")]
        public async Task<ActionResult<IEnumerable<StijgerDto>>> GetStijgers(int jaar, [FromQuery] int aantal = 10)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<StijgerDto>($"EXEC GetStijgers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen stijgers gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // 3. GetEvergreens (De 'Evergreens' of onverwoestbaren)
        // =================================================================
        [HttpGet("evergreens")]
        public async Task<ActionResult<IEnumerable<BasisStatistiekDto>>> GetEvergreens([FromQuery] int aantal = 50)
        {
            // Validatie aanpassen: jaar is null, aantal checken
            var validatieFout = ValideerInput(null, aantal);
            if (validatieFout != null) return validatieFout;

            // Hier roep je de nieuwe SP naam aan
            var result = await _context.Database
                .SqlQuery<BasisStatistiekDto>($"EXEC GetEvergreens @Aantal = {aantal}")
                .ToListAsync();

            return Ok(result);
        }

        // =================================================================
        // 4. NIEUWE BINNENKOMERS
        // =================================================================
        [HttpGet("nieuwe-binnenkomers/{jaar}")]
        public async Task<ActionResult<IEnumerable<PositieStatistiekDto>>> GetNieuweBinnenkomers(int jaar, [FromQuery] int aantal = 10)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<PositieStatistiekDto>($"EXEC GetNieuweBinnenkomers @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen nieuwe binnenkomers gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // 5. VERDWENEN
        // =================================================================
        [HttpGet("verdwenen/{jaar}")]
        public async Task<ActionResult<IEnumerable<VerdwenenDto>>> GetVerdwenen(int jaar, [FromQuery] int aantal = 10)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<VerdwenenDto>($"EXEC GetVerdwenen @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen verdwenen nummers gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // 6. OPNIEUW BINNEN
        // =================================================================
        [HttpGet("opnieuw-binnen/{jaar}")]
        public async Task<ActionResult<IEnumerable<PositieStatistiekDto>>> GetOpnieuwBinnen(int jaar, [FromQuery] int aantal = 10)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<PositieStatistiekDto>($"EXEC GetOpnieuwBinnen @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen 'opnieuw binnen' nummers gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // 7. STABIEL
        // =================================================================
        [HttpGet("stabiel/{jaar}")]
        public async Task<ActionResult<IEnumerable<PositieStatistiekDto>>> GetStabiel(int jaar, [FromQuery] int aantal = 10)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<PositieStatistiekDto>($"EXEC GetStabiel @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen stabiele nummers gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // 8. AANSLUITENDE POSITIES
        // =================================================================
        [HttpGet("aansluitende-posities/{jaar}")]
        public async Task<ActionResult<IEnumerable<PositieStatistiekDto>>> GetAansluitendePosities(int jaar, [FromQuery] int aantal = 10)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<PositieStatistiekDto>($"EXEC GetAansluitendePosities @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen aansluitende posities gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // 9. EENMALIGE NOTERINGEN (Geen jaar nodig)
        // =================================================================
        [HttpGet("eenmalige-noteringen")]
        public async Task<ActionResult<IEnumerable<EenmaligDto>>> GetEenmaligeNoteringen([FromQuery] int aantal = 20)
        {
            var validatieFout = ValideerInput(null, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<EenmaligDto>($"EXEC GetEenmaligeNoteringen @Aantal = {aantal}")
                .ToListAsync();

            return Ok(result);
        }

        // =================================================================
        // 10. TOP ARTIESTEN PER JAAR
        // =================================================================
        [HttpGet("top-artiesten/{jaar}")]
        public async Task<ActionResult<IEnumerable<TopArtiestDto>>> GetTopArtiesten(int jaar, [FromQuery] int aantal = 3)
        {
            var validatieFout = ValideerInput(jaar, aantal);
            if (validatieFout != null) return validatieFout;

            var result = await _context.Database
                .SqlQuery<TopArtiestDto>($"EXEC GetTopArtiestenPerJaar @Jaar = {jaar}, @Aantal = {aantal}")
                .ToListAsync();

            if (result.Count == 0) return NotFound($"Geen data gevonden voor het jaar {jaar}.");
            return Ok(result);
        }

        // =================================================================
        // PRIVATE HULPMETHODE VOOR VALIDATIE
        // =================================================================
        private ActionResult? ValideerInput(int? jaar, int aantal)
        {
            // 1. Check Aantal
            if (aantal <= 0 || aantal > 2000)
            {
                return BadRequest("Het aantal moet tussen 1 en 2000 liggen.");
            }

            // 2. Check Jaar (alleen als het niet null is)
            if (jaar.HasValue)
            {
                // De Top 2000 begon in 1999
                if (jaar.Value < 1999 || jaar.Value > 2024)
                {
                    return BadRequest($"Het jaar moet tussen 1999 en 2024 liggen.");
                }
            }

            // Geen fouten gevonden
            return null;
        }
    }

    // Specifieke DTO voor endpoint 10
    public class TopArtiestDto
    {
        public string Naam { get; set; } = string.Empty;
        public int AantalLiedjes { get; set; }
        public int? GemiddeldePositie { get; set; }
        public int HoogsteNotering { get; set; }
    }
}